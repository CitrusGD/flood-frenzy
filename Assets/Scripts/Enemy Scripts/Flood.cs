using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Flood : MonoBehaviour
{
    public gameManager rtManager;
    public Animator firstAnimator;
    public Animator secondAnimator;
    public SpriteRenderer srFloodFall;
    public SpriteRenderer srFloodSpout;
    public Sprite firstFrame;
    public bool startFlag;

    public float timeCheck;

    bool hasSecondAnimationPlayedOnce = false;
    bool flooding;

    public void Start()
    {
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
        firstAnimator.enabled = false;
        secondAnimator.enabled = false;
        secondAnimator.SetBool("isFilling", false);
        firstAnimator.Rebind();
        secondAnimator.Rebind();
        //srFloodFall.sprite = firstFrame;
        //srFloodSpout.sprite = firstFrame;

        timeCheck = 0;
        this.transform.position = new Vector3(0, -14.6616f, 0);
        startFlag = false;
        hasSecondAnimationPlayedOnce = false;
        flooding = false;
    }

    void Update()
    {
        if (rtManager.state == gameManager.GameStates.inPlay)
        {
            if (startFlag)
            {
                float accel = (rtManager.score + 0.015f) / 15000f;
                this.transform.position += new Vector3(0, accel * Time.deltaTime, 0);
            }

            if (GameObject.Find("Ratticus Hoy").transform.position.y - 8f > this.transform.position.y)
            {
                if (timeCheck == 0) timeCheck = Time.time;
                if (timeCheck <= Time.time - 6f)
                {
                    this.transform.position = new Vector3(0, GameObject.Find("Ratticus Hoy").transform.position.y - 10f, 0);
                }
            }
            if (GameObject.Find("Ratticus Hoy").transform.position.y - 10f <= this.transform.position.y) timeCheck = 0;

            //Need a frame independent way of only peforming the if statement if the condition is met for X seconds.
        }

        {
            if (!(this.transform.position.y >= -12.75f))
            {
                firstAnimator.enabled = true;
            }

            if (firstAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.769230769231f)
            {
                secondAnimator.enabled = true;
            }

            if (secondAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                secondAnimator.SetBool("isFilling", true);
                hasSecondAnimationPlayedOnce = true;
            }

            if (secondAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f) flooding = true;

            if (secondAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && hasSecondAnimationPlayedOnce)
            {
                srFloodFall.sprite = null;
                secondAnimator.enabled = false;
                hasSecondAnimationPlayedOnce = false;
            }

            if (flooding && !startFlag)
            {
                this.transform.position += new Vector3(0, 0.025f, 0);
            }


            if (this.transform.position.y >= -12.75f)
            {
                srFloodSpout.sprite = null;
                firstAnimator.enabled = false;
            }

            if (this.transform.position.y >= -8.5f)
            {
                startFlag = true;
            }
        }
    }
}
