using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage; // ������
    public float damageRate; // ������ ����

    private List<IDamagable> thingsToDamage = new List<IDamagable>(); 

    private void Start() // ���������� ���� �����ϴ���
    {
        InvokeRepeating("DealDamage", 0, damageRate); // �޼ҵ� ������ string����, 0=�ȱ�ٸ��� �ٷ� ����
    }

    void DealDamage() // ������ �޴� �޼ҵ�
    {
        for (int i = 0; i < thingsToDamage.Count; i++)
        {
            thingsToDamage[i].TakePhysicalDamage(damage); // ����Ʈ �ȿ� �ִ°� �� ������
        }
    }

    private void OnTriggerEnter(Collider other) // �������� ���� ��
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable)) // �������� �޾ƿ���
        {
            thingsToDamage.Add(damagable); // �������� ã�ƿԴٸ�? ������ �߰�
        }
    }

    private void OnTriggerExit(Collider other) // ķ�����̾�� ����� ��, ������x
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            thingsToDamage.Remove(damagable);
        }
    }

}