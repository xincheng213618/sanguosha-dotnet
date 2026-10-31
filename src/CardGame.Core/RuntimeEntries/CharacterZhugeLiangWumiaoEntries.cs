#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed partial class CharacterZhugeLiangWumiaoEntries
{
    private readonly Func<EngineOwners.CharacterZhugeLiangWumiaoRuntime> _resolve;
    private CharacterZhugeLiangWumiaoEntries(Func<EngineOwners.CharacterZhugeLiangWumiaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeLiangWumiaoEntries Create(Func<EngineOwners.CharacterZhugeLiangWumiaoRuntime> resolve) => new(resolve);
}
