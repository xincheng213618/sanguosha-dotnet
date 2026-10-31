#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterKanZeOlRuntime))]
internal sealed partial class CharacterKanZeOlEntries
{
    private readonly Func<EngineOwners.CharacterKanZeOlRuntime> _resolve;
    private CharacterKanZeOlEntries(Func<EngineOwners.CharacterKanZeOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKanZeOlEntries Create(Func<EngineOwners.CharacterKanZeOlRuntime> resolve) => new(resolve);
}
