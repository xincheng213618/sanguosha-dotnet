#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuJiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuJiMobileRuntime> _resolve;
    private CharacterLuJiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuJiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuJiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuJiMobileRuntime> resolve) => new(resolve);
}
