using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource; // Ссылка на AudioSource с музыкой
    public Slider volumeSlider; // Ссылка на слайдер для громкости

    private const string VolumePrefKey = "MusicVolume"; // Ключ для сохранения громкости

    private void Start()
    {
        // Загружаем сохраненную громкость (если есть)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
        musicSource.volume = savedVolume;

        // Устанавливаем значение слайдера в соответствии с громкостью
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // Метод для установки громкости
    public void SetVolume(float volume)
    {
        musicSource.volume = volume;

        // Сохраняем громкость
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }
}
