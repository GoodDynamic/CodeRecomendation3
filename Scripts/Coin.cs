using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    private event Action<Coin, Vector3> _picked;

    public event Action<Coin, Vector3> Picked
    {
        add => _picked += value;
        remove => _picked -= value;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out Player player))
        {
            _picked(this,transform.position);
        }
    }
}
