using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    LevelManager levelManager;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private Transform movePoint;
    [SerializeField] [Tooltip("nome della traccia da inserire")] private String _eventName;
    [SerializeField] private Animator playerAnimator;
    private Vector2 lastMove;


    [SerializeField] private LayerMask nonWalkableTiles;

    public bool blockMovement = false;
    private Vector2 prevDir;
    private float prevTime;

    // Start is called before the first frame update

    void Start()
    {
        levelManager = LevelManager.Get();
        movePoint.parent = null;
        lastMove = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

        if(Vector3.Distance(transform.position, movePoint.position) <= .05f && !blockMovement)
        {
            CheckAndPerformMovement();
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);



        if((transform.position - movePoint.position).magnitude == 0)
        {
            playerAnimator.SetBool("Up", false);
            playerAnimator.SetBool("Down", false);
            playerAnimator.SetBool("Left", false);
            playerAnimator.SetBool("Right", false);
        }

    }


    public void PerformMovement(Vector2 movement)
    {
        lastMove = movement;
        Debug.Log("last move " + lastMove);
        movePoint.position = (Vector2)movePoint.position + movement;
        PlayerAnimation(movement);
        FMODUnity.RuntimeManager.PlayOneShot("event:/" + _eventName);
        levelManager.UpdateStepsEvenet();
        prevTime = Time.time;
    }


    public bool CheckMovementPossible(Vector2 movement)
    {
        if(Physics2D.OverlapCircle((Vector2)movePoint.position + movement, .1f, nonWalkableTiles))
            return false;
        return true;

    }

    public void CheckAndPerformMovement()
    {
        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            if(CheckMovementPossible(new Vector2(Input.GetAxisRaw("Horizontal"), 0)))
                PerformMovement(new Vector2(Input.GetAxis("Horizontal"), 0));
        }
        else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            if(CheckMovementPossible(new Vector2(0, Input.GetAxisRaw("Vertical"))))
                PerformMovement(new Vector2(0, Input.GetAxis("Vertical")));
        }
    }

    private void PlayerAnimation(Vector2 direction)
    {
        if (prevDir != direction)
        {
            playerAnimator.SetBool("Up", false);
            playerAnimator.SetBool("Down", false);
            playerAnimator.SetBool("Left", false);
            playerAnimator.SetBool("Right", false);
        }

        prevDir = direction;


        if (direction == Vector2.up)
        {
            playerAnimator.SetBool("Up", true);
            Debug.Log("Walk Up");
        }
        if (direction == Vector2.down)
        {
            playerAnimator.SetBool("Down", true);
            Debug.Log("Walk Down");
        }
        if (direction == Vector2.left)
        {
            playerAnimator.SetBool("Left", true);
            Debug.Log("Walk Left");
        }
        if (direction == Vector2.right)
        {
            playerAnimator.SetBool("Right", true);
            Debug.Log("Walk Right");
        }
    }
}