#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLvQianOlRuntime))]
internal sealed partial class CharacterLvQianOlEntries
{
    private readonly Func<EngineOwners.CharacterLvQianOlRuntime> _resolve;
    private CharacterLvQianOlEntries(Func<EngineOwners.CharacterLvQianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvQianOlEntries Create(Func<EngineOwners.CharacterLvQianOlRuntime> resolve) => new(resolve);
}
