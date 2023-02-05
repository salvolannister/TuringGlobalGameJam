using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToVictory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Spike_Trembling");
        SceneManager.LoadScene("Victory");
    }
}
