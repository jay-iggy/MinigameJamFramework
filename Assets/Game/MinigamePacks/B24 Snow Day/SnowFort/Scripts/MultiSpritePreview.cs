using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowfort
{
    public class MultiSpritePreview : MonoBehaviour, PreviewController
    {
        public Color valid;
        public Color invalid;

        SpriteRenderer[] spriteRenderers;

        public void SetValid(bool validPlacement)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = validPlacement ? valid : invalid;
            }
        }

        void Awake()
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }
}