using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class DialogueEntry
{
    public string characterName; // имя персонажа
    [TextArea(2, 5)]
    public string text;
    public Sprite portrait;
    public UnityEvent onDialogueEnd;
}

public class DialogueController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image characterImage;
    public GameObject dialoguePanel;

    [Header("Dialogue Content")]
    public List<DialogueEntry> dialogueEntries = new List<DialogueEntry>();

    [Header("Settings")]
    public float typingSpeed = 0.03f;

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private Sprite lastImage;
    private string lastName;

    void Start()
    {
        if (dialogueEntries.Count > 0)
        {
            StartDialogue();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueEntries[currentIndex].text;
                isTyping = false;
            }
            else
            {
                InvokeDialogueEndEvent();
                NextDialogue();
            }
        }
    }

    void StartDialogue()
    {
        currentIndex = 0;
        ShowCurrentDialogue();
    }

    void ShowCurrentDialogue()
    {
        DialogueEntry entry = dialogueEntries[currentIndex];

        // Имя персонажа
        if (!string.IsNullOrWhiteSpace(entry.characterName))
        {
            nameText.text = entry.characterName;
            lastName = entry.characterName;
        }
        else if (!string.IsNullOrWhiteSpace(lastName))
        {
            nameText.text = lastName;
        }

        // Портрет персонажа
        if (entry.portrait != null)
        {
            characterImage.sprite = entry.portrait;
            lastImage = entry.portrait;
        }
        else if (lastImage != null)
        {
            characterImage.sprite = lastImage;
        }

        typingCoroutine = StartCoroutine(TypeText(entry.text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        string displayedText = "";
        int i = 0;

        while (i < fullText.Length)
        {
            if (fullText[i] == '<') // если начинается тег
            {
                int closingIndex = fullText.IndexOf('>', i);
                if (closingIndex == -1) break;

                string tag = fullText.Substring(i, closingIndex - i + 1);
                displayedText += tag; // добавляем тег сразу целиком, без задержки
                i = closingIndex + 1;
                continue;
            }

            displayedText += fullText[i];
            dialogueText.text = displayedText;

            i++;
            yield return new WaitForSeconds(typingSpeed);
        }

        dialogueText.text = displayedText; // на всякий случай
        isTyping = false;
    }

    void InvokeDialogueEndEvent()
    {
        DialogueEntry entry = dialogueEntries[currentIndex];
        entry.onDialogueEnd?.Invoke();
    }

    void NextDialogue()
    {
        currentIndex++;
        if (currentIndex < dialogueEntries.Count)
        {
            ShowCurrentDialogue();
        }
        else
        {
            dialogueText.text = "";
            nameText.text = "";
            characterImage.enabled = false;

            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
        }
    }

    // Новый метод с задержкой
    IEnumerator RestartDialogueWithDelay()
    {
        Debug.Log("Запущен RestartDialogue");
        currentIndex = 0;
        dialogueText.text = "";
        nameText.text = "";
        characterImage.enabled = true;

        // Активируем диалоговую панель
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            Debug.Log("Диалоговая панель активирована");
        }
        else
        {
            Debug.LogWarning("dialoguePanel не назначен!");
        }

        StopAllCoroutines(); // на всякий случай, если что-то ещё бежит
        
        // Пауза перед показом текста
        yield return new WaitForSeconds(0.1f); // добавляем небольшую задержку

        ShowCurrentDialogue();
    }

    // Обновленный метод для перезапуска
    public void RestartDialogue()
    {
        StartCoroutine(RestartDialogueWithDelay());
    }
}
