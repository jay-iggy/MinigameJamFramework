using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowFight {
    public class ButtonPressLogger : MonoBehaviour {

        public Transform firepoint;
        public GameObject snowballPrefab;

        void Update() {

            if (Input.GetButtonDown("Fire2")) {

                Shoot();
            }
        }

        void Shoot() {
            Instantiate(snowballPrefab, firepoint.position, firepoint.rotation);
        }

    }
}