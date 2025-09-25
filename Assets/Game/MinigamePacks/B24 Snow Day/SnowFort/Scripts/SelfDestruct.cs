using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowfort
{
    public class SelfDestruct : MonoBehaviour
    {
        public float time;

        void Start()
        {
            StartCoroutine(Destruct());
        }

        IEnumerator Destruct()
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}