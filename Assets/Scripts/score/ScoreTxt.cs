using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTxt : MonoBehaviour //�˾��Ǵ� ������ ���� ����
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

    private void Update() //�÷��̾����� �ٶ󺸰� �Ѵ�.
    {
        transform.LookAt(2 * transform.position-cam.transform.position);
    }

    public void SetText(int score)
    {
        popTxt.text = "+ " + score;
    }



}
