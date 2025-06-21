using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    public int variants;
    public float difficulty;
    public bool needsVariant;

    bool set;
    int timeToMoveScreen;

    gameManager rtManager;
    GameObject rtPlayer;

    void Start()
    {
        rtManager = GameObject.Find("Game Manager").GetComponent<gameManager>();
        rtPlayer = GameObject.Find("Ratticus Hoy");
        set = false;
    }

    void Update()
    {
        if (rtPlayer.transform.position.y - 30f > this.transform.position.y && this.transform.position.x == 0)
        {
            Destroy(gameObject);
        }

        if (needsVariant && !set)
        {
            int rand = Random.Range(0, variants + 1);
            this.transform.GetChild(rand).gameObject.SetActive(true);
            difficulty = this.transform.GetChild(rand).gameObject.transform.GetChild(0).gameObject.transform.localScale.x;
            set = true;
        }
        
        if (variants != 0 && !set)
        {
            int rand = Random.Range(0, variants + 1);
            if (rand != 0)
            {
                this.transform.GetChild(rand - 1).gameObject.SetActive(true);
                difficulty = this.transform.GetChild(rand - 1).gameObject.transform.GetChild(0).gameObject.transform.localScale.x;
            }
            set = true;
        }

        if (rtPlayer.GetComponent<Movement>().state == Movement.PlayerStates.Dead) timeToMoveScreen++;
        if (rtPlayer.GetComponent<Movement>().state == Movement.PlayerStates.Dead && timeToMoveScreen == 120)
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}
