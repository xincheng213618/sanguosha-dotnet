#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiMobileRuntime> _resolve;
    private CharacterDianWeiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiMobileRuntime> resolve) => new(resolve);
}
