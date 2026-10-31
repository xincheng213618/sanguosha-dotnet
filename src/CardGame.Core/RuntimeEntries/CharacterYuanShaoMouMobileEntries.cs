#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanShaoMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoMouMobileRuntime> _resolve;
    private CharacterYuanShaoMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanShaoMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanShaoMouMobileRuntime> resolve) => new(resolve);
}
