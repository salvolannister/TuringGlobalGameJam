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
    public Transform targetTrs;
    public LayerMask notWalkableLayer;

    private LevelManager levelManager;
    private Transform snakeTransformPos;
    private Vector3 snakeHeadPos;
    private Pathfinding2D snakePath;

    private int i = 0;
    private Grid2D localGrid;
    public bool AskForStart = false;

    [Tooltip("Position just after the snake in the gird, counting from bottom left")]
    public Vector2Int startPos;
    public Vector2Int targetPos;

    private Vector3 offset = new Vector3(0.1f, 0.1f, 0);
    private void Awake()
    {
        // Se nella scena è cambiato qualcosa
        // Deve scegliere la posizione migliore in cui spostarsi
        // che può essere dritto dx o sx

        // per fare questa scelta deve fare un path finding, da cui poi sceglierà
        // che posizione prendere.

        // cambiare posizione
        // reagire al cambio di posizione
        // Se non è cambiato nulla continua con il percorso scelto


        // quando non puoi spostarti aggiorna startPosition
    }
    //Check codemonkey a star code algorithm
    void Start()
    {
        snakeTransformPos = gameObject.transform;
        snakeHeadPos = snakeTransformPos.position;

        snakePath = GetComponent<Pathfinding2D>();


        localGrid = snakePath.GridOwner.GetComponent<Grid2D>();
        if (!localGrid.IsGridReady)
            localGrid.OnGridReady += InitSnake;
        else
        {
            InitSnake();
        }
    }

    public void InitSnake()
    {
        snakePath.FindPath(startPos, targetPos);
        localGrid.OnGridReady -= InitSnake;
        Invoke(nameof(MoveSnake), 1);
    }
    public void MoveSnake()
    {
        if (!AskForStart)
            return;

        var snakePos = Vector3Int.RoundToInt(snakeHeadPos);
        var newSnakeHeadPos = Vector3.one;
        if (localGrid == null)
        {
            return;
        }
      
        if (localGrid.path != null && i < localGrid.path.Count)
        {
            newSnakeHeadPos = localGrid.path[i].worldPosition;
            Invoke(nameof(MoveSnake), 1);
        }
        else
        {
            Debug.Log($" i is {i} and {localGrid.path.Count}");
            return;
        }
        i++;

        //if (IsTileWalkable(newSnakeHeadPos))
        //{
        snakeTileMap.SetTile(snakePos, bodyTile);
        snakeHeadPos = newSnakeHeadPos - offset;
        
        snakeTransformPos.position = snakeHeadPos;
        // snakeTileMap.SetTile(newSnakeHeadPos, headTile);
        Debug.Log("Player moved");

        //}
        //else
        //{

        //}

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
