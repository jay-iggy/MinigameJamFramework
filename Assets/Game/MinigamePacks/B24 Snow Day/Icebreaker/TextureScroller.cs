using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Icebreaker {
    public class TextureScroller : MonoBehaviour {
        private Material mat;
        public float Speed;

        private void Awake() {
            mat = GetComponent<MeshRenderer>().material;
        }

        void Update() {
            float x = Speed * Time.realtimeSinceStartup;
            Vector2 offset = new Vector2(Mathf.Cos(x), Mathf.Sin(x));
            mat.mainTextureOffset = offset;
        }
    }
}