using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public interface Controllable
    {
        public void Use();
        public bool CanUse();
        public void SetActive(bool active);
        public void SetTeam(bool rightTeam0);
    }
}