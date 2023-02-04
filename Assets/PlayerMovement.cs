using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private Transform movePoint;
    private Vector2 lastMove;


    [SerializeField] private LayerMask nonWalkableTiles;

    public bool blockMovement = false;
    // Start is called before the first frame update

    void Start()
    {
        movePoint.parent = null;
        lastMove = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f && !blockMovement)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) == 1f)
            {
                if (CheckMovementPossible(new Vector2(Input.GetAxisRaw("Horizontal"), 0)))
                    PerformMovement(new Vector2(Input.GetAxis("Horizontal"), 0));
            }
            else if (Mathf.Abs(Input.GetAxis("Vertical")) == 1f)
            {
                if (CheckMovementPossible(new Vector2(0, Input.GetAxisRaw("Vertical"))))
                    PerformMovement(new Vector2(0, Input.GetAxis("Vertical")));
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementSpeed * Time.deltaTime);
    }


    public void PerformMovement(Vector2 movement)
    {
        lastMove = movement;
        Debug.Log(lastMove);
        movePoint.position = (Vector2)movePoint.position + movement;
    }


    public bool CheckMovementPossible(Vector2 movement)
    {
        if (Physics2D.OverlapCircle((Vector2)movePoint.position + movement, .1f, nonWalkableTiles))
            return false;
        return true;

    }
}