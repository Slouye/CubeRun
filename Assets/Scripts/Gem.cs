using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
    private Transform m_Transform;
    private Transform m_gem_Transform;
    // Use this for initialization
    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        m_gem_Transform = m_Transform.FindChild("gem 3");

    }
	
	// Update is called once per frame
	void Update () {
        m_gem_Transform.Rotate(new Vector3( Random.Range(0.0F, 1.0F), Random.Range(0.0F, 1.0F), Random.Range(0.0F, 1.0F)));

    }
}
