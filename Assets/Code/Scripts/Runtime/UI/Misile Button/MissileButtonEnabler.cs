using Euphrates;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MissileButtonEnabler : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] FloatSO _timer;

    [SerializeField] GameObject _shootText;

    [Space]
    [SerializeField] Animator _buttonAnimator;
    [SerializeField] string _idleAnim;
    [SerializeField] string _shootAnim;

    public event Action<TickInfo> OnTimerTick;
    public event Action OnTimerEnd;

    readonly string TIMER_NAME = "MISSLE BUTTON TIMER";

    private void Start()
    {
        EnableButton();
    }

    private void OnDisable()
    {
        GameTimer.CancleTimer(TIMER_NAME);
        EnableButton();
    }

    public void EnableButton()
    {
        _button.interactable = true;
        _buttonAnimator.Play(_shootAnim);
        _shootText.SetActive(true);
        OnTimerEnd?.Invoke();
    }

    public void DisableButton()
    {
        _button.interactable = false;
        _shootText.SetActive(false);
        _buttonAnimator.Play(_idleAnim);
    }

    public void DisableButtonTimer()
    {
        DisableButton();
        GameTimer.CreateTimer(TIMER_NAME, _timer.Value, EnableButton, OnTimerTick);
    }
}
