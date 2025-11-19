using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

namespace Starter.PumpkinPlunge {
    public class ButtonPanel : MonoBehaviour {
        [Header("References")]
        public PumpkinPlungeSubmanager submanager;
        [SerializeField] private GameObject panel;
        public GameObject indicator;
        [SerializeField] private GameObject buttonPrefab;
        [Header("Parameters")]
        [SerializeField] private float panelSizeXPercent = 0.5f;
        [SerializeField] private float panelSizeYPercent = 0.25f;
        [SerializeField] private float panelSizeZ = 40;
        [SerializeField] private float buttonScaleDiameterFactor = 0.6f;
        [SerializeField] private float buttonOffsetYPercent = 0.1f;
        [SerializeField] private float buttonScaleThicknessFactor = 0.2f;
        [SerializeField] private float buttonIndicatorOffsetPercent = 0.1f;
        [SerializeField] private float panelSizePaddingFactor = 0.04f;
        // [SerializeField] private float panelButtonsPaddingX = 10;
        // [SerializeField] private float panelButtonsPaddingY = 10;
        // [SerializeField] private float panelExtraSizeX = 0;
        // [SerializeField] private float panelExtraSizeY = 10;
        // private Vector3 buttonSize;
        [SerializeField] private float panelHeightShowPercent = 1f;
        [SerializeField] private float panelHiddenOffsetFactor = 1.25f;
        [SerializeField] private float panelMoveVelocity = 15;
        [SerializeField] private float panelMoveTime = 0.25f;
        [SerializeField] private float indicatorMoveVelocity = 1;
        [SerializeField] private float indicatorMoveTime = 0.5f;

        private float canvasWidth;
        private float canvasHeight;
        private float materialFactor = 0.5f;
        
        [HideInInspector] public bool buttonPushed = false;
        [HideInInspector] public bool wrongCooldown = false;
        [HideInInspector] public bool canPush => !buttonPushed && !wrongCooldown;

        public static bool canUsePanel = false;
        private int selectedTypeIndex;
        private TrapdoorType selectedTrapdoorType => PumpkinPlungeManager.instance.trapdoorTypes[selectedTypeIndex];
        private Button selectedButton => buttons[selectedTrapdoorType];
        private Dictionary<TrapdoorType, Button> buttons = new();
        private bool updateIndicatorPosition;
        private Vector3 targetIndicatorPosition;
        private Vector3 indicatorVelocity;
        private bool updatePanelPosition;
        private Vector3 targetPanelPosition;
        private Vector3 panelVelocity;
        private bool allowPanelInputs => canUsePanel && isPanelShown;
        private bool isPanelShown = false;
        private Quaternion buttonRotate;

        void Start()
        {
            // Make buttons
            int typeCount = PumpkinPlungeManager.instance.trapdoorTypes.Count;
            for (int i = 0; i < typeCount; i++)
            {
                TrapdoorType type = PumpkinPlungeManager.instance.trapdoorTypes[i];
                Button button = Instantiate(buttonPrefab, gameObject.transform).GetComponent<Button>();
                if (i == 0)
                {
                    buttonRotate = button.transform.rotation;
                }
                button.panel = this;
                button.type = type;
                buttons.Add(type, button);
            }

            // Update scale of panel
            UpdatePanelScale();

            // Toggle panel if game started
            isPanelShown = PumpkinPlungeManager.gameStarted;
            updatePanelPosition = false;
            SetPanelPosition(GetPanelPosition());
            indicator.SetActive(true);
        }

        void Update()
        {
            if (updatePanelPosition)
            {
                // Move indicator to target
                Vector3 startPosition = panel.transform.localPosition;
                Vector3 newPosition = Vector3.SmoothDamp(
                    startPosition, targetPanelPosition, ref panelVelocity, panelMoveTime
                );
                SetPanelPosition(newPosition);
                if ((newPosition - startPosition).magnitude <= PumpkinPlungeManager.STOP_MARGIN)
                {
                    SetPanelPosition(targetPanelPosition);
                    updatePanelPosition = false;
                }
            }
            else if (updateIndicatorPosition)
            {
                // Move indicator to target
                Vector3 startPosition = indicator.transform.localPosition;
                Vector3 newPosition = Vector3.SmoothDamp(
                    startPosition, targetIndicatorPosition, ref indicatorVelocity, indicatorMoveTime
                );
                indicator.transform.localPosition = newPosition;
                if ((newPosition - startPosition).magnitude <= PumpkinPlungeManager.STOP_MARGIN)
                {
                    indicator.transform.localPosition = targetIndicatorPosition;
                    updateIndicatorPosition = false;
                }
            }
        }

