using Euphrates;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HitmarkerHandler : MonoBehaviour
{
    [SerializeField] float _openingSequenceLength = 0.25f;
    [SerializeField] float _closingSequenceLength = 0.25f;

    [SerializeField] TriggerChannelSO _enemyHitTrigger;
    CanvasGroup _group;
    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _enemyHitTrigger.AddListener(OnEnemyHit);
        _group.alpha = 0;
    }

    private void OnDisable()
    {
        _enemyHitTrigger.RemoveListener(OnEnemyHit);  
    }

    void OnEnemyHit()
    {
        void OnHitmarkShown()
        {
            _group.DoAlpha(0, _closingSequenceLength);
        }

        _group.alpha = 0;
        _group.DoAlpha(1, _openingSequenceLength, Ease.Lerp, null, OnHitmarkShown);
    }
}
