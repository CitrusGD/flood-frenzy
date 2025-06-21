using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject enemy;

    GameObject spawnedEnemy;

    void Start()
    {
        spawnedEnemy = Instantiate(enemy, this.transform);
        spawnedEnemy.transform.localPosition = new Vector3(0, 0, 0);
    }
}
