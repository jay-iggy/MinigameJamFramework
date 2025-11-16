using HotPotatoGame;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BounceManager : MonoBehaviour
{
    private float timer;
    [SerializeField] private float timeBetweenSwap;
    [SerializeField] private GameObject bounceIndicatorPrefab;
    [SerializeField] private GameObject bounceIndicatorManager;
    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        foreach (Transform bounce in transform)
        {
            bounce.GetComponent<BouncePad>()?.setUp(bounceIndicatorManager, bounceIndicatorPrefab, offset);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenSwap)
        {
            foreach (Transform bounce in transform)
            {
                bounce.GetComponent<BouncePad>()?.ChangeDirection();
            }
            timer = 0f;
        }
    }


}
