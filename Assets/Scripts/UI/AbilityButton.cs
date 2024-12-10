using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _iconImage;
    [SerializeField] Image _selectionFrameImage;
    [SerializeField] AbilitiesPanel _parentPanel;
    Ability _ability;
    [SerializeField] Button _button;
    [SerializeField] Color _inactiveIconTint;
    [SerializeField] Color _activeIconTint = Color.white;
    bool _isSelected;
    bool _hovered;

    public bool IsSelected { get => _isSelected; }
    public string AbilityName { get => _ability != null ? _ability.Name : "no name"; }
    public bool Hovered { get => _hovered; }
    public Ability Ability { get => _ability; }

    private void Start()
    {
#if DEBUG
        Debug.Assert(_button != null);
        Debug.Assert(_iconImage != null);
        Debug.Assert(_selectionFrameImage != null);
        Debug.Assert(_parentPanel != null);
#endif

        _button.onClick.AddListener(() => _parentPanel.SetSelectedButton(this));
        _button.onClick.AddListener(() => PlayerActionManager.Instance.SetSelectedAbility(_ability));
    }

    public void SetUp(Ability ability)
    {
        _ability = ability;
        if (_ability.Sprite != null)
        {
            _iconImage.sprite = _ability.Sprite;
        }
        else
        {
            //TODO if sprite == null load a default sprite
            Debug.Log("missing sprite for ability");
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _parentPanel.OnPointerEnteredButton(this);
        _selectionFrameImage.gameObject.SetActive(true);
        _hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _parentPanel.OnPointerExitedButton(this);

        if (_isSelected == false)
        {
            _selectionFrameImage.gameObject.SetActive(false);
        }
        _hovered = false;
    }

    public void SetSelected(bool value)
    {
        _isSelected = value;
        if (_isSelected == false && _hovered == false)
        {
            _selectionFrameImage.gameObject.SetActive(false);
        }
    }

    public void SetInteractable(bool value)
    {
        _button.interactable = value;
        if (value)
        {
            _iconImage.color = _activeIconTint;
        }
        else
        {
            _iconImage.color = _inactiveIconTint;
        }
    }
}
