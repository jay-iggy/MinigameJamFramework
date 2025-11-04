using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public class FortController : MonoBehaviour
    {
        int controllingIndex;

        bool enabledState;

        public List<Controllable> controllables = new List<Controllable>();

        PlayerController playerController;

        public void Use()
        {
            if (!enabledState || !controllables[controllingIndex].CanUse()) return;

            controllables[controllingIndex].Use();

            if (!controllables[controllingIndex].CanUse())
                Scroll(true);
        }

        public void Scroll(bool up)
        {
            if (!enabledState) return;

            for (int i = 0; i < controllables.Count; i++)
            {
                controllables[controllingIndex].SetActive(false);

                controllingIndex += up ? 1 : -1;
                if (controllingIndex == controllables.Count) controllingIndex -= controllables.Count;
                if (controllingIndex == -1) controllingIndex += controllables.Count;

                if (controllables[controllingIndex].CanUse())
                {
                    controllables[controllingIndex].SetActive(true);
                    return;
                }
            }

            playerController.SetState(PlayerController.PlayerState.OBSERVER);
        }

        public void SetActiveState(bool active)
        {
            enabledState = active;

            if (active)
            {
                controllingIndex = 0;
                Scroll(true);
            }
            else
            {
                if (controllables.Count >0)
                    controllables[controllingIndex].SetActive(false);
            }
        }

        void Start()
        {
            playerController = transform.parent.GetComponent<PlayerController>();
        }
    }
}