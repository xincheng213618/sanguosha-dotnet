#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDuXiOlRuntime))]
internal sealed partial class CharacterDuXiOlEntries
{
    private readonly Func<EngineOwners.CharacterDuXiOlRuntime> _resolve;
    private CharacterDuXiOlEntries(Func<EngineOwners.CharacterDuXiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuXiOlEntries Create(Func<EngineOwners.CharacterDuXiOlRuntime> resolve) => new(resolve);
}
