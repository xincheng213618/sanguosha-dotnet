#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangFeiBoundaryMobileEntries
{
    internal bool AdmitBoundaryZhangFeiSuppression(global::CardGame.Core.ProgramSkillFrame frame, int target) => _resolve().AdmitBoundaryZhangFeiSuppression(frame, target);
    internal void AssertBoundaryZhangFeiPenalty(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertBoundaryZhangFeiPenalty(frame);
    internal void AssertBoundaryZhangFeiUses() => _resolve().AssertBoundaryZhangFeiUses();
    internal int BoundaryZhangFeiLiyongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().BoundaryZhangFeiLiyongCardDamage(attack);
    internal bool BoundaryZhangFeiOwnPlay(int owner) => _resolve().BoundaryZhangFeiOwnPlay(owner);
    internal bool BoundaryZhangFeiPaoxiaoDistance(global::CardGame.Core.ProgramSlashDistanceQuery query) => _resolve().BoundaryZhangFeiPaoxiaoDistance(query);
    internal global::CardGame.Core.CardUseFrame? BoundaryZhangFeiPenaltyUse(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().BoundaryZhangFeiPenaltyUse(candidate, context);
    internal global::CardGame.Core.BoundaryLiyongReserve? BoundaryZhangFeiReady(int owner, string instance) => _resolve().BoundaryZhangFeiReady(owner, instance);
    internal global::CardGame.Core.CardConversionSource BoundaryZhangFeiSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundaryZhangFeiSource(frame);
    internal bool BoundaryZhangFeiSourceMatches(global::CardGame.Core.BoundaryLiyongUse right, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().BoundaryZhangFeiSourceMatches(right, candidate);
    internal global::CardGame.Core.CardUseFrame? BoundaryZhangFeiWindowUse(global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().BoundaryZhangFeiWindowUse(context);
    internal bool CanOfferBoundaryZhangFei(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryZhangFei(candidate, context);
    internal bool ContinueBoundaryZhangFeiPenalty(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueBoundaryZhangFeiPenalty(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeBoundaryZhangFeiPenalty(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeBoundaryZhangFeiPenalty(owner, window, ordinary);
    internal void IncludeBoundaryZhangFeiTargets(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().IncludeBoundaryZhangFeiTargets(action, window, result);
    internal bool IsBoundaryZhangFeiLiyongUnrespondable(long frameId) => _resolve().IsBoundaryZhangFeiLiyongUnrespondable(frameId);
    internal bool IsIssuedBoundaryZhangFei(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedBoundaryZhangFei(candidate, context);
    internal global::CardGame.Core.SkillProgramStepOutcome PayBoundaryZhangFeiLiyong(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PayBoundaryZhangFeiLiyong(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveBoundaryZhangFei(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ResolveBoundaryZhangFei(supplied, operation);
    internal bool ResumeBoundaryZhangFeiPenalty(long id) => _resolve().ResumeBoundaryZhangFeiPenalty(id);
    internal bool ValidBoundaryZhangFeiLiyongPenalty(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidBoundaryZhangFeiLiyongPenalty(frame);
}
