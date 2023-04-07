using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class AbsAINpc : MonoBehaviour
{
    public List<GameObject> _Target;
    protected IFSM _Fsm;
    protected Coroutine _SeeEnemy;
    protected MethodInfo _ActAdd;
    protected MethodInfo _ActRemove;
    //感知范围
    private float _ViewDistance = 500;
    protected abstract void Awake();
    protected abstract void Start();
    public  void SeeyEnemy(GameObject target)
    {
        if (!_Target.Contains(target))
        {
            _Target.Add(target);
            _SeeEnemy = StartCoroutine(SeeyEnemy());
        }
    }
    public  void RemoEnemy(GameObject target)
    {
        if (_Target.Contains(target))
        {
            _Target.Add(target);
        }
        if (_Target.Count <= 0)
        {
            StopCoroutine(_SeeEnemy);
        }
    }
    protected virtual void Update()
    {
        _Fsm.Update();
    }
    /// <summary>
    /// 由协程对进入地图的敌人判断
    /// </summary>
    /// <returns></returns>
   private IEnumerator SeeyEnemy()
    {
        //因为每一帧目标都可能离开了该位置
        while (true)
        {
            for (int i = 0; i < _Target.Count; i++)
            {
                if (Vector3.Distance(_Target[i].transform.position, transform.position) <= _ViewDistance)
                {
                    Debug.Log("发现敌人");
                    Vector3 playerDir = _Target[i].transform.position - transform.position;
                    _ActAdd.Invoke(_Fsm,new object[1] { _Target[i] });
                }
                else
                {
                    Debug.Log("敌人远去");
                    _ActRemove.Invoke(_Fsm, new object[1] { _Target[i] });
                }
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
