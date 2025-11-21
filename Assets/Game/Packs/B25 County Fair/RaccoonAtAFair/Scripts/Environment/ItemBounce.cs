using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaoHuanXiong.Common;


namespace XiaoHuanXiong.Game
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;

        private BoxCollider2D coll;

        public float gravity = -3.5f;

        public float initialHeight = 1.5f;

        public float verticalForce = 4f;

        public float horizontalDamping = 3f;

        private Vector2 _velocity;
        private float _verticalVelocity;


        private bool _isGround;
        private bool _isBouncing;
        public bool canBePickedUp = false;

        private float _distance;
        private Vector2 _direction;
        private Vector3 _targetPosition;

        private Vector3 _groundPosition;



        private GameObject shadow;

        private void Awake()
        {
            coll = GetComponent<BoxCollider2D>();
            spriteTrans = transform.GetChild(0);
            shadow = transform.GetChild(1).gameObject;
            canBePickedUp = false;
            coll.enabled = false;
            _groundPosition = transform.position;
            spriteTrans.position = transform.position;
        }

        private void Update()
        {
            if (_isBouncing)
                _Bounce();
        }

        public void InitBounceItem(Vector2 force)
        {
            //coll.enabled = false;
            coll.enabled = true;
            _groundPosition = transform.position;

            spriteTrans.position = transform.position + Vector3.up * initialHeight;

            _velocity = force;
            _verticalVelocity = verticalForce;

            _isBouncing = true;
            _isGround = false;

            shadow.SetActiveIfNecessary(true);
            shadow.transform.position = _groundPosition;

            //Debug.Log ($"InitBounceItem force:{force} velocity:{_velocity} verticalVelocity:{_verticalVelocity}");
        }

        private void _Bounce()
        {
            if (_velocity.sqrMagnitude > 0.01f)
            {
                // 位移
                var deltaPosition = _velocity * Time.deltaTime;
                transform.position += (Vector3)deltaPosition;

                _groundPosition = transform.position;

                Vector2 damping = _velocity.normalized * horizontalDamping * Time.deltaTime;

                if (damping.sqrMagnitude < _velocity.sqrMagnitude)
                {
                    _velocity -= damping;
                }
                else
                {
                    _velocity = Vector2.zero;
                }

                shadow.transform.position = _groundPosition;
            }
            else
            {
                _velocity = Vector2.zero;
            }

            // 垂直方向
            _verticalVelocity += gravity * Time.deltaTime;
            spriteTrans.position += Vector3.up * _verticalVelocity * Time.deltaTime;

            if (spriteTrans.position.y <= transform.position.y)
            {
                spriteTrans.position = transform.position;
                _OnLand();
            }
        }


        private void _OnLand()
        {
            _isGround = true;
            _isBouncing = false;
            coll.enabled = true;
            canBePickedUp = true;
            shadow.SetActiveIfNecessary(false);
        }
        private void TryBounce(Collider2D other)
        {
            if (!_isBouncing) return;
            if (other.gameObject.layer != 20) return;
         

            BounceOffWall(other);
        }
        private void BounceOffWall(Collider2D wall)
        {
            Vector2 hitPoint = wall.ClosestPoint(transform.position);
            Vector2 normal = ((Vector2)transform.position - hitPoint).normalized;

            _velocity = Vector2.Reflect(_velocity, normal);

            _velocity *= 0.8f;
        }
        /*private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isBouncing) return;
            if (other.gameObject.layer != 20) return;
           
          
            Vector2 hitNormal = (new Vector2(transform.position.x, transform.position.y) - other.ClosestPoint(transform.position)).normalized;

            _velocity = Vector2.Reflect(_velocity, hitNormal);
            _velocity *= 0.8f;

            _groundPosition = transform.position;
            shadow.transform.position = _groundPosition;
        }*/
        private void OnTriggerEnter2D(Collider2D other)
        {
            TryBounce(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TryBounce(other);   // ensures corner bouncing
        }
    }

}
