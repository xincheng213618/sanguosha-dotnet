#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime))]
internal sealed partial class CharacterLiJueOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> _resolve;
    private CharacterLiJueOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiJueOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiJueOlRuntime> resolve) => new(resolve);
}
