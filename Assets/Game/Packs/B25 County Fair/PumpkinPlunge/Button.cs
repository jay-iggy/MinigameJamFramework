using System;
using System.Collections;
using UnityEngine;

namespace Starter.PumpkinPlunge {
    public class Button : MonoBehaviour {
        [Header("References")]
        public GameObject indicatorSymbol;
        [SerializeField] private GameObject responsePrefab;
        [Header("Properties")]
        public TrapdoorType type;
        private float PUSH_PERCENT = 0.85f;
        private float PUSH_MAX_TIME = 0.2f;
        private float WRONG_COOLDOWN = 0.5f;
        private float responseShowDuration = 1.5f;
        private float responseRiseStartPosition = 1.5f;
        private float responseRiseEndPosition = 2.5f;
        private float responseColorAlphaOffset = 0.5f;
        private Color responseColorStart = Color.white;
        private Color responseColorEnd = new(1, 1, 1, 0);
        [HideInInspector] public ButtonPanel panel;

        private bool newPushCall = false;
        private bool positionInit = false;
        private float inPositionZ;
        private float spawnPositionZ;
        private float buttonMoveOffset => PUSH_PERCENT * transform.localScale.y;

        void Start()
        {
            UpdateType();
            panel.buttonPushed = false;
        }

        private float ButtonResponseEasing(float t)
        {
            return Mathf.Sin((float)(t * Math.PI * 0.5f));
        }

        IEnumerator ShowButtonResponseRoutine(bool isSuccess)
        {
            GameObject responseObject = Instantiate(responsePrefab, transform);
            ButtonResponse response = responseObject.GetComponent<ButtonResponse>();
            GameObject symbol = isSuccess ? response.successSymbol : response.failureSymbol;
            symbol.SetActive(true);
            SpriteRenderer spriteRenderer = symbol.GetComponent<SpriteRenderer>();
            response.transform.localPosition = Vector3.zero;
            float timer = 0f;
            while (timer < responseShowDuration)
            {
                float riseAlpha = ButtonResponseEasing(timer / responseShowDuration);
                float colorAlpha = (riseAlpha - responseColorAlphaOffset) / (1 - responseColorAlphaOffset);
                symbol.transform.localPosition = new Vector3(
                    0, 0, Mathf.Lerp(responseRiseStartPosition, responseRiseEndPosition, riseAlpha)
                );
                spriteRenderer.color = Color.Lerp(responseColorStart, responseColorEnd, colorAlpha);
                yield return null;
                timer += Time.deltaTime;
            }
            Destroy(responseObject);
        }

        public void MadeRightChoice()
        {
            StartCoroutine(RightChoiceRoutine());
        }

        IEnumerator RightChoiceRoutine()
        {
            yield return StartCoroutine(ShowButtonResponseRoutine(true));
        }

        public void MadeWrongChoice()
        {
            StartCoroutine(WrongChoiceRoutine());
        }

        IEnumerator WrongChoiceRoutine()
        {
            panel.wrongCooldown = true;
            StartCoroutine(ShowButtonResponseRoutine(false));
            yield return new WaitForSeconds(WRONG_COOLDOWN);
            panel.wrongCooldown = false;
        }

        public bool Push()
        {
            if (!positionInit)
            {
                positionInit = true;
                spawnPositionZ = transform.localPosition.z;
                inPositionZ = spawnPositionZ + buttonMoveOffset;
            }
            if (!panel.canPush)
            {
                return false;
            }
            panel.buttonPushed = true;
            StartCoroutine(PushRoutine());
            return true;
        }

        IEnumerator PushRoutine()
        {
            if (!newPushCall)
            {
                newPushCall = true;
                float startPositionZ = transform.localPosition.z;
                float timer = 0f;
                float pushTime = PUSH_MAX_TIME;
                while (timer < pushTime)
                {
                    float alpha = timer / pushTime;
                    transform.localPosition = new Vector3(
                        transform.localPosition.x,
                        transform.localPosition.y,
                        Mathf.Lerp(startPositionZ, inPositionZ, alpha)
                    );
                    yield return null;
                    timer += Time.deltaTime;
                }
                if (pushTime < PUSH_MAX_TIME)
                {
                    yield return new WaitForSeconds(PUSH_MAX_TIME - pushTime);
                }
                panel.buttonPushed = false;
                newPushCall = false;
                timer = 0f;
                while (timer < PUSH_MAX_TIME && !newPushCall)
                {
                    float alpha = timer / PUSH_MAX_TIME;
                    transform.localPosition = new Vector3(
                        transform.localPosition.x,
                        transform.localPosition.y,
                        Mathf.Lerp(inPositionZ, spawnPositionZ, alpha)
                    );
                    yield return null;
                    timer += Time.deltaTime;
                }
            }
        }

        void UpdateType()
        {
            GetComponent<MeshRenderer>().material = type.material;
            SpriteRenderer symbolRenderer = indicatorSymbol.GetComponent<SpriteRenderer>();
            symbolRenderer.sprite = type.symbol;
            symbolRenderer.color = type.symbolColor;
            indicatorSymbol.transform.localPosition += new Vector3(0, 0, type.symbolOffset);
        }
    }
}