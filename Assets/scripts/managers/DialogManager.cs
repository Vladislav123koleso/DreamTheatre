using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialoguePanel;
    private string currentLine;
    private bool isDisplaying;

    public void StartDialogue(string line)
    {
        dialoguePanel.SetActive(true);
        currentLine = line;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isDisplaying = true;
        dialogueText.text = "��� �� �������������";
        foreach (char c in currentLine)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f); // �������� ��������� ����
        }
        isDisplaying = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isDisplaying)
        {
            StopAllCoroutines();
            dialogueText.text = currentLine; // �������� ���� ����� �����
            isDisplaying = false;
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
