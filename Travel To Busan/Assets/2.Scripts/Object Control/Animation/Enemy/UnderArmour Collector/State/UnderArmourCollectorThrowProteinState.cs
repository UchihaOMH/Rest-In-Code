using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderArmourCollectorThrowProteinState : UnderArmourCollectorState
{
    public List<GameObject> proteinPool = new List<GameObject>();
    public Transform handSocket;

    public float damage = 20f;
    public float coolTime = 1f;

    private GameObject currProtein;

    public GameObject GetProtein
    {
        get
        {
            foreach (var protein in proteinPool)
            {
                if (!protein.activeSelf)
                    return protein;
            }
            GameObject newProtein = Instantiate(proteinPool[0].gameObject, transform);
            newProtein.GetComponent<ThrowableObject>().Pool = transform;
            proteinPool.Add(newProtein);
            return newProtein;
        }
    }

    private void OnEnable()
    {
        foreach (var protein in proteinPool)
        {
            protein.GetComponent<ThrowableObject>().Pool = transform;
            protein.gameObject.SetActive(false);
        }
    }
    public override string GetStateName()
    {
        return "UnderArmour Collector Throw Attack State";
    }
    public override void Process()
    {
        Vector2 dir = (collector.Target.transform.position - collector.transform.position);
        collector.LookAt(dir.x < 0f ? Vector2.left : Vector2.right);

        if (!collector.apPortrait.IsPlaying(UnderArmourCollector._UnderArmourCollectorAnim_.throwProtein))
            collector.apPortrait.Play(UnderArmourCollector._UnderArmourCollectorAnim_.throwProtein);
    }

    #region Animation Event
    public void OnAttack()
    {
        currProtein.transform.SetParent(null);
        currProtein.GetComponent<ThrowableObject>().ThrowTo((collector.Target.transform.position - currProtein.transform.position).normalized);
    }
    public void OnAttackEnter()
    {
        currProtein = GetProtein;
        currProtein.SetActive(true);
        currProtein.transform.SetParent(handSocket);
        currProtein.transform.localPosition = Vector3.zero;
        currProtein.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    public void OnAttackExit()
    {
        if (collector.CurrState == this)
        {
            collector.animState.patternBridge.timer = Time.time + coolTime;
            collector.TransitionProcess(collector.animState.patternBridge);
        }
    }
    #endregion
}