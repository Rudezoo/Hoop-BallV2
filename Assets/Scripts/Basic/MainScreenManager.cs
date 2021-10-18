using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainScreen;
    public GameObject SubMenu;
    public GameObject SelectMenu;
    public GameObject scoreMenu;
    public GameObject LoadingScreen;

    public Text topscoreFront;
    public Text topscore;
    public Text subscore;

    public AudioSource mainMusic;

    public AudioSource _MainAudio;
    public AudioClip menuClick;
    public AudioClip playClick;

    public static MainScreenManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Temp
        //DataController.instace._gameData.scores.Clear() ;

    }
    public void StartGame()
    {
        _MainAudio.PlayOneShot(playClick, 4f);
        MainScreen.SetActive(false);
        SubMenu.SetActive(false);
        mainMusic.Stop();
        StartCoroutine(Loading());
    }

    public IEnumerator Loading() //로딩화면 표시후 scene을 불러온다.
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("InGameScene", LoadSceneMode.Additive);
        while (!loadingOperation.isDone)
        {
            yield return null;
            Debug.Log(loadingOperation.progress);
            LoadingScreen.SetActive(true);
            
        }
        LoadingScreen.SetActive(false);

    }

    public void QuitGame() //게임을 종료한다
    {
       
        QuitGame();
    }

    public void ShowSubMenu() //서브메뉴를 보여준다
    {
        _MainAudio.PlayOneShot(menuClick,4f);
        MainScreen.SetActive(false);
        SubMenu.SetActive(true);
        SelectMenu.SetActive(true);
    }

    public void BackToSelect() //선택화면으로 돌아간다.
    {
        _MainAudio.PlayOneShot(menuClick, 4f);
        scoreMenu.SetActive(false);
        SelectMenu.SetActive(true);
    }

    public void ShowScore() //점수판을 보여준다
    {
        _MainAudio.PlayOneShot(menuClick, 4f);
        SelectMenu.SetActive(false);
        scoreMenu.SetActive(true);
        SetScoreBoard();
    }

    void SetScoreBoard() //점수판의 점수를 설정한다
    {
        List<int> scores = DataController.instace._gameData.scores;
        if (scores.Count != 0)
        {
            topscoreFront.text = "Top.";
            topscore.text = scores[0].ToString();
            string otherScores="";
            for(int i = 0; i < scores.Count - 1; i++)
            {
                otherScores += (i + 2).ToString() + "." + "\t" + "\t" + scores[i+1]+"\n";
            }
            subscore.text = otherScores;
        }
        else
        {
            topscoreFront.text = "No Score";
        }
    }


}
