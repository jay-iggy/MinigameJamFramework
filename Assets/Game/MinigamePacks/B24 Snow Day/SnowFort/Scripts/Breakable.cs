using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowfort
{
    public class Breakable : MonoBehaviour
    {
        public int health = 3;

        public GameObject corpse;

        public delegate void OnDamage(int health);

        public OnDamage onDamage;

        public void Damage(int damage)
        {
            health -= damage;
            onDamage(health);
            if (health <= 0)
            {
                Instantiate(corpse, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            onDamage += (h) => { };
        }
    }
}