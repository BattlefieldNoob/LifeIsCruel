using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    RectTransform WriteHereHint;
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

    string playerName = "Me";

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
    public AudioClip ending; 

    // Use this for initialization
    void Start()
    {

        RetriveUserName();

        GoToChatTime();
    }



    void RetriveUserName()
    {
        var username = Environment.UserName;
        if (!string.IsNullOrEmpty(username))
        {
            playerName = username;
            
            //aggiorno la distanza della scritta Write Here sulla base della lunghezza del nome utente

            //14 è la distanza di due spazi e il carattere ':', il moltiplicatore 8 è una media ed è stato ottenuto empiricamente
            var leftDistance = 14 + playerName.Length * 8;
            WriteHereHint.offsetMin=new Vector2(leftDistance,0);
        }
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
            GoToEnding(); 
        }
        else
        {
            GoToChatTime();
        }
    }

    void GoToEnding()
    {
        print("E' FINITO TUTTO");
        gunManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Computer.SetActive(false);
        audioSource.clip = ending;
        audioSource.Play(); 
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
                humanChatText.text = playerName+" : ";
                WriteHereHint.gameObject.SetActive(true);
            }
            if (Input.anyKeyDown)
            {
                if (humanPhraseIndex == 0)
                {
                    WriteHereHint.gameObject.SetActive(false);
                }
                //scrivo una cosa a caso 
                if (humanPhraseIndex < humanPhrases[choseHumanPhrase].Length)
                {
                    var character = ' ';
                    while (character.Equals(' ')) { //evito che la pressione di un tasto aggiunga solo uno spazio
                        character = humanPhrases[choseHumanPhrase][humanPhraseIndex];
                        humanChatText.text += character;
                        humanPhraseIndex++;
                    }

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