using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeball : MonoBehaviour //������ ���۽� arcamera�� ���ϰ��Ѵ�.
{
    // Start is called before the first frame update

    // Update is called once per frame
    GameObject cam;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("arCam");
    }

    private void Start()
    {
        transform.LookAt(cam.transform.position);
    }
    void Update()
    {
       
    }
}
