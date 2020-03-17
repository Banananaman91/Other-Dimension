using System;
using Controllers;
using GamePhysics;
using Puzzle.Laser;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle.Builder
{
    public class BuildingPieces : Controller
    {
        [SerializeField] private GameObject[] _walls;

        private void Awake()
        {
            if (_walls.Length == 0) return;
            var chance = Random.Range(0f, 1f);
            if (chance > 0.4f) return;
            if (_walls.Length > 2)
            {
                var multiWall = Random.Range(2, _walls.Length);
                for (int i = 0; i <= multiWall; i++)
                {
                    var wall = Random.Range(0, _walls.Length);
                    while (_walls[wall].GetComponent<RayDestructable>())
                    {
                        wall = Random.Range(0, _walls.Length);
                    }
                    _walls[wall].AddComponent<RayDestructable>();
                }
            }
            else
            {
                var wall = Random.Range(0, _walls.Length);
                _walls[wall].AddComponent<RayDestructable>();
            }
        }
    }
}
