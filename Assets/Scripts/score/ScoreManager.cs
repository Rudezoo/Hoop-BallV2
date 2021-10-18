using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] int _currentScore = 0;
    [SerializeField] int[] _targetScore = { 30, 60, 90 };
    [SerializeField] int[] _goalScore = { 2, 3 };
    [SerializeField] float[] _time = { 60f, 50f, 50f };


    int targetScore = 0;
    public float _timeLeft;
    public int goalScore;

    //UI
    [SerializeField] GameObject _timetxt;
    [SerializeField] GameObject _stagetxt;
    [SerializeField] GameObject _scoretxt;
    [SerializeField] GameObject _endScoretxt;
    [SerializeField] GameObject _goaltxt;

    static public ScoreManager instance;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
        instance = this;
    }

    //스코어 초기화
    public void ClearScore()
    {
        _currentScore = 0;
    }

    //스테이지 돌입 시 목표 점수, 제한 시간 설정 후 타이머 가동
    public void EnterStage()
    {
        _timetxt.SetActive(true);
        switch (_gameManager.currentGameState)
        {
            case GameManager.GameState.Stage1:
                Debug.Log(_time[0]);
                targetScore = _targetScore[0];
                _timeLeft = _time[0];
                StartCoroutine(TimeCal());
                break;
            case GameManager.GameState.Stage2:
                Debug.Log(_time[1]);
                targetScore = _targetScore[1];
                _timeLeft = _time[1];
                StartCoroutine(TimeCal());
                break;
            case GameManager.GameState.Stage3:
                Debug.Log(_time[2]);
                targetScore = _targetScore[2];
                _timeLeft = _time[2];
                StartCoroutine(TimeCal());
                break;
        }
    } 

    private void Update()
    {
        //TimeSys
        CheckGoalScore();

        //SetUI
        SetUI();
    }

    //타이머
    IEnumerator TimeCal()
    {
        while (_timeLeft >= 0)
        {
            _timeLeft -= Time.deltaTime;
            yield return null;
        }
        CheckScore();
    }

    //스테이지 종료 시 점수 체크
    private void CheckScore()
    {
        //공 삭제
       
        BallManager.instance.BurnModeOff();
        GameManager.instance.ClearScreen();
       
        BallManager.instance.DisableBall();

        SelectBall.instance.CheckUnlock();

        _timetxt.SetActive(false);
        if (GameManager.instance.currentGameState == GameManager.GameState.Stage3)
        {
            SaveHighScore();
            GameManager.instance.SetState(GameManager.GameState.EndMenu);
            return;
        }

        if (_currentScore < targetScore)
        {
            SaveHighScore();
            GameManager.instance.SetState(GameManager.GameState.FailMenu);
        }
        else
        {
            GameManager.instance.NextState();
          
        }
    }

    //15초 이하 일 시 점수 변동 및 버닝타임
    private void CheckGoalScore()
    {
        if (_timeLeft < 15f)
        {
            goalScore = _goalScore[1];
            if (_timeLeft > 0) { 
                BallManager.instance.BurnModeOn();
            }           
        }       
        else
        {
            goalScore = _goalScore[0];
        }     
    }

    //점수 Plus
    public void AddScore(int plus)
    {
        _currentScore += goalScore + plus;
    }

    //Stage 단계, 시간, 스코어 UI 업데이트
    public void SetUI()
    {
        if (_stagetxt)
            _stagetxt.GetComponent<Text>().text = "Stage" + ((int)_gameManager.currentGameState - 1);
        if (_timetxt)
            _timetxt.GetComponent<Text>().text = "Time\n" + Mathf.Round(_timeLeft).ToString();
        if (_scoretxt)
            _scoretxt.GetComponent<Text>().text = "Score\n" + _currentScore;
        if (_endScoretxt)
            _endScoretxt.GetComponent<Text>().text = _currentScore.ToString();
        if (_goaltxt)
            _goaltxt.GetComponent<Text>().text = "Goal\n" + targetScore;
    }

    public void SaveHighScore() //최고점수 및 이하 5개 점수 설정 및 저장
    {
        List<int> scores = DataController.instace._gameData.scores;

        if (FindSameScore(scores, _currentScore))
            return;

        if (scores.Count < 5)
        {
            scores.Add(_currentScore);
            scores.Sort();
            scores.Reverse();
            DataController.instace._gameData.scores = scores;
        }
        else
        {
            int idx = -1;
            for (int i = 0; i < scores.Count; i++)
            {
                if (scores[i] < _currentScore)
                {
                    idx = i;
                    break;
                }
            }

            if (idx != -1)
            {
                scores.RemoveAt(scores.Count-1); //끝 값제거
                scores.Add(_currentScore);
                scores.Sort();
                scores.Reverse();
                DataController.instace._gameData.scores = scores;
            }
        }

        DataController.instace.SaveGameData();

        /*        if (PlayerPrefs.HasKey("HighScore"))
                {
                    if(_currentScore > PlayerPrefs.GetInt("HighScore"))
                    {
                        PlayerPrefs.SetInt("HighScore", _currentScore);
                    }
                }
                PlayerPrefs.Save();*/
    }


    //같은 점수가 있는지 찾고 그 결과를 반환한다.
    bool FindSameScore(List<int> score, int current)
    {
        for (int i = 0; i < score.Count; i++)
        {
            if (score[i] == current) {
                return true;
            }
        }

        return false;

    }
}
