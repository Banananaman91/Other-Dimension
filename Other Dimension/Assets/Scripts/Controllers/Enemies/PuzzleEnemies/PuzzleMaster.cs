using System;
using System.Collections;
using UnityEngine;

namespace Controllers.Enemies.PuzzleEnemies
{
    [RequireComponent(typeof(BoxCollider))]
    public class PuzzleMaster : AiMaster
    {
        [SerializeField] protected Animator _an1, _an2;
        protected Animator _anim;
        protected static readonly int Player = Animator.StringToHash("Player");
        private Collider MyCollider => GetComponent<BoxCollider>();

        protected override void IdleAction()
        {
            if (_anim && _anim.GetBool(Player)) _anim.SetBool(Player, false);
        }

        protected override void DetermineGoalPosition()
        {
            StateChange.ToIdleState();
        }

        protected override void MoveCharacter()
        {
            StateChange.ToIdleState();
        }

        protected override IEnumerator VisualisePath()
        {
            StateChange.ToIdleState();
            return null;
        }

        protected override void Attack()
        {
            StateChange.ToIdleState();
        }

        protected override void Block()
        {
            if (_anim && _anim.GetBool(Player) == false) _anim.SetBool(Player, true);
        }

        private void OnTriggerEnter(Collider other)
        {
             if (other.gameObject.name == "Sphere") StateChange.ToBlockState();
        }

        private void OnTriggerExit(Collider other)
        {
            StateChange.ToIdleState();
        }
    }
}
