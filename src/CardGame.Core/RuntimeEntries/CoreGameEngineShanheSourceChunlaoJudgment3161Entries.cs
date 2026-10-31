#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceChunlaoJudgment3161Runtime))]
internal sealed partial class CoreGameEngineShanheSourceChunlaoJudgment3161Entries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceChunlaoJudgment3161Runtime> _resolve;
    private CoreGameEngineShanheSourceChunlaoJudgment3161Entries(Func<EngineOwners.CoreGameEngineShanheSourceChunlaoJudgment3161Runtime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceChunlaoJudgment3161Entries Create(Func<EngineOwners.CoreGameEngineShanheSourceChunlaoJudgment3161Runtime> resolve) => new(resolve);
}
