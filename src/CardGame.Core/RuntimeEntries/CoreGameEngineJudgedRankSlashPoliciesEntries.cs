#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime))]
internal sealed partial class CoreGameEngineJudgedRankSlashPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> _resolve;
    private CoreGameEngineJudgedRankSlashPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJudgedRankSlashPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJudgedRankSlashPoliciesRuntime> resolve) => new(resolve);
}
