using UnityEngine;
using UnityEngine.UI;

public class SliderParticleEffect : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public GameObject particleEffectPrefab;
    private GameObject particleEffectInstance;

    private RectTransform sliderRectTransform;

    private void Start()
    {
        sliderRectTransform = slider.GetComponent<RectTransform>();
        CreateParticleEffect();
    }

    private void Update()
    {
        UpdateParticleEffectPosition();
    }

    private void CreateParticleEffect()
    {
        if (particleEffectInstance == null)
        {
            particleEffectInstance = Instantiate(particleEffectPrefab, fillImage.transform);
            particleEffectInstance.transform.localPosition = Vector3.zero;
        }
    }

    private void UpdateParticleEffectPosition()
    {
        if (particleEffectInstance != null)
        {
            float fillAmount = slider.value / slider.maxValue;
            float xPos = fillAmount * sliderRectTransform.rect.width - sliderRectTransform.rect.width / 2;
            Vector3 newPosition = new Vector3(xPos, 0, 0);
            particleEffectInstance.transform.localPosition = newPosition;
        }
    }
}
