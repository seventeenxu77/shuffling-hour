using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        EnermyTurnGA enemyturnga=new EnermyTurnGA();//此处发出的敌人回合开始信号
        ActionSystem.instance.Perform(enemyturnga);
    }
}
