#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoZhenMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileRuntime> _resolve;
    private CharacterCaoZhenMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhenMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileRuntime> resolve) => new(resolve);
}
