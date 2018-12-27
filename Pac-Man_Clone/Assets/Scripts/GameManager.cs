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

    public AudioClip EatingCollactables_FX;
    public AudioClip EatingGhost_FX;
    public AudioClip EatingFruit_FX;
    public AudioClip PlayerDied_FX;

    public AudioSource FX_Source;
    public AudioSource FX_Source2;
    public AudioSource FX_Source_ExtraLife;


    public int baseGhostScore = 200;
    public int MaxGhostScore = 1600;

    public int LifePerPoints = 20000;

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

    public GameObject[] SpecialItems;

    private int PlayerLifes;
    private bool PlayerisStrong;
    private bool level_completed;
    private bool playerLost;
    private int Points;
    private int CollectablesAmount;
    private int initialBaseGhostScore;
    private int pointsCounterperlife;

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

    private void PutNewSpecialItem()
    {
        int selectedItem = Random.Range(0, SpecialItems.Length);
        Instantiate(SpecialItems[selectedItem],playerStartingPosition.position, playerStartingPosition.rotation);
    }

    public bool IsCompleted()
    {
        return level_completed;
    }

    void Start()
    {
        pointsCounterperlife = 0;
        Points = Save.GetPoints();
        PlayerLifes = Save.GetLifes();
        pointsCounterperlife = Save.GetScorePerLife();
        ScoreText.text = "Scores\n" + Points;
        LifesText.text = PlayerLifes.ToString();
        ResetGamePLayData();
        CollectablesAmount = GameObject.FindGameObjectsWithTag("Collectable").Length;
        initialBaseGhostScore = baseGhostScore;

        InvokeRepeating("PutNewSpecialItem", 20.0f, 20.0f);
    }

    private void CheckIfNewLife(int newPoints)
    {
        pointsCounterperlife += newPoints;
        if (pointsCounterperlife >= LifePerPoints)
        {
            PlayerLifes++;
            FX_Source_ExtraLife.Play();
            pointsCounterperlife = 0;
            LifesText.text = PlayerLifes.ToString();
        }
    }

    public void UpdatePoints(int points)
    {
        Points += points;
        CheckIfNewLife(points);

        FX_Source.clip = EatingCollactables_FX;
        if (!FX_Source.isPlaying)
        {
            FX_Source.Play();
        }

        ScoreText.text = "Scores\n" + Points;
        CollectablesAmount--;
        if (CollectablesAmount <= 0)
        {
            Info.text = "Level Completed!!";
            level_completed = true;
            StartCoroutine(NextLevel());
        }
    }

    public void UpdatePointsFromGhosts()
    {
        Points += baseGhostScore;
        FX_Source2.clip = EatingGhost_FX;
        if (!FX_Source2.isPlaying)
        {
            FX_Source2.Play();
        }
        CheckIfNewLife(baseGhostScore);
        ScoreText.text = "Scores\n" + Points;
        baseGhostScore *= 2;
        if (baseGhostScore > MaxGhostScore)
        {
            baseGhostScore = initialBaseGhostScore;
        }
    }

    public void UpdatePointsFromSpecialItem(int value)
    {
        Points += value;
        CheckIfNewLife(value);
        FX_Source.clip = EatingFruit_FX;
        FX_Source.Play();
        ScoreText.text = "Scores\n" + Points;
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3.0f);
        Save.SetPoints(Points);
        Save.SetLifes(PlayerLifes);
        Save.SetScorePerLife(pointsCounterperlife);
        SceneManager.LoadScene(1);
    }

    public bool IsLost()
    {
        return playerLost;
    }

    public void PlayerLost()
    {
        PlayerLifes--;
        FX_Source.Stop();
        FX_Source.clip = PlayerDied_FX;
        FX_Source.Play();
        LifesText.text = PlayerLifes.ToString();
        playerLost = true;
        if (PlayerLifes <= 0)
        {
            Info.text = "Game Over!!";
            StartCoroutine(LoadMainScreen());
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
