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
    [Header("Left tiles")]
    public TileBase leftBackTile;
    public TileBase leftBodyTile;
    public TileBase leftBodyTongueTile;
    public TileBase leftBottomBodyCurve;

    [Header("Tongue Tiles")]
    public TileBase rightTongueTile;
    public TileBase bottomTongueTile;
    public TileBase leftTongueTile;
    public TileBase upTongueTile;

    [Header("Body Tiles")]
    public TileBase horizontalBodyTile;
    public TileBase verticalBodyTile;
    public TileBase leftBottomBodyTile;
    public TileBase topRightBodyTile;
    public TileBase bottomRightBodyTile;

    [Header("Head Tiles")]
    public TileBase bottomHeadTile;
    public TileBase rightHeadTile;
    public TileBase rightBackTile;

    [Header("Bottom Tiles")]
    public TileBase bottomBodyTile;
    public TileBase bottomBodyTongueTile;

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

    private SpriteRenderer spriteRenderer;
    private Vector3 oldDir;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        var currentSnakePos = snakeTileMap.WorldToCell(snakeHeadPos);
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
            Debug.Log($" nSteps is {nSteps} "); //and {pathGrid.path.Count}");
            return;
        }
        nSteps++;

        snakeHeadPos = newSnakeHeadPos ;
        if (playerFound)
        {
            GameManager.GameOver();
        }

        snakeTransformPos.position = snakeHeadPos;
        SetTileBasedOnMovementDirection(currentSnakePos);
        //snakeTileMap.SetTile(currentSnakePos, headTile);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Footsteps");
        Debug.Log($"New position is {newSnakeHeadPos}, old pos is {currentSnakePos}");

    }

    private void SetTileBasedOnMovementDirection(Vector3Int oldSnakePos)
    {
        //Check if left 
        TileBase headTile = standardBodyTile;
        TileBase tongueTile = standardBodyTile;
        TileBase bodyTile = standardBodyTile;

        var newSnakePos = snakeTileMap.WorldToCell(snakeHeadPos);

        int tubero = nSteps - 3;
        Vector3 preOldSnakeWorldPose;
        Vector3Int tuberoInt = default;
        Vector3Int bodyDir = default;
        bool hasBodyInPrevSquares = tubero >= 0;

        if (hasBodyInPrevSquares)
        {
            preOldSnakeWorldPose = pathGrid.path[tubero].worldPosition;
            tuberoInt = snakeTileMap.WorldToCell(preOldSnakeWorldPose);
            bodyDir = tuberoInt - oldSnakePos;//oldSnakePos - tuberoInt;
        }

        var headDir = newSnakePos - oldSnakePos;

        if (headDir == Vector3Int.right)
        {
            headTile = rightHeadTile;
            tongueTile = rightTongueTile;
            bodyTile = horizontalBodyTile;

            if (hasBodyInPrevSquares)
            {
                if (bodyDir == Vector3Int.left)
                {
                    bodyTile = horizontalBodyTile;

                }
                else if (bodyDir == Vector3Int.up)
                {
                    bodyTile = topRightBodyTile;

                }
                else if (bodyDir == Vector3Int.down)
                {
                    bodyTile = bottomRightBodyTile;
                }
            }
        }
        else if (headDir == Vector3Int.down)
        {
            headTile = bottomHeadTile;
            tongueTile = bottomTongueTile;
            bodyTile = verticalBodyTile;

            if (hasBodyInPrevSquares)
            {
                if (bodyDir == Vector3Int.left)
                {
                    bodyTile = leftBottomBodyTile;
                }
                else if (bodyDir == Vector3Int.up)
                {
                    bodyTile = verticalBodyTile;
                }
                else if (bodyDir == Vector3Int.right)
                {
                    bodyTile = bottomRightBodyTile;
                }
            }
        }

        snakeTileMap.SetTile(newSnakePos, headTile);
        snakeTileMap.SetTile(newSnakePos + headDir, tongueTile);
        snakeTileMap.SetTile(oldSnakePos, bodyTile);
    }

    private bool IsGoingDown(float newPose, float oldPose)
    {
        return newPose < oldPose;
    }
    private bool CheckForPlayer(Vector3 worldPosition)
    {
        var collider = Physics2D.OverlapCircle(worldPosition, 0.1f, playerLayer);

        return collider && collider.CompareTag("Player");
    }

}






