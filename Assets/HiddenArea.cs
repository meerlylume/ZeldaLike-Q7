using UnityEngine;

public class HiddenArea : MonoBehaviour
{
    private Animator animator;

    private void Start() { animator = GetComponent<Animator>(); }

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.tag == "Player") animator.SetBool("isHidden", false); }

    private void OnTriggerExit2D(Collider2D collision)  { if (collision.tag == "Player") animator.SetBool("isHidden", true); }
}
