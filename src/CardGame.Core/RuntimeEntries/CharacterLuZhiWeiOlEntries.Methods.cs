#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiWeiOlEntries
{
    internal bool AllowsSettleQingzhongActualPlayDebtTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsSettleQingzhongActualPlayDebtTrigger(gate);
    internal bool CanContinueIssuedLuZhiWei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedLuZhiWei(f);
    internal global::CardGame.Core.QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(global::CardGame.Core.CharacterState owner) => _resolve().CaptureQingzhongEndingPhase(owner);
    internal global::CardGame.Core.QingzhongPlayPhaseKey? CaptureQingzhongStartingPhase(global::CardGame.Core.CharacterState owner) => _resolve().CaptureQingzhongStartingPhase(owner);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateQingzhongEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateQingzhongEndingContext(f, c, context);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateQingzhongStartingContext(global::CardGame.Core.PlayPhaseStartingBoundaryFrame boundary, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateQingzhongStartingContext(boundary, context);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawAndArmQingzhongActualPlayDebt(global::CardGame.Core.ProgramSkillFrame input) => _resolve().DrawAndArmQingzhongActualPlayDebt(input);
    internal void FinishQingzhong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishQingzhong(f);
    internal bool HasIssuedQingzhongDebtCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HasIssuedQingzhongDebtCandidate(c, w);
    internal bool HasLuZhiWeiBeforeDamageObserver(long id) => _resolve().HasLuZhiWeiBeforeDamageObserver(id);
    internal bool HasLuZhiWeiDamageObserver(long id) => _resolve().HasLuZhiWeiDamageObserver(id);
    internal bool IsExactQingzhongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.QingzhongPending p, global::CardGame.Core.CardMovementRecord m) => _resolve().IsExactQingzhongMovement(f, p, m);
    internal bool IsLuZhiWeiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsLuZhiWeiMovement(f, e, p);
    internal void IssueQingzhongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueQingzhongMovement(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedQingzhongEndingCandidates(global::CardGame.Core.CharacterState owner) => _resolve().IssuedQingzhongEndingCandidates(owner);
    internal bool LuZhiWeiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().LuZhiWeiFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? LuZhiWeiObserverRoot() => _resolve().LuZhiWeiObserverRoot();
    internal void ObserveQingzhongDebtTermination(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveQingzhongDebtTermination(payload);
    internal void PublishQingzhongMinimum(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishQingzhongMinimum(f);
    internal int[] QingzhongMinimumSeats() => _resolve().QingzhongMinimumSeats();
    internal void ResolveQingzhongMinimumChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveQingzhongMinimumChoice(choice);
    internal bool ResumeQingzhong(long id) => _resolve().ResumeQingzhong(id);
    internal bool ReturnQingzhongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnQingzhongMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleQingzhongActualPlayDebt(global::CardGame.Core.ProgramSkillFrame input) => _resolve().SettleQingzhongActualPlayDebt(input);
    internal bool TryAdvanceLuZhiWeiSubtree() => _resolve().TryAdvanceLuZhiWeiSubtree();
}
