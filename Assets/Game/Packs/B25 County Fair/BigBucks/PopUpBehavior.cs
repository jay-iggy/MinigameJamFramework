using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CountyFair.BigBucks {
    public class PopUpBehavior : MonoBehaviour {
        [SerializeField] TMP_Text text;

        float vel = 2;
        float lifespan = .75f;
        float timeAlive = 0;

        // Start is called before the first frame update
        void Start() {
            Destroy(gameObject, lifespan);
        }

        public void SetMessage(String message) {
            text.text = message;
        }

        // Update is called once per frame
        void Update() {
            transform.position += Vector3.up * vel * Time.deltaTime;

            Color textColor = text.color;
            textColor.a = 1 - timeAlive / lifespan;

            timeAlive += Time.deltaTime;
        }
    }
}
