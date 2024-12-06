public struct SelectedAbilityChangedEventArgs
{
    Ability _previousAbility;
    Ability _currentAbility;

    public SelectedAbilityChangedEventArgs(Ability previousAbility, Ability currentAbility)
    {
        _previousAbility = previousAbility;
        _currentAbility = currentAbility;
    }

    public Ability PreviousAbility { get => _previousAbility; }
    public Ability CurrentAbility { get => _currentAbility; }
}
