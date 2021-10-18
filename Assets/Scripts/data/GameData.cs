using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[Serializable]
public class GameData //저장할 게임 데이터, 현재 점수만 존재한다.
{
    public List<int> scores=new List<int>();
}
