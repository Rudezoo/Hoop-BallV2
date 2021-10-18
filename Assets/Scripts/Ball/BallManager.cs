using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BallManager : MonoBehaviour
{
    public ARRaycastManager arRaycaster;

    SelectBall _selectBallManager;
    [SerializeField] List<GameObject> Ball;
    GameObject _ball;

    public Camera arcam;
    public Transform spawnPoint;

    private Vector2 touchBeganPos;
    public float swipe_x;
    public float swipe_y;

    public GameObject tempText;
    public bool isDragging;

    [SerializeField]
    int maxball = 6;
    int cur_ball = 0;

    public float spawnTime =0.5f;
    public float destroyTime = 2.5f;
    public bool lastball;

    public bool burn;

    public bool once;

    public static BallManager instance;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _selectBallManager = GetComponent<SelectBall>();
        GameManager.instance.SetBallCountTxt(maxball - cur_ball);
        cur_ball = 0;
        _selectBallManager.CheckUnlock();
        //temp
        //SpawnBall();
    }

    void Update()
    {
        //공이 생성 되었는지, 골대에 너무 가까운지 확인 후 던지기 가능

        if (_ball && !_ball.GetComponent<CheckBall>().isTooClose)
            Throwing();


        /*        if (Input.touchCount > 0) //Dragging 중일때
                {
                    Touch t = Input.GetTouch(0);
                    if (t.phase == TouchPhase.Moved)
                    {
                        Debug.Log("Dragging");
                        tempText.SetActive(true);
                        isDragging = true;
                    }
                    else
                    {
                        tempText.SetActive(false);
                        isDragging = false;
                    }


                }*/

        if (lastball)
        {
            if (cur_ball != maxball)
            {
                SpawnBall();
                lastball = false;
            }
        }

    }

    //스테이지 시작/끝 시 공 스폰 및 삭제
    public void SpawnBall()
    {
        if (maxball-cur_ball>0)
        {
            _ball = Instantiate(Ball[_selectBallManager.CheckSelected()], spawnPoint.position, spawnPoint.rotation);
            if (burn)
            {
                _ball.GetComponent<CheckBall>().BurnParticle.SetActive(true);
                _ball.GetComponent<CheckBall>().burnball = true;

            }
            else
            {
                _ball.GetComponent<CheckBall>().BurnParticle.SetActive(false);
                _ball.GetComponent<CheckBall>().burnball = false;
            }
        }
    }

    IEnumerator DestroyBall(GameObject ball)  //공파괴로직, 이때 burn모드 인지를 체크해서 공을 count할지에 대해 결정
    {
        bool check = ball.GetComponent<CheckBall>().burnball;
        yield return new WaitForSeconds(destroyTime);
        
       if (!check)
        {
            cur_ball--;
            GameManager.instance.SetBallCountTxt(maxball - cur_ball);
            Debug.Log(maxball - cur_ball);
        }  
        Destroy(ball);

    }

    //던지기
    public void Throwing()
    {

        if (Input.GetMouseButtonDown(0))
        {
            touchBeganPos = Input.mousePosition; //처음 터치된 위치 저장


        }
        else if (Input.GetMouseButtonUp(0))
        {

            Vector2 touchEndedPos = Input.mousePosition; //터치를 땠을떄의 위치를 정해 공이 던져지는 힘을 정한다

            swipe_x = touchEndedPos.x - touchBeganPos.x;
            swipe_y = touchEndedPos.y - touchBeganPos.y;


            Debug.Log("End ::: x:" + swipe_x + ",y:" + swipe_y);
            if (swipe_y > 200) //던지는 방향이 아래쪽보다 큰 경우 그냥 던진다.
            {

                _ball.GetComponent<CheckBall>().ThrowBall(swipe_x, swipe_y);

                if (!burn) //버닝모드가 아니라면 공 개수를 count한다.
                {
                    cur_ball++;
                    GameManager.instance.SetBallCountTxt(maxball - cur_ball);
                }
                
                StartCoroutine(DestroyBall(_ball)); //던져진 공을 파괴하는 로직실행
                DisableBall();//현재 공의 정보를 없앤다
                if (cur_ball == maxball) //만약 모든 공을 다 던졌을떄
                {
                    lastball = true; //마지막 공에대한 로직실행
                }
                else
                {
                    Invoke("SpawnBall", spawnTime); //던질 수 있는 공이 있을때 던지고 공을 spawnTime에 맞춰 재생성
                }


            }
            else //공 아래쪽을 스와이프한 결과 회전시킨다.
            {
                _ball.GetComponent<CheckBall>().RotateBall(swipe_x);
                _ball.GetComponent<CheckBall>().rigid.maxAngularVelocity = 10;
            }

        }
    }

    public void BurnModeOn() //버닝모드 On
    {
        burn = true;
        spawnTime = 0.1f;
        GameManager.instance.SetBallCountTxt(int.MaxValue);
        GameManager.instance.burnTxt.SetActive(true);
        if (!once)
        {
            GameManager.instance.mainMusic.Stop();
            GameManager.instance.burnMusic.Play();
            GameManager.instance.crowdSound.Play();
            once = true;
        }
    }


    public void BurnModeOff() //버닝모드 off
    {
        burn = false;
        once = false;
        spawnTime = 1f;
        GameManager.instance.burnTxt.SetActive(false);
        cur_ball = 0;
        GameManager.instance.SetBallCountTxt(maxball-cur_ball);


        GameManager.instance.mainMusic.Play();
        GameManager.instance.burnMusic.Stop();
        GameManager.instance.crowdSound.Stop();

       
        if(_ball)
            Destroy(_ball);
    }

    public void DisableBall() //현재 공의 정보를 없앤다
    {
       
        _ball = null;
    }
}
