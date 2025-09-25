using UnityEngine;

namespace SnowDay.SnowFight {
    public class Billboard : MonoBehaviour {
        public Camera mainCamera;

        private void LateUpdate() {
            // Make the object face the camera
            if (mainCamera != null) {
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                    mainCamera.transform.rotation * Vector3.up);
            }
        }
    }
}