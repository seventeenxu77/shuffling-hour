using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermySystem : MonoBehaviour
{
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnermyTurnGA>(EnermyTurnPerform);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnermyTurnGA>();
    }
    private IEnumerator EnermyTurnPerform(EnermyTurnGA ga)
    {
        Debug.Log("Enermy Turn Begin");
        yield return new WaitForSeconds(2f);
        Debug.Log("Enermy Turn End");
    }
}
