using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneTransitionCollider : MonoBehaviour
{
    [SerializeField] string scene;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController playerMovement)) { SceneManager.LoadScene(scene); }
    }
}
