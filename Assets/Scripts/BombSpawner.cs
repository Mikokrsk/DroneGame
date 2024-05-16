using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private Transform _droneSpawnPositionTransform;
    public float spawnDelay = 1.0f;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time >= nextSpawnTime && Input.GetKey(KeyCode.Space))
        {
            SpawnBomb();
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    public void SpawnBomb()
    {
        var bombSpawnPosition = _droneSpawnPositionTransform;
        // bombSpawnPosition.position = _droneSpawnPositionTransform.localPosition;
        Instantiate(_bombPrefab, _droneSpawnPositionTransform.position, _bombPrefab.transform.rotation, transform);
    }
}

