#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangCanOlRuntime))]
internal sealed partial class CharacterWangCanOlEntries
{
    private readonly Func<EngineOwners.CharacterWangCanOlRuntime> _resolve;
    private CharacterWangCanOlEntries(Func<EngineOwners.CharacterWangCanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangCanOlEntries Create(Func<EngineOwners.CharacterWangCanOlRuntime> resolve) => new(resolve);
}
