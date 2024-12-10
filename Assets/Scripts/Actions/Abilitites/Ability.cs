using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "BaseAbility", menuName = "Abilitites/Base Ability")]
public abstract class Ability : ScriptableObject
{
    #region fields
    [SerializeField]
    [Required]
    private string _name;
    [SerializeField]
    [Required]
    [Multiline(3)]
    private string _description;
    [SerializeField]
    [Required, PreviewField]
    private Sprite _sprite;
    [SerializeField][MinValue(0)] private int _cost;
    #endregion


    #region properties
    public string Name { get => _name; }
    public string Description { get => _description; }
    public Sprite Sprite { get => _sprite; }
    public int Cost { get => _cost; }
    public abstract InputManager.State InputState { get; }
    #endregion


    #region methods
    //TODO give this method a better name
    public abstract List<ICommand> GetCommands(AbilityArgs args);
    #endregion
}
