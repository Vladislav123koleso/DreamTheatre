using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource; // ������ �� AudioSource � �������
    public Slider volumeSlider; // ������ �� ������� ��� ���������

    private const string VolumePrefKey = "MusicVolume"; // ���� ��� ���������� ���������

    private void Start()
    {
        // ��������� ����������� ��������� (���� ����)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
        musicSource.volume = savedVolume;

        // ������������� �������� �������� � ������������ � ����������
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // ����� ��� ��������� ���������
    public void SetVolume(float volume)
    {
        musicSource.volume = volume;

        // ��������� ���������
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }
}
