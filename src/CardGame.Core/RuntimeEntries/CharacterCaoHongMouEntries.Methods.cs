#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoHongMouEntries
{
    internal MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().CurrentMouCaoHongDebt(c, w);
    internal bool IsIssuedMouCaoHongCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedMouCaoHongCandidate(c, w);
    internal IEnumerable<ProgramTriggerCandidate> IssuedMouCaoHongPlayEndingCandidates(CharacterState owner) => _resolve().IssuedMouCaoHongPlayEndingCandidates(owner);
    internal bool MouCaoHongAdvanceChildren() => _resolve().MouCaoHongAdvanceChildren();
    internal bool MouCaoHongCanActivate(ProgramActivationLegalityContext c) => _resolve().MouCaoHongCanActivate(c);
    internal RuleQueryEvaluation MouCaoHongDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original) => _resolve().MouCaoHongDistance(source, target, original);
    internal bool MouCaoHongDying() => _resolve().MouCaoHongDying();
    internal bool MouCaoHongEdge(int i) => _resolve().MouCaoHongEdge(i);
    internal bool MouCaoHongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouCaoHongFirstChild(f, child);
    internal bool MouCaoHongMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouCaoHongMovement(f, effect, p);
    internal bool MouCaoHongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouCaoHongRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouCaoHongRoot() => _resolve().MouCaoHongRoot();
    internal bool MouCaoHongSuffix(int n) => _resolve().MouCaoHongSuffix(n);
    internal bool ValidMouCaoHongDebt(MouCaoHongXianjuIssuedEvent d) => _resolve().ValidMouCaoHongDebt(d);
    internal MouCaoHongRuntime MouCaoHong { get => _resolve().MouCaoHong; }
}
