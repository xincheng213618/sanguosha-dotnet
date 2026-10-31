#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangGuanOlRuntime))]
internal sealed partial class CharacterWangGuanOlEntries
{
    private readonly Func<EngineOwners.CharacterWangGuanOlRuntime> _resolve;
    private CharacterWangGuanOlEntries(Func<EngineOwners.CharacterWangGuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangGuanOlEntries Create(Func<EngineOwners.CharacterWangGuanOlRuntime> resolve) => new(resolve);
}
