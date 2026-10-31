#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWuTuGuOlRuntime))]
internal sealed partial class CharacterWuTuGuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuTuGuOlRuntime> _resolve;
    private CharacterWuTuGuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuTuGuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuTuGuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuTuGuOlRuntime> resolve) => new(resolve);
}
