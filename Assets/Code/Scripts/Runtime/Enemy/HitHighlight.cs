using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class HitHighlight : MonoBehaviour
{
    const string ANIM_TIMER = "hit_anim_";
    const float ANIM_DURATION = .1f;

    bool _isAnimating = false;

    IDamageable[] _damageables;

    List<MeshRenderer> _renderers = new List<MeshRenderer>();
    List<Material> _oldMaterials = new List<Material>();

    [SerializeField] Material _highlightedMat;

    private void Awake()
    {
        _damageables = GetComponentsInChildren<IDamageable>();

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        _renderers.AddRange(renderers);
    }

    private void OnEnable()
    {
        foreach (var dmg in _damageables)
            dmg.OnTakeDamage += OnTookDamage;
    }

    private void OnDisable()
    {
        foreach (var dmg in _damageables)
            dmg.OnTakeDamage -= OnTookDamage;
    }

    void OnTookDamage(int _)
    {
        if (_renderers.Count == 0 || _isAnimating)
            return;

        SetOldVals();
        StartAnim();
    }

    void SetOldVals()
    {
        if(_oldMaterials.Count != 0)
            _oldMaterials.Clear();

        for (int i = 0; i < _renderers.Count; i++)
            _oldMaterials.Add(_renderers[i].material);
    }

    private void StartAnim()
    {
        _isAnimating = true;

        foreach (var rend in _renderers)
            rend.material = _highlightedMat;

        GameTimer.CreateTimer(ANIM_TIMER + GetInstanceID(), ANIM_DURATION, BackToOldVals);
    }

    void BackToOldVals()
    {
        _isAnimating = false;

        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material = _oldMaterials[i];
        }
    }
}
