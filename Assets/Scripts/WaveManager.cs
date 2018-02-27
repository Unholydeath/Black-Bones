using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Spawns")]
    [SerializeField] GameObject m_wolfContainer = null;
    [SerializeField] AI m_wolf = null;
    [SerializeField] GameObject m_skeletonContainer = null;
    [SerializeField] AI m_skeleton = null;
    [SerializeField] GameObject m_shadeContainer = null;
    [SerializeField] AI m_shade = null;
    [SerializeField] GameObject m_shade_StaffContainer = null;
    [SerializeField] AI m_shade_Staff = null;
    [SerializeField] GameObject m_bossContainer = null;
    [SerializeField] AI m_boss = null;
    [SerializeField] GameObject[] m_spawnPoints = null;

    [Header("Timer")]
    [SerializeField] [Range(0.25f, 1.0f)] float m_checkTime = 0.33f;
    float timer = 0.0f;
    int m_waveCount = 0;

    [Header("GUI")]
    [SerializeField] TextMeshProUGUI m_waveNum;
    [SerializeField] TextMeshProUGUI m_enemyNum;

    private void Start()
    {
        NextWave();
        timer = m_checkTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            m_enemyNum.text = "Enemies: " + GetNumEnemies();
            if(GetNumEnemies() == 0)
            {
                NextWave();
            }            
        }
    }

    private int GetNumEnemies()
    {
        int enemies = m_wolfContainer.transform.childCount;
        enemies += m_skeletonContainer.transform.childCount;
        enemies += m_shadeContainer.transform.childCount;
        enemies += m_shade_StaffContainer.transform.childCount;
        enemies += m_bossContainer.transform.childCount;
        return enemies;
    }

    public void NextWave()
    {
        m_waveCount++;

        switch (m_waveCount)
        {
            case 1:
                Instantiate(m_wolf, m_spawnPoints[0].transform.position, Quaternion.identity, m_wolfContainer.transform);
                break;
            case 2:
                Instantiate(m_wolf, m_spawnPoints[0].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_wolf, m_spawnPoints[1].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_wolf, m_spawnPoints[2].transform.position, Quaternion.identity, m_wolfContainer.transform);
                break;
            case 3:
                Instantiate(m_skeleton, m_spawnPoints[3].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[4].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                break;
            case 4:
                Instantiate(m_shade, m_spawnPoints[6].transform.position, Quaternion.identity, m_shadeContainer.transform);
                Instantiate(m_shade, m_spawnPoints[7].transform.position, Quaternion.identity, m_shadeContainer.transform);
                break;
            case 5:
                Instantiate(m_skeleton, m_spawnPoints[3].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[4].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_shade_Staff, m_spawnPoints[6].transform.position, Quaternion.identity, m_shade_StaffContainer.transform);
                break;
            case 6:
                Instantiate(m_wolf, m_spawnPoints[0].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[4].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_shade_Staff, m_spawnPoints[7].transform.position, Quaternion.identity, m_shade_StaffContainer.transform);
                break;
            case 7:
                Instantiate(m_skeleton, m_spawnPoints[5].transform.position, Quaternion.identity, m_skeleton.transform);
                Instantiate(m_skeleton, m_spawnPoints[5].transform.position, Quaternion.identity, m_skeleton.transform);
                Instantiate(m_shade, m_spawnPoints[7].transform.position, Quaternion.identity, m_shadeContainer.transform);
                Instantiate(m_shade, m_spawnPoints[7].transform.position, Quaternion.identity, m_shadeContainer.transform);
                break;
            case 8:
                Instantiate(m_wolf, m_spawnPoints[0].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_wolf, m_spawnPoints[1].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_wolf, m_spawnPoints[2].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[4].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_shade_Staff, m_spawnPoints[7].transform.position, Quaternion.identity, m_shade_StaffContainer.transform);
                break;
            case 9:
                Instantiate(m_wolf, m_spawnPoints[0].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_wolf, m_spawnPoints[1].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[4].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[5].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_shade, m_spawnPoints[6].transform.position, Quaternion.identity, m_shadeContainer.transform);
                Instantiate(m_shade_Staff, m_spawnPoints[8].transform.position, Quaternion.identity, m_shade_StaffContainer.transform);
                break;
            case 10:
                Instantiate(m_wolf, m_spawnPoints[0].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_wolf, m_spawnPoints[1].transform.position, Quaternion.identity, m_wolfContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[4].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_skeleton, m_spawnPoints[5].transform.position, Quaternion.identity, m_skeletonContainer.transform);
                Instantiate(m_shade, m_spawnPoints[6].transform.position, Quaternion.identity, m_shadeContainer.transform);
                Instantiate(m_shade_Staff, m_spawnPoints[8].transform.position, Quaternion.identity, m_shade_StaffContainer.transform);
                Instantiate(m_boss, m_spawnPoints[9].transform.position, Quaternion.identity, m_bossContainer.transform);
                break;
        }

        m_waveNum.text = "Wave: " + m_waveCount;
        m_enemyNum.text = "Enemies: " + GetNumEnemies();
    }

    public void ShowWaveGUI()
    {
        m_waveNum.gameObject.SetActive(true);
        m_enemyNum.gameObject.SetActive(true);
    }
}
