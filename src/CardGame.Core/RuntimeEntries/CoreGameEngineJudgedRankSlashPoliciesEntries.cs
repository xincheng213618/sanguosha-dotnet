#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime))]
internal sealed partial class CoreGameEngineJudgedRankSlashPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> _resolve;
    private CoreGameEngineJudgedRankSlashPoliciesEntries(Func<EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgedRankSlashPoliciesEntries Create(Func<EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> resolve) => new(resolve);
}
