using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{
    private IObjectPool<HitEffect> hitPool;
    GameObject enemy;

    [Header("Reference")]
    [SerializeField] Transform muzzlePoint;

    [Header("Weapon 1 Attributes")]
    public float weapon1Damage;
    [SerializeField] float weapon1MaxRange;

    [Header("Weapon 2 Attributes")]
    public float weapon2Damage;
    [SerializeField] float weapon2MaxRange;
    [SerializeField] float maxBarrels;
    [SerializeField] float barrelsSpreadPos;

    [Header("Effects")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private HitEffect hitEffect;
    private Vector3 hitPos;

    private void Awake()
    {
        hitPool = new ObjectPool<HitEffect>(CreateHitEffect, OnGet, OnRelease, OnDestroyClone, maxSize: 20);
    }

    #region Hit Effect Pooling
    private HitEffect CreateHitEffect()
    {
        HitEffect effect = Instantiate(hitEffect);
        effect.SetPool(hitPool);
        return effect;
    }
    private void OnGet(HitEffect effect)
    {
        effect.gameObject.SetActive(true);
        effect.transform.position = hitPos;
    }

    private void OnRelease(HitEffect effect)
    {
        effect.gameObject.SetActive(false);
    }

    private void OnDestroyClone(HitEffect effect)
    {
        Destroy(effect.gameObject);
    }
    #endregion

    public void NormalWeaponMode()
    {
        Ray ray = new Ray(muzzlePoint.position, transform.TransformDirection(Vector3.forward));
        RaycastHit hit;
        muzzleFlash.SetActive(true);

        if (Physics.Raycast(ray, out hit, weapon1MaxRange))
        {
            hitPos = hit.point;
            hitPool.Get();
            if (hit.collider.tag == "Enemy")
            {
                enemy = hit.collider.gameObject;
                enemy.GetComponent<Enemy>().GetHit(weapon1Damage);
            }
        }
    }

    public void SpreadMode()
    {
        for (int i = 1; i <= maxBarrels; i++)
        {
            Ray ray = new Ray(muzzlePoint.position, transform.TransformDirection(new Vector3(RandomBarrelPos(), 0.0f, 1.0f)));
            RaycastHit hit;
            muzzleFlash.SetActive(true);

            if (Physics.Raycast(ray, out hit, weapon2MaxRange))
            {
                hitPos = hit.point;
                hitPool.Get();
                if (hit.collider.tag == "Enemy")
                {
                    enemy = hit.collider.gameObject;
                    enemy.GetComponent<Enemy>().GetHit(weapon2Damage);
                }
            }
        }
    }

    float RandomBarrelPos()
    {
        return Random.Range(-barrelsSpreadPos, barrelsSpreadPos);
    }
}
