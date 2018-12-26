using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Text ScoreText;
    public Text Info;
    public Text LifesText;
    public GamePlayData Save;

    public Transform playerStartingPosition;
    public Transform RedStartingPosition;
    public Transform PinkStartingPosition;
    public Transform lightYellowStartingPosition;
    public Transform lightBlueStartingPosition;

    public GameObject Player;
    public GameObject RedGhost;
    public GameObject PinkGhost;
    public GameObject LightYellowGhost;
    public GameObject LightBlueGhost;

    private int PlayerLifes;
    private bool PlayerisStrong;
    private bool level_completed;
    private bool playerLost;
    private int Points;
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
        //DontDestroyOnLoad(gameObject);
    }

    public bool IsCompleted()
    {
        return level_completed;
    }

    void Start()
    {
        Points = Save.GetPoints();
        PlayerLifes = Save.GetLifes();
        ScoreText.text = "Scores\n" + Points;
        LifesText.text = PlayerLifes.ToString();
        ResetGamePLayData();
        CollectablesAmount = GameObject.FindGameObjectsWithTag("Collectable").Length;
    }

    public void UpdatePoints(int points)
    {
        Points += points;
        ScoreText.text = "Scores\n" + Points;
        CollectablesAmount--;

        if (CollectablesAmount <= 0)
        {
            Info.text = "Level Completed!!";
            level_completed = true;
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3.0f);
        Save.SetPoints(Points);
        Save.SetLifes(PlayerLifes);
        SceneManager.LoadScene(1);
    }

    public bool IsLost()
    {
        return playerLost;
    }

    public void PlayerLost()
    {
        PlayerLifes--;
        LifesText.text = PlayerLifes.ToString();
        playerLost = true;
        if (PlayerLifes <= 0)
        {
            Info.text = "Game Over!!";
        }
        else
            StartCoroutine(ResumeTheGame());
    }

    IEnumerator LoadMainScreen()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }

    IEnumerator ResumeTheGame()
    {
        yield return new WaitForSeconds(3.0f);
        ResetGamePLayData();
    }

    private void ResetGamePLayData()
    {
        Player.transform.position = playerStartingPosition.position;
        RedGhost.transform.position = RedStartingPosition.position;
        PinkGhost.transform.position = PinkStartingPosition.position;
        LightBlueGhost.transform.position = lightBlueStartingPosition.position;
        LightYellowGhost.transform.position = lightYellowStartingPosition.position;
        PlayerisStrong = false;
        level_completed = false;
        playerLost = false;
        Info.text = "";

        Player.SetActive(false);
        Player.SetActive(true);
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
