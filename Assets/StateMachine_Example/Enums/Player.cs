using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachineExamples
{
    public class Player : MonoBehaviour
    {
        public int health;
        private WeaponType currentWeaponType = WeaponType.Unarmed;
        public enum MoveState
        {
            Idle,
            Walk,
            Run,
            Dash,
            Jump
        }

        private void Start()
        {
            print((int)currentWeaponType);
        }
    }
}
