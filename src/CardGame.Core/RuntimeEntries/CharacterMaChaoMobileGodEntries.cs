#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaChaoMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime> _resolve;
    private CharacterMaChaoMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChaoMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoMobileGodRuntime> resolve) => new(resolve);
}
