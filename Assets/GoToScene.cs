using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToScene : MonoBehaviour
{
    [SerializeField] private string SceneName = "Stage4";
    [SerializeField] private Button trigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneName);
        }
    }

    private void Awake()
    {
        if(trigger != null) trigger.onClick.AddListener(() => SceneManager.LoadScene(SceneName));   
    }

}
