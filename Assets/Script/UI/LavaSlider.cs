using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaSlider : MonoBehaviour
{
    public float value = 0;
    float maxValue = 100;
    float maxValueMultiplier = 1;
    public Image image;

    private static LavaSlider instance;
    public static LavaSlider Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("LavaSlider is null !!");

            return instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
            Destroy(Instance.gameObject);

        instance = this;

        updateSlider();
    }

    public void addValue(float f)
    {
        value = value + f;
        updateSlider();
    } 

    void updateSlider()
    {
        if(value >= maxValue * maxValueMultiplier)
        {
            value = 0;
            maxValueMultiplier = maxValueMultiplier * 1.2f;
            StartCoroutine(Flames());
        }
        image.fillAmount = value / (maxValue * maxValueMultiplier);
    }

    IEnumerator Flames()
    {
        yield return new WaitForEndOfFrame();
    }
}
