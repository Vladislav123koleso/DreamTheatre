using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad; // �������� ����� ��� ��������
    public Slider loadingBar;  // ������� ��� ���������� ��������
    public float minLoadingTime = 3f; // ����������� �����, ������� ������ ���� ����� ����� �������� (� ��������)
    public float loadSpeed = 1f; // �������� ��������� ��������

    private void Start()
    {
        // �������� ����������� �������� �����
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        // �������� ����������� �������� �����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // ��������� �������������� ��������� �����
        asyncLoad.allowSceneActivation = false;

        float elapsedTime = 0f;  // ����� ��������

        // ���� ����� �� ��������� �� 100%
        while (!asyncLoad.isDone)
        {
            // ��������� ��������� �������� ������
            float targetProgress = asyncLoad.progress < 0.9f ? asyncLoad.progress : 1f;

            // ������� ���������� �������� ��������
            float currentProgress = loadingBar.value;
            loadingBar.value = Mathf.MoveTowards(currentProgress, targetProgress, loadSpeed * Time.deltaTime);

            // ����� �������� �������� ����� 90%, ����� ������������ �����
            if (asyncLoad.progress >= 0.9f)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= minLoadingTime)
                {
                    asyncLoad.allowSceneActivation = true; // ���������� �����
                }
            }

            yield return null;
        }
    }
}
