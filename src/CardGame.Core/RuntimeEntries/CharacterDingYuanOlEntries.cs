#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDingYuanOlRuntime))]
internal sealed partial class CharacterDingYuanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingYuanOlRuntime> _resolve;
    private CharacterDingYuanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingYuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDingYuanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingYuanOlRuntime> resolve) => new(resolve);
}
