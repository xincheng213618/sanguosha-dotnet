namespace CardGame.Core;
public sealed partial record PlayerSnapshot
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<MouCaoPiControl>? MouCaoPiControls { get => _mouCaoPiControls; init => _mouCaoPiControls = value is null ? null : Array.AsReadOnly(value.ToArray()); }

    private readonly IReadOnlyList<MouCaoPiControl>? _mouCaoPiControls;
}
