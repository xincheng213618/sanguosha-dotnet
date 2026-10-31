#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineQuotaTopProgramsRuntime))]
internal sealed partial class CoreGameEngineQuotaTopProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> _resolve;
    private CoreGameEngineQuotaTopProgramsEntries(Func<EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineQuotaTopProgramsEntries Create(Func<EngineOwners.CoreGameEngineQuotaTopProgramsRuntime> resolve) => new(resolve);
}
