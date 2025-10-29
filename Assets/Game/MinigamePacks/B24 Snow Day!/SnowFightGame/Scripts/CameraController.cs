using UnityEngine;

namespace SnowDay.SnowFight {
    public class CameraController : MonoBehaviour {
        public float sensitivityX = 1f; // Sensitivity for horizontal (yaw) rotation
        public float sensitivityY = 1f; // Sensitivity for vertical (pitch) rotation

        private float rotationX = 0f; // To track the current horizontal rotation
        private float rotationY = 0f; // To track the current vertical rotation

        void Update() {
            // Get input from the right joystick
            float horizontalInput = Input.GetAxis("Camera X"); // Axis for right stick X (yaw)
            float verticalInput = Input.GetAxis("Camera Y"); // Axis for right stick Y (pitch)

            // Update rotation angles
            rotationX += horizontalInput * sensitivityX / 10f; // Adjust horizontal rotation (yaw)
            rotationY -= verticalInput * sensitivityY / 10f; // Adjust vertical rotation (pitch)

            // Clamp vertical rotation to prevent flipping
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            // Apply rotation to the camera
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }
    }
}
