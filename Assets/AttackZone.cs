using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameOver");
    }
}
