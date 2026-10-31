#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHanFuOlRuntime))]
internal sealed partial class CharacterHanFuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanFuOlRuntime> _resolve;
    private CharacterHanFuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanFuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanFuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanFuOlRuntime> resolve) => new(resolve);
}
