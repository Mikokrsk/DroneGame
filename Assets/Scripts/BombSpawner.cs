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
        var bombSpawnPosition = _droneSpawnPositionTransform.position;
        // bombSpawnPosition.position = _droneSpawnPositionTransform.localPosition;
        //  Instantiate(_bombPrefab, bombSpawnPosition, _bombPrefab.transform.rotation, transform);

        // Create random Euler angles for rotation
        float randomX = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 360f);
        float randomZ = Random.Range(0f, 360f);

        // Create quaternion using Euler angles
        Quaternion randomRotation = Quaternion.Euler(randomX, randomY, randomZ);
        var bomb = Instantiate(_bombPrefab, bombSpawnPosition, randomRotation, transform);
        bomb.GetComponent<Bomb>().StartExplosion();
    }
}

