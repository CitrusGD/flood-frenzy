using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBar : MonoBehaviour
{
    GameObject mainCamera;
    float lastHighestY;
    gameManager rtManager;

    public void Start()
    {
        this.transform.position = new Vector3(0, 0, 0);
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
        mainCamera = GameObject.Find("Main Camera");
        lastHighestY = 0;
    }

    void Update()
    {
        if (mainCamera.transform.position.y - 4 > lastHighestY)
        {
            lastHighestY = mainCamera.transform.position.y - 4;
            transform.position = new Vector3(transform.position.x, lastHighestY, transform.position.z);
        }

        if (rtManager.restartFlag) Start();
    }
}
