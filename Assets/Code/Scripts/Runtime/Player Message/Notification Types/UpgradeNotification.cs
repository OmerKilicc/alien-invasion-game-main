using Euphrates;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UpgradeNotification : MonoBehaviour
{
    const float ANIM_DURATION = 1f;

    [SerializeField] NotifyChannel _channel;
    [SerializeField] List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();

    Queue<TextMeshProUGUI> _queue = new Queue<TextMeshProUGUI>();

    private void Awake()
    {
        foreach (TextMeshProUGUI text in _texts)
            _queue.Enqueue(text);
    }

    private void OnEnable()
    {
        _channel.OnUpgradeNotification += NotificationSent;
    }

    private void OnDisable()
    {
        _channel.OnUpgradeNotification -= NotificationSent;
    }

    void NotificationSent(string message, float duration)
    {
        TextMeshProUGUI text = _queue.Dequeue();

        text.gameObject.SetActive(true);
        text.text = message;

        CanvasGroup cg = text.GetComponent<CanvasGroup>();
        cg.DoAlpha(1f, ANIM_DURATION, Ease.Lerp, null, () => HideNotification(text, duration));
    }

    async void HideNotification(TextMeshProUGUI text, float duration)
    {
        await Task.Delay((int)(duration * 1000f));

        void BackToQueue(TextMeshProUGUI txt)
        {
            _queue.Enqueue(txt);
            txt.gameObject.SetActive(false);
        }

        CanvasGroup cg = text.GetComponent<CanvasGroup>();
        cg.DoAlpha(0f, ANIM_DURATION, Ease.Lerp, null, () => BackToQueue(text));
    }
}
