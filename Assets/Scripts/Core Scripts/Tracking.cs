using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    GameObject target;
    gameManager rtManager;
    private bool isMovingToStartPosition;
    private float lerpSpeed = 4f;
    private float lerpThreshold = 0.05f;

    public void Start()
    {
        this.transform.position = new Vector3(0, -5, -10);
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
        target = GameObject.Find("Ratticus Hoy").gameObject;
        isMovingToStartPosition = true;
    }

    void Update()
    {
        if (rtManager.state == gameManager.GameStates.inPlay)
        {
            if (isMovingToStartPosition)
            {
                Vector3 targetPosition = new Vector3(this.transform.position.x, target.transform.position.y + 3f, -10f);
                this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, lerpSpeed * Time.deltaTime);

                if (Vector3.Distance(this.transform.position, targetPosition) < lerpThreshold)
                {
                    isMovingToStartPosition = false;
                }
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x, target.transform.position.y + 3f, -10f);
            }
        }
    }
}
