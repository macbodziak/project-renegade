using Navigation;

public class MovementArgs : AbilityArgs
{
    private Unit _unit;
    private Path _path;

    public MovementArgs(Unit unit, Path path)
    {
        _unit = unit;
        _path = path;
    }

    public Unit Unit { get => _unit; }
    public Path Path { get => _path; }
}
