using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time; // �Ϸ�
    public float fullDayLength;
    public float startTime = 0.4f; // �Ϸ� ���� �ð� 
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity; // �׷���(Ŀ��)�� ���缭 ���ϴ� ���� Ÿ�Կ� ���缭 ������

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength; // ��ŭ�� ���ؾ� �ϴ��� ���
        time = startTime;
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // �ð� ������ ����, Ÿ�Ӱ��� �ۼ��������� 1.0f�� ����,  0~0.999..����

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time); // ambient ȯ�汤, ���� �ּ�ȭ��Ű�� ����
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time); // 

    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time); // �ִϸ��̼�Ŀ�꿡 �ð����� �ָ� �� �ð��� �´� �׷������� ������

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f; // ������ ��ȭ => ���� ��ȭ
        lightSource.color = colorGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }
}