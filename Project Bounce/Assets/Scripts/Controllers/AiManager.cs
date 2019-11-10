using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class AiManager : MonoBehaviour
    {
        [SerializeField] private AiMovement Ai;
        // Start is called before the first frame update
//        void Start()
//        {
//            StartCoroutine(AiMove());
//        }
//
//        IEnumerator AiMove()
//        {
//            while (!Ai.PathFinderReady)
//            {
//                yield return null;
//            }
//            Ai.AiBehaviour(AiMovement.AiState.findingTarget);
//            yield return null;
//        }

    }
}
