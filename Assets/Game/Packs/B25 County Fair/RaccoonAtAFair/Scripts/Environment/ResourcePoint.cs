using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaoHuanXiong.Common;
using XiaoHuanXiong.Player;

namespace XiaoHuanXiong.Game
{
    public class ResourcePoint : MonoBehaviour
    {
        [SerializeField]
        private int _minResourceNum;
        [SerializeField]
        private int _maxResourceNum;

        [SerializeField]
        private float _spawnForce = 5.0f;

        [SerializeField]
        private float _spawnPosOffset = -0.5f;

        [SerializeField]
        private float _restoreTime = 10.0f;

        [Header("Spawn Area")]
        [SerializeField]
        private Vector2 _spawnRadius;

        private SpriteRenderer _spriteRenderer;

        [Header("Resource Point Image")]
        [SerializeField]
        private Sprite _explorableRPSprite;
        [SerializeField]
        private Sprite _emptyRPSprite;
        
        private bool _isExplorable = true;
        public bool IsExplorable
        {
            get { return _isExplorable; }
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer component is missing.");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RaccoonPawn>() != null)
            {
                collision.GetComponent<RaccoonPawn>().ActaiveSearchButton();
                collision.GetComponent<RaccoonPawn>().UpdateResourcePoint(this);
            }
            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<RaccoonPawn>() != null)
            {
                collision.GetComponent<RaccoonPawn>().DeactivaeSearchButton();
                collision.GetComponent<RaccoonPawn>().UpdateResourcePoint(null);
            }
        }

        public void GenerateResourceInScene()
        {
            _playBounce();
            int resourceNum = Random.Range(_minResourceNum, _maxResourceNum + 1);

            for (int i = 0; i < resourceNum; i++)
            {
                var spawnPos = new Vector3(transform.position.x + Random.Range(-_spawnRadius.x, _spawnRadius.x),
                    transform.position.y + _spawnPosOffset + Random.Range(-_spawnRadius.y, _spawnRadius.y), 0);

                var item = Instantiate(
                ResourceItemManager.Instance.resources[Random.Range(0, ResourceItemManager.Instance.resources.SafeCount())],
                spawnPos,
                Quaternion.identity,
                ResourceItemManager.Instance.transform
                );

                var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 0f), 0).normalized;


                item.GetComponent<ItemBounce>().InitBounceItem(direction * _spawnForce);
            }
        }

        public void SetToEmptyState()
        {
            _isExplorable = false;
            _spriteRenderer.sprite = _emptyRPSprite;

            StartCoroutine(RestoreRoutine());
        }

        private IEnumerator RestoreRoutine()
        {
            yield return new WaitForSeconds(_restoreTime);
            _spriteRenderer.sprite = _explorableRPSprite;
            _isExplorable = true;
        }
        private void _playBounce()
        {
            StartCoroutine(BounceRoutine());
        }

        IEnumerator BounceRoutine()
        {
            Vector3 original = transform.localScale;

            float t = 0f;
            float duration = 0.6f;

            while (t < duration)
            {
                float p = t / duration;
                float bounce = OvershootBounce(p); 
                float scale = 1 + bounce;
                transform.localScale = new Vector3(scale, scale, 1);

                t += Time.deltaTime;
                yield return null;
            }

            transform.localScale = original;  
        }

        float OvershootBounce(float x)
        {
            
            // Controls:
            float big = 0.20f;      // first large bounce
            float small = -0.10f;   // shrink bounce
            float settle = 0.05f;   // last tiny bounce

            if (x < 0.3f)
            {
                // Ease-out to big bounce
                float t = x / 0.3f;
                return Mathf.Lerp(0, big, Mathf.Sin(t * Mathf.PI * 0.5f));
            }
            else if (x < 0.6f)
            {
               
                float t = (x - 0.3f) / 0.3f;
                return Mathf.Lerp(big, small, Mathf.Sin(t * Mathf.PI * 0.5f));
            }
            else
            {
                
                float t = (x - 0.6f) / 0.4f;
                return Mathf.Lerp(small, 0, Mathf.Sin(t * Mathf.PI * 0.5f));
            }
        }
    }

}
