using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public interface Projectile
    {
        public abstract int GetDamage();
        public abstract void SetTeam(bool rightTeam);
    }
}