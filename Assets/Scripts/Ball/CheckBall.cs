using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBall : MonoBehaviour
{

    //Score
    ScoreManager _scoreManager;
    public bool isEntered = false;

    //Gameplay
    public Rigidbody rigid;
    public bool isThrowing = false;
    public bool isTooClose = false;

    //Debug
    [SerializeField] GameObject _text;

    //Audio
    AudioSource _audio;
    [SerializeField] AudioClip[] _clip;

    bool isRotating;

    float xdirect = 0;
    float count = 300;

    public GameObject HitParticle;
    public GameObject GoalParticle;
    public GameObject BurnParticle;

    public bool burnball;

    public int bonus = 1;


    [SerializeField] GameObject PopScore;
    // Start is called before the first frame update


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        _scoreManager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
        _audio = GetComponent<AudioSource>();

        rigid.isKinematic = false;
        rigid.useGravity = false;
       
        //Debug
        _text = GameManager.instance._text;
 
       
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isThrowing) //던질떄 고정
        {
            //transform.position = Camera.current.transform.position + Camera.current.transform.forward * 0.2f;
            transform.position = BallManager.instance.spawnPoint.position;
            //transform.rotation = Quaternion.identity;
        }

/*        if (isTooClose)    //active부분을 update에서 처리하지않고 trigger에서 관리
        {
            _text.SetActive(true);
        }
        else
        {
            _text.SetActive(false);
        }*/


    }
    void FixedUpdate() //회전을 위한 로직을 담는다
    {

        if (isRotating) //만약 회전하고 있다면 회전힘을 가하고,
        {
            
            rigid.AddTorque(Vector3.up*xdirect);
            //Debug.Log("Rotating!!");

            if (isThrowing ) //던질때 회전방향으로 곡률을 준다.
            {
                
                rigid.AddForce(BallManager.instance.spawnPoint.right * xdirect / count);
                count--;
            }

        }
        else
        {
            if (isThrowing)
                rigid.AddTorque(Vector3.right * 1000);
        }
    }


    //골대를 통과하였는지 검사
    private void OnTriggerEnter(Collider other)
    {
        //거리 체크
        if (other.tag == "DistanceTrigger" && !isThrowing)
        {
            isTooClose = true;
        }

        if (isTooClose)
            return;

        if (!isEntered)
        {
            //골대 윗부분
            if (other.tag == "1stTrigger")
            {
                isEntered = true;
                Instantiate(HitParticle, transform.gameObject.transform.position, Quaternion.identity);

            }
        }
        else
        {
            //골대 아랫부분
            if (other.tag == "2ndTrigger")
            {
                //골인
                
                GameObject pop=Instantiate(PopScore, other.gameObject.transform.position, Quaternion.identity);
                ScoreTxt poptxt = pop.GetComponent<ScoreTxt>();
          

                if (isRotating)
                {
                    _scoreManager.AddScore(bonus);
                    poptxt.SetText(ScoreManager.instance.goalScore+bonus);
                }
                else
                {
                    _scoreManager.AddScore(0);
                    poptxt.SetText(ScoreManager.instance.goalScore);
                }
                Instantiate(GoalParticle, transform.gameObject.transform.position, Quaternion.identity);
                Handheld.Vibrate();
                isEntered = false;
            }



        }
    }

    //거리 체크
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "DistanceTrigger" && !isThrowing)
        {
            _text.SetActive(true);
            isTooClose = true;
        }
    }

    //거리 체크
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "DistanceTrigger" && !isThrowing)
        {
            _text.SetActive(false);
            isTooClose = false;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "BackBoard")
        {
            _audio.PlayOneShot(_clip[0]);
        }
        else if(collision.transform.tag == "Rim")
        {
            _audio.PlayOneShot(_clip[1]);
        }
    }

    //공 던지기 물리 구현
    public void ThrowBall(float swipe_x, float swipe_y)
    {
        _audio.PlayOneShot(_clip[2]);
        isThrowing = true;
        rigid.useGravity = true;
        rigid.isKinematic = false;

        rigid.AddForce(BallManager.instance.spawnPoint.forward * swipe_y / 6f);
        rigid.AddForce(BallManager.instance.spawnPoint.up * swipe_y / 6f);
        rigid.AddForce(BallManager.instance.spawnPoint.right * swipe_x / 10f);
       

        //Destroy(this, 2f); //던져진 ball은 2초 있다 파괴된다.
    }



    public void RotateBall(float swipe_x) //swipe한 방향으로 공을 회전시킨다.
    {
        rigid.isKinematic = false;
        rigid.useGravity = false;

        xdirect = swipe_x;
        isRotating = true;
        
        Debug.Log("Rotating");
    }




}
