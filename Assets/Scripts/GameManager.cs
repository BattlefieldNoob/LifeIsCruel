using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum State { DEFAULT, PUNCHTIME, CHATTIME };

    #region serialized_fields
    [SerializeField]
    State gameState = State.DEFAULT;
    [SerializeField]
    Image gameStateImage;
    [SerializeField]
    GameObject zonaChat;
    [SerializeField]
    GameObject canefronte;
    [SerializeField]
    GameObject canelato;
    [SerializeField]
    Sprite punchTimeSprite;
    [SerializeField]
    Sprite chatTimeSprite;
    [SerializeField]
    float punchTimeCounter;
    [SerializeField]
    Text doggoChatText;
    [SerializeField]
    Text humanChatText;
    [SerializeField]
    float doggoWriteDelay = 0.5f;

	[SerializeField]
	GunManager gunManager;

	[SerializeField]
	GameObject Computer;
    #endregion

    #region conversation
    bool HumanTalked = false;
    bool DoggoTalked = false;
    bool DoggoWriting = false;

    string[] doggoWords = new string[] { "I love you Gerard", "You are so childish" };

    string[] humanPhrases = new string[] { "LOL u a dog", "Do the dab!" };

    string[] doggoNames = new string[] { "Jack" };

    int humanPhraseIndex = 0;

    int choseHumanPhrase = -1;
    #endregion

    bool doingStuff = false;
    int globalCounter = 0;
    public int iterationNumber = 0;

    public GameObject coltello1;
    public GameObject coltello2; 

    [Header("AUDIO")]
    public AudioSource audioSource;
    public AudioClip chatTime;
    public AudioClip punchTime;

    // Use this for initialization
    void Start()
    {
        GoToChatTime();
    }

    void GoToChatTime()
    {
        ResetPunchers();
        StartCoroutine(ChangePhaseShowingWrite(chatTimeSprite, State.CHATTIME));
        audioSource.clip = chatTime;
        audioSource.Play();
    }

    void GoToPunchTime()
    {
        globalCounter++;
        ResetChat();
        StartCoroutine(ChangePhaseShowingWrite(punchTimeSprite, State.PUNCHTIME));
        audioSource.clip = punchTime;
        audioSource.Play();
    }

    IEnumerator Punching()
    {
        if (globalCounter == 2)
        {
            coltello1.SetActive(true);
            coltello2.SetActive(true);
        }

        canefronte.GetComponent<Puncher>().StartPunching();
        yield return new WaitForSeconds(5);
        canefronte.GetComponent<Puncher>().StopPunching();
        yield return new WaitForSeconds(1);
        if (globalCounter >= iterationNumber)
        {
			print ("E' FINITO TUTTO");
			gunManager.gameObject.SetActive (true);
			gameObject.SetActive (false);
			Computer.SetActive (false);
        }
        else
        {
            GoToChatTime();
        }
    }

    IEnumerator DoggoWrite()
    {
        DoggoWriting = true;
        var charIndex = 0;
        doggoChatText.text = doggoNames[Random.Range(0, doggoNames.Length)] + " : ";
        string text = doggoWords[globalCounter];
        while (charIndex < text.Length)
        {
            doggoChatText.text += text[charIndex];
            charIndex++;
            yield return new WaitForSeconds(doggoWriteDelay);
        }
        DoggoTalked = true;
        DoggoWriting = false;
        StartCoroutine(HumanWrite());
        yield return null;
    }

    IEnumerator HumanWrite()
    {
        bool humanEndedWriting = false;
        while (!humanEndedWriting)
        {
            if (choseHumanPhrase == -1)
            {
                choseHumanPhrase = globalCounter;
                humanChatText.text = "Me : ";
            }
            if (Input.anyKeyDown)
            {
                //scrivo una cosa a caso 
                if (humanPhraseIndex < humanPhrases[choseHumanPhrase].Length)
                {
                    humanChatText.text += humanPhrases[choseHumanPhrase][humanPhraseIndex];
                    humanPhraseIndex++;
                }
                else
                {
                    humanEndedWriting = true;
                    yield return new WaitForSeconds(.5f);
                    GoToPunchTime();
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator ChangePhaseShowingWrite(Sprite toSprite, State stateToGo)
    {
        doingStuff = true;

        var startPos = new Vector3(0, 0, 0);
        var scaleTime = 1.4f;
        var actualScaleTime = 0f;
        var maxScale = 1.8f;
        var minScale = 0;
        var waitTime = 0.01f;

        gameStateImage.enabled = true;
        gameStateImage.rectTransform.localPosition = startPos;
        gameStateImage.sprite = toSprite;
        float counter = 0;
        actualScaleTime = scaleTime;
        float apex = actualScaleTime / 2;
        float a = (maxScale - minScale) / apex * waitTime;
        while (counter <= actualScaleTime)
        {
            if (counter <= apex)
            {
                gameStateImage.rectTransform.localScale += new Vector3(a, a, 0);
            }
            else
            {
                gameStateImage.rectTransform.localScale -= new Vector3(a, a, 0);
            }
            counter += waitTime;
            yield return new WaitForSeconds(waitTime);
        }
        transform.localScale = new Vector3(minScale, minScale, 0);
        gameStateImage.enabled = false;

        gameState = stateToGo;
        EndendPhaseChange(stateToGo);
        doingStuff = false;
    }

    void EndendPhaseChange(State s)
    {
        if (s == State.CHATTIME)
        {
            canelato.SetActive(true);
            HumanTalked = false;
            DoggoTalked = false;
            zonaChat.SetActive(true);
            StartCoroutine(DoggoWrite());
        }

        else if (s == State.PUNCHTIME)
        {
            canefronte.SetActive(true);
            StartCoroutine(Punching());
        }
    }

    void ResetChat()
    {
        choseHumanPhrase = -1;
        humanPhraseIndex = 0;
        zonaChat.SetActive(false);
        canelato.SetActive(false);
        doggoChatText.text = "";
        humanChatText.text = "";
    }

    void ResetPunchers()
    {
        canefronte.SetActive(false);
    }
}