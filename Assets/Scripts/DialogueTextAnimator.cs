using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class DialogueTextAnimator : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeAmount = 0.5f;
    public float shakeSpeed = 10f;

    private TMP_Text textComponent;
    private bool isShaking = false;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        StartAnimation();
    }

    void OnDisable()
    {
        isShaking = false;
    }

    public void StartAnimation()
    {
        if (!isShaking)
        {
            isShaking = true;
            StartCoroutine(ShakeText());
        }
    }

    IEnumerator ShakeText()
    {
        while (isShaking)
        {
            textComponent.ForceMeshUpdate();
            TMP_TextInfo textInfo = textComponent.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

                // Если цвет символа — НЕ белый, значит у него явно задан <color>
                bool hasColorTag = false;
                for (int j = 0; j < 4; j++)
                {
                    if (vertexColors[vertexIndex + j].r != 255 ||
                        vertexColors[vertexIndex + j].g != 255 ||
                        vertexColors[vertexIndex + j].b != 255)
                    {
                        hasColorTag = true;
                        break;
                    }
                }

                if (!hasColorTag)
                    continue;

                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                float offsetX = Mathf.Sin(Time.time * shakeSpeed + i) * shakeAmount;
                float offsetY = Random.Range(-shakeAmount * 0.5f, shakeAmount * 0.5f);

                for (int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j] += new Vector3(offsetX, offsetY, 0);
                }
            }

            // Обновляем геометрию
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }
}
