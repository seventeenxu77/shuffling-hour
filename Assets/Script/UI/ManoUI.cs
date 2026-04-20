using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text manotext;
    public void UpdateManoText(int currentMano)
    {
        manotext.text = currentMano.ToString();
    }
}
