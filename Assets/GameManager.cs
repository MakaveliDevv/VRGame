using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject spawnPoint;
    public GameObject player;

    void Awake() 
    {
        if(instance != null && instance != this) 
        {
            Destroy(this);
        }
        else 
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start() 
    {
        string name = SceneManager.GetActiveScene().name;

        if(name == "BlankScene2") 
        {
            spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point");
            player = GameObject.FindGameObjectWithTag("Player");
            
            player.transform.position = spawnPoint.transform.position;
        }
    }
}
