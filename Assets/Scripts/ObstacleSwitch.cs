using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSwitch : MonoBehaviour
{
    LevelManager levelManager;
    SpriteRenderer spriteRenderer;
    [SerializeField] private int EveryNOfTurn=2;
    [SerializeField] [Layer] int wall;
    [SerializeField] [Layer] int defaultLayer;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Get();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.RetrieveCurrentSteps() % (2 * EveryNOfTurn + 1) == 0)
        {
            ActivateObstacle();
        }
        else {
            DeactivateObstacle();
        }
    }

    public void ActivateObstacle() {
        gameObject.layer = wall;
        spriteRenderer.color = Color.red;
    }

    public void DeactivateObstacle() {
        gameObject.layer = defaultLayer;
        spriteRenderer.color = Color.white;
    }
}
