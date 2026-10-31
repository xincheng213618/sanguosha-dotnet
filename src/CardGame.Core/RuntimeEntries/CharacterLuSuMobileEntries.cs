#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileRuntime> _resolve;
    private CharacterLuSuMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileRuntime> resolve) => new(resolve);
}
