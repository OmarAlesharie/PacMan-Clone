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

    public GamePlayData Save;

    public float speed = 0f;

    public Text HitTheSpaseButton;

    private Animator PlayerAnimator;
    private Animator RedAnimator;
    private Animator PinkAnimator;
    private Animator LightBlueAnimator;
    private Animator LightYellowAnimator;

    private SpriteRenderer PLayerSpriteRenderer;

    private byte movingDirection;   //0 left, 1 right


    // Use this for initialization
    void Start () {
        Save.SetLifes(3);
        Save.SetPoints(0);
        PlayerAnimator = Player.GetComponent<Animator>();
        RedAnimator = Red.GetComponent<Animator>();
        PinkAnimator = Pink.GetComponent<Animator>();
        LightBlueAnimator = LightBlue.GetComponent<Animator>();
        LightYellowAnimator = LightYellow.GetComponent<Animator>();

        PLayerSpriteRenderer = Player.GetComponent<SpriteRenderer>();

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
            Player.transform.localScale = new Vector3(1.0F, 1.0F, 0);

            PlayerAnimator.SetTrigger("isMoving");
            PLayerSpriteRenderer.flipX = true;

            RedAnimator.SetBool("movingLeft", true);
            RedAnimator.SetBool("isScared", false);

            PinkAnimator.SetBool("movingLeft", true);
            PinkAnimator.SetBool("isScared", false);

            LightBlueAnimator.SetBool("movingLeft", true);
            LightBlueAnimator.SetBool("isScared", false);

            LightYellowAnimator.SetBool("movingLeft", true);
            LightYellowAnimator.SetBool("isScared", false);

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
            Player.transform.localScale = new Vector3(2.0F, 2.0F, 0);
            PlayerAnimator.SetTrigger("isMoving");
            PLayerSpriteRenderer.flipX = false;

            RedAnimator.SetBool("movingLeft", false);
            RedAnimator.SetBool("isScared", true);

            PinkAnimator.SetBool("movingLeft", false);
            PinkAnimator.SetBool("isScared", true);

            LightBlueAnimator.SetBool("movingLeft", false);
            LightBlueAnimator.SetBool("isScared", true);

            LightYellowAnimator.SetBool("movingLeft", false);
            LightYellowAnimator.SetBool("isScared", true);

            Player.transform.Translate(speed * Time.deltaTime, 0, 0);
            Red.transform.Translate(speed * Time.deltaTime, 0, 0);
            Pink.transform.Translate(speed * Time.deltaTime, 0, 0);
            LightBlue.transform.Translate(speed * Time.deltaTime, 0, 0);
            LightYellow.transform.Translate(speed * Time.deltaTime, 0, 0);

            if (Player.transform.position.x > 12)
            {
                //RedAnimator.SetBool("isScared", false);
                //PinkAnimator.SetBool("isScared", false);
                //LightBlueAnimator.SetBool("isScared", false);
                //LightYellowAnimator.SetBool("isScared", false);
                movingDirection = 0;
            }
        }
    }
}
