using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnField : MonoBehaviour
{

    public ARRaycastManager arRaycaster;
    public GameObject _basketField;
    public GameObject placementIndicator;

    GameManager _gameManager;

    GameObject BasketField;
    bool placed;


    public static SpawnField instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (BasketField && !placed && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Touch touch = Input.GetTouch(0);
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;
                    BasketField.transform.position = hitPose.position;
                    BasketField.transform.rotation = hitPose.rotation;
                }
            }



        }


    }

    //농구 골대 소환
    public void SpawnBasketField()
    {
        GameManager.instance._MainAudio.PlayOneShot(GameManager.instance.menuClick, 4f);

        if (_gameManager.currentGameState != GameManager.GameState.mainMenu)
            return;

        GameManager.instance.PlaceBtn.SetActive(false);
        GameManager.instance.SetBtn.SetActive(true);

        if (!BasketField)
        {
            BasketField = Instantiate(_basketField, placementIndicator.transform.position, placementIndicator.transform.rotation);
            placementIndicator.SetActive(false);
        }
/*        else //아래부분은 placeindicator로 대체한다.
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                BasketField.transform.position = hitPose.position;
                BasketField.transform.rotation = hitPose.rotation;
            }
        }*/
        /*        Touch touch = Input.GetTouch(0);

                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;

                    if (!BasketField)
                    {
                        BasketField = Instantiate(_basketField, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        BasketField.transform.position = hitPose.position;
                        BasketField.transform.rotation = hitPose.rotation;
                    }

                }*/

    }

    //골대 생성되어 있는지 확인(버튼에서 호출)
    public void CheckBuilt()
    {
        GameManager.instance._MainAudio.PlayOneShot(GameManager.instance.menuClick, 4f);
        if (BasketField)
        {
            _gameManager.SetState(GameManager.GameState.SelectBall);
            //gameObject.SetActive(false);
            placed = true;
        }
        else
            return;

    }

    public void DestroyBasket()
    {
        Destroy(BasketField);
    }

    public void ResetBasketPosition()
    {
        BasketField.GetComponentInChildren<BasketMover>().ResetPosition();
    }

}
