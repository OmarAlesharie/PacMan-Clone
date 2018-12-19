using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollected : MonoBehaviour {

    public int ItemValue;
    public bool SetGhostScared;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.UpdatePoints(ItemValue);
            Destroy(this.gameObject);
        }
    }
}
