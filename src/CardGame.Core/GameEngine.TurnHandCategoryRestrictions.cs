namespace CardGame.Core;

public sealed partial record PlayerSnapshot
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<TurnHandCategoryRestriction>? TurnHandCategoryRestrictions
    {
        get => _handCategoryRestrictions;
        init => _handCategoryRestrictions = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    private readonly IReadOnlyList<TurnHandCategoryRestriction>? _handCategoryRestrictions;
}
