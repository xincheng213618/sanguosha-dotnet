#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiWeiOlEntries
{
    internal bool AllowsSettleQingzhongActualPlayDebtTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsSettleQingzhongActualPlayDebtTrigger(gate);
    internal bool CanContinueIssuedLuZhiWei(ProgramSkillFrame f) => _resolve().CanContinueIssuedLuZhiWei(f);
    internal QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(CharacterState owner) => _resolve().CaptureQingzhongEndingPhase(owner);
    internal QingzhongPlayPhaseKey? CaptureQingzhongStartingPhase(CharacterState owner) => _resolve().CaptureQingzhongStartingPhase(owner);
    internal ProgramSkillWindowContext CreateQingzhongEndingContext(ProgramLifecycleTriggerWindowFrame f, ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CreateQingzhongEndingContext(f, c, context);
    internal ProgramSkillWindowContext CreateQingzhongStartingContext(PlayPhaseStartingBoundaryFrame boundary, ProgramSkillWindowContext context) => _resolve().CreateQingzhongStartingContext(boundary, context);
    internal SkillProgramStepOutcome DrawAndArmQingzhongActualPlayDebt(ProgramSkillFrame input) => _resolve().DrawAndArmQingzhongActualPlayDebt(input);
    internal void FinishQingzhong(ProgramSkillFrame f) => _resolve().FinishQingzhong(f);
    internal bool HasIssuedQingzhongDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().HasIssuedQingzhongDebtCandidate(c, w);
    internal bool IsExactQingzhongMovement(ProgramSkillFrame f, QingzhongPending p, CardMovementRecord m) => _resolve().IsExactQingzhongMovement(f, p, m);
    internal void IssueQingzhongMovement(ProgramSkillFrame f) => _resolve().IssueQingzhongMovement(f);
    internal IEnumerable<ProgramTriggerCandidate> IssuedQingzhongEndingCandidates(CharacterState owner) => _resolve().IssuedQingzhongEndingCandidates(owner);
    internal bool LuZhiWeiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().LuZhiWeiFirstChild(root, child);
    internal ProgramSkillFrame? LuZhiWeiObserverRoot() => _resolve().LuZhiWeiObserverRoot();
    internal void ObserveQingzhongDebtTermination(IGameEvent payload) => _resolve().ObserveQingzhongDebtTermination(payload);
    internal void PublishQingzhongMinimum(ProgramSkillFrame f) => _resolve().PublishQingzhongMinimum(f);
    internal int[] QingzhongMinimumSeats() => _resolve().QingzhongMinimumSeats();
    internal void ResolveQingzhongMinimumChoice(PromptChoice choice) => _resolve().ResolveQingzhongMinimumChoice(choice);
    internal bool ResumeQingzhong(long id) => _resolve().ResumeQingzhong(id);
    internal bool ReturnQingzhongMovement(ProgramSkillFrame f) => _resolve().ReturnQingzhongMovement(f);
    internal SkillProgramStepOutcome SettleQingzhongActualPlayDebt(ProgramSkillFrame input) => _resolve().SettleQingzhongActualPlayDebt(input);
    internal bool TryAdvanceLuZhiWeiSubtree() => _resolve().TryAdvanceLuZhiWeiSubtree();
}
