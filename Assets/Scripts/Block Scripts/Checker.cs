using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public bool isOccupied;

    void OnTriggerEnter2D(Collider2D collider)
    {
        isOccupied = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        isOccupied = false;
    }
}
