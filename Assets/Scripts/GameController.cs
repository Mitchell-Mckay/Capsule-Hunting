using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] float currScore = 0;

    [SerializeField] TextMeshProUGUI scoreAmount;

    private void Start()
    {
        currScore = 0;
        UpdateScoreUI();
    }

    public void AddScore(float amount)
    {
        currScore += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreAmount.text = currScore.ToString("0");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("StartMenuScene");
    }

}