        void UpdateButtons()
        {
            Vector3 panelPos = panel.transform.position;
            Vector3 panelScale = panel.transform.localScale;
            float diameter = panelScale.y * buttonScaleDiameterFactor;
            float thickness = panelScale.z * buttonScaleThicknessFactor;
            float offsetY = panelScale.y * buttonOffsetYPercent;
            float panelButtonStartX = (-0.5f + panelSizePaddingFactor) * panelScale.x;
            float panelButtonEndX = (0.5f - panelSizePaddingFactor) * panelScale.x;
            var values = buttons.Values;
            float padding = Math.Max(0, (panelButtonEndX - panelButtonStartX - (diameter * values.Count)) / values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                Button button = values.ElementAt(i);
                button.transform.position = panelPos;
                button.transform.localScale = new(diameter, thickness, diameter);
                float posX = panelButtonStartX + (0.5f + i) *(diameter + padding);
                button.transform.localPosition =
                    panel.transform.localPosition + (panel.transform.up * offsetY) +
                    (button.transform.up * (0.5f * (panelScale.z - thickness) + thickness));
                button.transform.localPosition = new(
                    posX, button.transform.localPosition.y, button.transform.localPosition.z
                );
                button.transform.rotation = panel.transform.rotation;
                button.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            }
            indicator.transform.rotation = selectedButton.transform.rotation;
            indicator.transform.localScale = selectedButton.transform.localScale;
            indicator.transform.localPosition = GetIndicatorPosition();
        }

        void UpdatePanelScale()
        {
            Rect canvasRect = GetComponent<RectTransform>().rect;
            canvasWidth = canvasRect.width;
            canvasHeight = canvasRect.height;
            panel.transform.localScale = new(
                canvasWidth * panelSizeXPercent,
                canvasHeight * panelSizeYPercent,
                panelSizeZ
            );
            Material material = panel.GetComponent<Renderer>().material;
            material.mainTextureScale = materialFactor * new Vector2(
                1, canvasHeight * panel.transform.localScale.y / (canvasWidth * panel.transform.localScale.x)
            );
            panel.transform.rotation = Quaternion.Euler(15, 0, 0);
            UpdateButtons();
        }

        private void SetPanelPosition(Vector3 newPosition)
        {
            panel.transform.localPosition = newPosition;
            UpdateButtons();
        }

        private Vector3 GetPanelPosition()
        {
            float height = panel.transform.localScale.y;
            return new Vector3(
                panel.transform.localPosition.x,
                0.5f * (-canvasHeight + (height * panelHeightShowPercent)) + (
                    isPanelShown ? 0 : -height * panelHiddenOffsetFactor
                ),
                panel.transform.localPosition.z
            );
        }

        void UpdatePanelPosition()
        {
            targetPanelPosition = GetPanelPosition();
            panelVelocity = panelMoveVelocity * (targetPanelPosition - panel.transform.localPosition).normalized;
            updatePanelPosition = true;
        }

        public void Show()
        {
            isPanelShown = true;
            UpdatePanelPosition();
        }

        public void Hide()
        {
            isPanelShown = false;
            updateIndicatorPosition = false;
            UpdatePanelPosition();
        }

        private Vector3 GetIndicatorPosition()
        {
            return selectedButton.transform.localPosition +
                (panel.transform.up * -(selectedButton.transform.localScale.z * (1 + buttonIndicatorOffsetPercent)))
                + (-selectedButton.transform.up * selectedButton.transform.localScale.y);
        }
        
        void UpdateIndicator()
        {
            targetIndicatorPosition = GetIndicatorPosition();
            indicatorVelocity = indicatorMoveVelocity * (targetIndicatorPosition - indicator.transform.localPosition).normalized;
            updateIndicatorPosition = true;
        }

        public void MoveLeft()
        {
            if (!allowPanelInputs || wrongCooldown)
            {
                return;
            }
            if (selectedTypeIndex > 0)
            {
                selectedTypeIndex -= 1;
            }
            UpdateIndicator();
        }

        public void MoveRight()
        {
            if (!allowPanelInputs || wrongCooldown)
            {
                return;
            }
            if (selectedTypeIndex < PumpkinPlungeManager.instance.trapdoorTypes.Count - 1)
            {
                selectedTypeIndex += 1;
            }
            UpdateIndicator();
        }

        public void Activate()
        {
            if (!allowPanelInputs)
            {
                return;
            }
            if (selectedButton.Push())
            {
                bool opened = submanager.TryOpenTrapdoor(selectedTrapdoorType);
                if (!opened)
                {
                    // Wrong trapdoor!
                    selectedButton.MadeWrongChoice();
                }
                else
                {
                    // Right trapdoor!
                    selectedButton.MadeRightChoice();
                }
            }
        }
    }
}