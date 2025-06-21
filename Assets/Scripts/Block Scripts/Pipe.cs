using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Sprite wGauge;
    public Sprite wWarning;
    public Sprite wBand;
    public Sprite wValve;

    SpriteRenderer rtSpriteRenderer;

    private void Start()
    {
        rtSpriteRenderer = GetComponent<SpriteRenderer>();
        AssignRandomSprite();
    }

    void AssignRandomSprite()
    {
        int randomNumber = Random.Range(1, 101); // Generates random number between 1 and 200 inclusive.

        if (randomNumber <= 4)
        {
            rtSpriteRenderer.sprite = wBand;
        }
        if (randomNumber == 5)
        {
            rtSpriteRenderer.sprite = wWarning;
        }
        if (randomNumber >= 6 && randomNumber <= 7)
        {
            rtSpriteRenderer.sprite = wValve;
        }
        if (randomNumber >= 8 && randomNumber <= 9)
        {
            rtSpriteRenderer.sprite = wGauge;
        }
    }
}
