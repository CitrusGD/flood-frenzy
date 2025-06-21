using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Litter : MonoBehaviour
{
    gameManager rtManager;

    void Start()
    {
        //this.GetComponent<SpriteRenderer>().Sprite = randSprite;
        int randomNumber = Random.Range(1, 5);
        if (randomNumber == 1) this.GetComponent<SpriteRenderer>().flipX = true;
        randomNumber = Random.Range(1, 5);
        if (randomNumber == 1) this.GetComponent<SpriteRenderer>().flipY = true;
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && this.name == ("Litter"))
        {
            rtManager.litterCollected = rtManager.litterCollected + 1;
            Destroy(this.gameObject);
        }
    }
}
