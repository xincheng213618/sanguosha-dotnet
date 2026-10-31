#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceBeigongChildrenEntries
{
    internal bool AdvanceSourceBeigongChildren() => _resolve().AdvanceSourceBeigongChildren();
    internal bool SourceBeigongDying() => _resolve().SourceBeigongDying();
    internal bool SourceBeigongEdge(int i) => _resolve().SourceBeigongEdge(i);
    internal bool SourceBeigongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SourceBeigongFirstChild(f, child);
    internal bool SourceBeigongJudgmentEdge(int i) => _resolve().SourceBeigongJudgmentEdge(i);
    internal bool SourceBeigongOwnAttack(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.AttackAttemptState attack) => _resolve().SourceBeigongOwnAttack(f, attack);
    internal bool SourceBeigongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().SourceBeigongRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? SourceBeigongRoot() => _resolve().SourceBeigongRoot();
    internal bool SourceBeigongSuffix(int n) => _resolve().SourceBeigongSuffix(n);
}
