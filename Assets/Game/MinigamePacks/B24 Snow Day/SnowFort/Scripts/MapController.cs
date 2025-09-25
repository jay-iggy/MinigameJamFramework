using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public class MapController : MonoBehaviour
    {
        public static MapController active;

        [Header("Timing")]
        public float distributionTime = 3;
        public float firstBuildTime = 60;
        public float buildTime = 40;
        public float countdownTime = 3;
        public float fightTime = 60;

        [Header("Building")]
        [Header("Left Team Bounds")]
        public Vector2Int boundsLtl = new Vector2Int(-3, 1);
        public Vector2Int boundsLbr = new Vector2Int(-1, -1);

        [Header("Right Team Bounds")]
        public Vector2Int boundsRtl = new Vector2Int(1, 1);
        public Vector2Int boundsRbr = new Vector2Int(3, -1);

        [Space]
        public LayerMask placementCheckMask;

        [Header ("Building Stock")]
        public List<Stock> stock;
        public List<Stock> restock;
        public List<Stock> bonus;

        void Start()
        {
            active = this;
            FindObjectOfType<GameManager>().StartGame();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(((float)boundsLtl.x + (float)boundsLbr.x) / 2, ((float)boundsLbr.y + (float)boundsLtl.y) / 2, 1f),
                new Vector3(boundsLbr.x - boundsLtl.x, boundsLtl.y - boundsLbr.y));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(((float)boundsRtl.x + (float)boundsRbr.x) / 2, ((float)boundsRbr.y + (float)boundsRtl.y) / 2, 1f),
                new Vector3(boundsRbr.x - boundsRtl.x, boundsRtl.y - boundsRbr.y));
        }
    }

    [System.Serializable]
    public struct Stock
    {
        public BuildingObject obj;
        public int count;
    }
}