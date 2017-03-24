using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {


	Transform gun;

	Transform hitler;

	[SerializeField]
	GameObject CaneFronte;

    AudioSource audioSource;

    public AudioClip gunShot; 

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
			} else{
				Debug.Log ("Clicked on Hitler");
			}

		}
	}
}
