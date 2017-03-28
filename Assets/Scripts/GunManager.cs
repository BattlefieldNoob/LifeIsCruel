using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour {


	Transform gun;

	Transform hitler;

	[SerializeField]
	GameObject CaneFronte;

    AudioSource audioSource;

    public AudioClip gunShot; 


	[SerializeField]
	Image fadePanel;

	[SerializeField]
	Text finalText;

	// Use this for initialization
	void Start () {
		gun = transform.FindChild ("gun");
		hitler = transform.FindChild ("hitler");
        audioSource = GameObject.Find("AGUN").GetComponent<AudioSource>(); 
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


		if (Input.GetMouseButtonDown (0)) {
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
	}
}
