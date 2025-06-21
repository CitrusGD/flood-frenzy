using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CapsuleCollider2D rtCollider;
    gameManager rtManager;
    Vector2 originalSize;
    Vector2 originalOffset;
    GameObject spotlight;
    GameObject fadeScreen;
    endScreen rtEndScript;
    GameObject rtEndScreen;

    [HideInInspector] public Rigidbody2D rtRigidbody;

    public enum PlayerStates { Running, Jumping, Sliding, Dead }
    public PlayerStates state;
    public float jumpStrength;
    public float runSpeed;
    public bool haveJumped;
    public bool isRight;
    bool reset = false;

    PlayerStates previousState;
    bool thrown;

    public void Start()
    {

        if (reset == false)
        {
            reset = true;
            this.gameObject.SetActive(true);
            rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
            rtEndScreen = GameObject.Find("End Screen Parent");
            rtEndScript = rtEndScreen.GetComponent<endScreen>();
            rtRigidbody = this.GetComponent<Rigidbody2D>();
            rtCollider = this.GetComponent<CapsuleCollider2D>();
            spotlight = this.transform.GetChild(0).gameObject;
            fadeScreen = GameObject.Find("Fade Screen");
            spotlight.gameObject.SetActive(false);
            spotlight.transform.localScale = new Vector3(32, 32, 1);
            fadeScreen.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

            originalSize = rtCollider.size;
            originalOffset = rtCollider.offset;

            GameObject.Find("Main Camera").GetComponent<Tracking>().enabled = true;
            GameObject.Find("Main Camera").GetComponent<Tracking>().Start();

            this.transform.position = new Vector3(0, -0.725f, 0);
            this.GetComponent<SpriteRenderer>().sortingOrder = -1;
            this.GetComponent<BoxCollider2D>().enabled = true;
            rtCollider.enabled = true;
            haveJumped = false; isRight = true; thrown = false;
            rtEndScript.enabled = true; rtEndScript.Start();
            rtEndScript.enabled = false; rtEndScreen.transform.localPosition = new Vector3(0, 0, 0);
            rtEndScript.dropSpeed = 0.05f; rtEndScript.countdown = 0;

            state = PlayerStates.Running;
        }
    }

    void Update()
    {
        if (state == PlayerStates.Dead)
        {
            if (!thrown)
            {
                reset = false;
                thrown = true;
                rtCollider.enabled = false;
                this.GetComponent<BoxCollider2D>().enabled = false;
                rtRigidbody.velocity = new Vector2(0, 7.5f);
                GameObject.Find("Main Camera").GetComponent<Tracking>().enabled = false;
            }
            if (thrown && this.transform.position.y <= -0.725f) this.gameObject.SetActive(false);

            //For Death Animation
            //Increase opacity of fadeScreen to 100, enable spotlight and decrease it's size to x=8 y=8. Increase sorting layer of Rat to 6.
            fadeScreen.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.02f);
            if (spotlight.transform.localScale.x >= 8) spotlight.transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            spotlight.gameObject.SetActive(true);
            this.GetComponent<SpriteRenderer>().sortingOrder = 6;

            rtEndScript.enabled = true;
        }

        if (rtManager.state == gameManager.GameStates.inPlay)
        {
            //Actually moves the player.
            if (state == PlayerStates.Running || state == PlayerStates.Jumping)
            {
                if (isRight)
                {
                    this.transform.localScale = new Vector2(0.3f, this.transform.localScale.y);
                    rtRigidbody.velocity = new Vector2(runSpeed * 3f, rtRigidbody.velocity.y);
                }
                if (!isRight)
                {
                    this.transform.localScale = new Vector2(-0.3f, this.transform.localScale.y);
                    rtRigidbody.velocity = new Vector2(runSpeed * -3f, rtRigidbody.velocity.y);
                }
            }

            //Virtual 'Friction' as if the player were sliding.
            if (state == PlayerStates.Sliding)
            {
                //Activate Sliding Anim
                haveJumped = false;
                rtRigidbody.velocity = new Vector2(0, rtRigidbody.velocity.y);
                if (rtRigidbody.gravityScale == 1.75f) rtRigidbody.gravityScale = rtRigidbody.gravityScale / 2;
            }

            //Resets the 'Friction' when no longer sliding.
            if (state != PlayerStates.Sliding)
            {
                rtRigidbody.gravityScale = 1.75f;
            }

            if (Input.GetMouseButtonDown(0))
            {
                //Mid-air after jumping once.
                if (state == PlayerStates.Jumping && !haveJumped)
                {
                    haveJumped = true;
                    rtRigidbody.velocity = new Vector2(rtRigidbody.velocity.x, 0);
                    rtRigidbody.AddForce(Vector2.up * jumpStrength);
                }

                //On ground, yet to jump
                if (state == PlayerStates.Running)
                {
                    state = PlayerStates.Jumping;
                    rtRigidbody.velocity = new Vector2(rtRigidbody.velocity.x, 0);
                    rtRigidbody.AddForce(Vector2.up * jumpStrength);
                }

                //Sliding down wall, perhaps after double jumping.
                if (state == PlayerStates.Sliding)
                {
                    isRight = !isRight;
                    state = PlayerStates.Jumping;
                    rtRigidbody.velocity = new Vector2(rtRigidbody.velocity.x, 0);
                    rtRigidbody.AddForce(Vector2.up * jumpStrength);
                }
            }

            if (state == PlayerStates.Dead)
            {
                rtManager.state = gameManager.GameStates.End;
            }
        }
        if (rtManager.state == gameManager.GameStates.Start)
        {
            //If you have time, add the Rat calm before the game begins as it's own animation.
        }

        if (previousState != state)
        {
            if (state == PlayerStates.Sliding)
            {
                //X Offset:0.4514546f   Y Offset:-1.753794
                //X Size:4.015742   Y Size:1.61451

                rtCollider.size = new Vector2(originalSize.y, originalSize.x + 0.75f);
                rtCollider.offset = new Vector2(-originalOffset.y, originalOffset.x - 0.5f);
                rtCollider.direction = CapsuleDirection2D.Vertical;
                
            }
            else
            {
                // Reset to original size and offset when not sliding
                rtCollider.size = originalSize;
                rtCollider.offset = originalOffset;
                rtCollider.direction = CapsuleDirection2D.Horizontal;
            }

            previousState = state;
        }

        if (rtManager.restartFlag && !reset)
        {
            Start();
        }
    }
}
