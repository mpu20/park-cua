using System.Collections;
using UnityEngine;

public class IceBox : MonoBehaviour
{
    private Vector2 initialPosition;
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private void Awake()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Falling());
        }
    }

    private IEnumerator Falling()
    {
        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        coll.enabled = false;
        yield return new WaitForSeconds(3f);
        transform.position = initialPosition;
        rb.bodyType = RigidbodyType2D.Static;
        coll.enabled = true;
    }
}
