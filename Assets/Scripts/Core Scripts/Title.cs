using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public gameManager rtManager;
    float scaleAmount = 0.001f;
    public bool isGrowing, isClockwise;
    int frameCount;

    void Start()
    {
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
    }

    void Update()
    {
        if(rtManager.state == gameManager.GameStates.Start)
        {
            // Grow/Shrink
            if (this.transform.localScale.x >= 2.25) isGrowing = false;
            if (this.transform.localScale.x <= 1.75) isGrowing = true;

            if (isGrowing) this.transform.localScale += new Vector3(scaleAmount, scaleAmount, 0);
            if (!isGrowing) this.transform.localScale -= new Vector3(scaleAmount, scaleAmount, 0);

            // Rotate
            frameCount++;
            if (frameCount < 300) isClockwise = true;
            if (frameCount > 300) isClockwise = false;
            if (frameCount > 600) frameCount = 0;

            if (!isClockwise) this.transform.eulerAngles += new Vector3(0, 0, 0.01f);
            if (isClockwise) this.transform.eulerAngles -= new Vector3(0, 0, 0.01f);
        }
    }
}
