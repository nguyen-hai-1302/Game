using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]

public class EnemyType
{
    public GameObject enemyPrefab; // Prefab của kẻ địch
    public int count; // Số lượng loại kẻ địch này
    public float spawnInterval; // Thời gian giữa mỗi lần spawn của loại kẻ địch này
}

[System.Serializable]
public class Wave
{
    public List<EnemyType> enemies; // Danh sách các loại kẻ địch cho đợt này
}

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves; // Danh sách các đợt sóng
    public List<Transform> spawnPoints; // Danh sách các điểm spawn
    public float waveInterval = 20f; // Khoảng thời gian giữa các đợt sóng (tính bằng giây)
    public TextMeshProUGUI WaveText;

    private int currentWave = 0; // Đợt sóng hiện tại
    private float nextWaveTime; // Thời gian cho đợt sóng tiếp theo
    private bool isFirstWaveStarted = false;
    public GameObject win;

    void Start()
    {
        // Tính toán thời gian cho đợt sóng đầu tiên
        nextWaveTime = Time.time + waveInterval;
    }

    void Update()
    {
        if (isFirstWaveStarted)
        {
            if (Time.time >= nextWaveTime)
            {
                StartNextWave();
            }
        }
    }

    public void StartFirstWave()
    {
        // Chỉ bắt đầu đợt sóng đầu tiên nếu nó chưa bắt đầu
        if (currentWave == 0)
        {
            StartNextWave();
            isFirstWaveStarted = true;
        }
    }

    void StartNextWave()
    {

        // Kiểm tra nếu hết đợt sóng
        if (currentWave >= waves.Count)
        {
            win.SetActive(true);
            Debug.Log("Đã hoàn thành tất cả các đợt sóng!");
            return;
        }

        // Lấy đợt sóng hiện tại
        Wave wave = waves[currentWave];

        // Tăng số đợt sóng lên
        currentWave++;
        WaveText.text = "Wave " + currentWave;
        // Spawn kẻ địch cho đợt sóng này
        foreach (EnemyType enemyType in wave.enemies)
        {
            StartCoroutine(SpawnEnemies(enemyType));
        }

        // Tính toán thời gian cho đợt sóng tiếp theo
        nextWaveTime = Time.time + waveInterval;
    }

    IEnumerator SpawnEnemies(EnemyType enemyType)
    {
        for (int i = 0; i < enemyType.count; i++)
        {
            SpawnEnemy(enemyType.enemyPrefab);
            yield return new WaitForSeconds(enemyType.spawnInterval); // Sử dụng spawnInterval cho từng kẻ địch
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Chọn ngẫu nhiên một điểm spawn từ danh sách
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 spawnPosition = spawnPoint.position;

        // Spawn kẻ địch tại vị trí ngẫu nhiên
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
