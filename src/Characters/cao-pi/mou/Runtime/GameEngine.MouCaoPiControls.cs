namespace CardGame.Core;
public sealed partial record PlayerSnapshot
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<MouCaoPiControl>? MouCaoPiControls { get => _mouCaoPiControls; init => _mouCaoPiControls = value is null ? null : Array.AsReadOnly(value.ToArray()); }

    private readonly IReadOnlyList<MouCaoPiControl>? _mouCaoPiControls;
}

public sealed partial class GameEngine
{
    // Issued lasting restrictions, independent of the source skill and pending
    // instruction. Normal command replay rebuilds this per-match state.
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.MouCaoPiControl> _mouCaoPiControls => ref _characterCaoPiMou.ReferenceToMouCaoPiControls;
}
