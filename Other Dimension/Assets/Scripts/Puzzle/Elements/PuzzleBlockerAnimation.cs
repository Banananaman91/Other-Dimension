using System;
using Controllers.Enemies.PuzzleEnemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle.Elements
{
    public class PuzzleBlockerAnimation : PuzzleMaster
    {
        private bool _initialiser;

        protected override void IdleAction()
        {
            if (!_initialiser) InitialiseObject();
            if (_anim && _anim.GetBool(Player)) _anim.SetBool(Player, false);
        }

        private void InitialiseObject()
        {
            var percent = Random.Range(0.0f, 1.0f);
            var rotation = transform.rotation;
            if (percent >= 0.0f && percent <= 0.2f)
            {
                _anim = _an2;
                _an1.gameObject.SetActive(false);
                _initialiser = true;
            }
            else if (percent > 0.2f && percent <= 0.4f)
            {
                transform.Rotate(rotation.x, rotation.y + 90, rotation.z, Space.Self);
                _anim = _an2;
                _an1.gameObject.SetActive(false);
                _initialiser = true;
            }
            else if (percent > 0.4f && percent <= 0.6f)
            {
                transform.Rotate(rotation.x, rotation.y + 180, rotation.z, Space.Self);
                _anim = _an2;
                _an1.gameObject.SetActive(false);
                _initialiser = true;
            } 
            else if (percent > 0.6f && percent <= 0.8f)
            {
                transform.Rotate(rotation.x, rotation.y + 270, rotation.z, Space.Self);
                _anim = _an2;
                _an1.gameObject.SetActive(false);
                _initialiser = true;
            } 
            else if (percent > 0.8f && percent <= 1.0f)
            {
                transform.Rotate(rotation.x, rotation.y, rotation.z + 90, Space.Self);
                _anim = _an1;
                _an2.gameObject.SetActive(false);
                _initialiser = true;
            } 
        }

    }
}
