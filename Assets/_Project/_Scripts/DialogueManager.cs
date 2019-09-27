using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Queue<string> grandmaSentences; //first in first out system, then loads new sentence from end of queue
    private void Start()
    {
        grandmaSentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Grandma started her dialogue!");
    }

}