using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage; // 데미지
    public float damageRate; // 데미지 비율

    private List<IDamagable> thingsToDamage = new List<IDamagable>(); 

    private void Start() // 딜데미지를 언제 실행하는지
    {
        InvokeRepeating("DealDamage", 0, damageRate); // 메소드 네임은 string으로, 0=안기다리고 바로 실행
    }

    void DealDamage() // 데미지 받는 메소드
    {
        for (int i = 0; i < thingsToDamage.Count; i++)
        {
            thingsToDamage[i].TakePhysicalDamage(damage); // 리스트 안에 있는건 다 데미지
        }
    }

    private void OnTriggerEnter(Collider other) // 데미지를 받을 때
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable)) // 데미저블 받아오게
        {
            thingsToDamage.Add(damagable); // 데미저블 찾아왔다면? 데미지 추가
        }
    }

    private void OnTriggerExit(Collider other) // 캠프파이어에서 벗어났을 때, 데미지x
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            thingsToDamage.Remove(damagable);
        }
    }

}