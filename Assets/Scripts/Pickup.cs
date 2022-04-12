using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public float pos;
    private bool isOnOrigin;
    private float origPos;
    // Start is called before the first frame update
    void Start()
    {
        origPos = this.transform.position.y;
        InvokeRepeating("StartAnimating", 0.25f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void StartAnimating()
    {
        try
        {
            StartCoroutine("Animate");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //Animation: up down
    IEnumerator Animate()
    {
        if (isOnOrigin)
        {
            LeanTween.moveY(this.gameObject, origPos, 1f).setEaseOutSine();
            StartCoroutine(Wait());
            isOnOrigin = false;
        }
        else
        {
            LeanTween.moveY(this.gameObject, origPos + pos, 1f).setEaseOutSine();
            StartCoroutine(Wait());
            isOnOrigin = true;
        }
        if (isOnOrigin)
            yield return new WaitForSecondsRealtime(0.01f);

        yield return new WaitForSecondsRealtime(0.01f);

        yield return new WaitForSecondsRealtime(0.01f);
        yield return null;

    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.01f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.coins++;
            Destroy(this.gameObject);
        }
    }
}
