#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaMiDiOlRuntime))]
internal sealed partial class CharacterMaMiDiOlEntries
{
    private readonly Func<EngineOwners.CharacterMaMiDiOlRuntime> _resolve;
    private CharacterMaMiDiOlEntries(Func<EngineOwners.CharacterMaMiDiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaMiDiOlEntries Create(Func<EngineOwners.CharacterMaMiDiOlRuntime> resolve) => new(resolve);
}
