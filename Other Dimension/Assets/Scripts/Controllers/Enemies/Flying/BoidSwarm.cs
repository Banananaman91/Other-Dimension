using UnityEngine;

namespace Controllers.Enemies.Flying
{
    public class BoidSwarm : BaseFlyingAi
    {
        [SerializeField] private GameObject _boid;
        [SerializeField] private int _flockTotal;
        [SerializeField] private int _spawnRadius;

        protected override void IdleAction()
        {
            for (int i = 0; i < _flockTotal; i++)
            {
                GameObject clone = Instantiate(_boid);
                clone.transform.position = Random.insideUnitSphere * _spawnRadius;
            }
            StateChange.ToFindTargetState();
        }
    }
}
