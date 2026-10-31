#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZouShiLeEntries
{
    internal bool AdvanceHuoxinChildren() => _resolve().AdvanceHuoxinChildren();
    internal void AnswerHuoxin(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHuoxin(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHuoxin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHuoxin(supplied);
    internal bool CanOfferHuoxin(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferHuoxin(candidate, trigger, context);
    internal bool CanOfferYunzheng(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferYunzheng(gate);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureYunzhengInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureYunzhengInitialEntities(window);
    internal long CaptureYunzhengQualificationStamp(long inner) => _resolve().CaptureYunzhengQualificationStamp(inner);
    internal void ContinueHuoxin(long id) => _resolve().ContinueHuoxin(id);
    internal void FinishHuoxin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHuoxin(f);
    internal bool HasLiveYunzhengSource(global::CardGame.Core.CharacterState owner) => _resolve().HasLiveYunzhengSource(owner);
    internal bool HasYunzhengEntityMark(int cardId) => _resolve().HasYunzhengEntityMark(cardId);
    internal bool HasYunzhengHandEntityMark(int ownerSeat, int cardId) => _resolve().HasYunzhengHandEntityMark(ownerSeat, cardId);
    internal global::CardGame.Core.PromptChoice HuoxinAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuoxinAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HuoxinChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuoxinChoices(f);
    internal bool HuoxinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HuoxinFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? HuoxinObserverRoot() => _resolve().HuoxinObserverRoot();
    internal bool HuoxinObserverSuffix(int index) => _resolve().HuoxinObserverSuffix(index);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeYunzheng(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().InitializeYunzheng(supplied);
    internal bool IsHuoxinMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsHuoxinMovement(f, effect, p);
    internal bool IsHuoxinRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsHuoxinRecoveryParent(f, recovery);
    internal bool IsIssuedYunzhengCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedYunzhengCandidate(candidate, context);
    internal bool IsYunzhengHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().IsYunzhengHandLimitExempt(owner, cardId);
    internal bool PermitIssuedYunzheng(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PermitIssuedYunzheng(frame);
    internal bool PermitPaidHuoxin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitPaidHuoxin(f);
    internal void ProjectYunzhengCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectYunzhengCardSkillMark(cardId, ref projection);
    internal void PublishHuoxin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHuoxin(f);
    internal bool ResumeHuoxin(long id) => _resolve().ResumeHuoxin(id);
    internal bool ReturnHuoxinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHuoxinMovement(f);
    internal bool TryGetHuoxinUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetHuoxinUse(owner, context, out window);
    internal bool ValidHuoxin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHuoxin(f);
    internal bool YunzhengBaseGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().YunzhengBaseGrantQualified(owner, grant);
    internal bool YunzhengNonLockedGrantQualified(global::CardGame.Core.CharacterState holder, global::CardGame.Core.SkillGrant grant) => _resolve().YunzhengNonLockedGrantQualified(holder, grant);
    internal bool HasLeZouShiCapability { get => _resolve().HasLeZouShiCapability; }
}
