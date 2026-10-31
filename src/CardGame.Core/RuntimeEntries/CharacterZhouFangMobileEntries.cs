#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFangMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFangMobileRuntime> _resolve;
    private CharacterZhouFangMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFangMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFangMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFangMobileRuntime> resolve) => new(resolve);
}
