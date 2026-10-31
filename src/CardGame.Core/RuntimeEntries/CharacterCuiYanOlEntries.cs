#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed partial class CharacterCuiYanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> _resolve;
    private CharacterCuiYanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCuiYanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> resolve) => new(resolve);
}
