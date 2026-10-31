#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMouMobileEntries
{
    internal bool AdvanceMouLuMengChildren() => _resolve().AdvanceMouLuMengChildren();
    internal void AnswerMouDuojing(PromptChoice selected) => _resolve().AnswerMouDuojing(selected);
    internal void AnswerMouDuojingTake(PromptChoice selected) => _resolve().AnswerMouDuojingTake(selected);
    internal SkillProgramStepOutcome BeginMouDuojing(ProgramSkillFrame supplied) => _resolve().BeginMouDuojing(supplied);
    internal bool CanActivateMouKeji(ProgramActivationLegalityContext c) => _resolve().CanActivateMouKeji(c);
    internal bool CanAwakenMouDujiang(ProgramTriggerLegalityContext c) => _resolve().CanAwakenMouDujiang(c);
    internal bool CanOfferMouDuojing(ProgramTriggerLegalityContext c) => _resolve().CanOfferMouDuojing(c);
    internal void CompleteMouDuojing(ProgramSkillFrame f) => _resolve().CompleteMouDuojing(f);
    internal void ContinueMouDuojing(long id) => _resolve().ContinueMouDuojing(id);
    internal bool DrainMouDuojing(long id) => _resolve().DrainMouDuojing(id);
    internal void FinishMouDuojingTake(ProgramSkillFrame f, int? card, int? sequence) => _resolve().FinishMouDuojingTake(f, card, sequence);
    internal bool IsMouLuMengDying() => _resolve().IsMouLuMengDying();
    internal bool IsMouLuMengPeachForbidden(int actor, CardKind kind, CardActionType type) => _resolve().IsMouLuMengPeachForbidden(actor, kind, type);
    internal bool IsMouLuMengPeachRescueAllowed(int actor) => _resolve().IsMouLuMengPeachRescueAllowed(actor);
    internal bool IsMouLuMengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouLuMengRecoveryParent(f, recovery);
    internal bool KeepPaidMouLuMeng(ProgramSkillFrame f) => _resolve().KeepPaidMouLuMeng(f);
    internal bool MouDujiangRoot(ProgramSkillFrame f) => _resolve().MouDujiangRoot(f);
    internal IReadOnlyList<PromptChoice> MouDuojingChoices(ProgramSkillFrame f) => _resolve().MouDuojingChoices(f);
    internal int MouDuojingPhaseSlashBonus(int owner) => _resolve().MouDuojingPhaseSlashBonus(owner);
    internal IReadOnlyList<PromptChoice> MouDuojingTakeChoices(ProgramSkillFrame f) => _resolve().MouDuojingTakeChoices(f);
    internal CardUseFrame? MouDuojingUse(ProgramSkillFrame f) => _resolve().MouDuojingUse(f);
    internal bool MouKejiCostWasPaid(ProgramSkillFrame f) => _resolve().MouKejiCostWasPaid(f);
    internal int MouKejiPhaseUses(int owner) => _resolve().MouKejiPhaseUses(owner);
    internal bool MouKejiRoot(ProgramSkillFrame f) => _resolve().MouKejiRoot(f);
    internal int MouLuMengArmorHandLimit(CharacterState owner) => _resolve().MouLuMengArmorHandLimit(owner);
    internal bool MouLuMengAwakened(int owner) => _resolve().MouLuMengAwakened(owner);
    internal bool MouLuMengCardUseForbidden(ProgramCardUseQuery query) => _resolve().MouLuMengCardUseForbidden(query);
    internal bool MouLuMengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouLuMengFirstChild(f, child);
    internal bool MouLuMengObserverOrigin(ProgramSkillFrame f) => _resolve().MouLuMengObserverOrigin(f);
    internal ProgramSkillFrame? MouLuMengObserverRoot() => _resolve().MouLuMengObserverRoot();
    internal bool MouLuMengObserverSuffix(int index) => _resolve().MouLuMengObserverSuffix(index);
    internal bool MouLuMengOwnDying(int owner) => _resolve().MouLuMengOwnDying(owner);
    internal ActualDiscardRecoveryPhaseKey MouLuMengPhase() => _resolve().MouLuMengPhase();
    internal int MouLuMengProjectedPhaseUses(int owner, string skill, int ordinary) => _resolve().MouLuMengProjectedPhaseUses(owner, skill, ordinary);
    internal void OfferMouDuojingTarget(long id) => _resolve().OfferMouDuojingTarget(id);
    internal void PublishMouDuojingChoice(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices, string message) => _resolve().PublishMouDuojingChoice(f, choices, message);
    internal SkillProgramStepOutcome ResolveMouDujiang(ProgramSkillFrame supplied) => _resolve().ResolveMouDujiang(supplied);
    internal SkillProgramStepOutcome ResolveMouKeji(ProgramSkillFrame supplied) => _resolve().ResolveMouKeji(supplied);
    internal bool ResumeMouDuojing(long id) => _resolve().ResumeMouDuojing(id);
    internal bool ReturnMouDuojingMovement(ProgramSkillFrame f) => _resolve().ReturnMouDuojingMovement(f);
    internal bool ReturnMouKejiDiscard(ProgramSkillFrame f) => _resolve().ReturnMouKejiDiscard(f);
    internal PromptChoice SelectAiMouDuojing(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiMouDuojing(p, f);
    internal bool ValidMouDujiang(ProgramSkillFrame f) => _resolve().ValidMouDujiang(f);
    internal bool ValidMouDuojing(ProgramSkillFrame f) => _resolve().ValidMouDuojing(f);
    internal bool ValidMouKeji(ProgramSkillFrame f) => _resolve().ValidMouKeji(f);
    internal bool HasMouLuMengCapability { get => _resolve().HasMouLuMengCapability; }
}
