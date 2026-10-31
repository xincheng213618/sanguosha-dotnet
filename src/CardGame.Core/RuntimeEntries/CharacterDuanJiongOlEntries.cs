#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime))]
internal sealed partial class CharacterDuanJiongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> _resolve;
    private CharacterDuanJiongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuanJiongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuanJiongOlRuntime> resolve) => new(resolve);
}
