using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowfort
{
    public interface BuildingOverride
    {
        public bool OverrideExists(BuildingObject obj);
        public bool CanOverride(BuildingObject obj);
        public BuildingOverrideData OverrideData(BuildingObject obj);
        public void BuildOverride(BuildingObject obj);
    }

    public struct BuildingOverrideData
    {
        public GameObject preview;
        public bool disablePreviewFlip;

        public BuildingOverrideData(GameObject previewObject, bool disableFlip)
        {
            preview = previewObject;
            disablePreviewFlip = disableFlip;
        }
    }
}
