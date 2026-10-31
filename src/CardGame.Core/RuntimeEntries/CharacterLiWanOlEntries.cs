#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiWanOlRuntime))]
internal sealed partial class CharacterLiWanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiWanOlRuntime> _resolve;
    private CharacterLiWanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiWanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiWanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiWanOlRuntime> resolve) => new(resolve);
}
