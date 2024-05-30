using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Bomb
{
    public override void StartExplosion()
    {
        if (isActive)
        {
            _explosionDelay = Random.Range(0.1f, _explosionDelay);
        }
        else
        {
            isActive = true;
            _animator.SetTrigger("DisconnectingGrenadeFittingTrigger");
        }
    }

    protected override void Explosion()
    {
        _sphereExploder.StartExploded(_explosionDamage);
        Destroy(_rb);
        Instantiate(_VolumetricObject, transform);
        Invoke("DestroyBomb", _destroyDelay);
    }
}