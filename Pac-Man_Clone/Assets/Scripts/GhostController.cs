using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour {

    public Transform GhostsRoom;
    public Transform FrontOfGhostsRoom;
    public float speed = 1.0f;
    public float rayScanRange = 5.0f;
    public Transform Player;

    enum GhostStatus
    {
        InRoom, ReadyToExit, ReadyToEnter, ScanForPlayer, Chasing, Patroling, Scared, Eaten
    }

    enum GhostDirection
    {
        Up,Right,Down,Left
    }

    private LayerMask Playermask;
    private LayerMask Wallsmask;

    private Animator animator;

    private GhostStatus ghostStatus;
    private GhostDirection ghostDirection;



    private List<string> AvailableDirections = new List<string>();
    
    // Use this for initialization
    void Start () {
        
        Playermask = LayerMask.GetMask("Player");
        Wallsmask = LayerMask.GetMask("Walls","GhostsDoor");

        ghostStatus = GhostStatus.InRoom;

        ChooseTheFirtDirection();

        animator = GetComponent<Animator>();
    }

    void ChooseTheFirtDirection()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0) ghostDirection = GhostDirection.Left;
        if (rand == 1) ghostDirection = GhostDirection.Right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (ghostStatus == GhostStatus.Scared)
            {
                animator.SetBool("movingUp", false);
                animator.SetBool("movingDown", false);
                animator.SetBool("movingRight", false);
                animator.SetBool("movingLeft", false);
                animator.SetBool("isScared", false);
                animator.SetBool("isEaten", true);
                ghostStatus = GhostStatus.Eaten;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.instance.IsCompleted()) return;

        if (GameManager.instance.IsLost())
        {
            ghostStatus = GhostStatus.InRoom;
            ChooseTheFirtDirection();
            return;
        }

        if (GameManager.instance.IsStrong())
        {
            if(ghostStatus == GhostStatus.Patroling || ghostStatus == GhostStatus.Chasing || ghostStatus == GhostStatus.ScanForPlayer)
                ghostStatus = GhostStatus.Scared;
        }
        switch (ghostStatus)
        {
            case GhostStatus.InRoom:
            case GhostStatus.ReadyToExit:
                GetOutTheRoom();
                break;
            case GhostStatus.ReadyToEnter:
                break;
            case GhostStatus.ScanForPlayer:
                //make a decision Patroling or Chasing
                ScanForThePlayer();
                break;
            case GhostStatus.Chasing:
                LetChase();
                break;
            case GhostStatus.Patroling:
                Patrol();
                break;
            case GhostStatus.Scared:
                Scared();
                break;
            case GhostStatus.Eaten:
                StartCoroutine(BackToTheRoom());
                break;
        }
    }

    IEnumerator BackToTheRoom()
    {
        transform.position = Vector3.Lerp(transform.position, GhostsRoom.position, speed * Time.deltaTime);
        yield return null;
        ghostStatus = GhostStatus.InRoom;
    }

    private void LetChase()
    {
        RaycastHit2D hit;
        if (ghostStatus != GhostStatus.Chasing) return;
        switch (ghostDirection)
        {
            case GhostDirection.Up:
                hit = Physics2D.Raycast(transform.position, new Vector2(0.0f,1.0f), rayScanRange, Playermask);
                if (hit.collider == null)
                {
                    ghostStatus = GhostStatus.Patroling;
                }else
                    transform.Translate(0.0f * speed * Time.deltaTime, 1.0f * speed * Time.deltaTime, 0);
                break;
            case GhostDirection.Right:
                hit = Physics2D.Raycast(transform.position, new Vector2(1.0f, 0.0f), rayScanRange, Playermask);
                if (hit.collider == null)
                {
                    ghostStatus = GhostStatus.Patroling;
                }else
                    transform.Translate(1.0f * speed * Time.deltaTime, 0.0f * speed * Time.deltaTime, 0);

                break;
            case GhostDirection.Down:
                hit = Physics2D.Raycast(transform.position, new Vector2(0.0f, -1.0f), rayScanRange, Playermask);
                if (hit.collider == null)
                {
                    ghostStatus = GhostStatus.Patroling;
                }else
                    transform.Translate(0.0f * speed * Time.deltaTime, -1.0f * speed * Time.deltaTime, 0);

                break;
            case GhostDirection.Left:
                hit = Physics2D.Raycast(transform.position, new Vector2(-1.0f, 0.0f), rayScanRange, Playermask);
                if (hit.collider == null)
                {
                    ghostStatus = GhostStatus.Patroling;
                }else
                    transform.Translate(-1.0f * speed * Time.deltaTime, 0.0f * speed * Time.deltaTime, 0);
                break;
        }
    }

    private void Patrol()
    {
        if (ghostStatus != GhostStatus.Patroling) return;
        Vector3 Diretion = ChooseDirection();
        SetGhstAnimation();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Diretion.x,Diretion.y), rayScanRange, Playermask);
        if (hit.collider != null)
        {
            ghostStatus = GhostStatus.Chasing;
        }

        transform.Translate(Diretion.x * speed * Time.deltaTime, Diretion.y * speed * Time.deltaTime, 0);
    }

    private void Scared()
    {
        if (!GameManager.instance.IsStrong())
        {
            ghostStatus = GhostStatus.Patroling;
            return;
        }
        Vector3 Diretion = ChooseDirection();
        SetGhstAnimation();
        transform.Translate(Diretion.x * speed * Time.deltaTime, Diretion.y * speed * Time.deltaTime, 0);
    }

    private void SetGhstAnimation()
    {
        if (ghostStatus == GhostStatus.Scared)
        {
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
            animator.SetBool("movingRight", false);
            animator.SetBool("movingLeft", false);
            animator.SetBool("isScared", true);
            animator.SetBool("isEaten", false);
            return;
        }

        switch (ghostDirection)
        {
            case GhostDirection.Up:
                animator.SetBool("isScared", false);
                animator.SetBool("movingUp", true);
                animator.SetBool("movingDown", false);
                animator.SetBool("movingRight", false);
                animator.SetBool("movingLeft", false);
                animator.SetBool("isEaten", false);
                break;
            case GhostDirection.Right:
                animator.SetBool("isScared", false);
                animator.SetBool("movingUp", false);
                animator.SetBool("movingDown", false);
                animator.SetBool("movingRight", true);
                animator.SetBool("movingLeft", false);
                animator.SetBool("isEaten", false);
                break;
            case GhostDirection.Down:
                animator.SetBool("isScared", false);
                animator.SetBool("movingUp", false);
                animator.SetBool("movingDown", true);
                animator.SetBool("movingRight", false);
                animator.SetBool("movingLeft", false);
                animator.SetBool("isEaten", false);
                break;
            case GhostDirection.Left:
                animator.SetBool("isScared", false);
                animator.SetBool("movingUp", false);
                animator.SetBool("movingDown", false);
                animator.SetBool("movingRight", false);
                animator.SetBool("movingLeft", true);
                animator.SetBool("isEaten", false);
                break;
        }
    }

    private Vector3 ChooseDirection()
    {
        Vector3 Direction = new Vector3();
        RaycastHit2D hitup;
        RaycastHit2D hitleft;
        RaycastHit2D hitdown;
        RaycastHit2D hitright;

        switch (ghostDirection)
        {
            case GhostDirection.Up:
                hitup = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, Wallsmask);
                if (hitup.collider == null)
                {
                    ghostDirection = GhostDirection.Up;
                    return new Vector3(0.0f, 1.0f);
                }
                break;
            case GhostDirection.Right:
                hitright = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, Wallsmask);
                if (hitright.collider == null)
                {
                    ghostDirection = GhostDirection.Right;
                    return new Vector3(1.0f, 0.0f);
                }
                break;
            case GhostDirection.Down:
                hitdown = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, Wallsmask);
                if (hitdown.collider == null)
                {
                    ghostDirection = GhostDirection.Down;
                    return new Vector3(0.0f, -1.0f);
                }
                break;
            case GhostDirection.Left:
                hitleft = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, Wallsmask);
                if (hitleft.collider == null)
                {
                    ghostDirection = GhostDirection.Left;
                    return new Vector3(-1.0f, 0.0f);
                }
                break;
        }


        AvailableDirections.TrimExcess();
        AvailableDirections.Clear();

        hitup = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, Wallsmask);
        if (hitup.collider == null)
            AvailableDirections.Add("Up");

        hitleft = Physics2D.Raycast(transform.position, Vector2.left, 1.0f, Wallsmask);
        if (hitleft.collider == null)
            AvailableDirections.Add("Left");

        hitdown = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, Wallsmask);
        if (hitdown.collider == null)
            AvailableDirections.Add("Down");

        hitright = Physics2D.Raycast(transform.position, Vector2.right, 1.0f, Wallsmask);
        if (hitright.collider == null)
            AvailableDirections.Add("Right");


        int SelectedDirection = Random.Range(0, AvailableDirections.Count);

        if (AvailableDirections[SelectedDirection] == "Up")
        {
            ghostDirection = GhostDirection.Up;
            Direction = new Vector3(0.0f, 1.0f);
        }
        if (AvailableDirections[SelectedDirection] == "Left")
        {
            ghostDirection = GhostDirection.Left;
            Direction = new Vector3(-1.0f, 0.0f);
        }
        if (AvailableDirections[SelectedDirection] == "Down")
        {
            ghostDirection = GhostDirection.Down;
            Direction = new Vector3(0.0f, -1.0f);
        }
        if (AvailableDirections[SelectedDirection] == "Right")
        {
            ghostDirection = GhostDirection.Right;
            Direction = new Vector3(1.0f, 0.0f);
        }
        return Direction;
    }

    private void ScanForThePlayer()
    {
        RaycastHit2D hitup = Physics2D.Raycast(transform.position, Vector2.up, rayScanRange, Playermask);
        if (hitup.collider != null)
        {
            if (hitup.collider.gameObject.tag == "Player")
            {
                ghostStatus = GhostStatus.Chasing;
                return;
            }
        }
        
        RaycastHit2D hitright = Physics2D.Raycast(transform.position, Vector2.right, rayScanRange, Playermask);
        if (hitup.collider != null)
        {
            if (hitright.collider.gameObject.tag == "Player")
            {
                ghostStatus = GhostStatus.Chasing;
                return;
            }
        }
        
        RaycastHit2D hitdown = Physics2D.Raycast(transform.position, Vector2.down, rayScanRange, Playermask);
        if (hitup.collider != null)
        {
            if (hitdown.collider.gameObject.tag == "Player")
            {
                ghostStatus = GhostStatus.Chasing;
                return;
            }
        }
        
        RaycastHit2D hitleft = Physics2D.Raycast(transform.position, Vector2.left, rayScanRange, Playermask);
        if (hitup.collider != null)
        {
            if (hitleft.collider.gameObject.tag == "Player")
            {
                ghostStatus = GhostStatus.Chasing;
                return;
            }
        }
        
        ghostStatus = GhostStatus.Patroling;
    }

    private void GetOutTheRoom()
    {
        if (ghostStatus == GhostStatus.InRoom)
        {
            transform.position = Vector3.Lerp(transform.position, GhostsRoom.position, speed * Time.deltaTime);
            if (transform.position == GhostsRoom.position) ghostStatus = GhostStatus.ReadyToExit;
        }
        if (ghostStatus == GhostStatus.ReadyToExit)
        {
            transform.position = Vector3.Lerp(transform.position, FrontOfGhostsRoom.position, speed * Time.deltaTime);
        }
        if (transform.position == FrontOfGhostsRoom.position)
        {
            ghostStatus = GhostStatus.ScanForPlayer;
        }
            
    }
}
