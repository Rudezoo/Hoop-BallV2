using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBall : MonoBehaviour
{

    [SerializeField] ToggleGroup _toggleGroup;
    [SerializeField] List<Toggle> BallSelection;

    public GameObject[] unlockIcons;
    int[] unlockscores = { 10, 30, 50, 100, 150 };

    public int selected;

    public static SelectBall instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        BallSelection[0].isOn = true;

    }

    //선택된 공 배열 번호 리턴
    public int CheckSelected()
    {
        for (int i = 0; i < BallSelection.Count; i++)
        {
            if (BallSelection[i].isOn)
                selected = i;
        }
        return selected;
    }

    public void CheckUnlock()
    {
        if (DataController.instace._gameData.scores.Count > 0)
        {
            for (int i = 0; i < unlockscores.Length; i++)
            {
                if (DataController.instace._gameData.scores[0] >= unlockscores[i])
                {
                    unlockIcons[i].SetActive(false);
                }
            }
        }


    }
}
