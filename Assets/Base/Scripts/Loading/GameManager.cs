using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // define the interface of GameManger
    static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameManager)FindObjectOfType(typeof(GameManager));
            }
            return instance;
        }
    }

    public void GoToNextScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
		if (++scene == SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("no next scene!!!");
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void GoToPreviousScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (--scene == -1)
        {
            Debug.Log("no previous scene!!!");
        }
        else
        {
            SceneManager.LoadScene(scene);
        }

    }
    public void RestartScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);

    }
    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("DestroyedObjectPersist");
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}