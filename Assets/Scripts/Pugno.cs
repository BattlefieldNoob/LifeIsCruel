using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugno : MonoBehaviour
{

    public float maxScale;
    public float minScale;
    public float punchTime;
    float actualPunchTime; 
    public float waitTime;
    bool punching;
    public bool going; 
    public void Punch()
    {
        if (!punching)
            StartCoroutine(PunchCoroutine());
    }

    IEnumerator PunchCoroutine()
    {
        punching = true;
        float counter = 0;
        actualPunchTime = punchTime * Random.Range(0.75f, 1.5f); 
        float apex = actualPunchTime / 2;
        while (counter <= actualPunchTime)
        {
            float a = (maxScale - minScale) / apex * waitTime;
            if (counter <= apex)
            {
                going = true; 
                Gonfia(a);
            }
            else
            {
                going = false; 
                Sgonfia(a);
            }
            counter += waitTime;
            yield return new WaitForSeconds(waitTime);
        }
        transform.localScale = new Vector3(minScale, minScale, 0);
        punching = false;
    }

    void Gonfia(float amount)
    {
        transform.localScale += new Vector3(amount, amount, 0);
    }

    void Sgonfia(float amount)
    {
        transform.localScale -= new Vector3(amount, amount, 0);
    }
}
