using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XiaoHuanXiong.Player;

namespace XiaoHuanXiong.Game
{
    [RequireComponent(typeof(Collider2D))]
    public class ResourceItem : MonoBehaviour
    {
        [SerializeField]
        private int resourceValue = 1;
        [SerializeField]
        private ItemBounce itemBounce;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RaccoonPawn>() != null&&itemBounce.canBePickedUp)
            {
                collision.GetComponent<RaccoonPawn>().AddScore(resourceValue);
                // TODO: Play collection audio or visual effect here (maybe not)

                // Destroy the resource item after collection
                Destroy(gameObject);
            }
        }
    }
}