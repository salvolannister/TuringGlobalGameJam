using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSwitch : MonoBehaviour
{
    private GameManager _gameManager;
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
    private bool IsTriggered = true;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Get();
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
    {
        int currentPlayerSteps = _gameManager.GetCurrentPlayerSteps();  

        if (currentPlayerSteps != 0 && (currentPlayerSteps + Offset) % (EveryNOfTurn) == 0 && state==State.Near)
        {
            Debug.Log("Activate2");
            ActivateObstacle();
            state = State.Active;
        }
        else if (currentPlayerSteps != 0 && ((currentPlayerSteps + Offset) + 1) % (EveryNOfTurn ) == 0 && state==State.NotActive) {
            Debug.Log("Near");
            NearActivateObstacle();
            state = State.Near;
        }
        else if ((currentPlayerSteps + Offset)% (EveryNOfTurn ) != 0 && state == State.Active)
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
   

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D");
        if(state == State.Active && IsTriggered && collision.CompareTag("Player"))
        {
            Debug.Log("Morte");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Spikes_Kill");
            GameManager.GameOver();
            IsTriggered= false;
        }
    }
}
