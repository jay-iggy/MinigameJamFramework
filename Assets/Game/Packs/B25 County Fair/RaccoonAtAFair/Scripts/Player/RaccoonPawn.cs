using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.MinigameFramework.Scripts.Framework.Input;
using XiaoHuanXiong.Game;
using XiaoHuanXiong.Common;


namespace XiaoHuanXiong.Player
{
    public class RaccoonPawn : Pawn
    {
        public static bool isPawnInputEnabled = true;

        private Rigidbody2D _rb;

        public Action OnScoreChanged;

        [Header("Player Stats")]
        private int _score = 0;


        public int Score
        {
            get { return _score; }
            private set
            {
                _score = value;
                //Debug.Log($"Player {playerIndex} Score updated to {_score}");
                OnScoreChanged?.Invoke();
            }
        }

        [Header("Player Movement")]
        [SerializeField]
        private float _speed;
        [SerializeField]
        private bool _isMoving = false;



        [Header("Player Attack")]
        [SerializeField]
        private int _attackCost = 1;
        [SerializeField]
        private float _attackInterval;

        [SerializeField]
        private float _attackDuration;

        [SerializeField]
        private int _minDamage;

        [SerializeField]
        private int _maxDamage;

        [SerializeField]
        private float _onHitInvincibleDuration;

        [SerializeField]
        private Vector2 _onHitItemSpawnRadius;

        [SerializeField]
        private float _onHitSpawnItemForce = 5.0f;

        [SerializeField]
        private bool _isAttacking;

        [SerializeField]
        private bool _isInvincible;

        [SerializeField]
        private Vector2 _attackDirection;


        [SerializeField]
        private List<ItemThrow> _attackItemPrefabs;

        [SerializeField]
        private Transform _attackItemSpawnTransform;

        private ItemThrow _currentAttackItem;

        [Header("Player Search")]
        [SerializeField]
        private float _searchDuration = 2.0f;
        [SerializeField]
        private GameObject _searchInteractiveButton;


        [Header("Player Animation")]
        public Animator animator;
        // 0 = small, 1 = mid, 2 = big
        [SerializeField]
        private AnimatorOverrideController[] _raccoonAnimatorOverrideControllers;

        [Header("Player Bag Level")]
        [SerializeField]
        private int _midBagThreshold = 6;
        [SerializeField]
        private int _bigBagThreshold = 12;
        // current bag size
        private int _currentBagLevel = -1;

        [Header("Audio Source")]
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _getItemAudioClip;

        [SerializeField]
        private AudioClip _attackReleaseAudioClip;

        [SerializeField]
        private AudioClip _searchAudioClip;

        [SerializeField]
        private AudioClip _onHitAudioClip;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private ResourcePoint _currentResourcePoint;
        private Coroutine _attackingCoroutine;
        private Coroutine _searchingCoroutine;

        [SerializeField]
        private Vector2 _moveInput;

        // Facing direction: 1 = right, -1 = left
        [SerializeField]
        private int _faceDirection;

        [SerializeField]
        private bool _isPressingMoveButton;

        public Collider2D playerFeet;

        [SerializeField]
        private PlayerPermission _playerPermission;
        private IDisposable _attackLock;
        private IDisposable _moveLock;
        private IDisposable _searchLock;


        #region Unity Lifecycle
        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (_playerPermission == null)
            {
                _playerPermission = GetComponent<PlayerPermission>();
            }
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _UpdateBagLevel();
        }

        private void OnEnable()
        {
            OnScoreChanged += _UpdateBagLevel;
        }

        private void OnDisable()
        {
            OnScoreChanged -= _UpdateBagLevel;
        }

        private void Update()
        {
            _PlayerMoveInput();

            _SwitchAnimation();
        }

        private void FixedUpdate()
        {
            _PlayerMovement();
        }
        #endregion

        private void _UpdateBagLevel()
        {
            //Debug.Log($"Player {_score} bag level check");
            if (_score >= _bigBagThreshold)
            {
                if (_currentBagLevel != 2)
                {
                    _currentBagLevel = 2;
                    animator.runtimeAnimatorController = _raccoonAnimatorOverrideControllers[2];
                }
            }
            else if (_score >= _midBagThreshold)
            {
                if (_currentBagLevel != 1)
                {
                    _currentBagLevel = 1;
                    animator.runtimeAnimatorController = _raccoonAnimatorOverrideControllers[1];
                }
            }
            else
            {
                if (_currentBagLevel != 0)
                {
                    _currentBagLevel = 0;
                    animator.runtimeAnimatorController = _raccoonAnimatorOverrideControllers[0];
                }
            }
        }

        public void AddScore(int amount)
        {
            Score += amount;
            _audioSource.PlayOneShot(_getItemAudioClip);
        }

        public void UpdateResourcePoint(ResourcePoint resourcePoint)
        {
            _currentResourcePoint = resourcePoint;
        }

