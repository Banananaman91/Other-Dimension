using Puzzle.Laser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDeflector : RayMaster, IRayReceiver
{
    [SerializeField] private CubeColour _colourType;
    [SerializeField] private Material _material;
    private Color _laserColour;
    private bool _userLaserColourProperty;

    private void Start()
    {
        switch (_colourType)
        {
            case CubeColour.Blue:
                _laserColour = Color.blue;
                _material.color = _laserColour;
                break;
            case CubeColour.Green:
                _laserColour = Color.green;
                _material.color = _laserColour;
                break;
            case CubeColour.Red:
                _laserColour = Color.red;
                _material.color = _laserColour;
                break;
            case CubeColour.White:
                _userLaserColourProperty = true;
                break;

        }
    }
    public new void HitWithRay()
    {
        if (_userLaserColourProperty) _laserColour = LaserColour;
        _hitWithRay = true;
        _rayRunOutTime = Time.time + _hitByRayRefreshTime;
        _laserVisual.startColor = _laserColour;
        _laserVisual.endColor = _laserColour;
        var laserParticleMain = _laserParticle.main;
        laserParticleMain.startColor = _laserColour;
    }
}
