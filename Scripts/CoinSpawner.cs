using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    [SerializeField] private GameObject _coinsField;
    [SerializeField] private float _resurrectionDelay;
    
    private List<Coin> _coins = new List<Coin>();

    private AudioSource _audioSource;
    private Transform[] _spawnPoints;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _spawnPoints = GetSpawnPoints(_coinsField);
        SpawnField(_coin.gameObject, _spawnPoints);
    }

    private Transform[] GetSpawnPoints(GameObject spawnField)
    {
        Transform[] _spawnPoints = spawnField.GetComponentsInChildren<Transform>().
               OrderBy(transform => transform.gameObject.name).ToArray();
        return _spawnPoints.Except(new Transform[] { spawnField.GetComponent<Transform>() }).ToArray();
    }

    private void OnCoinPicked(Coin coin, Vector3 position)
    {
        _audioSource.Play();
        coin.Picked -= OnCoinPicked;
        _coins.Remove(coin);
        StartCoroutine(SpawnWithDelay(_resurrectionDelay, _coin.gameObject, position));
        Destroy(coin.gameObject);
    }

    private void SpawnField(GameObject gameObject, Transform[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject coin = Instantiate(gameObject, positions[i].position, Quaternion.identity);
            _coins.Add(coin.GetComponent<Coin>());
            _coins[i].Picked += OnCoinPicked;
        }
    }

    private void OnDisable()
    {
        foreach (Coin coin in _coins)
            coin.Picked -= OnCoinPicked;
    }

    private IEnumerator SpawnWithDelay(float delay, GameObject gameObject, Vector3 position)
    {
        var waitForSeconds = new WaitForSeconds(delay);
        yield return waitForSeconds;

        GameObject coin = Instantiate(gameObject, position, Quaternion.identity);
        _coins.Add(coin.GetComponent<Coin>());
        coin.GetComponent<Coin>().Picked += OnCoinPicked;
    }
}
