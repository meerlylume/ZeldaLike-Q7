using UnityEngine;

public class DummyRange : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;

    private void Start() { tutorial.SetActive(false); }

    private void OnTriggerEnter2D(Collider2D collision) { if (collision.tag == "Player") tutorial.SetActive(true); }

    private void OnTriggerExit2D(Collider2D collision)  { if (collision.tag == "Player") tutorial.SetActive(false); }
}
