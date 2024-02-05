using Euphrates;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMessageHandler))]
public class PlayerMessageHandlerInspector : Editor
{
    PlayerMessageHandler _target;

    private void OnEnable()
    {
        _target = (PlayerMessageHandler)target;
    }

    string _newName = "";
    PlayerMessageType _newType = PlayerMessageType.CommanderMessage;
    string _newMessage = "";
    float _newDuration = 1f;
    List<TriggerChannelSO> _newTriggers = new List<TriggerChannelSO>();
    TriggerChannelSO _newTrigger;

    Dictionary<int, bool> _toggles = new Dictionary<int, bool>();

    void ResetNewValsToDefault()
    {
        _newName = "";
        _newType = PlayerMessageType.CommanderMessage;
        _newMessage = "";
        _newDuration = 1f;
        _newTriggers.Clear();
        _newTrigger = null;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        EditorGUILayout.Space(10);

        if (_target.Messages == null)
            _target.Messages = new PlayerMessageData[0];

        if (_target.Messages.Length == 0)
            using (new EditorGUILayout.VerticalScope("HelpBox"))
            {
                EditorGUILayout.LabelField("No Messages");
            }
        else
            DisplayMessageDatas();

        EditorGUILayout.Space(20);

        DisplayAddNewData();
    }

