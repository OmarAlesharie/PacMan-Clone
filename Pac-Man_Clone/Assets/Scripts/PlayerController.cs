using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public float speed = 5f;
    public float rayCastRange = 0.1f;

    public Transform UpperRight;
    public Transform UpperLeft;
    public Transform LowerRight;
    public Transform LowerLeft;
    private Animator animator;
    private Rigidbody2D Thisrigidbody;
    private LayerMask mask;



    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        Thisrigidbody = GetComponent<Rigidbody2D>();
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
                    RaycastHit2D hit1 = Physics2D.Raycast(UpperRight.transform.position, Vector2.up, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(UpperLeft.transform.position, Vector2.up, rayCastRange, mask);

                    if (hit1.collider == null && hit2.collider == null)
                    {
                        ResetAllTriggers();
                        animator.SetTrigger("MoveUp");
                        Horizontal = 0.0f;
                        Vertical = 1.0f;
                    }  
                }
                break;
            case 2:
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    RaycastHit2D hit1 = Physics2D.Raycast(UpperRight.transform.position, Vector2.right, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(LowerRight.transform.position, Vector2.right, rayCastRange, mask);

                    if (hit1.collider == null && hit2.collider == null)
                    {
                        ResetAllTriggers();
                        animator.SetTrigger("MoveRight");
                        Horizontal = 1.0f;
                        Vertical = 0.0f;
                    }   
                }
                break;
            case 3:
                if (Input.GetKey(KeyCode.DownArrow))
                { 
                    RaycastHit2D hit1 = Physics2D.Raycast(LowerLeft.transform.position, Vector2.down, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(LowerRight.transform.position, Vector2.down, rayCastRange, mask);

                    if (hit1.collider == null && hit2.collider == null)
                    {
                        ResetAllTriggers();
                        animator.SetTrigger("MoveDown");
                        Horizontal = 0.0f;
                        Vertical = -1.0f;
                    }
                }
                break;
            case 4:
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    RaycastHit2D hit1 = Physics2D.Raycast(UpperLeft.transform.position, Vector2.left, rayCastRange, mask);
                    RaycastHit2D hit2 = Physics2D.Raycast(LowerLeft.transform.position, Vector2.left, rayCastRange, mask);

                    if (hit1.collider == null && hit2.collider == null)
                    {
                        ResetAllTriggers();
                        animator.SetTrigger("MoveLeft");
                        Horizontal = -1.0f;
                        Vertical = 0.0f;
                    } 
                }
                break;
            default:
                ResetAllTriggers();
                Horizontal = 0.0f;
                Vertical = 0.0f;
                break;
        }

        //animator.SetInteger("Horizontal", (int)Horizontal);
        //animator.SetInteger("Vertical", (int)Vertical);

        

        Vector2 position = new Vector2(Horizontal * speed * Time.deltaTime, Vertical * speed * Time.deltaTime);
        Thisrigidbody.MovePosition(Thisrigidbody.position + position);


    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("MoveUp");
        animator.ResetTrigger("MoveRight");
        animator.ResetTrigger("MoveLeft");
        animator.ResetTrigger("MoveDown");
    }
}
