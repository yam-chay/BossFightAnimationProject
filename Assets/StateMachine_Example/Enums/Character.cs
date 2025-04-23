using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineExamples
{
    public class Character : MonoBehaviour
    {
        private Sword weapon = new Sword(); 

        public void PerformAttack()
        {
            weapon.Attack();
        }
    }

    public class Sword
    {
        public void Attack()
        {
            Debug.Log("Swinging the sword!");
        }
    }
}
