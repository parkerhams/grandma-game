using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager _dialogueManager;

    //singleton behavior
    public static DialogueManager Instance
    {
        get
        {
            if (_dialogueManager == null)
            {
                // Search for existing instance.
                _dialogueManager = (DialogueManager)FindObjectOfType(typeof(DialogueManager));
 
                // Create new instance if one doesn't already exist.
                if (_dialogueManager == null)
                {
                    // Need to create a new GameObject to attach the singleton to.
                    GameObject singletonObject = new GameObject();
                    _dialogueManager = singletonObject.AddComponent<DialogueManager>();
                    singletonObject.name = typeof(DialogueManager).ToString() + " (Singleton)";
 
                    // Make instance persistent.
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _dialogueManager;
        }
 
        set{}
    }

    public static Dialogue _dialogue;

    [SerializeField]
    public TextMeshProUGUI grandmaSpeechBubble;

    [SerializeField]
    private Queue<string> grandmaSentences; //first in first out system, then loads new sentence from end of queue
    [SerializeField]
    private List<string> alreadySpokenSentences;

    public delegate void OnTVPowerHasPower();
	public event OnTVPowerHasPower OnTVPowerOn;

    private void Start()
    {
        grandmaSentences = new Queue<string>();
        alreadySpokenSentences = new List<string>();
        _dialogueManager = this;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        _dialogue = dialogue;
        Debug.Log("Grandma started her dialogue!");
        grandmaSentences.Clear();

        foreach(string sentence in dialogue.grandmaSentences)
        {
            //compare string sentence to anything that's already been spoken
            //if it's been spoken, don't display it 
            if(sentence == alreadySpokenSentences[alreadySpokenSentences.Count-1])
            {
                grandmaSentences.Enqueue(sentence);
            }
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(grandmaSentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = grandmaSentences.Dequeue();
        alreadySpokenSentences.Add(sentence);
        Debug.Log(sentence);

        StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        Debug.Log("End of conversation");
    }

    public IEnumerator TypeSentence (string sentence)
	{
		grandmaSpeechBubble.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			grandmaSpeechBubble.text += letter;
			yield return null;
		}
	}

}