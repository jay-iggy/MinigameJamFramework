using UnityEngine;

namespace XiaoHuanXiong.Common
{
    public static class GameObjectExtensions
    {
        public static void SetActiveIfNecessary(this GameObject obj, bool active)
        {
            if (obj == null) return;
            if (obj.activeSelf != active)
            {
                obj.SetActive(active);
            }
            return;
        }
    }
}