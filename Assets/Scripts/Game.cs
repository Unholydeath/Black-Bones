using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject m_intro = null;
    [SerializeField] GameObject m_pause = null;
    [SerializeField] GameObject m_controls = null;
    [SerializeField] GameObject m_message = null;
    [SerializeField] WaveManager m_waveManager = null;
    [SerializeField] TextMeshProUGUI m_hitPointCount = null;

    private void Start()
    {
        Time.timeScale = 0.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !m_pause.activeInHierarchy)
        {
            m_pause.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GiveMessage()
    {
        m_intro.SetActive(false);
        m_message.SetActive(true);
    }

    public void StartGame()
    {
        m_message.SetActive(false);
        m_hitPointCount.gameObject.SetActive(true);
        m_waveManager.ShowWaveGUI();
        Time.timeScale = 1.0f;
    }

    public void Unpause()
    {
        m_pause.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Controls()
    {
        m_pause.SetActive(false);
        m_controls.SetActive(true);
    }

    public void MainMenu()
    {
        m_controls.SetActive(false);
        m_pause.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