        public void OnHit(Vector2 attackDirection)
        {
            if (_isInvincible == true)
            {
                return;
            }

            _audioSource.PlayOneShot(_onHitAudioClip);

            int damage = UnityEngine.Random.Range(_minDamage, _maxDamage + 1);
            damage = Mathf.Min(damage, Score);
            Debug.Log($"Player {playerIndex} hit for {damage} damage");

            Score -= damage;

            // TODO: 生成掉落物
            var spawnPos = new Vector3(transform.position.x + UnityEngine.Random.Range(-_onHitItemSpawnRadius.x, _onHitItemSpawnRadius.x),
                     transform.position.y + UnityEngine.Random.Range(-_onHitItemSpawnRadius.y, _onHitItemSpawnRadius.y), 0);
            for (int i = 0; i < damage; i++)
            {
                var item = Instantiate(
                ResourceItemManager.Instance.resources[UnityEngine.Random.Range(0, ResourceItemManager.Instance.resources.SafeCount())],
                spawnPos,
                Quaternion.identity,
                ResourceItemManager.Instance.transform
                );

                var direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0).normalized;

                item.GetComponent<ItemBounce>().InitBounceItem(direction * _onHitSpawnItemForce);
            }

            _SwitchOnHitAnimation(attackDirection);

            if (_attackingCoroutine != null)
            {
                StopCoroutine(_attackingCoroutine);
                _attackingCoroutine = null;
            }
            if (_searchingCoroutine != null)
            {
                StopCoroutine(_searchingCoroutine);
                _searchingCoroutine = null;
            }

