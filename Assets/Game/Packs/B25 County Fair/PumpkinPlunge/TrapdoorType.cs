using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starter.PumpkinPlunge {
    [CreateAssetMenu(fileName = "TrapdoorType", menuName = "Starter/TrapdoorType")]
    public class TrapdoorType : ScriptableObject {
        [Header("Properties")]
        public Material material;
        public Sprite symbol;
        public Color symbolColor;
        public float symbolOffset = 0;
    }
}