#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _resolve;
    private CharacterCaoPiAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoPiAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiAnniversaryGodRuntime> resolve) => new(resolve);
}
