#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuYiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiMobileRuntime> _resolve;
    private CharacterWuYiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuYiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiMobileRuntime> resolve) => new(resolve);
}
