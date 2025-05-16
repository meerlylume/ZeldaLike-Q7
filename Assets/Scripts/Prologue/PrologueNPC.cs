using System.Collections;
using UnityEngine;

public class PrologueNPC : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] private float loadDelay;
    private bool stopNextLine;

    protected override void Start()
    {
        Time.timeScale = 1f;
        branchDialogueData = rootDialogueData;

        parent = transform.parent.GetComponent<NPCParent>();

        dialoguePanel = parent.GetDialoguePanel();
        dialogueText = parent.GetDialogueText();
        nextArrow = parent.GetNextArrow();

        stopNextLine = false;
        StartCoroutine(SceneLoadDelay());
    }

    protected override void SwapPortraitSide()
    {
        return; //No portrait here
    }

    protected override void CheckPortraitPosition()
    {
        return; //No portrait here
    }

    public override void StartDialogue()
    {
        tagDetector = "";
        isDialogueActive = true;
        lineIndex = 0;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    public override void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        isTyping = false;
        stopNextLine = true;

        animator.SetTrigger("triggerFade");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse0))
            Interact();
    }

    IEnumerator SceneLoadDelay()
    {
        yield return new WaitForSecondsRealtime(loadDelay);
        Interact();
    }

    public override void Interact()
    {
        if (stopNextLine) return;
        base.Interact();
    }
}
