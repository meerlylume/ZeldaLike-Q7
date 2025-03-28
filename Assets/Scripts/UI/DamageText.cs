using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TextMeshProUGUI text;
    void Start()
    {
        rb   = GetComponent<Rigidbody2D>();
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Push(Vector2 pushDir, float lifetime, float pushStrength, string damage)
    {
        if (text) text.text = damage;
        StartCoroutine(PushRoutine(pushDir, pushStrength, lifetime));
    }

    private IEnumerator PushRoutine(Vector2 pushDir, float pushStrength, float lifetime)
    {
        rb.linearVelocity = pushDir.normalized * pushStrength;

        yield return new WaitForSeconds(lifetime);

        Die();

        yield break;
    }

    private void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
