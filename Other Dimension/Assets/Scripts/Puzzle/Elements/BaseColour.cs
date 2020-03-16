using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Puzzle.Elements
{
    public class BaseColour : Controller
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private Material TreeMaterial => GetComponent<MeshRenderer>().material;

        private Color[] ColoursArray = { Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.white, Color.yellow};

        private int _colour1;
        private int _colour2;
        private float _t;
        private int _speed = 5;
        private bool _countUp;
        private bool _complete;
        
        

        private void Awake()
        {
            _colour1 = Random.Range(0, ColoursArray.Length);
            _colour2 = Random.Range(0, ColoursArray.Length);
            StartCoroutine(ChangeColour());
        }

        private IEnumerator ChangeColour()
        {
            while (!_complete)
            {
                if (_countUp)
                {
                    if (_t < 1)
                    {
                        _t += Time.deltaTime / _speed;
                        TreeMaterial.SetColor(BaseColor,
                            Color.Lerp(ColoursArray[_colour1], ColoursArray[_colour2], _t));
                    }
                    else _countUp = false;
                }

                else if (!_countUp)
                {
                    if (_t > 0.1f)
                    {
                        _t -= Time.deltaTime / _speed;
                        TreeMaterial.SetColor(BaseColor,
                            Color.Lerp(ColoursArray[_colour1], ColoursArray[_colour2], _t));
                    }
                    else _countUp = true;
                }

                yield return null;
            }
        }
    }
}
