using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle.Builder
{
    public class PuzzleConstructor : MonoBehaviour
    {
        [SerializeField] private BuildingPieces[] _buildingPieces;
        [SerializeField] private int _sizeRange;
        [SerializeField] private int _widthRange;
        [SerializeField] private int _lengthRange;
        private bool _isSquare;
        private int _distance = 9;

        private void Awake()
        {
            var style = Random.Range(0, 2);
            _isSquare = style == 0;
            ConstructPuzzle();
        }

        private void ConstructPuzzle()
        {
            if (_isSquare) SquarePuzzle();
            else RectangularPuzzle();
        }

        private void SquarePuzzle()
        {
            var size = Random.Range(1, _sizeRange);
            while (size % 2 != 0)
            {
                size = Random.Range(1, _sizeRange);
            }
            var position = transform.position;
            var xPos = position.x;
            var zPos = position.z;
            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <= size; j++)
                {
                    var randomPicker = Random.Range(0, _buildingPieces.Length);
                    GameObject go = Instantiate(_buildingPieces[randomPicker].gameObject);
                    go.transform.position = new Vector3(xPos, position.y, zPos);


                    xPos += _distance;
                }

                xPos = position.x;
                zPos += _distance;
            }
        }

        private void RectangularPuzzle()
        {
            var width = Random.Range(1, _widthRange);
            while (width % 2 != 0)
            {
                width = Random.Range(1, _widthRange);
            }
            var length = Random.Range(1, _lengthRange);
            while (length % 2 != 0)
            {
                length = Random.Range(1, _lengthRange);
            }
            var position = transform.position;
            var xPos = position.x;
            var zPos = position.z;
            for (int i = 0; i <= length; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    var randomPicker = Random.Range(0, _buildingPieces.Length);
                    GameObject go = Instantiate(_buildingPieces[randomPicker].gameObject);
                    go.transform.position = new Vector3(xPos, position.y, zPos);
                    xPos += _distance;
                }

                xPos = position.x;
                zPos += _distance;
            }
        }
    }
}
