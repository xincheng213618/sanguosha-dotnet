#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoHongMouEntries
{
    internal global::CardGame.Core.MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CurrentMouCaoHongDebt(c, w);
    internal bool IsIssuedMouCaoHongCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedMouCaoHongCandidate(c, w);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedMouCaoHongPlayEndingCandidates(global::CardGame.Core.CharacterState owner) => _resolve().IssuedMouCaoHongPlayEndingCandidates(owner);
    internal bool MouCaoHongAdvanceChildren() => _resolve().MouCaoHongAdvanceChildren();
    internal bool MouCaoHongBeforeDamage(long id) => _resolve().MouCaoHongBeforeDamage(id);
    internal bool MouCaoHongCanActivate(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().MouCaoHongCanActivate(c);
    internal bool MouCaoHongDamageWindow(long id) => _resolve().MouCaoHongDamageWindow(id);
    internal global::CardGame.Core.RuleQueryEvaluation MouCaoHongDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.RuleQueryEvaluation original) => _resolve().MouCaoHongDistance(source, target, original);
    internal bool MouCaoHongDying() => _resolve().MouCaoHongDying();
    internal bool MouCaoHongEdge(int i) => _resolve().MouCaoHongEdge(i);
    internal bool MouCaoHongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouCaoHongFirstChild(f, child);
    internal bool MouCaoHongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouCaoHongMovement(f, effect, p);
    internal bool MouCaoHongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouCaoHongRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouCaoHongRoot() => _resolve().MouCaoHongRoot();
    internal bool MouCaoHongSuffix(int n) => _resolve().MouCaoHongSuffix(n);
    internal bool ValidMouCaoHongDebt(global::CardGame.Core.MouCaoHongXianjuIssuedEvent d) => _resolve().ValidMouCaoHongDebt(d);
    internal global::CardGame.Core.MouCaoHongRuntime MouCaoHong { get => _resolve().MouCaoHong; }
}
