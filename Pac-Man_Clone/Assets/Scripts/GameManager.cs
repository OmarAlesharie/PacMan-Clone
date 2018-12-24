using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Text ScoreText;
    public Text Info;

    private int PlayerLifes = 3;
    public bool PlayerisStrong;
    private bool level_completed;
    private bool playerLost;
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

    void Start()
    {
        PlayerisStrong = false;
        level_completed = false;
        playerLost = false;

        CollectablesAmount = GameObject.FindGameObjectsWithTag("Collectable").Length;
        Info.text = "";
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

    public bool IsLost()
    {
        return playerLost;
    }

    public void PlayerLost()
    {
        PlayerLifes--;
        playerLost = true;
        if (PlayerLifes <= 0)
        {
            Info.text = "Game Over!!";
        }
    }

    public bool IsStrong()
    {
        return PlayerisStrong;
    }

    public void PlayerBecomeStrong()
    {
        PlayerisStrong = true;
        StartCoroutine(PowerModePeriod());
    }

    IEnumerator PowerModePeriod()
    {
        yield return new WaitForSeconds(10.0f);
        PlayerisStrong = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
