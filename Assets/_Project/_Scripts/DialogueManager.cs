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

    [SerializeField]
    public Canvas grandmaCanvas;

    [SerializeField]
    public TextMeshProUGUI grandmaSpeechBubble;

    [SerializeField]
    private List<string> alreadySpokenSentences = new List<string>();

    [SerializeField]
    [Header("Dialogue Strings for Grandma")]
    public string gameStartDialogue = "Could you plug the TV in for me, sweetie?";
    public string tvHasBeenPluggedIn = "Oh, thank you! Now can you handle the rest of this stuff, too?";
    public string bananaInVCRDialogue = "Yo fool that's a banana";
    public string vcrHasBeenPluggedIn = "You got it working! Come and put one of these tapes in, and we can watch it.";
    public string tvHasBeenTurnedOff = "Oh, why'd you turn it off?";
    public string tvIsOnChannel1 = "Ooh, this reminds me of that movie you watched as a kid. You know, the one with the penguins!";
    public string tvIsOnChannel2 = "I love this show! Isn't it just the cutest thing?";
    public string tvIsOnStaticChannel = "Oh, no! What made the picture go away?";
    public string bandClipIsPlaying = "Oh my, such a lovely sound! I was so proud of you that night, and I still am!";
    public string santaClipIsPlaying = "Fuck that Santa is terrifying";
    public string santaClipIsPlayingAtEnd = "You know, your grandfather was so excited to play Santa that day. He wanted to see how you'd react!";
    public string bandClipIsPlayingAtEnd = "We always loved hearing you play so passionately. You stole the stage at all your performances!";
    public string vcrVideoCablePluggedIntoAudioSlot = "Oh, my! This sounds like those radio broadcasts they played during the war.";
    public string vcrAudioCablePluggedIntoVideoSlot = "I've never understood why the snow happens. And with that sound!";
    public string vcrAndTVHaveVideoButNotAudio = "Where's the noise? Or is everyone just not talking?";
    public string vcrAndTVHaveAudioButNotVideo = "I can hear it! But where's the picture?";
    public string bananaIsInTV = "My, how exciting! It's like those scary movies your dad liked to watch.";
    public string playerPutsPhoto1IntoVCR = "Look, it's that picture you drew when you were little!";
    public string playerPutsPhoto2IntoVCR = "Aw, I love that drawing of us.";
    public string playerPutsPhoto3IntoVCR = "I love this photo! It was so nice of that man to take it for us.";
    public string grandmaPromptsPlayersToPutStuffIntoVCR = "I wonder what would happen if you put something else in the slot...";

    private void Start()
    {
        _dialogueManager = this;
        Bark(gameStartDialogue);
    }

    public void Bark(string dialogue)
    {
         grandmaCanvas.GetComponent<CanvasGroup>().alpha = 1f;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogue));
    }


    IEnumerator TypeSentence (string sentence)
	{
		grandmaSpeechBubble.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			grandmaSpeechBubble.text += letter;
			yield return new WaitForSeconds(.1f);
		}

        yield return new WaitForSeconds(5);

        grandmaCanvas.GetComponent<CanvasGroup>().alpha = 0f;
	} 

}