using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeball : MonoBehaviour //눈알이 시작시 arcamera를 향하게한다.
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
