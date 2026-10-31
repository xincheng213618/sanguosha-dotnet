#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> _resolve;
    private CharacterDengAiMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMouOlRuntime> resolve) => new(resolve);
}
