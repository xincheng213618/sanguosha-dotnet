#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> _resolve;
    private CharacterLuBuMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> resolve) => new(resolve);
}
