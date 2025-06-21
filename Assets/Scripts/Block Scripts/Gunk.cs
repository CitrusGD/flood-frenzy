using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunk : MonoBehaviour
{
    SpriteRenderer rtSpriteRenderer;
    BoxCollider2D rtCollider;
    Checker rtChecker;
    gameManager rtManager;

    public bool isBroken = false;

    int frameCount = 0;

    void Start()
    {
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
        rtSpriteRenderer = GetComponent<SpriteRenderer>();
        rtCollider = GetComponent<BoxCollider2D>();
        rtChecker = this.transform.GetChild(0).GetComponent<Checker>();
    }

    void Update()
    {
        if (rtManager.state == gameManager.GameStates.inPlay)
        {
            if (isBroken)
            {
                frameCount++;

                if (frameCount > 900 && frameCount < 1650)
                {
                    int flashingFrequency = 750 / 5;
                    if (frameCount % flashingFrequency == 0)
                    {
                        rtSpriteRenderer.enabled = !rtSpriteRenderer.enabled;
                    }
                }
                else if (frameCount >= 1650 && !rtChecker.isOccupied)
                {
                    isBroken = false;
                    rtSpriteRenderer.enabled = true;
                    rtCollider.enabled = true;
                    frameCount = 0;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (rtManager.state == gameManager.GameStates.inPlay)
        {
            // Only handle collision from Player and from below.
            if (collision.gameObject.tag == "Player")
            {
                Vector2 contactNormal = collision.contacts[0].normal;

                if (!isBroken && contactNormal == Vector2.up)
                {
                    isBroken = true;
                    rtSpriteRenderer.enabled = false;
                    rtCollider.enabled = false;
                }
            }
        }
    }
}
