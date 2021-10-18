using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerThree : MonoBehaviour
{
    Text timeText;
    private float time = 3f;

    ScoreManager _scoreManager;
    BallManager _ballManager;

    private void Start()
    {
        timeText = GetComponent<Text>();
        _scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        _ballManager = GameObject.Find("GameManager").GetComponent<BallManager>();
    }

    private void OnEnable()
    {
        time = 3f;
        StartCoroutine(TimeCal());
    }

    //Å¸ÀÌ¸Ó
    IEnumerator TimeCal()
    {
        while (time >= 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        _scoreManager.EnterStage();
        _ballManager.SpawnBall();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        GetComponent<Text>().text = "Time\n" + Mathf.Round(time).ToString();
    }
}
