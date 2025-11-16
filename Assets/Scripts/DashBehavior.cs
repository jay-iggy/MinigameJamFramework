using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame{
    public class DashBehavior : MonoBehaviour
    {
        public float dashCooldown;
        private float cooldownTimer;

        public float dashTime;
        public float dashForce;
        private float dashTimer;

        public float knockbackForce;
        public float knockbackLift;
        public float knockbackRecoveryTime;

        private ScarecrowPawn pawn;
        public LayerMask playerMask;

        private Vector3 dashDir;
        private Rigidbody rb;
        private float olddrag;

        private void Awake()
        {
            rb = transform.parent.GetComponent<Rigidbody>();
            olddrag = rb.drag;
            pawn = transform.parent.GetComponent<ScarecrowPawn>();
        }
        public void Dash(Vector2 dir)
        {
            if (cooldownTimer > 0) return;
            dashDir = new Vector3(dir.x, 0f, dir.y);
            dashTimer = dashTime;
            pawn.inactiveTimer = dashTime;
            GetComponent<BoxCollider>().enabled = true;
        }

        public void StopDash()
        {
            dashTimer = 0;
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            pawn.inactiveTimer = 0;
            GetComponent<BoxCollider>().enabled = false;
            cooldownTimer = dashCooldown;
        }

        public void Update()
        {
            if(cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }

            if (dashTimer > 0)
            {
                rb.velocity = dashDir * dashForce;
                dashTimer -= Time.deltaTime;
                if(dashTimer <= 0)
                {
                    StopDash();
                }
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            GameObject obj = collider.gameObject;
            // check if dashing into another player
            if((dashTimer >= 0) && ((playerMask & (1 << obj.layer)) != 0))
            {
                // stun the punched player
                obj.GetComponent<ScarecrowPawn>().inactiveTimer = knockbackRecoveryTime;

                // remove the player's momentum
                obj.GetComponent<Rigidbody>().velocity = Vector3.zero;

                // calculate the direction to apply the force (away from the punching player)
                float rot = (rb.rotation.eulerAngles.y);
                // add the impulse
                Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * rot), knockbackLift, Mathf.Cos(Mathf.Deg2Rad * rot));
                // add the impulse
                obj.GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
                // make the player drop the potato
                obj.gameObject.GetComponentInChildren<CatchBehavior>().Drop(); // TODO: this reference is very brittle and could cause issues

                // finish dashing
                StopDash();
            }
        }


    }
}