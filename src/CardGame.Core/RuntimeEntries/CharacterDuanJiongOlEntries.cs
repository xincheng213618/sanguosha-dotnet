#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuanJiongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> _resolve;
    private CharacterDuanJiongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuanJiongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> resolve) => new(resolve);
}
