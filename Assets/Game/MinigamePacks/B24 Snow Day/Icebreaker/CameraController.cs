using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnowDay.Icebreaker {
    public class CameraController : MonoBehaviour {
        public GameObject[] players = new GameObject[4];
        private bool follow = false;

        // Start is called before the first frame update
        void Start() {
            StartCoroutine(Startup());
        }

        // Update is called once per frame
        void Update() {
            if (!follow) {
                return;
            }

            float x = 0;
            float z = 0;
            int count = 0;

            foreach (GameObject player in players) {
                if (player.transform.position.y > 0) {
                    count++;
                    x += player.transform.position.x;
                    z += player.transform.position.z;
                }
            }

            transform.position +=
                (new Vector3(x / Math.Max(count, 1) * 0.8f, 15, z / Math.Max(count, 1) * 0.8f - 15) -
                 transform.position) * Time.deltaTime;
        }

        IEnumerator Startup() {
            GetComponent<SoundEffectController>().Play("Theme");
            float timer = -1;
            int lastSwung = 0;
            while (timer < 4) {
                if (timer - 0.5 > lastSwung && players[lastSwung]) {
                    players[lastSwung].GetComponent<IcebreakerPawn>().Swing();
                    StartCoroutine(CameraShake());
                    lastSwung++;
                }

                GameObject playerToZoomTo = players[Math.Max(0, (int)Mathf.Floor(timer))];
                transform.position += Time.deltaTime * 3 *
                                      (new Vector3(playerToZoomTo.transform.position.x, 6,
                                          playerToZoomTo.transform.position.z - 4) - transform.position);
                timer += Time.deltaTime;

                yield return null;
            }

            follow = true;
            foreach (GameObject player in players) {
                player.GetComponent<IcebreakerPawn>().canMove = true;
            }
        }

        IEnumerator CameraShake() {
            yield return new WaitForSeconds(0.2f);
            float timer = 0;
            while (timer < 0.25) {
                transform.rotation = Quaternion.Euler(new Vector3(50 + (0.25f - timer) * 20, 0, 0));
                timer += Time.deltaTime;
                yield return null;
            }

            transform.rotation = Quaternion.Euler(new Vector3(50, 0, 0));
        }

        public IEnumerator EndSequence(GameObject lastDead) {
            GetComponent<SoundEffectController>().Stop("Theme");
            GetComponent<SoundEffectController>().Play("Win");
            GameObject winner = null;

            foreach (GameObject player in players) {
                if (player.GetComponent<IcebreakerPawn>().alive) {
                    winner = player;
                    break;
                }
            }

            if (winner == null) {
                winner = lastDead;
            }

            winner.GetComponent<IcebreakerPawn>().canMove = false;
            foreach (TrailRenderer t in winner.GetComponentsInChildren<TrailRenderer>()) {
                t.enabled = false;
            }

            winner.transform.position = new Vector3(0, 1.9f, 12);
            winner.transform.rotation = Quaternion.Euler(0, 180, 0);
            winner.GetComponent<Animation>().Play();

            follow = false;
            yield return null;
            while (true) {
                transform.position += 6 * Time.deltaTime *
                                      (new Vector3(winner.transform.position.x, 7, winner.transform.position.z - 4) -
                                       transform.position);
                yield return null;
            }
        }
    }
}