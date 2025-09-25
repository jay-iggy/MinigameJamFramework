using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public class SpritePreview : MonoBehaviour, PreviewController
    {
        public Color valid;
        public Color invalid;

        SpriteRenderer spriteRenderer;

        public void SetValid(bool validPlacement)
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.color = validPlacement ? valid : invalid;
        }
    }
}