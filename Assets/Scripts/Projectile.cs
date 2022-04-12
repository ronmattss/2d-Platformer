using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] float duration;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Launch();
    }
    public void Launch()
    {
        float direction = this.gameObject.transform.localScale.x;
        rb = this.gameObject.GetComponent<Rigidbody2D>();

        if (direction > 0)
        {
            rb.AddForce(Vector2.right * speed);
            rb.velocity = Vector2.right * speed;
        }
        else if (direction < 0)
        {
            rb.AddForce(Vector2.left * speed);
            rb.velocity = Vector2.left * speed;
            this.transform.localScale = new Vector2(direction, 1);
        }
        Destroy(this.gameObject, duration);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.instance.coins++;
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
