using UnityEngine;
using UnityEngine.Events;

public class Anims : MonoBehaviour
{
    public UnityEvent AttackFrameEvent;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool GetFlipSprite() { return animator.GetBool("flipSprite"); }

    public void SetFlipSprite(bool value)
    {
        //animator.SetBool("flip", value); // The animator's "mirror" parameter doesn't work at all for some reason,
                                               // not even without a bool, so. um. hard code :/
                                               // the bool stays here for Cannelle's attack though

        if (value)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            animator.SetBool("flip", true);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            animator.SetBool("flip", false);
        }
    }

    public void SetAttack()
    {
        animator.SetTrigger("attackTrigger");
    }

    public void SetHurt()
    {
        animator.SetTrigger("hurtTrigger");
    }

    public void AnimationAttackFrame()
    {
        AttackFrameEvent.Invoke();
    }
    public void StopAnims()
    {
        animator.StopPlayback();
    }
}
