using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort {
    public class DamageSpriteChanger : MonoBehaviour
    {
        public List<Sprite> sprites;
        public List<int> health;

        SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            GetComponent<Breakable>().onDamage += (h) =>
            {
                int i = health.IndexOf(h);
                if (i != -1) spriteRenderer.sprite = sprites[i];
            };
        }
    }
}