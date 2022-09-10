using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource),typeof(Rigidbody2D),typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _jumpHigh;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;
    private const string _horizontalSpeed = "speed";
    private const string _verticalSpeed = "verticalSpeed";

    private float _jumpStartSpeed;
    private float _speed = 2;

    private Quaternion leftDirection = new Quaternion(0, 1, 0, 0);
    private Quaternion rightDirection = new Quaternion(0, 0, 0, 0);

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _jumpStartSpeed = GetJumpSpeed(Physics2D.gravity.y, _jumpHigh);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            Move(leftDirection);
        else if (Input.GetKey(KeyCode.D))
            Move(rightDirection);
        else
            _animator.SetFloat(_horizontalSpeed, 0);

        if (Input.GetKey(KeyCode.W))
            if (GetComponent<BoxCollider2D>().IsTouchingLayers())
                Jump();
    }

    private void Move(Quaternion direction)
    {
        _animator.SetFloat(_horizontalSpeed, _speed);
        transform.rotation = direction;

        if (direction == leftDirection)
            transform.Translate(_speed * Time.deltaTime, 0, 0);
        else
            transform.Translate(_speed * Time.deltaTime, 0, 0);
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(0, _jumpStartSpeed);
        _animator.SetFloat(_verticalSpeed, _jumpStartSpeed);
    }

    private float GetJumpSpeed(float gravity, float jumpHigh)
    {
        //result of solve H = Vy0t+gt^2/2 for V0 when V0t = gt^2/2.
        return Mathf.Sqrt(-2 * gravity * jumpHigh);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _animator.SetFloat(_verticalSpeed, 0);

        if (collision.gameObject.TryGetComponent(out Enemy enemy) ||
            collision.gameObject.TryGetComponent(out DeadZone deadZone))
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        StopAllCoroutines();
        _audioSource.Play();
        GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(ReloadSceneWithDelay(_audioSource.clip.length,
        SceneManager.GetActiveScene().name));
    }

    private IEnumerator ReloadSceneWithDelay(float delay, string sceneName)
    {
        var waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;
        SceneManager.LoadScene(sceneName);
    }
}
