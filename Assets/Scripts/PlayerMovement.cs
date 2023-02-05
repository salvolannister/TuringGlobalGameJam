using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    LevelManager levelManager;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private Transform movePoint;
    [SerializeField][Tooltip("nome della traccia da inserire")] private String _eventName;
    public Vector2 lastMove;


    [SerializeField] private LayerMask nonWalkableTiles;

    public bool blockMovement = false;
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

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f && !blockMovement)
        {
            CheckAndPerformMovement();
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);

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


    private void PerformMovement(Vector2 movement)
    {
        lastMove = movement;
        Debug.Log(lastMove);
        movePoint.position = (Vector2)movePoint.position + movement;
        FMODUnity.RuntimeManager.PlayOneShot("event:/"+ _eventName);
        levelManager.UpdateStepsEvenet();

    }


    public bool CheckMovementPossible(Vector2 movement)
    {
        if (Physics2D.OverlapCircle((Vector2)movePoint.position + movement, .1f, nonWalkableTiles))
            return false;
        return true;

    }
}