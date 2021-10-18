using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTxt : MonoBehaviour //팝업되는 점수에 대한 설정
{
    [SerializeField]
    Text popTxt;

    public GameObject cam;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("arCam");
    }

    void Start()
   {
        Destroy(gameObject, 3f);
   }

    private void Update() //플레이어쪽을 바라보게 한다.
    {
        transform.LookAt(2 * transform.position-cam.transform.position);
    }

    public void SetText(int score)
    {
        popTxt.text = "+ " + score;
    }



}
