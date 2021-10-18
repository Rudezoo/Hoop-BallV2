using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketMover : MonoBehaviour
{
    GameManager _gameManger;

    public int _horizontal = 0;
    public int _vertical = 0;
    public float _posX;
    public float _posY;
    public float _origin_posX;
    public float _origin_posY;

    void Start()
    {
        _gameManger = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(Move());
    }

    void CheckMyPos()
    {
        _posX = transform.position.x;
        _posY = transform.position.y;
    }

    public void ResetPosition()
    {
        _horizontal = 0;
        _vertical = 0;
        transform.DOMoveX(_origin_posX, 1f);
        transform.DOMoveY(_origin_posY, 1f);
    }

    IEnumerator Move()
    {
        switch (_gameManger.currentGameState)
        {
            case GameManager.GameState.Stage1:
                _origin_posX = transform.position.x;
                _origin_posY = transform.position.y;
                break;
            case GameManager.GameState.Stage2:
                MoveHorizontal();
                break;
            case GameManager.GameState.Stage3:
                int dir = Random.Range(0, 2);
                if (dir == 0)
                    MoveHorizontal();
                else if (dir == 1)
                    MoveVertical();
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Move());
    }

    void MoveHorizontal()
    {
        if (_horizontal == 0)
        {
            int hor = Random.Range(-1, 2);
            if (hor == -1)
            {
                CheckMyPos();
                transform.DOMoveX(_posX - 0.5f, 1.5f);
                _horizontal = -1;
            }
            else if (hor == 0)
            {
                CheckMyPos();
                transform.DOMoveX(_posX + 0f, 1.5f);
                _horizontal = 0;
            }
            else if (hor == 1)
            {
                CheckMyPos();
                transform.DOMoveX(_posX + 0.5f, 1.5f);
                _horizontal = 1;
            }
        }
        else if (_horizontal == 1)
        {
            int hor = Random.Range(-1, 1);
            if (hor == -1)
            {
                CheckMyPos();
                transform.DOMoveX(_posX - 0.5f, 1.5f);
                _horizontal = 0;
            }
            else if (hor == 0)
            {
                CheckMyPos();
                transform.DOMoveX(_posX + 0f, 1.5f);
                _horizontal = 1;
            }
        }
        else if (_horizontal == -1)
        {
            int hor = Random.Range(0, 2);
            if (hor == 1)
            {
                CheckMyPos();
                transform.DOMoveX(_posX + 0.5f, 1.5f);
                _horizontal = 0;
            }
            else if (hor == 0)
            {
                CheckMyPos();
                transform.DOMoveX(_posX + 0f, 1.5f);
                _horizontal = -1;
            }
        }
    }

    void MoveVertical()
    {
        if (_vertical == 0)
        {
            int hor = Random.Range(-1, 2);
            if (hor == -1)
            {
                CheckMyPos();
                transform.DOMoveY(_posY - 0.3f, 1.5f);
                _vertical = -1;
            }
            else if (hor == 0)
            {
                CheckMyPos();
                transform.DOMoveY(_posY + 0f, 1.5f);
                _vertical = 0;
            }
            else if (hor == 1)
            {
                CheckMyPos();
                transform.DOMoveY(_posY + 0.3f, 1.5f);
                _vertical = 1;
            }
        }
        else if (_vertical == 1)
        {
            int hor = Random.Range(-1, 1);
            if (hor == -1)
            {
                CheckMyPos();
                transform.DOMoveY(_posY - 0.3f, 1.5f);
                _vertical = 0;
            }
            else if (hor == 0)
            {
                CheckMyPos();
                transform.DOMoveY(_posY + 0f, 1.5f);
                _vertical = 1;
            }
        }
        else if (_vertical == -1)
        {
            int hor = Random.Range(0, 2);
            if (hor == 1)
            {
                CheckMyPos();
                transform.DOMoveY(_posY + 0.3f, 1.5f);
                _vertical = 0;
            }
            else if (hor == 0)
            {
                CheckMyPos();
                transform.DOMoveY(_posY + 0f, 1.5f);
                _vertical = -1;
            }
        }
    }
}
