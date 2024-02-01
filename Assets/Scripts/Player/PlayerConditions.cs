using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IDamagable // 데미지처리
{
    void TakePhysicalDamage(int damageAmount);
}

[System.Serializable] // 인스펙터창에서 보이게
public class Condition
{
    [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate;
    public float decayRate;
    public Image uiBar;

    public void Add(float amount) //각 컨디션 값들을 변경하기 위한 메소드 111
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount) // 222
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); // 0보다는 낮아지지 않게!
    }

    public float GetPercentage() // 
    {
        return curValue / maxValue;
    }

}


public class PlayerConditions : MonoBehaviour, IDamagable //필요한 컨디션들 선언, 데미저블 상속받게
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    public float noHungerHealthDecay; // 배고픔이 다 닳았을 때 체력 닳도록 변수 선언

    public UnityEvent onTakeDamage; // 데미지를 받았을 때 처리하는 이벤트

    void Start() // 초기 컨디션값 설정
    {
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        stamina.curValue = stamina.startValue;
    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.decayRate * Time.deltaTime); // 시간이 지날수록 배고픔 감소 !!델타타음 공부다시!!
        stamina.Add(stamina.regenRate * Time.deltaTime); // 시간이 지날수록 스테미너 채움

        if (hunger.curValue == 0.0f)
            health.Subtract(noHungerHealthDecay * Time.deltaTime);

        if (health.curValue == 0.0f) // 체력값이 0이면 죽음.
            Die();

        health.uiBar.fillAmount = health.GetPercentage(); // UI바에서 컨디션값 보여줄 수 있게
        hunger.uiBar.fillAmount = hunger.GetPercentage();
        stamina.uiBar.fillAmount = stamina.GetPercentage();
    }

    public void Heal(float amount) // 체력값 증가하게
    {
        health.Add(amount);
    }

    public void Eat(float amount) // 배고픔값 증가하게
    {
        hunger.Add(amount);
    }

    public bool UseStamina(float amount) // 스테미너는 쓰도록
    {
        if (stamina.curValue - amount < 0) // 스테미너값이 0보다 작으면 못씀
            return false;

        stamina.Subtract(amount); // 아니면 쓸수있음
        return true;
    }

    public void Die() // 체력 0일때 죽음 알려줌
    {
        Debug.Log("플레이어가 죽었다.");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}