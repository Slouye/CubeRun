using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {
    private Transform m_Transform;
    private Transform son_Transform;

    private Vector3 normalPOS;
    private Vector3 targetPOS;

    // Use this for initialization
    void Start () {
        m_Transform = gameObject.GetComponent<Transform>();
        son_Transform = m_Transform.FindChild("moving_spikes_b").GetComponent<Transform>();

        normalPOS = son_Transform.position;
        targetPOS = son_Transform.position + new Vector3(0, 0.15f, 0);
        StartCoroutine("UpAndDown");
    }
	
	private IEnumerator UpAndDown()
    {
        while (true)
        {
            StopCoroutine("Down");
            yield return new WaitForSeconds(2f);
            StartCoroutine("Up");
            yield return new WaitForSeconds(2f);
            StopCoroutine("Up");
            StartCoroutine("Down");
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator Up()
    {
        while (true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, targetPOS, Time.deltaTime * 25);
            yield return null;
        }
    }

    private IEnumerator Down()
    {
        while (true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, normalPOS, Time.deltaTime * 25);
            yield return null;
        }
    }
}
