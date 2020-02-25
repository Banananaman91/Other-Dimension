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
        [SerializeField, Range(4, 20)] private int _widthRange;
        [SerializeField, Range(4, 20)] private int _lengthRange;
        [SerializeField] protected GameObject[] _puzzleElements;
        [SerializeField] protected GravityController _gravityController;
        [SerializeField] private GameObject _base;
        [SerializeField] private int _yOffset;
        [HideInInspector] public Transform Origin;
        private List<BuildingPieces> _pieces = new List<BuildingPieces>();
        private bool _isSquare = true;
        private int _distance = 20;
        private int _minRange = 4;
        private int _elementChoice;

        public void Activate()
        {
            //var style = Random.Range(0, 2);
            //_isSquare = style == 0;
            ConstructPuzzle();
        }

        private void ConstructPuzzle()
        {
            if (_isSquare) SquarePuzzle();
            else RectangularPuzzle();
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
                        goal.transform.position = new Vector3(xPos, go.transform.position.y + 1, zPos);
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

        private void RectangularPuzzle()
        {
            var width = Random.Range(_minRange, _widthRange);
            while (width % 2 != 0)
            {
                width = Random.Range(_minRange, _widthRange);
            }
            var length = Random.Range(_minRange, _lengthRange);
            while (length % 2 != 0)
            {
                length = Random.Range(_minRange, _lengthRange);
            }
            var position = transform.position;
            var xPos = position.x;
            var zPos = position.z;
            for (int i = 0; i <= length; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    var randomPicker = Random.Range(0, _buildingPieces.Length);
                    GameObject go = Instantiate(_buildingPieces[randomPicker].gameObject, transform);
                    go.transform.position = new Vector3(xPos, position.y, zPos);
                    go.transform.rotation = Quaternion.FromToRotation(-Vector3.up, Origin.localPosition);
                    if (i == length / 2 && j == width / 2)
                    {
                        GameObject goal = Instantiate(_goal, transform);
                        goal.transform.position = new Vector3(xPos, transform.position.y + 3, zPos);
                    }
                    else
                    {
                        GameObject puzzleElement = CreateElements();
                        if (puzzleElement)
                        {
                            var puzzleElementComponent = puzzleElement.GetComponent<IRayReceiver>();
                            puzzleElement.transform.position = puzzleElementComponent == null ? new Vector3(xPos, transform.position.y + 4.5f, zPos) : new Vector3(xPos, transform.position.y + 3, zPos);
                        }
                    }
                    xPos += _distance;
                }

                xPos = position.x;
                zPos += _distance;
            }
        }
        
        public GameObject CreateElements()
        {
            var elementChange = Random.Range(0.0f, 1.0f);
            if (!(elementChange >= 0.0f) || !(elementChange <= 0.2f)) return null;
            _elementChoice = Random.Range(0, _puzzleElements.Length);
            var position = transform.position;
            GameObject go = Instantiate(_puzzleElements[_elementChoice], transform);
            //go.transform.position = new Vector3(position.x, position.y + 4.5f, position.z);
            return go;
        }
    }
}
