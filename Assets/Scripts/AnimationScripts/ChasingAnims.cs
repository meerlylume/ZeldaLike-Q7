using UnityEngine;
using UnityEngine.Events;

public class ChasingAnims : MonoBehaviour
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
        //animator.SetBool("flipSprite", value); // This doesn't work at all for some reason, not even without 
        // a bool, so. um. hard code :/

        if (value) transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        else       transform.eulerAngles = new Vector3(transform.eulerAngles.x,   0, transform.eulerAngles.z);
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
