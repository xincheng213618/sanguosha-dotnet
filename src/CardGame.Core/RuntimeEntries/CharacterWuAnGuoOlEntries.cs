#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWuAnGuoOlRuntime))]
internal sealed partial class CharacterWuAnGuoOlEntries
{
    private readonly Func<EngineOwners.CharacterWuAnGuoOlRuntime> _resolve;
    private CharacterWuAnGuoOlEntries(Func<EngineOwners.CharacterWuAnGuoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuAnGuoOlEntries Create(Func<EngineOwners.CharacterWuAnGuoOlRuntime> resolve) => new(resolve);
}
