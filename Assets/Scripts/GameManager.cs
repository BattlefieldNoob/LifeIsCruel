using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	enum State{PUNCHTIME,CHATTIME};

	[SerializeField]
	State gameState=State.CHATTIME;


	[SerializeField]
	Image gameStateImage;

	[SerializeField]
	GameObject zonaChat;

	[SerializeField]
	CaneFronte canefronte;

	[SerializeField]
	CaneLato canelato;


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
	float doggoWriteDelay=0.5f;

	// Use this for initialization
	void Start () {
		GoToChatTime();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == State.CHATTIME) {
			//aspetta che ci sia una conversazione
			Conversate();
		} else if (gameState == State.PUNCHTIME) {
			//aspetta finchè il punch time non sia finito
			punchTimeCounter-=Time.deltaTime;
			if (punchTimeCounter <= 0) {
				//vai alla fase chat time
				GoToChatTime();
			}
		}
		
	}

	private void GoToChatTime(){
		gameState = State.CHATTIME;
		canelato.gameObject.SetActive (true);
		canefronte.gameObject.SetActive (false);
	//	gameStateImage.sprite = chatTimeSprite;
		StartCoroutine (ChangePhaseSprite (chatTimeSprite));
		HumanTalked=false;
		DoggoTalked=false;
		//riprodurre suono fastidioso
	}

	private void GoToPunchTime(){
		gameState = State.PUNCHTIME;
		punchTimeCounter = Random.Range (2f, 4f); //la durata del punchTime è casuale 
		canelato.gameObject.SetActive (false);
		canefronte.gameObject.SetActive (true);
		StartCoroutine (ChangePhaseSprite (punchTimeSprite));
		//gameStateImage.sprite = punchTimeSprite;
		//riprodurre suono fastidioso
	}

	bool HumanTalked=false;
	bool DoggoTalked=false;
	bool DoggoWriting=false;

	string[] doggoWords=new string[]{"bork", "bark", "doge", "meow"};

	string[] humanPhrases=new string[]{"hello", "i'm faggot!", "dog piece of shit!", "please i have a family!"};

	int humanPhraseIndex=0;

	int choseHumanPhrase = -1;

	private void Conversate(){
		if (DoggoTalked) {
			//aspetto la risposta (inutile) del giocatore
			if (HumanTalked) {
				//aspetto un pò di tempo e poi passo al punch time
				choseHumanPhrase = -1;
				humanPhraseIndex = 0;
				GoToPunchTime ();
			} else {
				if (choseHumanPhrase == -1) {
					choseHumanPhrase = Random.Range (0, humanPhrases.Length - 1);
					humanChatText.text = "Me : ";
				}
				//aspetto che il giocatore scriva e prema invio
				if(Input.GetKeyDown(KeyCode.Return)){
					humanChatText.text=humanPhrases[choseHumanPhrase];
					HumanTalked = true;
				}else if (Input.anyKeyDown) {
					//scrivo una cosa a caso 
					humanChatText.text+=humanPhrases[choseHumanPhrase][humanPhraseIndex];
					if (humanPhraseIndex < humanPhrases [choseHumanPhrase].Length-1)
						humanPhraseIndex++;
					else
						HumanTalked = true;
				}

			}
		} else {
			//il doggo dice:
			if(!DoggoWriting)
				StartCoroutine(DoggoWrite(doggoWords[Random.Range(0,doggoWords.Length-1)]));
			DoggoTalked = true;
		}
	}

	string[] doggoNames=new string[]{"Martin", "Lerry", "Alvaro", "Arcibaldo","Coboldo","Michele Misseri"};


	IEnumerator DoggoWrite(string text){
		DoggoWriting = true;
		var charIndex = 0;
		doggoChatText.text = doggoNames[Random.Range(0,doggoNames.Length)]+" : ";
		while (charIndex < text.Length) {
			doggoChatText.text += text [charIndex];
			charIndex++;
			yield return new WaitForSeconds (doggoWriteDelay);
		}
		DoggoTalked = true;
		DoggoWriting = false;
		yield return null;
	}



	IEnumerator ChangePhaseSprite(Sprite toSprite){
		var startPos = new Vector3 (0, 0, 0);
		var scaleTime = 1.4f;
		var actualScaleTime = 0f;

		var maxScale = 3;
		var minScale = 0;

		var waitTime = 0.1f;
		gameStateImage.enabled = true;
		gameStateImage.rectTransform.localPosition=startPos;
		gameStateImage.sprite = toSprite;
		float counter = 0;
		actualScaleTime = scaleTime * Random.Range (0.75f, 1.5f); 
		float apex = actualScaleTime / 2;
		while (counter <= actualScaleTime) {
			float a = (maxScale - minScale) / apex * waitTime;
			if (counter <= apex) {
				gameStateImage.rectTransform.localScale += new Vector3 (a, a, 0);

			} else {
				gameStateImage.rectTransform.localScale -= new Vector3 (a, a, 0);
			}
			counter += waitTime;
			yield return new WaitForSeconds (waitTime);
		}
		transform.localScale = new Vector3 (minScale, minScale, 0);
		gameStateImage.enabled = false;
		if (zonaChat.activeSelf)
			zonaChat.SetActive (false);
		else
			zonaChat.SetActive (true);
	
	}
}


	