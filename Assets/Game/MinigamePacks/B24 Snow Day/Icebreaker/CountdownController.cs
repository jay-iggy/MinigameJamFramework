using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Icebreaker {
    public class CountdownController : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            StartCoroutine(Countdown());
        }

        // Update is called once per frame
        void Update() { }

        IEnumerator Countdown() {
            yield return new WaitForSeconds(1.5f);
            GetComponent<TextMeshPro>().enabled = true;
            float time = 0;
            GetComponent<TextMeshPro>().text = "3";
            while (time < 1) {
                transform.localEulerAngles = new Vector3(0, time * 360 + 90, 0);
                time += Time.deltaTime;
                yield return null;
            }

            GetComponent<TextMeshPro>().text = "2";
            while (time < 2) {
                transform.localEulerAngles = new Vector3(0, time * 360 + 90, 0);
                time += Time.deltaTime;
                yield return null;
            }

            GetComponent<TextMeshPro>().text = "1";
            while (time < 3) {
                transform.localEulerAngles = new Vector3(0, time * 360 + 90, 0);
                time += Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
