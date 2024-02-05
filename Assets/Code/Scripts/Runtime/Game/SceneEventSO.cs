using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Scene Event", menuName = "SO Channels/Scene")] 
public class SceneEventSO : ScriptableObject
{
    public UnityEvent<Scene> OnTrigger;

    public void Invoke(Scene scene) => OnTrigger?.Invoke(scene);
}
