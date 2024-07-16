using UnityEngine;

public class BackgroundFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float posX = 0f;
    [SerializeField] private float posY = 0f;
    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(target.position.x + posX, target.position.y + posY), speed * Time.deltaTime);
    }
}
