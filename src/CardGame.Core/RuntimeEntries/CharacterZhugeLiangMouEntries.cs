#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhugeLiangMouRuntime))]
internal sealed partial class CharacterZhugeLiangMouEntries
{
    private readonly Func<EngineOwners.CharacterZhugeLiangMouRuntime> _resolve;
    private CharacterZhugeLiangMouEntries(Func<EngineOwners.CharacterZhugeLiangMouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeLiangMouEntries Create(Func<EngineOwners.CharacterZhugeLiangMouRuntime> resolve) => new(resolve);
}
