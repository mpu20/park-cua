using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private string Stage  = "Stage4";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void OpenStage()
    {
        SceneManager.LoadScene(Stage);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }
}
