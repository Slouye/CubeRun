using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {
    private GameObject m_prefab_tile;
    private GameObject m_prefab_wall;
    private GameObject m_prefab_spikes;
    private GameObject m_prefab_sky_spikes;
    private GameObject m_prefab_gem;

    public List<GameObject[]> mapList = new List<GameObject[]>();//地图数据存储
    private Transform m_Transform;
    public float bottomLength = Mathf.Sqrt(2) * 0.254f;//地板间隔
    private Color colorWall = new Color(87 / 255f, 93 / 255f, 169 / 255f);//墙壁颜色
    private Color colorOne = new Color(124 / 255f, 155 / 255f, 230 / 255f);//单排颜色
    private Color colorTwo = new Color(125 / 255f, 169 / 255f, 233 / 255f);//双排颜色
    private int index = 0;
    private PlayerController m_PlayerController;
    //随机值
    private int pr_hole = 0;
    private int pr_spikes = 0;
    private int pr_sky_spikes = 0;
    private int pr_gem = 2;

    // Use this for initialization
    void Start () {
        m_prefab_tile = Resources.Load("tile_white") as GameObject;
        m_prefab_wall = Resources.Load("wall2") as GameObject;
        m_prefab_spikes = Resources.Load("moving_spikes") as GameObject;
        m_prefab_sky_spikes = Resources.Load("smashing_spikes") as GameObject;
        m_prefab_gem = Resources.Load("gem 2") as GameObject;

        m_Transform = gameObject.GetComponent<Transform>();
        m_PlayerController = GameObject.Find("cube_books").GetComponent<PlayerController>();
        CreateMapItem(0);
    }
	/// <summary>
    /// 创建地图元素
    /// </summary>
    public void CreateMapItem(float offsetZ)
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject[] item = new GameObject[6];
            //单排
            for (int j = 0; j < 6; j++)
            {
                Vector3 pos = new Vector3(j * bottomLength, 0, offsetZ + i * bottomLength);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;
                if (j==0||j==5)
                {
                    tile = GameObject.Instantiate(m_prefab_wall, pos, Quaternion.Euler(rot)) as GameObject;
                    tile.GetComponent<MeshRenderer>().material.color = colorWall;
                }
                else
                {
                    int pr = CalcPR();
                    if (pr == 0)
                    {
                        tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot)) as GameObject;
                        tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorOne;
                        tile.GetComponent<MeshRenderer>().material.color = colorOne;
                    }
                    else if(pr == 1)
                    {
                        tile = new GameObject();
                        tile.GetComponent<Transform>().position = pos;
                        tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                    }
                    else if (pr == 2)
                    {
                        tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                    }
                    else if (pr == 3)
                    {
                        tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                    }
                    
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item[j] = tile;
            }
            mapList.Add(item);
            GameObject[] item2 = new GameObject[5];
            //双排
            for (int j = 0; j < 5; j++)
            {
                Vector3 pos = new Vector3(j * bottomLength + bottomLength / 2, 0, offsetZ + i * bottomLength + bottomLength / 2);
                Vector3 rot = new Vector3(-90, 45, 0);
                GameObject tile = null;
                int pr = CalcPR();
                if (pr == 0)
                {
                    tile = GameObject.Instantiate(m_prefab_tile, pos, Quaternion.Euler(rot)) as GameObject;
                    tile.GetComponent<Transform>().FindChild("normal_a2").GetComponent<MeshRenderer>().material.color = colorTwo;
                    tile.GetComponent<MeshRenderer>().material.color = colorTwo;

                    if (GemPR() ==1 )
                    {
                        GameObject gem = GameObject.Instantiate(m_prefab_gem, tile.GetComponent<Transform>().position + new Vector3(0, 0.06f, 0), Quaternion.identity) as GameObject;
                        gem.GetComponent<Transform>().SetParent(tile.GetComponent<Transform>());
                    }
                }
                else if (pr == 1)
                {
                    tile = new GameObject();
                    tile.GetComponent<Transform>().position = pos;
                    tile.GetComponent<Transform>().rotation = Quaternion.Euler(rot);
                }
                else if (pr == 2)
                {
                    tile = GameObject.Instantiate(m_prefab_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                }
                else if (pr == 3)
                {
                    tile = GameObject.Instantiate(m_prefab_sky_spikes, pos, Quaternion.Euler(rot)) as GameObject;
                }
                tile.GetComponent<Transform>().SetParent(m_Transform);
                item2[j] = tile;
            }
            mapList.Add(item2);
        }
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string str = "";
            for (int i = 0; i < mapList.Count; i++)
            {
                for (int j = 0; j < mapList[i].Length; j++)
                {
                    str += mapList[i][j].name;
                    mapList[i][j].name = i + "-" + j;
                }
                str += "\n";
            }
            Debug.Log(str);
        }
	}

    /// <summary>
    /// 开启地面塌陷效果
    /// </summary>
    public void StartTileDown()
    {
        StartCoroutine("TileDown");
    }

    /// <summary>
    /// 停止地面塌陷效果
    /// </summary>
    public void SpotTileDown()
    {
        StopCoroutine("TileDown");
    }

    /// <summary>
    /// 地面塌陷
    /// </summary>
    /// <returns></returns>
    private IEnumerator TileDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < mapList[index].Length; i++)
            {
                Rigidbody rb = mapList[index][i].AddComponent<Rigidbody>();
                rb.angularVelocity = new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * Random.Range(1,10);
                GameObject.Destroy(mapList[index][i], 1.0f);
            }
            if (m_PlayerController.z == index)
            {
                SpotTileDown();
                m_PlayerController.gameObject.AddComponent<Rigidbody>();
                m_PlayerController.StartCoroutine("GameOver", true);
            }
            index++;
        }
    }

    /// <summary>
    /// 计算概率
    /// 0：瓷砖
    /// 1：坑洞
    /// 2：地面陷阱
    /// 3：天空陷阱
    /// </summary>
    /// <returns></returns>
    private int CalcPR()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_hole)
        {
            return 1;
        }
        else if (31 < pr && pr< pr_spikes + 30 )
        {
            return 2;
        }
        else if (61 < pr && pr < pr_sky_spikes +60)
        {
            return 3;
        }
        return 0;
    }

    /// <summary>
    /// 宝石概率
    /// </summary>
    /// <returns>0：不生成，1：生成</returns>
    private int GemPR()
    {
        int pr = Random.Range(1, 100);
        if (pr <= pr_gem)
        {
            return 1;
        }
        return 0;
    }

    public void AddPR()
    {
        pr_hole += 2;
        pr_spikes += 2;
        pr_sky_spikes += 2;
    }

    public void ReserGameMap()
    {
        Transform[] son_Transform = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 1; i < son_Transform.Length; i++)
        {
            GameObject.Destroy(son_Transform[i].gameObject);
        }
        pr_hole = 0;
        pr_spikes = 0;
        pr_sky_spikes = 0;
        pr_gem = 2;
        SpotTileDown();
        mapList.Clear();
        index = 0;
        CreateMapItem(0);
    }
}
