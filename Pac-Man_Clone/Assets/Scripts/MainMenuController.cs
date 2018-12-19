using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MainMenuController : MonoBehaviour {

    public GameObject Player;
    public GameObject Red;
    public GameObject Pink;
    public GameObject LightBlue;
    public GameObject LightYellow;

    public float speed = 0f;

    public Text HitTheSpaseButton;

    private Animator PlayerAnimator;
    private Animator RedAnimator;
    private Animator PinkAnimator;
    private Animator LightBlueAnimator;
    private Animator LightYellowAnimator;

    private byte movingDirection;   //0 left, 1 right


    // Use this for initialization
    void Start () {
        PlayerAnimator = Player.GetComponent<Animator>();
        RedAnimator = Red.GetComponent<Animator>();
        PinkAnimator = Pink.GetComponent<Animator>();
        LightBlueAnimator = LightBlue.GetComponent<Animator>();
        LightYellowAnimator = LightYellow.GetComponent<Animator>();

        StartCoroutine(MainScreen());

        movingDirection = 0;
    }
	

    IEnumerator MainScreen()
    {
        while (true)
        {
            HitTheSpaseButton.enabled = !HitTheSpaseButton.enabled;
            yield return new WaitForSeconds(.5f);
        }
    }

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StopCoroutine(MainScreen());
            SceneManager.LoadScene(1);
        }


        if (movingDirection == 0)
        {
            PlayerAnimator.SetTrigger("MoveLeft");
            RedAnimator.SetInteger("Horizontal", -1);
            PinkAnimator.SetInteger("Horizontal", -1);
            LightBlueAnimator.SetInteger("Horizontal", -1);
            LightYellowAnimator.SetInteger("Horizontal", -1);

            Player.transform.Translate(-speed * Time.deltaTime, 0, 0);
            Red.transform.Translate(-speed * Time.deltaTime, 0, 0);
            Pink.transform.Translate(-speed * Time.deltaTime, 0, 0);
            LightBlue.transform.Translate(-speed * Time.deltaTime, 0, 0);
            LightYellow.transform.Translate(-speed * Time.deltaTime, 0, 0);

            if (Player.transform.position.x < -15)
            {
                movingDirection = 1;
            }
        }

        if (movingDirection == 1)
        {
            PlayerAnimator.SetTrigger("MoveRight");
            RedAnimator.SetBool("isScared", true);
            PinkAnimator.SetBool("isScared", true);
            LightBlueAnimator.SetBool("isScared", true);
            LightYellowAnimator.SetBool("isScared", true);

            Player.transform.Translate(speed * Time.deltaTime, 0, 0);
            Red.transform.Translate(speed * Time.deltaTime, 0, 0);
            Pink.transform.Translate(speed * Time.deltaTime, 0, 0);
            LightBlue.transform.Translate(speed * Time.deltaTime, 0, 0);
            LightYellow.transform.Translate(speed * Time.deltaTime, 0, 0);

            if (Player.transform.position.x > 12)
            {
                RedAnimator.SetBool("isScared", false);
                PinkAnimator.SetBool("isScared", false);
                LightBlueAnimator.SetBool("isScared", false);
                LightYellowAnimator.SetBool("isScared", false);
                movingDirection = 0;
            }
        }
    }
}
