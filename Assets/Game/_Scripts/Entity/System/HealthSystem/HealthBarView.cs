using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private float _timeDamageEffectInSeconds = 0.3f;
        [SerializeField] private int _timeDelayEffectInMilliseconds = 150;
        [SerializeField] private Slider _mainSlider;
        [SerializeField] private Slider _sliderEffects;

        public void Init(int maxHealth)
        {
            _mainSlider.maxValue = maxHealth;
            _sliderEffects.maxValue = maxHealth;

            _mainSlider.value = maxHealth;
            _sliderEffects.value = maxHealth;
        }

        public void UpdateView(float currentHealth)
        {
            _mainSlider.value = currentHealth;
            AnimateAsync(currentHealth);
        }

        private async void AnimateAsync(float targetValue)
        {
            await Task.Delay(_timeDelayEffectInMilliseconds);

            float startValue = _sliderEffects.value;
            float elapsed = 0f;

            while (elapsed < _timeDamageEffectInSeconds)
            {
                elapsed += Time.deltaTime;
                _sliderEffects.value = Mathf.Lerp(startValue, targetValue, elapsed / _timeDamageEffectInSeconds);
                await Task.Yield();
            }

            _sliderEffects.value = targetValue;
        }
    }
}
