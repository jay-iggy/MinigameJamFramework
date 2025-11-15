using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class BillboardBehavior : MonoBehaviour
    {
        private Transform cam;

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.allCameras[0].transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(cam.position);
        }
    }
}
