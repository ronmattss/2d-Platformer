using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class EnemyMovement : MonoBehaviour

{

    public float speed;                 // the speed of the enemy
    Rigidbody2D toPushBackRigidbody;   // rigidbody reference
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask playerMask;
    public float distance;
    private bool movingRight = true;
    private int flipper = 1;
    public Transform groundDetection;
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void Start()
    {

    }
    void OnDrawGizmos()
    {   // Debugging purposes
        //  Debug.DrawRay(groundDetection.position, Vector2.down, Color.blue);
        //  Debug.DrawLine(this.transform.position, new Vector2(this.transform.position.x + 1 * (this.transform.position.x + (1 * (flipper))), this.transform.position.y), Color.red);
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
    }
    private void FixedUpdate()

    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 1, playerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Player")
            {
                Debug.Log("Player is Hit");
                if (GameManager.instance.canBeDamaged)
                {
                    Knockback(colliders[i].gameObject);
                    GameManager.instance.PlayerDamage();
                }
            }

        }



    }

    // Knock the player back 
    public void Knockback(GameObject player)
    {
        Transform playerTransform = this.GetComponent<Transform>(); // Transform reference Contains position, rotation, and scale properties of the object
        if (playerTransform.position.x > player.transform.position.x)   // if player is on left of the enemy
        {
            player.GetComponent<Rigidbody2D>().AddForce((Vector2.left * 50), ForceMode2D.Impulse); // push the player to the left
        }
        else
            player.GetComponent<Rigidbody2D>().AddForce((Vector2.right * 50), ForceMode2D.Impulse); // push the player to the right
        // (250) needs to be substituted to a variable, als the 100



    }

}

