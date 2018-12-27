using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScpecialItem : MonoBehaviour {

    public int value;
    public float LifeTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.instance.UpdatePointsFromSpecialItem(value);
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
        StartCoroutine(LifeTimeBeforeGo());
	}

    IEnumerator LifeTimeBeforeGo()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}
