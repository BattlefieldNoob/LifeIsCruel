using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour {

    public GameObject ready;
    public GameObject punch;
    public GameObject parata;
    public Sprite puchFace;
    public Sprite parataFace;
    public AudioClip bark;  

    SpriteRenderer sp; 
    AudioSource aus; 

	// Update is called once per frame
	void Update () {
        if (sp == null)
        {
            sp = GetComponent<SpriteRenderer>(); 
        }

        if(aus == null)
        {
            aus = GetComponent<AudioSource>(); 
        }

        if (Input.GetMouseButtonDown(0))
            Punch();
        if (Input.GetMouseButtonDown(1))
            Parata(); 
	}

    public void Punch()
    {
        punch.SetActive(true);
        parata.SetActive(false);
        ready.SetActive(false);
        sp.sprite = puchFace;
        aus.PlayOneShot(bark); 
    }

    public void Parata()
    {
        punch.SetActive(false);
        parata.SetActive(true);
        ready.SetActive(false);
        sp.sprite = parataFace;
    }

    public void Ready()
    {
        punch.SetActive(false);
        parata.SetActive(false);
        ready.SetActive(true);
    }
}
