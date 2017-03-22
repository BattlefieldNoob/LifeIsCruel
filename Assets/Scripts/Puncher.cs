using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puncher : MonoBehaviour
{

    public GameObject ready;
    public GameObject punch;
    public GameObject parata;
    public Sprite puchFace;
    public Sprite parataFace;
    public AudioClip bark;

    public GameObject pugno_dx;
    public GameObject pugno_sx;

    bool b;

    SpriteRenderer sp;
    AudioSource aus;

    // Update is called once per frame
    void Update()
    {
        if (sp == null)
        {
            sp = GetComponent<SpriteRenderer>();
        }

        if (aus == null)
        {
            aus = GetComponent<AudioSource>();
        }

        if (Input.GetMouseButtonDown(0))
            StartCoroutine(ContinuousPunch()); 
    }

    public void Punch()
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
                pd.Punch();
            }
        }
        else
        {
            if (!pugno_dx.GetComponent<Pugno>().going)
            {
                Pugno ps = pugno_sx.GetComponent<Pugno>();
                ps.Punch();
            }
        }

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
