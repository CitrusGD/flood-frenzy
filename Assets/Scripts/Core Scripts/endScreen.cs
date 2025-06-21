using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScreen : MonoBehaviour
{
    public List<GameObject> digits = new List<GameObject>();
    public float dropSpeed;
    public int countdown;
    public Sprite one, two, three, four, five, six, seven, eight, nine, zero;
    public float distance, score;
    public GameObject digit;
    GameObject rtGameOver;
    bool recieved;
    string sDistance, sScore;

    public void Start()
    {
        recieved = false;
        distance = 0; score = 0;
        rtGameOver = GameObject.Find("Game Over");
        rtGameOver.transform.localPosition = new Vector3(0, 15, 1);
        dropSpeed = 0.05f;
        this.transform.localPosition = new Vector2(0, 0);
        for (int i = 0; i < digits.Count; i++)
        {
            Destroy(digits[i]);
        }
        digits.Clear();
    }

    
    void Update()
    {
        if (!recieved)
        {
            if (distance == 0) distance = GameObject.Find("Game Manager").GetComponent<gameManager>().totalDistance;
            if (score == 0) score = GameObject.Find("Game Manager").GetComponent<gameManager>().score;
            sDistance = distance.ToString(); sScore = score.ToString();

            for (int i = 0; i < sScore.Length; i++)
            {
                GameObject currentDigit = Instantiate(digit);
                digits.Add(currentDigit);
                if (sScore[i] == '1') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = one;
                if (sScore[i] == '2') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = two;
                if (sScore[i] == '3') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = three;
                if (sScore[i] == '4') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = four;
                if (sScore[i] == '5') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = five;
                if (sScore[i] == '6') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = six;
                if (sScore[i] == '7') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = seven;
                if (sScore[i] == '8') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = eight;
                if (sScore[i] == '9') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = nine;
                if (sScore[i] == '0') currentDigit.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = zero;
                currentDigit.transform.SetParent(rtGameOver.transform);
                currentDigit.transform.localScale = new Vector2(1.25f / 40f, 1.25f / 45f);
                // (1x) - y = -0.05, (2x) - y = -0.025, (3x) - y = 0
                // x = y - 0.05, 2y - y -0.1 = -0.025, y = = 0.075
                currentDigit.transform.localPosition = new Vector2(((i + 1) * 0.025f) - 0.075f, -0.1f);
            }

            recieved = true;
        }

        if (this.transform.localPosition.y > -32.7f) this.transform.localPosition -= new Vector3(0, dropSpeed, 0);
        if (rtGameOver.transform.localPosition.y >= 2) rtGameOver.transform.localPosition -= new Vector3(0, dropSpeed, 0);
        if (countdown <= 720) countdown++;
        if (countdown > 600 && countdown < 719) dropSpeed = dropSpeed - 0.00015625f;
        if (this.transform.localPosition.y <= -32.6f) this.transform.localPosition -= new Vector3(0, dropSpeed, 0);
        if (this.transform.localPosition.y <= -56f) this.transform.localPosition = new Vector3(0, -32.7f, 0);
    }
}
