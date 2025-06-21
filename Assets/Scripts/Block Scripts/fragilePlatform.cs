using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fragilePlatform : MonoBehaviour
{
    Checker rtChecker;
    GameObject rtChild;
    SpriteRenderer rtSpriteRenderer;

    int frameCount;
    float zAngle;
    bool isClockwise, isShaking, hasFallen;

    void Start()
    {
        rtChild = this.transform.GetChild(0).gameObject;
        rtSpriteRenderer = rtChild.GetComponent<SpriteRenderer>();
        rtChecker = this.transform.GetChild(1).GetComponent<Checker>();
    }

    void FixedUpdate()
    {
        if (isShaking)
        {
            if (frameCount != 90) frameCount++;
            if (frameCount == 90)
            {
                frameCount = 0;
                isShaking = false;
                hasFallen = true;
            }

            zAngle = rtChild.transform.eulerAngles.z;

            if (zAngle > 180f) zAngle -= 360f;

            if (zAngle >= 5f) isClockwise = false;
            else if (zAngle <= -5f) isClockwise = true;

            if (isClockwise) rtChild.transform.Rotate(0, 0, 1f);
            else rtChild.transform.Rotate(0, 0, -1f);
        }

        if (hasFallen)
        {

            if (frameCount == 0)
            {
                rtChild.transform.eulerAngles = new Vector3(0, 0, 0);

                rtChild.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY;
                rtChild.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 9.81f);

                this.GetComponent<BoxCollider2D>().enabled = false;
            }

            frameCount++;

            if (frameCount > 150 && frameCount < 300)
            {
                if (frameCount == 151)
                {
                    rtChild.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    rtChild.transform.position = this.transform.position;
                }
                if (frameCount % 30 == 0)
                {
                    rtSpriteRenderer.enabled = !rtSpriteRenderer.enabled;
                }
            }

            if (frameCount >= 300 && !rtChecker.isOccupied)
            {
                hasFallen = false; frameCount = 0;
                this.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactNormal = collision.contacts[0].normal;

        if (collision.gameObject.CompareTag("Player") && contactNormal == Vector2.down && !isShaking)
        {
            isShaking = true;
        }
    }

}
