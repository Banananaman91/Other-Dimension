using UnityEngine;

namespace Puzzle.Elements
{
    public class PuzzleBlockerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _an1;
        [SerializeField] private GameObject _player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _player) _an1.SetBool("Player", true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _player) _an1.SetBool("Player", false);
        }
    }
}
