using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public GameObject parent;

    private Animator animator;
    private SpriteRenderer spriteRenderer; 
    private Sprite originalSprite;
    private Collider2D collider;

    void Start()
    {
        animator = parent.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        collider = GetComponent<Collider2D>();

        collider.enabled = false;

        StartCoroutine(EnableCollider());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
        {
            animator.Play("BubbleDestroyingAnimation");
        }
    }

    public void DestroyBubble()
    {
        transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
        spriteRenderer.sprite = originalSprite;
        CrabController.itemActived = false;
        CrabController.activeItemIndex = 0;
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1.5f);

        collider.enabled = true;
    }
}
