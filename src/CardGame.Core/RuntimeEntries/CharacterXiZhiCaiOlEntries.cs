#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXiZhiCaiOlRuntime))]
internal sealed partial class CharacterXiZhiCaiOlEntries
{
    private readonly Func<EngineOwners.CharacterXiZhiCaiOlRuntime> _resolve;
    private CharacterXiZhiCaiOlEntries(Func<EngineOwners.CharacterXiZhiCaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiZhiCaiOlEntries Create(Func<EngineOwners.CharacterXiZhiCaiOlRuntime> resolve) => new(resolve);
}