            StartCoroutine(_OnHitLockRoutine());
        }

        private void _SearchResourcePoint()
        {
            if (_playerPermission.CanPerformAction(PlayerAction.Search))
            {
                if (_currentResourcePoint != null && _currentResourcePoint.IsExplorable && _searchingCoroutine == null)
                {
                    _audioSource.PlayOneShot(_searchAudioClip);
                    _searchingCoroutine = StartCoroutine(_SearchingRoutine());
                }
            }
        }

        private void _OnMoveToPosition(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }



        private void _ResetPlayerMoveState()
        {
            _rb.velocity = Vector2.zero;
            _isMoving = false;
        }

        private void _PlayerAttackStartInput()
        {
            if (_playerPermission.CanPerformAction(PlayerAction.Attack))
            {
                // entrance of attack
                if (_isAttacking == false && Score > 0)
                {
                    _isAttacking = true;
                    if (_attackDirection != Vector2.zero)
                    {
                        _attackDirection.Normalize();
                    }

                    _isMoving = false;

                    _moveLock?.Dispose();
                    _moveLock = _playerPermission.ScopedLock(PlayerAction.Move);
                    _attackLock?.Dispose();
                    _attackLock = _playerPermission.ScopedLock(PlayerAction.Attack);
                    _searchLock?.Dispose();
                    _searchLock = _playerPermission.ScopedLock(PlayerAction.Search);

                    Score -= _attackCost;
                    Score = Mathf.Max(Score, 0);

                    int attackItemIndex = UnityEngine.Random.Range(0, _attackItemPrefabs.SafeCount());

                    // Spawn attack item
                    _currentAttackItem = Instantiate(_attackItemPrefabs[attackItemIndex], _attackItemSpawnTransform.position, Quaternion.identity);

                    _SwitchAttackStartAnimation();

                    _isAttacking = true;
                }
            }
        }

        private void _PlayerAttackReleaseInput()
        {
            if (_isAttacking == true)
            {
                _isAttacking = false;

                // 扔出投掷物
                
                _currentAttackItem?.InitThrowingItem(_attackDirection == Vector2.zero ? new Vector2(_faceDirection, 0) : _attackDirection, playerIndex);

                _audioSource.PlayOneShot(_attackReleaseAudioClip);

                _SwitchAttackReleaseAnimation();

                _attackingCoroutine = StartCoroutine(_AttackLockRoutine());
            }
        }

        /// <summary>
        /// Handle Player's input
        /// </summary>
        private void _PlayerMoveInput()
        {
            if (_playerPermission.CanPerformAction(PlayerAction.Move) && _isPressingMoveButton == true)
            {
                _isMoving = _moveInput == Vector2.zero ? false : (_isAttacking == true ? false : true);
            }
        }

        /// <summary>
        /// The actual execution of the Player's movement is done through moveInput data
        /// </summary>
        private void _PlayerMovement()
        {
            if (_playerPermission.CanPerformAction(PlayerAction.Move))
            {
                if (_isAttacking == true)
                {
                    return;
                }
                else
                {

                    if (_isMoving == false)
                    {
                        _rb.velocity = Vector2.zero;
                    }
                    else
                    {
                        _rb.velocity = _moveInput * _speed;
                    }
                }
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }

        }

        private void _SwitchAnimation()
        {
            if (_playerPermission.CanPerformAction(PlayerAction.Move) == false)
            {
                //animator.SetFloat("InputX", 0);
                //animator.SetFloat("InputY", 0);
                animator.SetBool("IsMoving", false);
                
            }

            if (_isMoving == true)
            {
                if (_moveInput.x != 0)
                {
                    animator.SetFloat("InputX", _moveInput.x);
                }

                animator.SetFloat("InputY", _moveInput.y);

            }
            else if (_isAttacking)
            {
                animator.SetFloat("AttackX", _faceDirection);
                Debug.Log($"AttackX {_faceDirection}");
            }

            animator.SetBool("IsMoving", _isMoving);

        }

        private void _SwitchOnHitAnimation(Vector2 attackDirection)
        {
            animator.SetTrigger("OnHit");
            if (attackDirection.x > 0)
            {
                animator.SetFloat("OnHitX", -1);
            }
            else if (attackDirection.x < 0)
            {
                animator.SetFloat("OnHitX", 1);
            }
            else
            {
                animator.SetFloat("OnHitX", 0);
            }
        }

        private void _SwitchAttackStartAnimation()
        {
            animator.SetFloat("AttackX", _faceDirection);
            animator.SetFloat("AttackY", _attackDirection.y);
            animator.SetFloat("InputX", _faceDirection);
            animator.SetFloat("InputY", _attackDirection.y);
            animator.SetBool("AttackRelease", false);
            animator.SetTrigger("AttackHold");
        }

        private void _SwitchAttackReleaseAnimation()
        {
            animator.SetFloat("AttackX", _faceDirection);
            animator.SetFloat("AttackY", _attackDirection.y);
            animator.SetFloat("InputX", _faceDirection);
            animator.SetFloat("InputY", _attackDirection.y);
            animator.SetBool("AttackRelease", true);
        }

        #region Animation Events
        /// <summary>
        /// Animation Event: called when attack animation is over
        /// </summary>

        public void AttackOver()
        {
            _isAttacking = false;
            _moveLock?.Dispose();
            _moveLock = null;
        }
        #endregion

        protected override void OnActionPressed(InputAction.CallbackContext context)
        {
            if (!isPawnInputEnabled) return;

            // Move
            if (context.action.name == PawnAction.Move)
            {
                _isPressingMoveButton = true;

                if (context.ReadValue<Vector2>() != Vector2.zero)
                {
                    _moveInput = context.ReadValue<Vector2>();
                    _attackDirection = _moveInput;
                    if (_moveInput.x != 0)
                    {
                        _faceDirection = _moveInput.x > 0 ? 1 : -1;
                    }
                }
                else
                {
                    _isMoving = false;
                    _isPressingMoveButton = false;
                }

                //Debug.Log($"Move Released direction {context.ReadValue<Vector2>()}");
            }
            // Attack
            else if (context.action.name == PawnAction.ButtonA)
            {
                _PlayerAttackStartInput();
            }
            else if (context.action.name == PawnAction.ButtonB)
            {
                _SearchResourcePoint();
            }


        }

        protected override void OnActionReleased(InputAction.CallbackContext context)
        {
            if (context.action.name == PawnAction.ButtonA)
            {
                _PlayerAttackReleaseInput();
            }
        }

        private IEnumerator _AttackLockRoutine()
        {
            var time = _attackInterval;

            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            _searchLock?.Dispose();
            _searchLock = null;
            _moveLock?.Dispose();
            _moveLock = null;
            _attackLock?.Dispose();
            _attackLock = null;

            _attackingCoroutine = null;
        }

        private IEnumerator _OnHitLockRoutine()
        {
            var time = _onHitInvincibleDuration;
            _isInvincible = true;
            _moveLock?.Dispose();
            _moveLock = _playerPermission.ScopedLock(PlayerAction.Move);
            _attackLock?.Dispose();
            _attackLock = _playerPermission.ScopedLock(PlayerAction.Attack);
            _searchLock?.Dispose();
            _searchLock = _playerPermission.ScopedLock(PlayerAction.Search);

            bool visible = true;
            while (time > 0)
            {
                if (_spriteRenderer != null)
                {
                    visible = !visible;
                    _spriteRenderer.enabled = visible;
                }

                
                yield return new WaitForSeconds(0.2f);
                time -= 0.2f;
            }
            _spriteRenderer.enabled = true;
            _isInvincible = false;
            _searchLock?.Dispose();
            _searchLock = null;
            _moveLock?.Dispose();
            _moveLock = null;
            _attackLock?.Dispose();
            _attackLock = null;
        }

        private IEnumerator _SearchingRoutine()
        {
            _searchLock?.Dispose();
            _searchLock = _playerPermission.ScopedLock(PlayerAction.Search);
            _moveLock?.Dispose();
            _moveLock = _playerPermission.ScopedLock(PlayerAction.Move);
            _attackLock?.Dispose();
            _attackLock = _playerPermission.ScopedLock(PlayerAction.Attack);
            // Start searching animation
            animator.SetTrigger("Search");

            yield return new WaitForSeconds(_searchDuration);

            if (_currentResourcePoint != null)
            {
                _currentResourcePoint.SetToEmptyState();
                _currentResourcePoint.GenerateResourceInScene();
            }

            _searchLock?.Dispose();
            _searchLock = null;
            _moveLock?.Dispose();
            _moveLock = null;
            _attackLock?.Dispose();
            _attackLock = null;

            _searchingCoroutine = null;
        }
        public void ActaiveSearchButton() {
            _searchInteractiveButton.SetActive(true);
        }
        public void DeactivaeSearchButton() {
            _searchInteractiveButton.SetActive(false);
        }
    }

}
