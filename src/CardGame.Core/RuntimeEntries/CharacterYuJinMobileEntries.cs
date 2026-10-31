#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJinMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMobileRuntime> _resolve;
    private CharacterYuJinMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuJinMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMobileRuntime> resolve) => new(resolve);
}