    void DisplayMessageDatas()
    {
        List<int> removedMessages = new List<int>();

        using (new EditorGUILayout.VerticalScope("HelpBox"))
        {
            for (int i = 0; i < _target.Messages.Length; i++)
            {
                int id = _target.Messages[i].ID;

                if (!_toggles.ContainsKey(id))
                    _toggles.Add(id, false);

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(_target.Messages[i].Name);

                    string toggleStr = _toggles[id] ? "Hide" : "Show";

                    if (GUILayout.Button(toggleStr))
                        _toggles[id] = !_toggles[id];
                }

                if (!_toggles[id])
                {
                    Seperate(i, _target.Messages.Length);
                    continue;
                }

                using (new EditorGUILayout.VerticalScope("GroupBox"))
                {
                    _target.Messages[i].Name = EditorGUILayout.TextField("Name: ", _target.Messages[i].Name);
                    _target.Messages[i].MessageType = (PlayerMessageType)EditorGUILayout.EnumPopup("Message Type: ", _target.Messages[i].MessageType);

                    if (_target.Messages[i].MessageType != PlayerMessageType.CommanderMessageConstant)
                        _target.Messages[i].Duration = EditorGUILayout.FloatField("Duration: ", _target.Messages[i].Duration);

                    EditorStyles.textField.wordWrap = true;
                    EditorGUILayout.LabelField("Message:");
                    _target.Messages[i].Message = EditorGUILayout.TextArea(_target.Messages[i].Message, GUILayout.ExpandHeight(true));

                    EditorGUILayout.Space(5);

                    List<TriggerChannelSO> triggers = _target.Messages[i].Triggers.ToList();
                    DisplayObjectList(id, triggers, "Triggers");
                    _target.Messages[i].Triggers = triggers.ToArray();

                    if (GUILayout.Button("Remove Message"))
                        removedMessages.Add(_target.Messages[i].ID);
                }

                Seperate(i, _target.Messages.Length);
            }

            List<PlayerMessageData> msgs = _target.Messages.ToList();

            foreach (var id in removedMessages)
            {
                msgs.RemoveAll(m => m.ID == id);
                _toggles.Remove(id);
            }

            _target.Messages = msgs.ToArray();
        }
    }

    void DisplayAddNewData()
    {
        using (new EditorGUILayout.VerticalScope("HelpBox"))
        {
            EditorGUILayout.LabelField("Add New Message");

            using (new EditorGUILayout.VerticalScope("GroupBox"))
            {
                _newName = EditorGUILayout.TextField("Name: ", _newName);
                _newType = (PlayerMessageType)EditorGUILayout.EnumPopup("Message Type: ", _newType);

                if (_newType != PlayerMessageType.CommanderMessageConstant)
                    _newDuration = EditorGUILayout.FloatField("Duration: ", _newDuration);

                EditorStyles.textField.wordWrap = true;
                EditorGUILayout.LabelField("Message:");
                _newMessage = EditorGUILayout.TextArea(_newMessage, GUILayout.ExpandHeight(true));

                EditorGUILayout.Space(5);

                using (new EditorGUILayout.VerticalScope("GroupBox"))
                {
                    EditorGUILayout.LabelField("Triggers:");

                    List<TriggerChannelSO> toBeRemoved = new List<TriggerChannelSO>();

                    for (int j = 0; j < _newTriggers.Count; j++)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            _newTriggers[j] = (TriggerChannelSO)EditorGUILayout.ObjectField(_newTriggers[j], typeof(TriggerChannelSO), false);

                            if (GUILayout.Button("Remove"))
                                toBeRemoved.Add(_newTriggers[j]);
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        _newTrigger = (TriggerChannelSO)EditorGUILayout.ObjectField(_newTrigger, typeof(TriggerChannelSO), false);

                        if (_newTrigger != null)
                        {
                            if (GUILayout.Button("Add"))
                                _newTriggers.Add(_newTrigger);
                        }
                    }

                    foreach (var tbr in toBeRemoved)
                        _newTriggers.Remove(tbr);
                }

                EditorGUILayout.Space(5);

                if (GUILayout.Button("Create"))
                {
                    List<PlayerMessageData> datas = _target.Messages.ToList();
                    int id = Random.Range(1000000, 9999999);

                    datas.Add(new PlayerMessageData()
                    {
                        ID = id,
                        Name = _newName,
                        MessageType = _newType,
                        Message = _newMessage,
                        Duration = _newDuration,
                        Triggers = _newTriggers.ToArray()
                    });

                    _target.Messages = datas.ToArray();

                    ResetNewValsToDefault();

                    _toggles.Add(id, true);
                }
            }
        }
    }

    void Seperate(int index, int count)
    {
        if (index == count - 1)
            return;

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    Dictionary<int, Object> _savedAddedObjects = new Dictionary<int, Object>();
    void DisplayObjectList<T>(int id, List<T> list, string name) where T : Object
    {
        using (new EditorGUILayout.VerticalScope("GroupBox"))
        {
            EditorGUILayout.LabelField(name + ":");
            List<T> toBeRemoved = new List<T>();

            for (int j = 0; j < list.Count; j++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    list[j] = (T)EditorGUILayout.ObjectField(list[j], typeof(T), false);

                    if (GUILayout.Button("Remove"))
                        toBeRemoved.Add(list[j]);
                }
            }

            if (!_savedAddedObjects.ContainsKey(id))
                _savedAddedObjects.Add(id, null);

            using (new EditorGUILayout.HorizontalScope())
            {
                _savedAddedObjects[id] = EditorGUILayout.ObjectField(_savedAddedObjects[id], typeof(T), false);

                if (_savedAddedObjects[id] != null)
                {
                    if (GUILayout.Button("Add"))
                    {
                        list.Add((T)_savedAddedObjects[id]);
                        _savedAddedObjects[id] = null;
                    }
                }
            }

            foreach (var tbr in toBeRemoved)
                list.Remove(tbr);
        }
    }

    Dictionary<int, int> _savedAddedInts = new Dictionary<int, int>();
    void DisplayIntList(int id, List<int> list, string name)
    {
        using (new EditorGUILayout.VerticalScope("GroupBox"))
        {
            EditorGUILayout.LabelField(name + ":");
            List<int> toBeRemoved = new List<int>();

            for (int j = 0; j < list.Count; j++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    list[j] = EditorGUILayout.IntField(list[j]);

                    if (GUILayout.Button("Remove"))
                        toBeRemoved.Add(j);
                }
            }

            if (!_savedAddedObjects.ContainsKey(id))
                _savedAddedObjects.Add(id, null);

            using (new EditorGUILayout.HorizontalScope())
            {
                _savedAddedInts[id] = EditorGUILayout.IntField(_savedAddedInts[id]);

                if (_savedAddedInts.ContainsKey(id))
                {
                    if (GUILayout.Button("Add"))
                    {
                        list.Add(_savedAddedInts[id]);
                        _savedAddedInts[id] = 0;
                    }
                }
            }

            foreach (var tbr in toBeRemoved.OrderByDescending(v => v))
                list.RemoveAt(tbr);
        }
    }
}
