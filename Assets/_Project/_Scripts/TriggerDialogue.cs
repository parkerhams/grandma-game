using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public Dialogue grandmaDialogue;

    public void TriggerGrandmaDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(grandmaDialogue);
    }

    //trigger a dialogue with Grandma to begin
            //DialogueManager.Instance.StartDialogue(GetComponent<TriggerDialogue>().grandmaDialogue);
}
