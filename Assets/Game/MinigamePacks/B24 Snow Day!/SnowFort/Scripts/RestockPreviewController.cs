using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public class RestockPreviewController : MonoBehaviour, PreviewController
    {
        public Color valid;
        public Color invalid;

        SpriteRenderer spriteRenderer;
        public TextMeshPro text;

        public void SetValid(bool v)
        {
            spriteRenderer.color = v ? valid : invalid;
            text.color = v ? valid : invalid;
        }

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}