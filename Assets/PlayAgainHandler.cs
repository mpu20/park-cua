using UnityEngine;
using UnityEngine.UI;

public class PlayAgainHandler : MonoBehaviour
{
    [SerializeField] private Button playAgain;

    private void Awake()
    {
        playAgain.onClick.AddListener(() => GameManager.Instance.OpenStage());
    }
}
