using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HitEffect : MonoBehaviour
{
    private IObjectPool<HitEffect> hitPool;

    private void OnEnable()
    {
        StartCoroutine(OnHit());
    }

    public void SetPool(IObjectPool<HitEffect> pool)
    {
        hitPool = pool;
    }

    IEnumerator OnHit()
    {
        yield return new WaitForSeconds(0.5f);
        hitPool.Release(this);
    }

    
}
