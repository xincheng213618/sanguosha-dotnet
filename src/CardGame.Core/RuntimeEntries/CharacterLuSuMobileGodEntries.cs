#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileGodRuntime> _resolve;
    private CharacterLuSuMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileGodRuntime> resolve) => new(resolve);
}
