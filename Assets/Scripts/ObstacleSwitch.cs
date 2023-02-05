using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSwitch : MonoBehaviour
{
    LevelManager levelManager;
    SpriteRenderer spriteRenderer;
    [SerializeField] private int Offset = 1;
    [SerializeField] private int EveryNOfTurn=2;
    [SerializeField] [Layer] int wall;
    [SerializeField] [Layer] int defaultLayer;
    [SerializeField] private Sprite noSpike;
    [SerializeField] private Sprite nearSpike;
    [SerializeField] private Sprite spike;
    [SerializeField] private bool hasAudio = true;
    public enum State {Active, Near, NotActive};
    public State state;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = LevelManager.Get();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (Offset == 0)
        {
            state = State.Active;
            ActivateObstacle();
        }
        else if (Offset == EveryNOfTurn - 1)
        {
            state = State.Near;
            NearActivateObstacle();
        }
        else
        {
            state = State.NotActive;
            DeactivateObstacle();
        }
    }

    // Update is called once per frame
    void Update()
    {/*
        if (levelManager.RetrieveCurrentSteps() == 0 && state==State.Active)
        {
            ActivateObstacle();
            Debug.Log("Activate1");
            state = State.NotActive;
        }
        else if(levelManager.RetrieveCurrentSteps() == 0 && state == State.NotActive)
        {
            Debug.Log("Deactivate1");
            DeactivateObstacle();
            state = State.NotActive;
        }*/
        if (levelManager.RetrieveCurrentSteps() != 0 && (levelManager.RetrieveCurrentSteps() + Offset) % (EveryNOfTurn) == 0 && state==State.Near)
        {
            Debug.Log("Activate2");
            ActivateObstacle();
            state = State.Active;
        }
        else if (levelManager.RetrieveCurrentSteps() != 0 && ((levelManager.RetrieveCurrentSteps() + Offset) + 1) % (EveryNOfTurn ) == 0 && state==State.NotActive) {
            Debug.Log("Near");
            NearActivateObstacle();
            state = State.Near;
        }
        else if ((levelManager.RetrieveCurrentSteps() + Offset)% (EveryNOfTurn ) != 0 && state == State.Active)
        {
            Debug.Log("Deactivate2");
            DeactivateObstacle();
            state = State.NotActive;
        }
    }

    public void ActivateObstacle() {
        gameObject.layer = wall;
        spriteRenderer.sprite = spike;
        if(hasAudio)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Spikes_Up");
        }
        
    }

    public void NearActivateObstacle()
    {
        spriteRenderer.sprite = nearSpike;
    }

    public void DeactivateObstacle() {
        gameObject.layer = defaultLayer;
        spriteRenderer.sprite = noSpike;
    }
}
