using Snowfort;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowfort {
    public class BuildTester : MonoBehaviour
    {
        public BuildingObject toAdd;
        public int count;

        void Start()
        {
            transform.GetChild(0).GetComponent<PlacementCursor>().AddObject(toAdd, count);
        }
    }
}