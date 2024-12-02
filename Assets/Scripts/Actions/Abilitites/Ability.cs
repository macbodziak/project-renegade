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
    private Texture _texture;
    [SerializeField][MinValue(0)] private int _cost;
    [SerializeField]
    InputManager.State _inputState;




    public string Name { get => _name; }
    public string Description { get => _description; }
    public Texture Texture { get => _texture; }
    public int Cost { get => _cost; }


    public abstract IAction GetAction();
}
