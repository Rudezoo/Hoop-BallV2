using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;

    //�޴� ����Ʈ
    public enum GameState
    {
        mainMenu,
        SelectBall,
        Stage1,
        Stage2,
        Stage3,
        EndMenu,
        FailMenu
    }

    ScoreManager _scoreManager;
    BallManager _ballManager;

    [SerializeField] ARPlaneManager arPlaneManager;

    [SerializeField] GameObject placementIndicator;

    [SerializeField] GameObject[] UIObjects;

    [SerializeField] GameObject _stageStartTimer;

    [SerializeField] SpawnField _spawnField;

    [SerializeField] public GameObject _text;

    public GameObject SetBtn;
    public GameObject PlaceBtn;
    public GameObject ArmoveMenu;

    public Text ballCount;
    public GameObject burnTxt;

    public AudioSource mainMusic;
    public AudioSource burnMusic;
    public AudioSource crowdSound;
    public AudioSource Endmusic;

    public AudioSource _MainAudio;
    public AudioClip menuClick;

    public static GameManager instance;

 
    private void Awake()
    {
        
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.mainMenu;

        _scoreManager = GetComponent<ScoreManager>();
        _ballManager = GetComponent<BallManager>();

        placementIndicator.SetActive(true);

        _text = GameObject.Find("DistanceTxt");
        _text.SetActive(false);

        ArmoveMenu.SetActive(true);

    }

    //���� ������Ʈ�� �̵�
    public void NextState()
    {

        if (currentGameState != GameState.Stage3)
            currentGameState += 1;
        else
            currentGameState = GameState.EndMenu;

        CheckState();
    }

    //������ ������Ʈ�� �̵�
    public void SetState(GameState _stageName)
    {
        currentGameState = _stageName;

        CheckState();
    }

    //������Ʈ�� �´� ����
    void CheckState()
    {
        if (currentGameState == GameState.mainMenu)
        {
            //arPlaneManager.enabled = true;
            placementIndicator.SetActive(true);
            _scoreManager.ClearScore();
            DestroyBalls();
            //_ballManager.DestroyBall();
            
        }
        else if(currentGameState == GameState.SelectBall)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                Destroy(plane.gameObject);
            }
            //arPlaneManager.enabled = false;
            UIObjects[0].SetActive(false);
            UIObjects[1].SetActive(true);
        }
        else if(currentGameState == GameState.Stage1)
        {
            DestroyBalls();
            UIObjects[1].SetActive(false);  
            _stageStartTimer.SetActive(true);
            UIObjects[2].SetActive(true);
        }
        else if (currentGameState == GameState.Stage2)
        {
            DestroyBalls();
            _stageStartTimer.SetActive(true);
        }
        else if (currentGameState == GameState.Stage3)
        {
            DestroyBalls();
            _stageStartTimer.SetActive(true);
        }
        else if (currentGameState == GameState.EndMenu)
        {
            DestroyBalls();
            UIObjects[2].SetActive(false);
            UIObjects[3].SetActive(true);
            Endmusic.Play();
           
        }
        else if (currentGameState == GameState.FailMenu)
        {
            DestroyBalls();
            UIObjects[2].SetActive(false);
            UIObjects[4].SetActive(true);
            Endmusic.Play();
        }

    }

    // �������� �̵�
    public void GoMain()
    {
        Endmusic.Stop();
        _MainAudio.PlayOneShot(menuClick,4f);
        mainMusic.Play();
        ClearScreen();
        GameObject center = GameObject.FindGameObjectWithTag("DistanceTrigger");
        Destroy(center);


        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        MainScreenManager.instance.SubMenu.SetActive(true);
        MainScreenManager.instance.SelectMenu.SetActive(true);
        SceneManager.UnloadSceneAsync("InGameScene");
    }

    //�ش� ���� ������ ���� �ʿ��� �����
    public void Retry()
    {
        Endmusic.Stop();
        mainMusic.Play();
        _MainAudio.PlayOneShot(menuClick, 4f);
        for (int i = 0; i < UIObjects.Length; i++)
        {
            UIObjects[i].SetActive(false);
        }
        GetComponent<ScoreManager>().ClearScore();

      
        _spawnField.ResetBasketPosition();
        SetState(GameState.Stage1);
    }




    public void ClearScreen() //ȭ���� ���� �Լ�
    {
        DestroyBalls();

        GameObject[] particles = GameObject.FindGameObjectsWithTag("particle");
        foreach (GameObject p in particles)
        {
            Destroy(p);
        }

    }

    void DestroyBalls() //ȭ��� ��� ���� �����
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Destroy(ball);
        }
    }

    public void SetBallCountTxt(int ballcnt) //���� ������ ǥ���ϴ� ui ����
    {
        if (ballcnt == int.MaxValue)
        {
            ballCount.text = "Balls : INF";
           
        }
        else
        {
            ballCount.text = "Balls : " + ballcnt;
        }
        
    }

    public void Restart() //���� ����� ��ư
    {
        Endmusic.Stop();
        _MainAudio.PlayOneShot(menuClick, 4f);
        ClearScreen();
        GameObject center = GameObject.FindGameObjectWithTag("DistanceTrigger");
        Destroy(center);

        SceneManager.UnloadSceneAsync("InGameScene");
        SceneManager.LoadSceneAsync("InGameScene");
        //StartCoroutine(MainScreenManager.instance.Loading());
    }
}
