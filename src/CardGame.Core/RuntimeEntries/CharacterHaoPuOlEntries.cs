#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime))]
internal sealed partial class CharacterHaoPuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime> _resolve;
    private CharacterHaoPuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHaoPuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoPuOlRuntime> resolve) => new(resolve);
}
