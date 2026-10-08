using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.Snowfort
{
    public class BasicProjectile : MonoBehaviour, Projectile
    {
        public int damage = 1;

        public float gravity;

        Rigidbody2D body;

        bool hit;

        bool rightTeam;

        public int GetDamage()
        {
            return (hit) ? 0 : damage;
        }

        public void SetTeam(bool rTeam)
        {
            rightTeam = rTeam;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Breakable b = collision.gameObject.GetComponent<Breakable>();
            if (b != null)
                b.Damage(damage);
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y + gravity * Time.fixedDeltaTime);
        }

        void Start()
        {
            body = GetComponent<Rigidbody2D>();
        }
    }
}