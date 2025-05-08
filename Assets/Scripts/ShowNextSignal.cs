using UnityEngine;

public class ShowNextDialogueSignal : MonoBehaviour
{
    [Header("Объект, который нужно активировать")]
    public GameObject targetToShow;

    public void ShowTarget()
    {
        if (targetToShow != null)
        {
            targetToShow.SetActive(true);

            // Попробуем найти и запустить диалог
            DialogueController dialogue = targetToShow.GetComponent<DialogueController>();
            if (dialogue != null)
            {
                dialogue.RestartDialogue(); // или StartDialoguePublic(), если используешь динамические 

            }
            else
            {
                Debug.LogWarning("ShowNextDialogueSignal: DialogueController не найден на целевом объекте.");
            }
        }
        else
        {
            Debug.LogWarning("ShowNextDialogueSignal: targetToShow не назначен!");
        }
    }
}
