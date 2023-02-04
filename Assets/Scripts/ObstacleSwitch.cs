using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSwitch : MonoBehaviour
{
    LevelManager levelManager;
    SpriteRenderer spriteRenderer;
    [SerializeField] private bool StartActive=false;
    [SerializeField] private int Offset = 1;
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
        if (StartActive && levelManager.RetrieveCurrentSteps() == 0)
        {
            ActivateObstacle();
        }
        else {
            DeactivateObstacle();
        }
        if (levelManager.RetrieveCurrentSteps()!=0 && levelManager.RetrieveCurrentSteps() % (EveryNOfTurn + Offset) == 0)
        {
            ActivateObstacle();
        }
        else if(levelManager.RetrieveCurrentSteps() % (EveryNOfTurn + Offset) != 0)
        {
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
