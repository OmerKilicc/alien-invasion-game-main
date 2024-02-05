using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    const int CLICK_AMOUNT = 5;
    const float CLICK_WAIT = 1f;

    [SerializeField] TMP_InputField _input;
    [SerializeField] Button _enableButton;

    List<IConsoleCommand> _commands;

    private void Awake() => _commands = GetComponentsInChildren<IConsoleCommand>().ToList();

    private void OnEnable()
    {
        _input.onSubmit.AddListener(Submitted);
        _enableButton.onClick.AddListener(EnableButtonClicked);
    }

    private void OnDisable()
    {
        _input.onSubmit.RemoveListener(Submitted);
        _enableButton.onClick.RemoveListener(EnableButtonClicked);
    }

    void Submitted(string text)
    {
        string[] command = text.Split(' ');
        foreach (var cmd in _commands)
        {
            if (cmd.CommandString.ToLower() != command[0].ToLower())
                continue;

            cmd.Execute(this, command.Skip(1).ToArray());
        }
    }

    float _timePassed = 0;
    int _clickCount = 0;
    void EnableButtonClicked()
    {
        _timePassed = 0f;

        if (++_clickCount < CLICK_AMOUNT)
            return;

        _clickCount = 0;

        _input.gameObject.SetActive(true);
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;

        if (_timePassed < CLICK_WAIT)
            return;

        _timePassed = 0f;
        _clickCount = 0;
    }
}
