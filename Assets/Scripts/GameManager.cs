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
		gameStateImage.sprite = chatTimeSprite;
		HumanTalked=false;
		DoggoTalked=false;
		//riprodurre suono fastidioso
	}

	private void GoToPunchTime(){
		gameState = State.PUNCHTIME;
		punchTimeCounter = Random.Range (2f, 4f); //la durata del punchTime è casuale 
		canelato.gameObject.SetActive (false);
		canefronte.gameObject.SetActive (true);
		gameStateImage.sprite = punchTimeSprite;
		//riprodurre suono fastidioso
	}

	bool HumanTalked=false;
	bool DoggoTalked=false;

	string[] doggoWords=new string[]{"bork", "bark", "doge", "meow"};

	string[] humanPhrases=new string[]{"hello", "i'm faggot!", "dog piece of shit!", "please i have a family!"};

	int humanPhraseIndex=0;

	int choseHumanPhrase = -1;

	private void Conversate(){
		if (DoggoTalked) {
			//aspetto la risposta (inutile) del giocatore
			if (HumanTalked) {
				//aspetto un pò di tempo e poi passo al punch time
				GoToPunchTime ();
			} else {
				if (choseHumanPhrase == -1) {
					choseHumanPhrase = Random.Range (0, humanPhrases.Length - 1);
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
			doggoChatText.text=doggoWords[Random.Range(0,doggoWords.Length-1)];
			DoggoTalked = true;
		}
	}
}
	