#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMobileRuntime> _resolve;
    private CharacterLuZhiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMobileRuntime> resolve) => new(resolve);
}
