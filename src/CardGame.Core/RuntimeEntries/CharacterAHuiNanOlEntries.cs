#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime))]
internal sealed partial class CharacterAHuiNanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> _resolve;
    private CharacterAHuiNanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterAHuiNanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> resolve) => new(resolve);
}
