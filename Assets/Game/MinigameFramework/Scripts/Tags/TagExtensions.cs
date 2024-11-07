using UnityEngine;

namespace Game.MinigameFramework.Scripts.Tags {
    public static class TagExtensions
    {
        public static bool HasCustomTag(this GameObject obj, string tag) {
            CustomTag customTag;
            /*if (obj.TryGetComponent(out customTag)) return customTag.HasTag(tag);
            return UnityEditorInternal.InternalEditorUtility.tags.Contains(tag) && obj.CompareTag(tag);*/
            
            return obj.TryGetComponent(out customTag) && customTag.HasTag(tag);
        }
        public static bool HasCustomTag(this Collider collider, string tag) {
            return collider.gameObject.HasCustomTag(tag);
        }
    }
}