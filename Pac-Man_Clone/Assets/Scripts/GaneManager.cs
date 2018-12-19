using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Text ScoreText;
    public Text Info;

    private int PlayerLifes = 3;
    private int Points = 0;
    private int CollectablesAmount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void UpdatePoints(int points)
    {
        Points += points;
        ScoreText.text = "Scores\n" + Points;
        CollectablesAmount--;

        if (CollectablesAmount <= 0)
        {
            Info.text = "Level Completed!!";
        }
    }

    // Use this for initialization
    void Start()
    {
        CollectablesAmount = GameObject.FindGameObjectsWithTag("Collectable").Length;
        Info.text = "";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
