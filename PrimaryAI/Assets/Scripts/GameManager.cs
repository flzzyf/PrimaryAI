using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject prefab_bots;

    public float createRange = 10;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-1, 1) * createRange, Random.Range(-1, 1) * createRange);
            Instantiate(prefab_bots, pos, Quaternion.identity);
        }

        StartCoroutine(QWE());
    }

    void Update()
    {
        
    }

    IEnumerator QWE()
    {
        yield break;
        print("qwe");
    }
}
