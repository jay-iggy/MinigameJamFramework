using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaoHuanXiong.Player;

namespace XiaoHuanXiong.Game
{
    public class ItemThrow : MonoBehaviour
    {
        [SerializeField]
        private int _itemValue = 1;

        [SerializeField]
        private float _throwForce = 5f;

        [SerializeField]
        private float _angularVelocity = 30f;

        [SerializeField]
        private float _maxThrowDistance = 10f;

        private Rigidbody2D _rb;

        private Collider2D _collider;

        private Vector2 _throwDirection;
        private Vector2 _startPosition;

        [SerializeField]
        private bool _isFlying;

        private bool _isInitialized = false;

        private int _playerIndex;

        [Header("Audio")]
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _hitObjectSound;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _audioSource = GetComponent<AudioSource>();
            if (_rb == null)
            {
                Debug.LogError("Rigidbody2D component is missing.");
            }
            if (_collider == null)
            {
                Debug.LogError("Collider2D component is missing.");
            }
            if (_audioSource == null)
            {
                Debug.LogError("AudioSource component is missing.");
            }
        }

        private void FixedUpdate()
        {
            if (!_isFlying)
            {
                return;
            }
            float traveled = Vector2.Distance(_startPosition, _rb.position);
            if (traveled >= _maxThrowDistance)
            {
                _StopFlying();
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RaccoonPawn>() != null)
            {
                if (_isInitialized && !_isFlying)
                {
                    collision.GetComponent<RaccoonPawn>().AddScore(_itemValue);
                    Destroy(gameObject);
                }
                else if (_isFlying && collision.GetComponent<RaccoonPawn>().playerIndex != _playerIndex)
                {
                    collision.GetComponent<RaccoonPawn>().OnHit(_throwDirection);
                    Destroy(gameObject);
                }

                
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<RaccoonPawn>() != null)
            {
                _collider.isTrigger = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<RaccoonPawn>() == null)
            {
                if (_isFlying)
                {
                    _audioSource.PlayOneShot(_hitObjectSound);
                    _StopFlying();
                }
            }
        }

        public void InitThrowingItem(Vector2 direction, int playerIndex)
        {
            _isInitialized = true;

            _throwDirection = direction.normalized;
            _startPosition = _rb.position;

            _rb.velocity = _throwDirection * _throwForce;
            _rb.angularVelocity = _angularVelocity;

            _playerIndex = playerIndex;

            _isFlying = true;
        }

        private void _StopFlying()
        {
            _isFlying = false;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.freezeRotation = true;
            _collider.isTrigger = true;
        }


    }
}

