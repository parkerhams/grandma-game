using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueEvents : MonoBehaviour
{
    [Header("Dialogue Event For TV ON")]
    public UnityEvent GrandmaDialogueOnTVOn;

    public void TriggerGrandmaTVOnDialogue()
    {
        if(GrandmaDialogueOnTVOn != null)
        {
            GrandmaDialogueOnTVOn.Invoke();
            Debug.Log("GrandmaDialogueOnTVOn event should occur");
        }
    }
}
