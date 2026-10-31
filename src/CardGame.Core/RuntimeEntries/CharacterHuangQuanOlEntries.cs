#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed partial class CharacterHuangQuanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> _resolve;
    private CharacterHuangQuanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangQuanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangQuanOlRuntime> resolve) => new(resolve);
}
