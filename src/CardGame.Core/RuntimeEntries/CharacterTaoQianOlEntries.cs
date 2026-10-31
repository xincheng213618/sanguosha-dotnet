#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTaoQianOlRuntime))]
internal sealed partial class CharacterTaoQianOlEntries
{
    private readonly Func<EngineOwners.CharacterTaoQianOlRuntime> _resolve;
    private CharacterTaoQianOlEntries(Func<EngineOwners.CharacterTaoQianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaoQianOlEntries Create(Func<EngineOwners.CharacterTaoQianOlRuntime> resolve) => new(resolve);
}
