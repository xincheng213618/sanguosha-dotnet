#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDuFuRenOlRuntime))]
internal sealed partial class CharacterDuFuRenOlEntries
{
    private readonly Func<EngineOwners.CharacterDuFuRenOlRuntime> _resolve;
    private CharacterDuFuRenOlEntries(Func<EngineOwners.CharacterDuFuRenOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDuFuRenOlEntries Create(Func<EngineOwners.CharacterDuFuRenOlRuntime> resolve) => new(resolve);
}
