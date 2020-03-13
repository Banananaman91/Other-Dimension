using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using GamePhysics;
using Puzzle.Laser;
using UnityEngine;
using Random = UnityEngine.Random;
using Ray = Puzzle.Laser.Ray;

namespace Puzzle.Builder
{
    public class PuzzleConstructor : MonoBehaviour
    {
        [SerializeField] private BuildingPieces[] _buildingPieces;
        [SerializeField] private GameObject _goal;
        [SerializeField, Range(4, 20)] private int _sizeRange;
        [SerializeField] protected GameObject[] _puzzleElements;
        [SerializeField] private GameObject _base;
        [SerializeField] private int _yOffset;
        private List<BuildingPieces> _pieces = new List<BuildingPieces>();
        private int _distance = 20;
        private int _minRange = 4;
        private int _elementChoice;

        public void ConstructPuzzle()
        {
            SquarePuzzle();
        }

        private void SquarePuzzle()
        {
            var size = Random.Range(_minRange, _sizeRange);
            
            while (size % 2 != 0)
            {
                size = Random.Range(_minRange, _sizeRange);
            }
            _base.transform.localScale = new Vector3(size * _distance + 10, _yOffset, size * _distance + 10);
            var baseTransformPosition = _base.transform.position;
            _base.transform.position = new Vector3(baseTransformPosition.x - 5, baseTransformPosition.y, baseTransformPosition.z + size * _distance + 5);
            var half = size / 2;
            var position = transform.position;
            var xPos = position.x;
            var zPos = position.z;
            var yPos = position.y;
            int adjust = 1;
            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <= size; j++)
                {
                    var randomPicker = Random.Range(0, _buildingPieces.Length);
                    GameObject go = Instantiate(_buildingPieces[randomPicker].gameObject, transform);
                    go.transform.position = new Vector3(xPos, yPos + _yOffset, zPos);

                    if (i == half && j == half)
                    {
                        GameObject goal = Instantiate(_goal, transform);
                        List<GameObject> children = new List<GameObject>();
                        for (int k = 0; k < go.transform.childCount; k++)
                        {
                            children.Add(go.transform.GetChild(k).gameObject);
                        }
                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                if (child.GetComponent<RayDestructable>()) continue;
                                child.AddComponent<RayDestructable>();
                            }
                        }

                        goal.transform.position = new Vector3(xPos, go.transform.position.y + 1, zPos);
                    }
                    else if ((i == 0 && j == 0) || (i == size && j == size) || (i == 0 && j == size) ||
                             (i == size && j == 0))
                    {
                        GameObject newElement = Instantiate(_puzzleElements[1], transform);
                        newElement.transform.position = new Vector3(xPos, transform.position.y + 3 + _yOffset, zPos);
                        List<GameObject> children = new List<GameObject>();
                        for (int k = 0; k < go.transform.childCount; k++)
                        {
                            children.Add(go.transform.GetChild(k).gameObject);
                        }
                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                if (child.GetComponent<RayDestructable>()) continue;
                                child.AddComponent<RayDestructable>();
                            }
                        }
                    }
                    else
                    {
                        GameObject puzzleElement = CreateElements();
                        if (puzzleElement)
                        {
                            var puzzleElementComponent = puzzleElement.GetComponent<Ray>();
                            puzzleElement.transform.position = puzzleElementComponent == null ? new Vector3(xPos, transform.position.y + 4.5f + _yOffset, zPos) : new Vector3(xPos, transform.position.y + 3 + _yOffset, zPos);
                        }
                    }
                    xPos += _distance;
                }

                xPos = position.x;
                zPos += _distance;
                
            }
        }

        private GameObject CreateElements()
        {
            var elementChange = Random.Range(0.0f, 1.0f);
            if (!(elementChange >= 0.0f) || !(elementChange <= 0.2f)) return null;
            var elementChoice = Random.Range(0.0f, 1.0f);
            GameObject go;
            if (elementChoice >= 0.0f && elementChoice <= 0.7f) go = Instantiate(_puzzleElements[0], transform);
            else
            {
                _elementChoice = Random.Range(2, _puzzleElements.Length);
                go = Instantiate(_puzzleElements[_elementChoice], transform);
            }
            return !go ? null : go;
        }
    }
}
