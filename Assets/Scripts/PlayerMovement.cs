using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private SnakeController snakeController;
    LevelManager levelManager;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private Transform movePoint;
    [SerializeField] [Tooltip("nome della traccia da inserire")] private String _eventName;
    [SerializeField] private Animator playerAnimator;
    private Vector2 lastMove;
    private bool isMoving = false;

    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;

    public bool detectSwipeAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;



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

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f && !blockMovement && Time.time - prevTime >= .5f)
        {
            CheckAndPerformMovement();
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);



        if ((transform.position - movePoint.position).magnitude == 0)
        {
            if (isMoving)
            {
                levelManager.UpdateStepsEvenet();
                isMoving = false;
            }
            playerAnimator.SetBool("Up", false);
            playerAnimator.SetBool("Down", false);
            playerAnimator.SetBool("Left", false);
            playerAnimator.SetBool("Right", false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnSwipeUp();
        }
    }


    public void PerformMovement(Vector2 movement, bool lastMovement = false)
    {
        prevTime = Time.time;
        lastMove = movement;
        Debug.Log("last move " + lastMove);
        movePoint.position = (Vector2)movePoint.position + movement;
        PlayerAnimation(movement);
        FMODUnity.RuntimeManager.PlayOneShot("event:/" + _eventName);
        if (!lastMovement)
        {
            isMoving = true;
        }

        prevTime = Time.time;
    }


    public bool CheckMovementPossible(Vector2 movement)
    {
        snakeController = FindObjectOfType<SnakeController>();

        if (snakeController)
        {
            var x = snakeController.snakeTileMap.WorldToCell((Vector2)movePoint.position + movement);
            var t = snakeController.snakeTileMap.GetTile(x);

            if (t != null)
                return false;
        }

        if (Physics2D.OverlapCircle((Vector2)movePoint.position + movement, .1f, nonWalkableTiles))
            return false;
        return true;

    }

    public void CheckAndPerformMovement()
    {

        RunSwipeDetection();

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            if (CheckMovementPossible(new Vector2(Input.GetAxisRaw("Horizontal"), 0)))
                PerformMovement(new Vector2(Input.GetAxis("Horizontal"), 0));
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            if (CheckMovementPossible(new Vector2(0, Input.GetAxisRaw("Vertical"))))
                PerformMovement(new Vector2(0, Input.GetAxis("Vertical")));
        }
    }

    public void CheckAndPerformLastMovement()
    {
        if (CheckMovementPossible(lastMove))
            PerformMovement(lastMove, true);
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

    private void RunSwipeDetection()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
            }

            //Detects Swipe while finger is still moving on screen
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeAfterRelease)
                {
                    fingerDownPos = touch.position;
                    DetectSwipe();
                }
            }

            //Detects swipe after finger is released from screen
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPos = touch.position;
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {

        if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
        {
            Debug.Log("Vertical Swipe Detected!");
            if (fingerDownPos.y - fingerUpPos.y > 0)
            {
                OnSwipeUp();
            }
            else if (fingerDownPos.y - fingerUpPos.y < 0)
            {
                OnSwipeDown();
            }
            fingerUpPos = fingerDownPos;

        }
        else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
        {
            Debug.Log("Horizontal Swipe Detected!");
            if (fingerDownPos.x - fingerUpPos.x > 0)
            {
                OnSwipeRight();
            }
            else if (fingerDownPos.x - fingerUpPos.x < 0)
            {
                OnSwipeLeft();
            }
            fingerUpPos = fingerDownPos;

        }
        else
        {
            Debug.Log("No Swipe Detected!");
        }
    }

    float VerticalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }

    float HorizontalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    void OnSwipeUp()
    {
        Vector2 move = new Vector2(0, 1);

        if (CheckMovementPossible(move))
        {
            PerformMovement(move);
        }
    }

    void OnSwipeDown()
    {
        Vector2 move = new Vector2(0, -1);

        if (CheckMovementPossible(move))
        {
            PerformMovement(move);
        }
    }

    void OnSwipeLeft()
    {
        Vector2 move = new Vector2(-1, 0);

        if (CheckMovementPossible(move))
        {
            PerformMovement(move);
        }
    }

    void OnSwipeRight()
    {
        Vector2 move = new Vector2(1, 0);

        if (CheckMovementPossible(move))
        {
            PerformMovement(move);
        }
    }
}