using Controllers;
using UnityEngine;

namespace GamePhysics
{
    public class WhiteHole : MonoBehaviour
    {
        public float gravity = -12;

        public void Repel(PlayerController playerTransform)
        {
            Vector3 gravityUp = (transform.position - playerTransform.PlayerTransform.position).normalized;
            Vector3 localUp = playerTransform.PlayerTransform.up;

            playerTransform.Rb.AddForce(gravityUp * gravity);

            Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * playerTransform.PlayerTransform.rotation;
            playerTransform.PlayerTransform.rotation = Quaternion.Slerp(playerTransform.PlayerTransform.rotation, targetRotation, 50f * Time.deltaTime);
        }
    }
}
