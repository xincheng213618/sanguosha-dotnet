#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWuTuGuOlRuntime))]
internal sealed partial class CharacterWuTuGuOlEntries
{
    private readonly Func<EngineOwners.CharacterWuTuGuOlRuntime> _resolve;
    private CharacterWuTuGuOlEntries(Func<EngineOwners.CharacterWuTuGuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuTuGuOlEntries Create(Func<EngineOwners.CharacterWuTuGuOlRuntime> resolve) => new(resolve);
}
