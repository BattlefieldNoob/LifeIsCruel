﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GunManager : MonoBehaviour {


	Transform gun;

	Transform hitler;

	[SerializeField]
	GameObject CaneFronte;

    AudioSource audioSource;

    public AudioClip gunShot;
    public GameObject chooseCarefullyText; 

	[SerializeField]
	Image fadePanel;

	[SerializeField]
	Text finalText;

	private bool Shooted = false;

	// Use this for initialization
	void Start () {
		gun = transform.Find ("gun");
		hitler = transform.Find ("hitler");
        audioSource = GameObject.Find("AGUN").GetComponent<AudioSource>();
        chooseCarefullyText.SetActive(true); 
	}
	
	// Update is called once per frame
	void Update () {
		var mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		var distancefromHitler = Vector3.Distance (mouseWorldPosition, hitler.position);
		var distancefromCane = Vector3.Distance (mouseWorldPosition, CaneFronte.transform.position);


		if (distancefromCane < distancefromHitler) {
			gun.position = Vector3.Lerp (gun.position, CaneFronte.transform.position, 5 * Time.deltaTime);
		} else {
			gun.position = Vector3.Lerp (gun.position, hitler.position, 5 * Time.deltaTime);
		}


		if (!Shooted && Input.GetMouseButtonDown (0))
		{
			Shooted = true;
            audioSource.clip = gunShot; 
            audioSource.Play(); 
			if (distancefromCane < distancefromHitler) {
				Debug.Log ("Clicked on cane");
				FadeAndShowFinalMessage (true);
			} else{
				Debug.Log ("Clicked on Hitler");
				FadeAndShowFinalMessage (false);
			}

		}
	}
	
    [Space]
    [TextArea]
	public string shotAtDoggoMessage="YOU KILLED DOGGO!";
    [TextArea]
    public string shotAtHitlerMessage="YOU KILLED HITLER!";


	private void FadeAndShowFinalMessage(bool shotAtDoggo){
        chooseCarefullyText.SetActive(false); 
        finalText.text = shotAtDoggo ? shotAtDoggoMessage : shotAtHitlerMessage;
		StartCoroutine (FadeAndText ());
	}


	IEnumerator FadeAndText(){
		while (fadePanel.color.a < 0.99f) {
			fadePanel.color = Color.Lerp (fadePanel.color, Color.black, Time.deltaTime * 2);
			yield return new WaitForEndOfFrame ();
		}

		while (finalText.color.a < 0.99f) {
			finalText.color = Color.Lerp (finalText.color, Color.white, Time.deltaTime * 2);
			yield return new WaitForEndOfFrame ();
		}

		StartCoroutine(WaitAndReset());
	}

	IEnumerator WaitAndReset()
	{
		//User can read the message
		yield return new WaitForSeconds(5f);
		
		
		while (finalText.color.a > 0.01f) {
			finalText.color = Color.Lerp (finalText.color, Color.clear, Time.deltaTime * 2);
			yield return new WaitForEndOfFrame ();
		}
		
		//Suspance
		yield return new WaitForSeconds(2f);
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
