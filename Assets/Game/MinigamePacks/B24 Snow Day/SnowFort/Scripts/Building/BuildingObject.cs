using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    [CreateAssetMenu(menuName = "BuildingObject", fileName = "Building Object")]
    public class BuildingObject : ScriptableObject
    {
        public GameObject preview;
        public GameObject gameobject;
        public Vector2Int size = new Vector2Int(1, 1);
    }
}
