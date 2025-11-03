using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI body;
    public Transform childParent;

    public void SetText(string titleText, Color color, string bodyText = "") {
        title.color = color;
        title.text = titleText;
        if (!string.IsNullOrEmpty(bodyText)) {
            body.text = bodyText;
        }
        else {
            body.gameObject.SetActive(false);
        }
    }
}
