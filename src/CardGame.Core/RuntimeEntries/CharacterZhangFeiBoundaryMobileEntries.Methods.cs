#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangFeiBoundaryMobileEntries
{
    internal bool AdmitBoundaryZhangFeiSuppression(ProgramSkillFrame frame, int target) => _resolve().AdmitBoundaryZhangFeiSuppression(frame, target);
    internal void AssertBoundaryZhangFeiUses() => _resolve().AssertBoundaryZhangFeiUses();
    internal int BoundaryZhangFeiLiyongCardDamage(GameEngine.IDamageAttempt attack) => _resolve().BoundaryZhangFeiLiyongCardDamage(attack);
    internal bool BoundaryZhangFeiOwnPlay(int owner) => _resolve().BoundaryZhangFeiOwnPlay(owner);
    internal bool BoundaryZhangFeiPaoxiaoDistance(ProgramSlashDistanceQuery query) => _resolve().BoundaryZhangFeiPaoxiaoDistance(query);
    internal CardUseFrame? BoundaryZhangFeiPenaltyUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().BoundaryZhangFeiPenaltyUse(candidate, context);
    internal BoundaryLiyongReserve? BoundaryZhangFeiReady(int owner, string instance) => _resolve().BoundaryZhangFeiReady(owner, instance);
    internal CardConversionSource BoundaryZhangFeiSource(ProgramSkillFrame frame) => _resolve().BoundaryZhangFeiSource(frame);
    internal bool BoundaryZhangFeiSourceMatches(BoundaryLiyongUse right, ProgramTriggerCandidate candidate) => _resolve().BoundaryZhangFeiSourceMatches(right, candidate);
    internal CardUseFrame? BoundaryZhangFeiWindowUse(ProgramSkillWindowContext? context) => _resolve().BoundaryZhangFeiWindowUse(context);
    internal bool CanOfferBoundaryZhangFei(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryZhangFei(candidate, context);
    internal bool ContinueBoundaryZhangFeiPenalty(ProgramSkillFrame frame) => _resolve().ContinueBoundaryZhangFeiPenalty(frame);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeBoundaryZhangFeiPenalty(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeBoundaryZhangFeiPenalty(owner, window, ordinary);
    internal void IncludeBoundaryZhangFeiTargets(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().IncludeBoundaryZhangFeiTargets(action, window, result);
    internal bool IsBoundaryZhangFeiLiyongUnrespondable(long frameId) => _resolve().IsBoundaryZhangFeiLiyongUnrespondable(frameId);
    internal bool IsIssuedBoundaryZhangFei(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedBoundaryZhangFei(candidate, context);
    internal SkillProgramStepOutcome PayBoundaryZhangFeiLiyong(ProgramSkillFrame frame) => _resolve().PayBoundaryZhangFeiLiyong(frame);
    internal SkillProgramStepOutcome ResolveBoundaryZhangFei(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().ResolveBoundaryZhangFei(supplied, operation);
    internal bool ResumeBoundaryZhangFeiPenalty(long id) => _resolve().ResumeBoundaryZhangFeiPenalty(id);
    internal bool ValidBoundaryZhangFeiLiyongPenalty(ProgramSkillFrame frame) => _resolve().ValidBoundaryZhangFeiLiyongPenalty(frame);
}
