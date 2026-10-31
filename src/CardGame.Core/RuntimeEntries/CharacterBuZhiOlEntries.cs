#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBuZhiOlRuntime))]
internal sealed partial class CharacterBuZhiOlEntries
{
    private readonly Func<EngineOwners.CharacterBuZhiOlRuntime> _resolve;
    private CharacterBuZhiOlEntries(Func<EngineOwners.CharacterBuZhiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBuZhiOlEntries Create(Func<EngineOwners.CharacterBuZhiOlRuntime> resolve) => new(resolve);
}
