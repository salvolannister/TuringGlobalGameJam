using System.Collections;
using System.Collections.Generic;
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

    public LayerMask walkableLayers;

    private LevelManager levelManager;
   
    private void Awake()
    {

    }
    //Check codemonkey a star code algorithm
    void Start()
    {
        var snakePos = Vector3Int.RoundToInt(gameObject.transform.position);
        var newSnakeHeadPos = snakePos;
        var snakeBodyPos = snakePos;
        newSnakeHeadPos.y += 1;
        if (IsTileWalkable(snakeBodyPos))
        {
            snakeTileMap.SetTile(snakeBodyPos, bodyTile);
            snakeTileMap.SetTile(newSnakeHeadPos, headTile);
            Debug.Log("Player moved");
        }
        Debug.Log($"New position is {snakePos}, walkable tile pos is {newSnakeHeadPos}");

    }

    public void MoveSnake()
    {

    }
    void Update()
    {


    }

    public bool IsTileWalkable(Vector3Int tilePos)
    {
        if (Physics2D.OverlapCircle(new Vector2(tilePos.x, tilePos.y), 0.1f, walkableLayers))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
