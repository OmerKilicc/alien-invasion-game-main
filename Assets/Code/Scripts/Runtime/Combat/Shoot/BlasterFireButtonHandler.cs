using Euphrates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlasterFireButtonHandler : MonoBehaviour
{
    [SerializeField] FloatSO _fireInterval;
    Button _fireButton;
    float _nextFire = 0f;

    private void Start()
    {
        _fireButton = GetComponent<Button>();
    }

    private void Update()
    {
        if (Time.time > _nextFire)
        {
            _fireButton.interactable = true;
            _nextFire = Time.time + _fireInterval;
        }
        else
        {
            _fireButton.interactable = false;
        }

    }

}
