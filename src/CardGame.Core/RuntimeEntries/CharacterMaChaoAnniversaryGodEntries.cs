#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChaoAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _resolve;
    private CharacterMaChaoAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChaoAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> resolve) => new(resolve);
}
