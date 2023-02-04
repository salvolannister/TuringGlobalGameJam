using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SnakeController : MonoBehaviour
{
    public Tilemap snakeTileMap;
    public Tilemap walkableTileMap;
    public TileBase bodyTile;
    public TileBase headTile;

    public int delayTime;
    public int playerStepsTrigger;

    public LayerMask notWalkableLayer;

    private LevelManager levelManager;
    private Transform snakeTransformPos;
    private Vector3 snakeHeadPos;

    private void Awake()
    {

    }
    //Check codemonkey a star code algorithm
    void Start()
    {
        snakeTransformPos = gameObject.transform;
        snakeHeadPos = snakeTransformPos.position;

        Invoke(nameof(MoveSnake), 1);

    }

    public void MoveSnake()
    {
        var snakePos = Vector3Int.RoundToInt(snakeHeadPos);
        var newSnakeHeadPos = snakeHeadPos;

        newSnakeHeadPos.y += 1;
        if (IsTileWalkable(newSnakeHeadPos))
        {
            snakeTileMap.SetTile(snakePos, bodyTile);
            snakeHeadPos.y += 1;
            snakeTransformPos.position = snakeHeadPos;
            // snakeTileMap.SetTile(newSnakeHeadPos, headTile);
            Debug.Log("Player moved");
            Invoke(nameof(MoveSnake), 1);
        }
        else
        {

        }

        Debug.Log($"New position is {newSnakeHeadPos}, old pos is {snakePos}");
    }

    
    private void WalkStraight(bool up)
    {
        if (up)
            snakeHeadPos.y += 1;
        else
        {
            snakeHeadPos.y -= 1;
        }
       
    }

    private void WalkRight(bool right)
    {
        if (right)
        {
            snakeHeadPos.y += 1;
        }
        else
        {
            snakeHeadPos.x -= 1;
        }
    }

    void Update()
    {


    }

    public bool IsTileWalkable(Vector3 tilePos)
    {
        Vector2 areaToTest = new Vector2(tilePos.x, tilePos.y);
        bool isWalkable = !Physics2D.OverlapCircle(areaToTest, 0.1f, notWalkableLayer);
        // if doesn't find opponened 
        if (isWalkable)
        {
            Debug.Log($"Walkable tile is ${tilePos} ");
            return true;
        }
        else
        {
            Debug.Log("Not walkable!!!");
            return false;
        }
    }
}
