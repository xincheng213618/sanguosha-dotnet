#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangYunOlRuntime))]
internal sealed partial class CharacterWangYunOlEntries
{
    private readonly Func<EngineOwners.CharacterWangYunOlRuntime> _resolve;
    private CharacterWangYunOlEntries(Func<EngineOwners.CharacterWangYunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangYunOlEntries Create(Func<EngineOwners.CharacterWangYunOlRuntime> resolve) => new(resolve);
}
