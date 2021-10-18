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

    public IEnumerator Loading() //�ε�ȭ�� ǥ���� scene�� �ҷ��´�.
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

    public void QuitGame() //������ �����Ѵ�
    {
       
        QuitGame();
    }

    public void ShowSubMenu() //����޴��� �����ش�
    {
        _MainAudio.PlayOneShot(menuClick,4f);
        MainScreen.SetActive(false);
        SubMenu.SetActive(true);
        SelectMenu.SetActive(true);
    }

    public void BackToSelect() //����ȭ������ ���ư���.
    {
        _MainAudio.PlayOneShot(menuClick, 4f);
        scoreMenu.SetActive(false);
        SelectMenu.SetActive(true);
    }

    public void ShowScore() //�������� �����ش�
    {
        _MainAudio.PlayOneShot(menuClick, 4f);
        SelectMenu.SetActive(false);
        scoreMenu.SetActive(true);
        SetScoreBoard();
    }

    void SetScoreBoard() //�������� ������ �����Ѵ�
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
