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
    public TileBase topLeftBodyTile;

    [Header("Head Tiles")]
    public TileBase bottomHeadTile;
    public TileBase rightHeadTile;
    public TileBase leftHeadTile;
    public TileBase upHeadTile;

    [Header("Bottom Tiles")]
    public TileBase bottomBodyTile;
    public TileBase bottomBodyTongueTile;

    [Header("Setings")]
    public int delayTime;
    public int playerStepsTrigger;
    public Transform targetTrs;
    public LayerMask stoppingLayers;
    public Pathfinding2D snakePath;

    private LevelManager levelManager;
    private Transform snakeTransformPos;
    private Vector3 snakeHeadPos;

    private int nSteps = 0;
    private Grid2D pathGrid;

    [Tooltip("Position just after the snake in the gird, counting from bottom left")]
    public Vector2Int startPos;
    public Vector2Int targetPos;

    private SpriteRenderer spriteRenderer;
    private Vector3 oldDir;
    private Vector3Int oldTonguePos = default;


    //Check codemonkey a star code algorithm
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        snakeTransformPos = gameObject.transform;
        snakeHeadPos = snakeTransformPos.position;

        pathGrid = snakePath.GridOwner.GetComponent<Grid2D>();
        if (!pathGrid.IsGridReady)
            pathGrid.OnGridReady += InitSnake;
        else
        {
            InitSnake();
        }

        levelManager = LevelManager.Get();
        levelManager.OnPlayerMove += MoveSnake;
    }

    public void InitSnake()
    {
        snakePath.FindPath(startPos, targetPos);
        pathGrid.OnGridReady -= InitSnake;
    }
    public void MoveSnake()
    {

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
            Collider2D colliderFound = CheckForObstacle(newSnakeHeadPos);
            playerFound = false;
            if (colliderFound != null)
            {
                playerFound = IsPlayer(colliderFound);

                if (!playerFound && colliderFound.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    // A wall was hit
                    //levelManager.OnPlayerMove -= MoveSnake;
                    return;

                }


            }


        }
        else
        {
            Debug.Log($" nSteps is {nSteps} "); //and {pathGrid.path.Count}");
            return;
        }
        nSteps++;

        snakeHeadPos = newSnakeHeadPos;
        if (playerFound)
        {
            GameManager.GameOver();
        }

        snakeTransformPos.position = snakeHeadPos;
        SetTileBasedOnMovementDirection(currentSnakePos);
        //snakeTileMap.SetTile(currentSnakePos, headTile);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Snake/Crawl");
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
        else if (headDir == Vector3Int.left)
        {
            headTile = leftHeadTile;
            tongueTile = leftTongueTile;
            bodyTile = horizontalBodyTile;

            if (hasBodyInPrevSquares)
            {
                if (bodyDir == Vector3Int.up)
                {
                    bodyTile = topLeftBodyTile;
                }
                else if (bodyDir == Vector3Int.down)
                {
                    bodyTile = leftBottomBodyTile;
                }
                else if (bodyDir == Vector3Int.right)
                {
                    bodyTile = horizontalBodyTile;
                }
            }
        }
        else if (headDir == Vector3Int.up)
        {
            headTile = upHeadTile;
            tongueTile = upTongueTile;
            bodyTile = verticalBodyTile;

            if (hasBodyInPrevSquares)
            {
                if (bodyDir == Vector3Int.left)
                {
                    bodyTile = topLeftBodyTile;
                }
                else if (bodyDir == Vector3Int.down)
                {
                    bodyTile = verticalBodyTile;
                }
                else if (bodyDir == Vector3Int.right)
                {
                    bodyTile = topRightBodyTile;
                }
            }

        }


        var newTonguePos = newSnakePos + headDir;
        snakeTileMap.SetTile(newSnakePos, headTile);
        snakeTileMap.SetTile(newTonguePos, tongueTile);
        snakeTileMap.SetTile(oldSnakePos, bodyTile);

        if (hasBodyInPrevSquares)
        {

            if (oldTonguePos != newSnakePos)
            {
                snakeTileMap.SetTile(oldTonguePos, null);
            }
        }

        oldTonguePos = newSnakePos + headDir;
    }


    private Collider2D CheckForObstacle(Vector3 worldPosition)
    {
        var collider = Physics2D.OverlapCircle(worldPosition, 0.1f, stoppingLayers);

        return collider;
    }

    private bool IsPlayer(Collider2D collider)
    {
        return collider.CompareTag("Player");
    }

    private void OnDestroy()
    {
        if (levelManager != null)
            levelManager.OnPlayerMove -= MoveSnake;
    }
}






