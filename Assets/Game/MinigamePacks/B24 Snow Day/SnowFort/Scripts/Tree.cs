using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowfort
{
    public class Tree : MonoBehaviour
    {
        public Vector2 scaleRange = new Vector2(0.8f, 1.2f);

        public Vector2 angleRange = new Vector2(-10, 10);

        void Start()
        {
            transform.localScale = (Random.Range(0, 2) == 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

            transform.localScale *= Mathf.Lerp(scaleRange.x, scaleRange.y, Random.Range(0f, 1f));

            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(angleRange.x, angleRange.y, Random.Range(0f, 1f)));
        }
    }
}