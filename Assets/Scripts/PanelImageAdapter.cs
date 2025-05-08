using UnityEngine;
using UnityEngine.UI;

public class PanelImageAdapter : MonoBehaviour
{
    public GameObject panel; // Ссылка на Панель (GameObject)
    public Image contentImage; // Ссылка на Изображение внутри панели

    public float padding = 30f; // Паддинг вокруг изображения

    private RectTransform panelRectTransform;
    private RectTransform imageRectTransform;

    private Image panelImage; // Для получения цвета панели

    void Start()
    {
        panelRectTransform = panel.GetComponent<RectTransform>(); // Получаем RectTransform панели
        imageRectTransform = contentImage.GetComponent<RectTransform>(); // Получаем RectTransform изображения
        panelImage = panel.GetComponent<Image>(); // Получаем компонент Image панели для работы с цветом

        // Устанавливаем начальный размер изображения и прозрачность
        SetImageSize();
    }

    void Update()
    {
        // Если размер или прозрачность панели изменяются, обновляем размер и прозрачность изображения
        SetImageSize();
        SyncTransparency();
    }

    void SetImageSize()
    {
        // Получаем текущую ширину и высоту панели
        float panelWidth = panelRectTransform.rect.width;
        float panelHeight = panelRectTransform.rect.height;

        // Обновляем размер изображения с учетом паддинга
        imageRectTransform.sizeDelta = new Vector2(panelWidth - 2 * padding, panelHeight - 2 * padding);

        // По желанию, можно установить изображение по центру панели
        imageRectTransform.localPosition = Vector3.zero;
    }

    void SyncTransparency()
    {
        // Если у панели есть компонент Image (для работы с цветом)
        if (panelImage != null)
        {
            // Получаем альфа-канал цвета панели
            float panelAlpha = panelImage.color.a;

            // Устанавливаем альфа-канал изображения на тот же уровень
            Color imageColor = contentImage.color;
            imageColor.a = panelAlpha; // Синхронизируем прозрачность
            contentImage.color = imageColor; // Применяем новый цвет
        }
    }
}
