using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomType
{
    public GameObject roomType;
    public int height;
    public bool eligible;
}

public class gameManager : MonoBehaviour
{
    /**Notes: 
    2. *Reapproach the rising sludge, perhaps keep it permanently in the previous room?*
    3. Rat sprites, include jumping and maybe the relaxing one at the start.
    4. Vary Slime sprites.
    5. Consider including additional Slime types as enemies.
    **/


    public List<RoomType> roomTypes = new List<RoomType>();
    public enum GameStates { inPlay, Paused, Start, End }
    public GameStates state;

    public int litterCollected;
    public float score;
    public float difficultyLevel;
    public Flood rtFlood;
    public GameObject scoreBoard;
    public GameObject scoreBar;
    public GameObject startRoom;
    public List<RoomType> eligibleRooms;
    public float totalDistance;

    int rand, rand1, rand2, lastRand, numberOfRooms;
    int timeToMoveScreen;
    float currentHeight = 1;
    bool roomReady;
    bool finalTotaled;
    bool startRoomSet;

    public bool restartFlag;

    Movement playerMovement;

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(918, 1080, false);

        restartFlag = false;
        litterCollected = 0; difficultyLevel = 0; score = 0;
        currentHeight = 8; roomReady = false; finalTotaled = false;
        state = GameStates.Start;
        playerMovement = GameObject.Find("Ratticus Hoy").GetComponent<Movement>();
        scoreBoard.GetComponent<TextMesh>().text = ("Distance:\n" + score);
        rtFlood.enabled = false;
        numberOfRooms = 0;
        for (int i = 0; i < roomTypes.Count; i++) roomTypes[i].eligible = false;
    }

    void Update()
    {
        //If there is time before submission, add an if statement that forces a specific room at certain
        //difficulty levels that can function as 'Boss Room'.

        difficultyLevel = (score / 1000f) + 1;

        if ((playerMovement.gameObject.transform.position.y >= currentHeight - 10f) && !roomReady)
        {
            for (int i = 0; i < roomTypes.Count; i++)
            {
                if (difficultyLevel >= roomTypes[i].roomType.GetComponent<Rooms>().difficulty && roomTypes[i].eligible == false)
                {
                    roomTypes[i].eligible = true;
                    numberOfRooms++;
                }
            }

            rand = Random.Range(0, numberOfRooms);
            if (rand == lastRand && rand != numberOfRooms - 1) rand++;
            else if (rand == lastRand && rand != 0) rand--;

            rand1 = Random.Range(0, numberOfRooms);
            if (rand1 == lastRand && rand1 != numberOfRooms - 1) rand1++;
            else if (rand1 == lastRand && rand1 != 0) rand1--;

            rand2 = Random.Range(0, numberOfRooms);
            if (rand2 == lastRand && rand2 != numberOfRooms - 1) rand2++;
            else if (rand2 == lastRand && rand2 != 0) rand2--;

            if (rand1 > rand) rand = rand1;
            if (rand2 > rand) rand = rand2;

            GameObject currentRoom = Instantiate(roomTypes[rand].roomType);
            currentRoom.transform.position = new Vector2(0f, currentHeight);
            currentHeight += roomTypes[rand].height;
            lastRand = rand;
        }

        if (state == GameStates.Start)
        {
            if (Input.GetMouseButtonDown(0))
            {
                rtFlood.enabled = true;
            }

            if (rtFlood.startFlag)
            {
                state = GameStates.inPlay;
                playerMovement.rtRigidbody.AddForce(Vector2.up * playerMovement.jumpStrength);
            }
        }

        if (state == GameStates.inPlay)
        {
            score = Mathf.Round(scoreBar.transform.position.y) * 100;
            scoreBoard.GetComponent<TextMesh>().text = ("Distance:\n" + score);
        }

        if (state == GameStates.End)
        {
            if (!finalTotaled)
            {
                totalDistance = score;
                score += (litterCollected * 10);
                finalTotaled = true;
            }

            if (finalTotaled)
            {
                timeToMoveScreen++;
            }

            if (Input.GetMouseButtonDown(0) && timeToMoveScreen >= 360)
            {
                rtFlood.Start();
                playerMovement.Start();
                scoreBar.GetComponent<ScoreBar>().Start();
                restartFlag = true;
                Start();
                state = GameStates.Start;
                timeToMoveScreen = 0;
            }
        }
    }
}
