//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Code/Scripts/Runtime/Inputs/InputControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControls"",
    ""maps"": [
        {
            ""name"": ""GameInputs"",
            ""id"": ""198c3075-008a-4c33-a90d-5f6f4d35b979"",
            ""actions"": [
                {
                    ""name"": ""TouchInput"",
                    ""type"": ""Value"",
                    ""id"": ""cb12a1fe-8eea-4ace-b340-c4b229b7d39a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ad3efeb1-997b-4618-a464-55875c884f54"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameInputs
        m_GameInputs = asset.FindActionMap("GameInputs", throwIfNotFound: true);
        m_GameInputs_TouchInput = m_GameInputs.FindAction("TouchInput", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GameInputs
    private readonly InputActionMap m_GameInputs;
    private List<IGameInputsActions> m_GameInputsActionsCallbackInterfaces = new List<IGameInputsActions>();
    private readonly InputAction m_GameInputs_TouchInput;
    public struct GameInputsActions
    {
        private @InputControls m_Wrapper;
        public GameInputsActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchInput => m_Wrapper.m_GameInputs_TouchInput;
        public InputActionMap Get() { return m_Wrapper.m_GameInputs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameInputsActions set) { return set.Get(); }
        public void AddCallbacks(IGameInputsActions instance)
        {
            if (instance == null || m_Wrapper.m_GameInputsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameInputsActionsCallbackInterfaces.Add(instance);
            @TouchInput.started += instance.OnTouchInput;
            @TouchInput.performed += instance.OnTouchInput;
            @TouchInput.canceled += instance.OnTouchInput;
        }

        private void UnregisterCallbacks(IGameInputsActions instance)
        {
            @TouchInput.started -= instance.OnTouchInput;
            @TouchInput.performed -= instance.OnTouchInput;
            @TouchInput.canceled -= instance.OnTouchInput;
        }

        public void RemoveCallbacks(IGameInputsActions instance)
        {
            if (m_Wrapper.m_GameInputsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameInputsActions instance)
        {
            foreach (var item in m_Wrapper.m_GameInputsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameInputsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameInputsActions @GameInputs => new GameInputsActions(this);
    public interface IGameInputsActions
    {
        void OnTouchInput(InputAction.CallbackContext context);
    }
}
