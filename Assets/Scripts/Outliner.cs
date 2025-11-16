using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Outliner : MonoBehaviour
{
    public Color outlineColor;
    public float outlineWidth;

    private void Start()
    {
        TextMeshProUGUI tmpg = GetComponent<TextMeshProUGUI>();
        TextMeshPro tmp = GetComponent<TextMeshPro>();

        if (tmpg != null) 
        {
            tmpg.outlineColor = outlineColor;
            tmpg.outlineWidth = outlineWidth;
            tmpg.ForceMeshUpdate();
        }

        if(tmp != null)
        {
            tmp.outlineColor = outlineColor;
            tmp.outlineWidth = outlineWidth;
            tmp.ForceMeshUpdate();
        }
    }
}
