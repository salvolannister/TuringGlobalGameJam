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

        var snakePos = Vector3Int.RoundToInt(snakeHeadPos);
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


        snakeTileMap.SetTile(snakePos, bodyTile);
        snakeHeadPos = newSnakeHeadPos - offset;
        if (playerFound)
        {
            GameManager.GameOver();
        }
        snakeTransformPos.position = snakeHeadPos;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Footsteps");
        Debug.Log($"New position is {newSnakeHeadPos}, old pos is {snakePos}");

    }


    private bool CheckForPlayer(Vector3 worldPosition)
    {
        var collider = Physics2D.OverlapCircle(worldPosition, 0.1f, playerLayer);

        return collider && collider.CompareTag("Player");
    }
}






