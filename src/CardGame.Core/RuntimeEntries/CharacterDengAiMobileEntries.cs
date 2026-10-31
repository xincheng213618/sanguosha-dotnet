#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileRuntime> _resolve;
    private CharacterDengAiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileRuntime> resolve) => new(resolve);
}
