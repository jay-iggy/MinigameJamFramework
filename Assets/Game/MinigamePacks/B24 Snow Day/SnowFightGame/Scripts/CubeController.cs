using UnityEngine;

namespace SnowFight {
    public class CubeController : MonoBehaviour {
        // Movement speed
        public float moveSpeed = 15f;
        public float sensitivityX = 1f; // Sensitivity for horizontal (yaw) rotation
        public float sensitivityY = 1f; // Sensitivity for vertical (pitch) rotation

        private float rotationX = 0f; // To track the current horizontal rotation
        private float rotationY = 0f; // To track the current vertical rotation

        // Update is called once per frame
        void Update() {
            // Get input from the game controller's left joystick
            float horizontalInput1 = Input.GetAxis("Horizontal"); // Left Stick X axis (left/right)
            float verticalInput1 = Input.GetAxis("Vertical"); // Left Stick Y axis (up/down)

            // Calculate the movement direction
            Vector3 movement = new Vector3(horizontalInput1, 0, verticalInput1);

            // Move the cube using the controller input
            transform.Translate(movement * moveSpeed * Time.deltaTime);






            // Get input from the right joystick
            float horizontalInput = Input.GetAxis("Camera X"); // Axis for right stick X (yaw)
            float verticalInput = Input.GetAxis("Camera Y"); // Axis for right stick Y (pitch)

            // Update rotation angles
            rotationX += horizontalInput * sensitivityX; // Adjust horizontal rotation (yaw)
            rotationY -= verticalInput * sensitivityY; // Adjust vertical rotation (pitch)

            // Clamp vertical rotation to prevent flipping
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            // Apply rotation to the camera
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0f);







        }
    }
}

