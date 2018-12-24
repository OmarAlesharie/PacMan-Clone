using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float speed = 5f;
    public float rayCastRange = 0.1f;
    public GameObject player;

    public Transform UpperRight;
    public Transform UpperLeft;
    public Transform LowerRight;
    public Transform LowerLeft;

    private Animator animator;
    private LayerMask mask;
    
    // Use this for initialization
    void Start () {
        
        animator = GetComponentInChildren<Animator>();
        mask = LayerMask.GetMask("Walls", "GhostsDoor");
    }
	
	// Update is called once per frame
	void Update () {


        float Horizontal = 0.0f;
        float Vertical = 0.0f;

        int lastInput = 0;

        //up = 1, right = 2, down = 3, left = 4

        if (Input.GetKey(KeyCode.UpArrow))
        {
            lastInput = 1;   
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            lastInput = 2;  
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            lastInput = 3;  
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            lastInput = 4;
        }

        switch (lastInput)
        {
            case 1:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.5f, mask);
                    RaycastHit2D hit1 = Physics2D.Raycast(UpperRight.transform.position, Vector2.up, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(UpperLeft.transform.position, Vector2.up, rayCastRange, mask);

                    if (hit.collider == null && hit1.collider == null && hit2.collider == null)
                    {
                        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                        animator.SetBool("isMoving", true);
                        Horizontal = 0.0f;
                        Vertical = 1.0f;
                    }  
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.5f, mask);
                    RaycastHit2D hit1 = Physics2D.Raycast(UpperRight.transform.position, Vector2.right, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(LowerRight.transform.position, Vector2.right, rayCastRange, mask);

                    if (hit.collider == null && hit1.collider == null && hit2.collider == null)
                    {
                        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        animator.SetBool("isMoving",true);
                        Horizontal = 1.0f;
                        Vertical = 0.0f;
                    }   
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, mask);
                    RaycastHit2D hit1 = Physics2D.Raycast(LowerLeft.transform.position, Vector2.down, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(LowerRight.transform.position, Vector2.down, rayCastRange, mask);

                    if (hit.collider == null && hit1.collider == null && hit2.collider == null)
                    {
                        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                        animator.SetBool("isMoving", true);
                        Horizontal = 0.0f;
                        Vertical = -1.0f;
                    }
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 0.5f, mask);
                    RaycastHit2D hit1 = Physics2D.Raycast(UpperLeft.transform.position, Vector2.left, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(LowerLeft.transform.position, Vector2.left, rayCastRange, mask);
                    

                    if (hit.collider == null && hit1.collider == null && hit2.collider == null)
                    {
                        player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                        animator.SetBool("isMoving", true);
                        Horizontal = -1.0f;
                        Vertical = 0.0f;
                    } 
                }
                break;
            default:
                animator.SetBool("isMoving", false);
                Horizontal = 0.0f;
                Vertical = 0.0f;
                break;
        }

        transform.Translate(Horizontal * speed * Time.deltaTime, Vertical * speed * Time.deltaTime, 0);
    }
}
