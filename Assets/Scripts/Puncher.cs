using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour
{

    public GameObject ready;
    public AudioClip bark;

    public GameObject pugno_dx;
    public GameObject pugno_sx;

    Coroutine c; 

    bool b;

    SpriteRenderer sp;
    public AudioSource aus;

    // Update is called once per frame
    void Update()
    {
        if (sp == null)
        {
            sp = GetComponent<SpriteRenderer>();
        }

        //if (Input.GetMouseButtonDown(0))
        //    StartCoroutine(ContinuousPunch()); 
    }

    public void StartPunching()
    {
        c = StartCoroutine(ContinuousPunch());
    }

    public void StopPunching()
    {
        StopCoroutine(c); 
    }

    void Punch()
    {
        b = !b; 
        ready.SetActive(false);
        pugno_sx.SetActive(true);
        pugno_dx.SetActive(true);
        if (b)
        {
            if (!pugno_sx.GetComponent<Pugno>().going)
            {
                Pugno pd = pugno_dx.GetComponent<Pugno>();
                if (!pd.punching)
                {
                    PlayBork(); 
                }
                pd.Punch();

            }
        }
        else
        {
            if (!pugno_dx.GetComponent<Pugno>().going)
            {
                Pugno ps = pugno_sx.GetComponent<Pugno>();
                if (!ps.punching)
                {
                    PlayBork(); 
                }
                ps.Punch();

            }
        }
    }

    void PlayBork()
    {
        //if (!aus.isPlaying)
        //{
        //    aus.clip = bark;
        //    aus.Play();
        //}
        aus.PlayOneShot(bark); 
    }

    IEnumerator ContinuousPunch()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            Punch();
        }
    }

    public void Ready()
    {
        ready.SetActive(true);
        pugno_dx.SetActive(false);
        pugno_sx.SetActive(false);
    }
}
