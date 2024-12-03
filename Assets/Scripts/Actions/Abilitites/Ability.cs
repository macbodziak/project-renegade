using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "BaseAbility", menuName = "Abilitites/Base Ability")]
public abstract class Ability : ScriptableObject
{
    [SerializeField]
    [Required]
    private string _name;
    [SerializeField]
    [Required]
    [TextArea(3, 6)]
    private string _description;
    [SerializeField]
    [Required]
    private Sprite _sprite;
    [SerializeField][MinValue(0)] private int _cost;
    [SerializeField]
    InputManager.State _inputState;




    public string Name { get => _name; }
    public string Description { get => _description; }
    public Sprite Sprite { get => _sprite; }
    public int Cost { get => _cost; }
    public InputManager.State InputState { get => _inputState; }

    public abstract IAction GetAction();
}
