using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class AbilitiesPanel : MonoBehaviour
{
    AbilityButtonController[] _abilityButtons;
    [SerializeField][Required] TextMeshProUGUI _abilityNameTextMesh;
    AbilityButtonController _selectedButton;
    AbilityButtonController _hoveredButton;
    int _count;
    bool _interactable;

    public bool interactable { get => _interactable; set => SetInteractable(value); }

    void Start()
    {
        _interactable = true;
        _abilityButtons = GetComponentsInChildren<AbilityButtonController>(true);
        _count = _abilityButtons.Length;
        HideAbilityName();

        for (int i = 0; i < _count; i++)
        {
            _abilityButtons[i].gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateAbilities(List<Ability> abilities)
    {
        if (abilities == null)
        {
            for (int i = 0; i < _count; i++)
            {
                _abilityButtons[i].gameObject.SetActive(false);
            }
            return;
        }


        for (int i = 0; i < abilities.Count; i++)
        {
            _abilityButtons[i].SetUp(abilities[i]);
            _abilityButtons[i].gameObject.SetActive(true);
        }
        for (int i = abilities.Count; i < _count; i++)
        {
            _abilityButtons[i].gameObject.SetActive(false);
        }
    }

    public void ShowAbilityName()
    {
        _abilityNameTextMesh.gameObject.SetActive(true);
    }

    public void HideAbilityName()
    {
        _abilityNameTextMesh.gameObject.SetActive(false);
    }

    public void UpdateAbilityName(string newName)
    {
        _abilityNameTextMesh.text = newName;
    }

    public void OnPointerEnteredButton(AbilityButtonController abc)
    {
        if (_interactable == false)
        {
            return;
        }

        _hoveredButton = abc;
        UpdateAbilityName(abc.AbilityName);
        ShowAbilityName();
    }

    public void OnPointerExitedButton(AbilityButtonController abc)
    {
        _hoveredButton = null;

        if (_selectedButton == null)
        {
            HideAbilityName();
        }
        else if (abc != _selectedButton)
        {
            UpdateAbilityName(_selectedButton.AbilityName);
        }
    }

    public void SetSelectedButton(AbilityButtonController btnController)
    {
        _selectedButton = btnController;

        foreach (var button in _abilityButtons)
        {
            if (button == btnController)
            {
                button.SetSelected(true);
            }
            else
            {
                button.SetSelected(false);
            }
        }

        if (_hoveredButton == null)
        {
            HideAbilityName();
        }
        else
        {
            UpdateAbilityName(_hoveredButton.AbilityName);
            ShowAbilityName();
        }
    }


    public void Reset()
    {
        SetSelectedButton(null);
    }

    public void SetInteractable(bool value)
    {
        _interactable = value;
        foreach (var button in _abilityButtons)
        {
            button.SetInteractable(value);
        }
    }


    public void SetSelectedAbility(Ability ability)
    {
        foreach (var button in _abilityButtons)
        {
            if (button.Ability == ability)
            {
                button.SetSelected(true);
            }
            else
            {
                button.SetSelected(false);
            }
        }

        if (_hoveredButton == null)
        {
            HideAbilityName();
        }
        else
        {
            UpdateAbilityName(_hoveredButton.AbilityName);
            ShowAbilityName();
        }
    }

}
