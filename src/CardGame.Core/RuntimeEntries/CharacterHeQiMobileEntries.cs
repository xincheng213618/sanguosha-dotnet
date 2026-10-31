#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileRuntime> _resolve;
    private CharacterHeQiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeQiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeQiMobileRuntime> resolve) => new(resolve);
}
