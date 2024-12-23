using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;
    public FadeInOut fadeInOut;
    public GameObject abilitiesPanel; // ������ � ��������� �������������
    private float defaultCameraSize; // ��� �������� ���������� ������� ������

    public bool isFight = false; // ��� ������������ ���������� ���

    public TextMeshProUGUI introText;  // ����� ��������
    public float typingSpeed = 0.05f;     // �������� ��������� ������

    private IEnumerator Start()
    {
        
        abilitiesPanel.SetActive(false);
        // ��������� ��������� ������ ������
        defaultCameraSize = mainCamera.orthographicSize;

        // ���� ... ������� ����� ������
        //yield return new WaitForSeconds(0);
        // ������ ����������
        yield return StartCoroutine(fadeInOut.FadeOut());

        string intro = "����� �� ����: ����� ���������� �� ����� ������ ������, " +
            "����� �� �������� �������� � ������. ������ � ����� ������ ����� ������� �����";
        yield return StartCoroutine(TypeText(intro));


        yield return new WaitForSeconds(1);

        yield return StartCoroutine(fadeInOut.FadeIn());
        //-------------------------------

        //---------------------------
        //����� ���������� �������

        //----------------------------------
        //����� ������� ���������� ���
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeOut());
        yield return StartCoroutine(TypeText("����� �� ����: ������ ����������. ������ ���� �������"));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(fadeInOut.FadeIn());

        abilitiesPanel.SetActive(true);
        StartCoroutine(FocusCameraOnBattle());
        isFight = true;



    }











    // ����� ������ �� �����
    private IEnumerator TypeText(string message)
    {
        introText.text = "";  // ������� ����� ����� �������

        foreach (char letter in message.ToCharArray())
        {
            introText.text += letter;  // ��������� ������
            yield return new WaitForSeconds(typingSpeed);  // �������� ����� ���������
        }
        yield return new WaitForSeconds(2);
        introText.text = "";
    }

    // ������ � �������
    IEnumerator FocusCameraOnBattle()
    {
        float targetSize = 5f; // ������� ������ ������ ��� �����������
        float duration = 0f; // ������������ �����������
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
    }
    // ����� ��� �������� ������ � ���������� �������
    public void ResetCameraToDefault()
    {
        StartCoroutine(ResetCameraSize());
    }

    private IEnumerator ResetCameraSize()
    {
        float duration = 1f; // �����, �� ������� ������ �������� � ���������� �������
        float initialSize = mainCamera.orthographicSize;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(initialSize, defaultCameraSize, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = defaultCameraSize; // ���������, ��� ������ ����������� �� ������ ��������
    }

}
