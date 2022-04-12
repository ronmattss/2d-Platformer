using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Start is called before the first frame update

       // If a Collider is Present on a GameObject and isTrigger is checked
       // then this script will check the OnCollisionEnter2D if another gameObject is collided with the gameobject this script is attached to

     void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")       // Checks if the "other" is tagged as a Player
        {
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);      // disable the Player
            UIManager.instance.endLevelScreen.SetActive(true); // enable the UI
        }
        else
        {
            Destroy(other.gameObject);          // if other than the player collides with this object, destroy it.
        }
    }
}
