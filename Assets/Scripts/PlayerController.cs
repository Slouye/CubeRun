using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    private Color colorOne = new Color(122/255f,85/255f,179/255f);
    private Color colorTwo = new Color(126/255f,93/255f,183/255f);

    public int z = 3;
    private int x = 2;
    private Transform m_Transform;
    private MapManager m_MapManager;
    private CamerFollow m_CamerFollow;
    private UIManager m_UIManager;

    private bool life = false;
    private int gemCount = 0;
    private int count = 0;




    private void SaveDate()
    {
        PlayerPrefs.SetInt("gem", gemCount);
        if (count > PlayerPrefs.GetInt("count",0))
        {
            PlayerPrefs.SetInt("count", count);
        }
    }
    // Use this for initialization
    void Start () {
        gemCount = PlayerPrefs.GetInt("gem", 0);
        m_Transform = gameObject.GetComponent<Transform>();
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        m_CamerFollow = GameObject.Find("Main Camera").GetComponent<CamerFollow>();
        m_UIManager = GameObject.Find("UI Root").GetComponent<UIManager>();

    }


    private void AddGetCount()
    {
        gemCount++;
        Debug.Log("宝石：" + gemCount);
        m_UIManager.UpdateData(count, gemCount);
    }

    private void AddCount()
    {
        count++;
        Debug.Log("分数：" + count);
        m_UIManager.UpdateData(count, gemCount);
    }

    public void StartGame()
    {
        life = true;
        SetPlayPos();
        m_CamerFollow.startFollow = true;
        m_MapManager.StartTileDown();
    }



	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartGame();
        }

        if (life)
        {
            PlayControl();
        }
        
        
	}

    public void Left()
    {
        if (life)
        {
            if (x != 0)
            {
                z++;
                AddCount();
            }
            if (z % 2 == 1 && x != 0)
            {
                x--;
            }
            SetPlayPos();
            CalcPosition();
        }
    }

    public void Right()
    {
        if (life)
        {
            if (x != 4 || z % 2 != 1)
            {
                z++;
                AddCount();
            }
            if (z % 2 == 0 && x != 4)
            {
                x++;
            }
            SetPlayPos();
            CalcPosition();
        }
    }

    /// <summary>
    /// 角色移动控制
    /// </summary>
    private void PlayControl()
    {
        //左移
        if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }

        //右移
        if (Input.GetKeyDown(KeyCode.D))
        {
            Right();
        }
    }

    /// <summary>
    /// 角色位置、蜗牛痕迹
    /// </summary>
    private void SetPlayPos()
    {
        Transform playPos = m_MapManager.mapList[z][x].GetComponent<Transform>();
        
        MeshRenderer normal_a2 = null;
        m_Transform.position = playPos.position + new Vector3(0, 0.254f / 2.0f, 0);
        m_Transform.rotation = playPos.rotation;
        if (playPos.tag == "Tile")
        {
            normal_a2 = playPos.FindChild("normal_a2").GetComponent<MeshRenderer>();
        }
        else if (playPos.tag == "Spikes")
        {
            normal_a2 = playPos.FindChild("moving_spikes_a2").GetComponent<MeshRenderer>();
        }
        else if (playPos.tag == "Sky_Spikes")
        {
            normal_a2 = playPos.FindChild("smashing_spikes_a2").GetComponent<MeshRenderer>();
        }
        if (normal_a2 != null)
        {
            if (z % 2 == 0)
            {
                normal_a2.material.color = colorOne;
            }
            else
            {
                normal_a2.material.color = colorTwo;
            }
        }
        else
        {
            gameObject.AddComponent<Rigidbody>();
            StartCoroutine("GameOver", true);
        }


        
       
    }

    /// <summary>
    /// 计算角色是否快靠近顶点
    /// </summary>
    private void CalcPosition()
    {
        if (m_MapManager.mapList.Count - z <= 12)
        {
            Debug.Log("生成地图");
            m_MapManager.AddPR();
            float offsetZ = m_MapManager.mapList[m_MapManager.mapList.Count - 1][0].GetComponent<Transform>().position.z + m_MapManager.bottomLength/2;
            m_MapManager.CreateMapItem(offsetZ);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Soikes_Attack")
        {
            StartCoroutine("GameOver", false);
        }

        if (coll.tag == "Gem")
        {
            GameObject.Destroy(coll.gameObject.GetComponent<Transform>().parent.gameObject);
            AddGetCount();
        }
    }

    //角色死亡
    public IEnumerator GameOver(bool b)
    {
        if (b)
        {
            yield return new WaitForSeconds(0.5f);
        }
        if (life)
        {
            Debug.Log("游戏结束");
            m_CamerFollow.startFollow = false;
            life = false;
            SaveDate();
            StartCoroutine("ResetGame");
        }
    }

    private void RestPlayer()
    {
        GameObject.Destroy(gameObject.GetComponent<Rigidbody>());
        z = 3;
        x = 2;
        life = false;
        count = 0;
}

    private IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(2.0f);
        RestPlayer();
        m_MapManager.ReserGameMap();
        m_CamerFollow.ResetCamer();
        m_UIManager.ResetUI();
    }
}
