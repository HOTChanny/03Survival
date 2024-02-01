using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IDamagable // ������ó��
{
    void TakePhysicalDamage(int damageAmount);
}

[System.Serializable] // �ν�����â���� ���̰�
public class Condition
{
    [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;

    public void Add(float amount) //�� ����� ������ �����ϱ� ���� �޼ҵ� 111
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount) // 222
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); // 0���ٴ� �������� �ʰ�!
    }

    public float GetPercentage() // 
    {
        return curValue / maxValue;
    }

}


public class PlayerConditions : MonoBehaviour, IDamagable //�ʿ��� ����ǵ� ����, �������� ��ӹް�
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    public float noHungerHealthDecay; // ������� �� ����� �� ü�� �⵵�� ���� ����

    public UnityEvent onTakeDamage; // �������� �޾��� �� ó���ϴ� �̺�Ʈ

    void Start() // �ʱ� ����ǰ� ����
    {
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        stamina.curValue = stamina.startValue;
    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.decayRate * Time.deltaTime); // �ð��� �������� ����� ���� !!��ŸŸ�� ���δٽ�!!
        stamina.Add(stamina.regenRate * Time.deltaTime); // �ð��� �������� ���׹̳� ä��

        if (hunger.curValue == 0.0f)
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f) // ü�°��� 0�̸� ����.
            Die();

        health.uiBar.fillAmount = health.GetPercentage(); // UI�ٿ��� ����ǰ� ������ �� �ְ�
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        stamina.uiBar.fillAmount = stamina.GetPercentage();
    }

    public void Heal(float amount) // ü�°� �����ϰ�
    {
        health.Add(amount);
    }

    public void Eat(float amount) // ����İ� �����ϰ�
    {
        hunger.Add(amount);
    }

    public bool UseStamina(float amount) // ���׹̳ʴ� ������
    {
        if (stamina.curValue - amount < 0) // ���׹̳ʰ��� 0���� ������ ����
            return false;

        stamina.Subtract(amount); // �ƴϸ� ��������
        return true;
    }

    public void Die() // ü�� 0�϶� ���� �˷���
    {
        Debug.Log("�÷��̾ �׾���.");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}