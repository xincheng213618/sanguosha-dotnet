#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceBeigongChildrenEntries
{
    internal bool AdvanceSourceBeigongChildren() => _resolve().AdvanceSourceBeigongChildren();
    internal bool SourceBeigongDying() => _resolve().SourceBeigongDying();
    internal bool SourceBeigongEdge(int i) => _resolve().SourceBeigongEdge(i);
    internal bool SourceBeigongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SourceBeigongFirstChild(f, child);
    internal bool SourceBeigongJudgmentEdge(int i) => _resolve().SourceBeigongJudgmentEdge(i);
    internal bool SourceBeigongOwnAttack(ProgramSkillFrame f, AttackAttemptState attack) => _resolve().SourceBeigongOwnAttack(f, attack);
    internal bool SourceBeigongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().SourceBeigongRecoveryParent(f, recovery);
    internal ProgramSkillFrame? SourceBeigongRoot() => _resolve().SourceBeigongRoot();
    internal bool SourceBeigongSuffix(int n) => _resolve().SourceBeigongSuffix(n);
}
