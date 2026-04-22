using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : Singleton<DamageSystem>
{
    [SerializeField]private GameObject damagevx;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<DealDamageGA>(DamagePerform);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<DealDamageGA>();
    }
    private IEnumerator DamagePerform(DealDamageGA ga)
    {
        foreach(CombantantViewBase target in ga.targets)
        {
            GameObject damagevx=Instantiate(this.damagevx,target.transform.position,Quaternion.identity);
            target.TakeDamage(ga.amount);
            yield return new WaitForSeconds(0.15f);
            Destroy(damagevx);
            if(target.CurrentHealth<=0)
            {
                if(target is  EnermyView enermyView)
                {
                    KillEnermyGA killEnermyGA=new(enermyView);
                    ActionSystem.instance.AddReaction(killEnermyGA);
                }
                else
                {
                    //other
                }
            }
        }
    }

}
