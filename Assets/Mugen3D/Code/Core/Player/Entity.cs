using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class Entity : MonoBehaviour
    {
        public int hp = 100;
        public int MaxHP = 100;
        public Action<int> onHpChange;
        public Action<Entity> onDead;

        public void MakeDamage(int damage)
        {
            this.hp -= damage;
            if (onHpChange != null)
            {
                onHpChange(this.hp);
            }
            if (this.hp <= 0)
            {
                if (onDead != null)
                {
                    onDead(this);
                }
            }
        }
    }
}
