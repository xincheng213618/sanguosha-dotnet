#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGuoTuOlRuntime))]
internal sealed partial class CharacterGuoTuOlEntries
{
    private readonly Func<EngineOwners.CharacterGuoTuOlRuntime> _resolve;
    private CharacterGuoTuOlEntries(Func<EngineOwners.CharacterGuoTuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoTuOlEntries Create(Func<EngineOwners.CharacterGuoTuOlRuntime> resolve) => new(resolve);
}
