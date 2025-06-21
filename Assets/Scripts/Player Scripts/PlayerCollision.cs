using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Movement playerMovement;
    GameObject rtPlayer;
    gameManager rtManager;
    Vector2 lastContactNormal;

    bool bounced;
    bool isColliding;
    int collisionCount;

    void Start()
    {
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
        rtPlayer = this.gameObject;
        playerMovement = rtPlayer.GetComponent<Movement>();

        bounced = false;
        isColliding = false;
        collisionCount = 0;
    }

    void FixedUpdate()
    {
        if (rtManager.state == gameManager.GameStates.inPlay)
        {
            if (playerMovement.state == Movement.PlayerStates.Sliding && !isColliding) playerMovement.rtRigidbody.gravityScale = 1.74f;
            if (playerMovement.state == Movement.PlayerStates.Sliding && isColliding) playerMovement.rtRigidbody.gravityScale = 0.875f;
            if (playerMovement.state == Movement.PlayerStates.Running && !isColliding) playerMovement.state = Movement.PlayerStates.Jumping;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        collisionCount++;
        isColliding = true;

        if (rtManager.state == gameManager.GameStates.inPlay)
        {
            Vector2 contactNormal = collision.contacts[0].normal;
            lastContactNormal = contactNormal;

            // Handle collisions from below.
            if (contactNormal == Vector2.up && !collision.gameObject.CompareTag("Slime"))
            {

                if (playerMovement.state == Movement.PlayerStates.Sliding)
                {
                    playerMovement.isRight = !playerMovement.isRight;
                }
                playerMovement.state = Movement.PlayerStates.Running;
                playerMovement.haveJumped = false;
            }

            // Handle collisions from below.
            if (contactNormal == Vector2.down)
            {

            }

            // Handle horizontal collisions.
            if (contactNormal == Vector2.left && !collision.gameObject.CompareTag("One Way")) //Debug to test how much this happens.
            {
                if (playerMovement.state == Movement.PlayerStates.Running)
                {
                    playerMovement.isRight = false;
                }

                if (playerMovement.state == Movement.PlayerStates.Jumping)
                {
                    playerMovement.state = Movement.PlayerStates.Sliding;
                }
            }

            if(contactNormal == Vector2.right && !collision.gameObject.CompareTag("One Way"))
            {
                if (playerMovement.state == Movement.PlayerStates.Running)
                {
                    playerMovement.isRight = true;
                }

                if (playerMovement.state == Movement.PlayerStates.Jumping)
                {
                    playerMovement.state = Movement.PlayerStates.Sliding;
                }
            }

            if (collision.gameObject.CompareTag("Slime") || collision.gameObject.CompareTag("Hazard"))
            {
                if (contactNormal != Vector2.up && collision.gameObject.CompareTag("Slime"))
                {
                    playerMovement.state = Movement.PlayerStates.Dead;
                }

                if (contactNormal == Vector2.up && collision.gameObject.CompareTag("Slime"))
                {
                    playerMovement.state = Movement.PlayerStates.Jumping;
                    if (!bounced)
                    {
                        playerMovement.haveJumped = false;
                        playerMovement.rtRigidbody.velocity = new Vector2(playerMovement.rtRigidbody.velocity.x, 0);
                        playerMovement.rtRigidbody.AddForce((Vector2.up * playerMovement.jumpStrength) / 2);
                        bounced = true;
                    }
                }

                if (collision.gameObject.CompareTag("Hazard"))
                {
                    playerMovement.state = Movement.PlayerStates.Dead;
                }    
            }

            if (rtManager.restartFlag) Start();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        collisionCount--;
        bounced = false;
        
        if (collisionCount == 0)
        {
            isColliding = false;
        }
    }
}

