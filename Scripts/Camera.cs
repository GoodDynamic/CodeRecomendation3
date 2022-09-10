using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Player _player;

    Transform _playerTransform;

    private void Start()
    {
        _playerTransform = _player.transform;
    }

    private void Update()
    {
        transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y,transform.position.z);
    }
}
