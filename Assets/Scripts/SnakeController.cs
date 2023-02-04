using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SnakeController : MonoBehaviour
{
    [Header("Tiles"), Space(2)]
    public Tilemap snakeTileMap;
    public Tilemap walkableTileMap;
    public TileBase standardBodyTile;
    public TileBase headTile;
    public TileBase leftBodyTile;
    public TileBase rightBodyTile;

    [Header("Setings")]
    public int delayTime;
    public int playerStepsTrigger;
    public Transform targetTrs;
    public LayerMask playerLayer;

    private LevelManager levelManager;
    private Transform snakeTransformPos;
    private Vector3 snakeHeadPos;
    private Pathfinding2D snakePath;

    private int nSteps = 0;
    private Grid2D pathGrid;
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


        pathGrid = snakePath.GridOwner.GetComponent<Grid2D>();
        if (!pathGrid.IsGridReady)
            pathGrid.OnGridReady += InitSnake;
        else
        {
            InitSnake();
        }
    }

    public void InitSnake()
    {
        snakePath.FindPath(startPos, targetPos);
        pathGrid.OnGridReady -= InitSnake;
        Invoke(nameof(MoveSnake), 1);
    }
    public void MoveSnake()
    {
        if (!AskForStart)
            return;

        var oldSnakePos = Vector3Int.RoundToInt(snakeHeadPos);
        var newSnakeHeadPos = Vector3.one;
        bool playerFound;
        if (pathGrid == null)
        {
            return;
        }

        if (pathGrid.path != null && nSteps < pathGrid.path.Count)
        {

            newSnakeHeadPos = pathGrid.path[nSteps].worldPosition;
            playerFound = CheckForPlayer(newSnakeHeadPos);
            if (!playerFound)
            {
                Invoke(nameof(MoveSnake), 1);
            }

        }
        else
        {
            Debug.Log($" nSteps is {nSteps} and {pathGrid.path.Count}");
            return;
        }
        nSteps++;

        snakeHeadPos = newSnakeHeadPos - offset;
        if (playerFound)
        {
            GameManager.GameOver();
        }
        snakeTransformPos.position = snakeHeadPos;
        TileBase bodyTile = GetTileBasedOnMovementDirection(oldSnakePos);
        snakeTileMap.SetTile(oldSnakePos, bodyTile);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Footsteps");
        Debug.Log($"New position is {newSnakeHeadPos}, old pos is {oldSnakePos}");

    }

    private TileBase GetTileBasedOnMovementDirection(Vector3Int oldSnakePos)
    {
        //Check if left 
        var newSnakePos = Vector3Int.RoundToInt(snakeHeadPos);
        if (newSnakePos.x > oldSnakePos.x)
        {
            // moving right
           
        }
        else if (newSnakePos.x < oldSnakePos.x)
        {
            // moving left
        }
        else
        {
            // moving up or down
        }

        return null;
        //Check if right
    }

    private bool CheckForPlayer(Vector3 worldPosition)
    {
        var collider = Physics2D.OverlapCircle(worldPosition, 0.1f, playerLayer);

        return collider && collider.CompareTag("Player");
    }
}






