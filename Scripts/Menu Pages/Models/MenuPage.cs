using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MenuPage : MonoBehaviour
{
    public abstract Transform PageObject { get; }

    protected PlayerInput _playerInput;
    
    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    protected virtual void OnEnable()
    {
        _playerInput.EnableUIInput();
        DoSelectFirst();
        StartCoroutine(SelectFirst());
    }

    protected virtual void OnDisable()
    {
        _playerInput.EnableGameplayInput();
    }

    protected IEnumerator SelectFirst()
    {
        yield return new WaitForSeconds(.2f);
        DoSelectFirst();
    }
    
    protected void DoSelectFirst()
    {
        var firstChild = GetComponentsInChildren<Selectable>().First();
        EventSystem.current.SetSelectedGameObject(firstChild.gameObject);
    }
}
