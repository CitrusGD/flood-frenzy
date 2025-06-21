using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Slime : MonoBehaviour
{
    //Consider moving the collision detection for player into here so the Jumping On Head thing does not f#@k up.
    Rigidbody2D rtRigidbody;
    CapsuleCollider2D rtCollider;
    gameManager rtManager;

    public bool onEdge;
    public bool isRight;
    public float jumpStrength = 250f;
    public float runSpeed = 0.25f;
    public int triggersOnPlatform = 0;

    bool swappedDir;

    void Start()
    {
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
        rtRigidbody = this.GetComponent<Rigidbody2D>();
        rtCollider = this.GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        if (rtManager.state != gameManager.GameStates.Start && rtManager.state != gameManager.GameStates.Paused)
        {
            if (onEdge && !swappedDir)
            {
                isRight = !isRight;
                swappedDir = true;
            }
            else if (!onEdge)
            {
                swappedDir = false;
            }

            if (isRight)
            {
                rtRigidbody.velocity = new Vector2(runSpeed * 3f, rtRigidbody.velocity.y);
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (!isRight)
            {
                rtRigidbody.velocity = new Vector2(runSpeed * -3f, rtRigidbody.velocity.y);
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform") || collider.CompareTag("One Way"))
        {
            triggersOnPlatform++;
            onEdge = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform") || collider.CompareTag("One Way"))
        {
            triggersOnPlatform--;

            if (triggersOnPlatform < 2)
            {
                onEdge = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactNormal = collision.contacts[0].normal;

        if (contactNormal == Vector2.left || contactNormal == Vector2.right)
        {
           isRight = !isRight;
        }

        if (contactNormal == Vector2.down && collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Hazard"))
        {
            Destroy(gameObject);
        }
    }
}
