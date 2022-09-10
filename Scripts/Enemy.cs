using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] _pathPoints;

    private Transform[] _path;
    private Transform[] _backPath;

    private int _reachedPoints;
    private readonly float _stepSize = 0.01f;
    private readonly float _accessTime = 2;

    private bool _isGoingBack;
    private bool _isAccessing;

    private Animator _animator;
    private float _walkSpeed = 1f;
    private const string _enemySpeed = "enemySpeed";


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat(_enemySpeed, _walkSpeed);
        _path = _pathPoints;    
        _backPath = _path.Reverse().ToArray();
    }

    private void FixedUpdate()
    {
            if (_isAccessing == false)
                if (_isGoingBack)
                    FollowPath(_backPath);
                else
                   FollowPath(_path);   
    }

    private void FollowPath(Transform[] path)
    {
        if (_reachedPoints + 1 < path.Length)
        {
            Transform target = path[_reachedPoints + 1];
            _animator.SetFloat(_enemySpeed, _walkSpeed);

            if ((transform.position == target.position))
            {
                _reachedPoints++;

                if (_reachedPoints + 1 < path.Length)
                {
                    transform.rotation = GetDirectRotation2D(transform.position, target.position);
                }
            }
            else
            {
                transform.rotation = GetDirectRotation2D(transform.position, target.position);
                transform.position = Vector3.MoveTowards(transform.position, target.position, _stepSize);
            }
        }
        else
        {
            AccessSituation();
        }
    }

    private Quaternion GetDirectRotation2D(Vector3 startPosition, Vector3 nextPosition)
    {
        Vector3 lookDirection = (startPosition- nextPosition);
        (lookDirection.x, lookDirection.z) = (lookDirection.z, lookDirection.x);
        return Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    private void AccessSituation()
    {
        _animator.SetFloat(_enemySpeed, 0);
        _isAccessing = true;
        _isGoingBack = !_isGoingBack;

        Invoke(nameof(StartNewMotion), _accessTime);
    }

    private void StartNewMotion()
    {
        _animator.SetFloat(_enemySpeed, _walkSpeed);
        _reachedPoints = 0;
        _isAccessing = false;
    }
}
