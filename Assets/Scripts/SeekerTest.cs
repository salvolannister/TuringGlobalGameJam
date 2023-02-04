using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerTest : MonoBehaviour
{
    // Start is called before the first frame update
    private Pathfinding2D seekerPath;
    [Tooltip("Position just after the snake in the gird, counting from bottom left")]
    public Vector2Int startPos;
    public Vector2Int targetPos;
    void Start()
    {
        
        seekerPath = GetComponent<Pathfinding2D>();
        seekerPath.FindPath(startPos, targetPos);
        var Grid = seekerPath.GridOwner.GetComponent<Grid2D>();
        var path = Grid.path;

        foreach (Node2D node in path)
        {
            seekerPath.seeker.position = node.worldPosition;
            Debug.Log($" start pos " + node.worldPosition);
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }
}
