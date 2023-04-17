using UnityEngine;
using static UnityEngine.ParticleSystem;



public class DamageFeedback : MonoBehaviour
{
    private Health healthComponent;
    public ParticleSystem feedbackParticles;
    private bool hasPlayedParticleEffect = false;

    private void Start()
    {
        healthComponent = GetComponent<Health>();
        healthComponent.OnHealthChanged += HandleHealthChange;
    }

    private void OnDestroy()
    {
        healthComponent.OnHealthChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(float newHealth)
    {
        if (newHealth < healthComponent.maxHealth && !hasPlayedParticleEffect)
        {
            feedbackParticles.Play();
            hasPlayedParticleEffect = true;
        }
        else if (newHealth == healthComponent.maxHealth && hasPlayedParticleEffect)
        {
            hasPlayedParticleEffect = false;
        }
    }
}
