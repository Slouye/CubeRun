using UnityEngine;
using System.Collections;

public class CamerFollow : MonoBehaviour {
    private Transform m_Transform;
    private Transform m_Player;
    public bool startFollow = false;
    private Vector3 normalPOS;
    // Use this for initialization
    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        normalPOS = m_Transform.position;
        m_Player = GameObject.Find("cube_books").GetComponent<Transform>();

    }

    public void ResetCamer()
    {
        m_Transform.position = normalPOS;
    }

    // Update is called once per frame
    void Update () {
        CamerMove();

    }

    void CamerMove()
    {
        if (startFollow)
        {
            Vector3 nextPos = new Vector3(m_Transform.position.x, m_Player.position.y + 1.5f, m_Player.position.z);
            m_Transform.position = Vector3.Lerp(m_Transform.position, nextPos, Time.deltaTime);
        }
        
    }
}
