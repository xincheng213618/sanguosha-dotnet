#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangXiuMouOlRuntime))]
internal sealed partial class CharacterZhangXiuMouOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangXiuMouOlRuntime> _resolve;
    private CharacterZhangXiuMouOlEntries(Func<EngineOwners.CharacterZhangXiuMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangXiuMouOlEntries Create(Func<EngineOwners.CharacterZhangXiuMouOlRuntime> resolve) => new(resolve);
}
