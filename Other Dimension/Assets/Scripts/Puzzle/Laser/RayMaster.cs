using Puzzle.Laser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMaster : MonoBehaviour, IRayReceiver
{
    [SerializeField] protected LineRenderer _laserVisual;
    [SerializeField] protected ParticleSystem _laserParticle;
    [SerializeField] protected Direction _directionType;
    protected Vector3 _transformDirection;
    protected float _distance = 100;
    protected RaycastHit _hit;
    protected bool _hitWithRay;
    protected float _rayRunOutTime;
    protected float _hitByRayRefreshTime = 1f;
    public Color LaserColour { get; set; }

    private void Awake()
    {
        switch (_directionType)
        {
            case Direction.Forward:
                _transformDirection = transform.forward;
                break;
            case Direction.Up:
                _transformDirection = transform.up;
                break;
        }
        _laserVisual.startColor = Color.black;
        _laserVisual.endColor = Color.black;
        var laserParticleMain = _laserParticle.main;
        laserParticleMain.startColor = Color.black;
        _laserVisual.SetPosition(1, new Vector3(0, 0, 0));
    }

    public void HitWithRay()
    {
        _hitWithRay = true;
        _rayRunOutTime = Time.time + _hitByRayRefreshTime;
        _laserVisual.startColor = LaserColour;
        _laserVisual.endColor = LaserColour;
        var laserParticleMain = _laserParticle.main;
        laserParticleMain.startColor = LaserColour;
    }

    public void NotHitWithRay()
    {
        _laserVisual.startColor = Color.black;
        _laserVisual.endColor = Color.black;
        var laserParticleMain = _laserParticle.main;
        laserParticleMain.startColor = Color.black;
        _laserVisual.SetPosition(1, new Vector3(0, 0, 0));
    }

    private void FixedUpdate()
    {
        if (Time.time > _rayRunOutTime)
        {
            _hitWithRay = false;
            NotHitWithRay();
        }
        if (!_hitWithRay) return;
        Physics.Raycast(transform.position, transform.TransformDirection(_transformDirection), out _hit, _distance);
        _laserVisual.SetPosition(1, _transformDirection * _distance);
        if (!_hit.collider) return;
        var distance = Vector3.Distance(transform.position, _hit.point);
        _laserVisual.SetPosition(1, _transformDirection * distance);
        var rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
        rayReceiver?.HitWithRay();
    }
}
