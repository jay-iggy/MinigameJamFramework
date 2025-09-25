using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Icebreaker {
    public class IcecubeController : MonoBehaviour {
        public Material[] materials = new Material[3];

        public IcecubeController neighborUp;
        public IcecubeController neighborDown;
        public IcecubeController neighborLeft;
        public IcecubeController neighborRight;
        public ParticleSystem refreezeEffect;

        private int stability = 2;

        private float startingY;
        private float falling = 0;

        // Start is called before the first frame update
        void Start() {
            startingY = transform.position.y;
        }

        // Update is called once per frame
        void Update() {
            if (stability > 0) {
                if (
                    ((neighborUp == null || neighborUp.stability > 0) &&
                     (neighborDown == null || neighborDown.stability > 0)) ||
                    ((neighborLeft == null || neighborLeft.stability > 0) &&
                     (neighborRight == null || neighborRight.stability > 0))
                ) {
                    transform.position = new Vector3(transform.position.x, startingY, transform.position.z);
                    falling = 0;
                }
                else {
                    stability = 0;
                    gameObject.GetComponent<MeshRenderer>().material = materials[stability];
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
            else if (falling > -6) {
                falling -= Time.deltaTime * 10;
                transform.position = new Vector3(transform.position.x, startingY + falling, transform.position.z);
            }
        }

        public IEnumerator Flash(float duration, int flashNum) {
            if (stability == 0) {
                yield break;
            }

            float timer = duration;
            float flashPerSecond = (float)flashNum / duration;
            Material mat = GetComponent<MeshRenderer>().material;
            Color defaultCol = mat.color;
            while (timer > 0) {
                timer -= Time.deltaTime;
                float t = Mathf.Abs(Mathf.Sin(Mathf.PI * timer * flashPerSecond));
                mat.color = Color.Lerp(defaultCol, Color.yellow, t);
                yield return null;
            }

            stability = 0;
        }

        public void Smash() {
            if (stability > 0) {
                stability--;
                AudioSource source = GetComponent<SoundEffectController>()
                    .GetSource(stability == 0 ? "Crack" : "Shatter");
                source.pitch = Random.Range(0.8f, 1.2f);
                source.Play();

                gameObject.GetComponent<MeshRenderer>().material = materials[stability];
            }
        }

        public void Freeze() {
            if (stability != 0) {
                return;
            }

            if (
                ((neighborUp == null || neighborUp.stability > 0) &&
                 (neighborDown == null || neighborDown.stability > 0)) ||
                ((neighborLeft == null || neighborLeft.stability > 0) &&
                 (neighborRight == null || neighborRight.stability > 0))
            ) {
                stability++;
                GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<MeshRenderer>().material = materials[stability];
            }
        }

        public void FreezeUnconditional() {
            if (stability == 0) {
                stability = 1;
                GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<MeshRenderer>().material = materials[stability];
            }
        }

        public IEnumerator StartFreeze(bool conditional, float time) {
            if (stability != 0) {
                yield break;
            }

            if (conditional)
                if (!
                        ((neighborUp == null || neighborUp.stability > 0) &&
                         (neighborDown == null || neighborDown.stability > 0)) ||
                    ((neighborLeft == null || neighborLeft.stability > 0) &&
                     (neighborRight == null || neighborRight.stability > 0))
                   )
                    yield break;

            refreezeEffect.Play();
            yield return new WaitForSeconds(time);
            refreezeEffect.Pause();
            refreezeEffect.Clear();
            FreezeUnconditional();
        }
    }
}