using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonJuice : MonoBehaviour, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

    [Header("Dependencies")]
    public RectTransform graphic;
    public RectTransform shadow;
    public UnityEvent onClick = new();

    [Header("Hover")]
    public AnimationCurve hoverPosCurve;
    public AnimationCurve hoverScaleCurve;
    public float hoverDuration = 0.2f;

    [Header("Select")]
    public AnimationCurve selectPosCurve;
    public float selectDuration = 0.6f;
    

    private Button _button;
    private bool _isHovered = false;

    void Awake() {
        _button = GetComponent<Button>();
    }

    
    public void OnSelect(BaseEventData eventData) {
        if (!_button.interactable) return;
        StartCoroutine(EvaluateCurve(hoverPosCurve, hoverScaleCurve, hoverDuration));
        _isHovered = true;
    }
    public void OnDeselect(BaseEventData eventData) {
        if (!_isHovered) return;
        StartCoroutine(EvaluateCurve(hoverPosCurve, hoverScaleCurve, hoverDuration, 0.43f, -1));
        _isHovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        OnSelect(eventData);
    }

    public void OnPointerExit(PointerEventData eventData) {
        OnDeselect(eventData);
    }

    void Update() {
        // Unhover if button is disabled
        if(_isHovered) {
            if(!_button.interactable) {
                StartCoroutine(EvaluateCurve(hoverPosCurve, hoverScaleCurve, hoverDuration, .4f, -1));
                _isHovered = false;
            }
        }
    }
    

    private IEnumerator EvaluateCurve(AnimationCurve posCurve, AnimationCurve scaleCurve, float dur, float t=0, float deltaScale = 1) {        
        bool condition = true;
        
        while (condition) {
            Vector2 pos = graphic.anchoredPosition;
            pos.y = posCurve.Evaluate(t / dur);
            graphic.anchoredPosition = pos;

            Vector2 scale = graphic.localScale;
            scale.y = scaleCurve.Evaluate(t / dur);
            graphic.localScale = scale;

            t += Time.deltaTime * deltaScale;
            yield return null;
            condition = deltaScale>0? t < dur : t >= 0;
        }
    }

    public void OnSubmit(BaseEventData eventData) {
        StartCoroutine(SubmitAnimation());
    }
    public void OnSubmit() {
        StartCoroutine(SubmitAnimation());
    }

    private IEnumerator SubmitAnimation() {
        float t = 0;
        while (t<selectDuration) {
            Vector2 pos = graphic.anchoredPosition;
            pos.y = selectPosCurve.Evaluate(t / selectDuration);
            graphic.anchoredPosition = pos;
            t += Time.deltaTime;
            yield return null;
        }
        onClick.Invoke();
    }
}
