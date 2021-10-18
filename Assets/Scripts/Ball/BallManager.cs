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
        //���� ���� �Ǿ�����, ��뿡 �ʹ� ������� Ȯ�� �� ������ ����

        if (_ball && !_ball.GetComponent<CheckBall>().isTooClose)
            Throwing();


        /*        if (Input.touchCount > 0) //Dragging ���϶�
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

    //�������� ����/�� �� �� ���� �� ����
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

    IEnumerator DestroyBall(GameObject ball)  //���ı�����, �̶� burn��� ������ üũ�ؼ� ���� count������ ���� ����
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

    //������
    public void Throwing()
    {

        if (Input.GetMouseButtonDown(0))
        {
            touchBeganPos = Input.mousePosition; //ó�� ��ġ�� ��ġ ����


        }
        else if (Input.GetMouseButtonUp(0))
        {

            Vector2 touchEndedPos = Input.mousePosition; //��ġ�� �������� ��ġ�� ���� ���� �������� ���� ���Ѵ�

            swipe_x = touchEndedPos.x - touchBeganPos.x;
            swipe_y = touchEndedPos.y - touchBeganPos.y;


            Debug.Log("End ::: x:" + swipe_x + ",y:" + swipe_y);
            if (swipe_y > 200) //������ ������ �Ʒ��ʺ��� ū ��� �׳� ������.
            {

                _ball.GetComponent<CheckBall>().ThrowBall(swipe_x, swipe_y);

                if (!burn) //���׸�尡 �ƴ϶�� �� ������ count�Ѵ�.
                {
                    cur_ball++;
                    GameManager.instance.SetBallCountTxt(maxball - cur_ball);
                }
                
                StartCoroutine(DestroyBall(_ball)); //������ ���� �ı��ϴ� ��������
                DisableBall();//���� ���� ������ ���ش�
                if (cur_ball == maxball) //���� ��� ���� �� ��������
                {
                    lastball = true; //������ �������� ��������
                }
                else
                {
                    Invoke("SpawnBall", spawnTime); //���� �� �ִ� ���� ������ ������ ���� spawnTime�� ���� �����
                }


            }
            else //�� �Ʒ����� ���������� ��� ȸ����Ų��.
            {
                _ball.GetComponent<CheckBall>().RotateBall(swipe_x);
                _ball.GetComponent<CheckBall>().rigid.maxAngularVelocity = 10;
            }

        }
    }

    public void BurnModeOn() //���׸�� On
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


    public void BurnModeOff() //���׸�� off
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

    public void DisableBall() //���� ���� ������ ���ش�
    {
       
        _ball = null;
    }
}
