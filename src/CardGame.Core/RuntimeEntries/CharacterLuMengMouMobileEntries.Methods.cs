#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMouMobileEntries
{
    internal bool AdvanceMouLuMengChildren() => _resolve().AdvanceMouLuMengChildren();
    internal void AnswerMouDuojing(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouDuojing(selected);
    internal void AnswerMouDuojingTake(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouDuojingTake(selected);
    internal void AssertMouLuMeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouLuMeng(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouDuojing(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouDuojing(supplied);
    internal bool CanActivateMouKeji(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateMouKeji(c);
    internal bool CanAwakenMouDujiang(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanAwakenMouDujiang(c);
    internal bool CanOfferMouDuojing(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferMouDuojing(c);
    internal void CompleteMouDuojing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteMouDuojing(f);
    internal void ContinueMouDuojing(long id) => _resolve().ContinueMouDuojing(id);
    internal bool DrainMouDuojing(long id) => _resolve().DrainMouDuojing(id);
    internal void FinishMouDuojingTake(global::CardGame.Core.ProgramSkillFrame f, int? card, int? sequence) => _resolve().FinishMouDuojingTake(f, card, sequence);
    internal bool HasMouLuMengBeforeDamageObserver(long id) => _resolve().HasMouLuMengBeforeDamageObserver(id);
    internal bool HasMouLuMengDamageObserver(long id) => _resolve().HasMouLuMengDamageObserver(id);
    internal bool IsMouLuMengDying() => _resolve().IsMouLuMengDying();
    internal bool IsMouLuMengPeachForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().IsMouLuMengPeachForbidden(actor, kind, type);
    internal bool IsMouLuMengPeachRescueAllowed(int actor) => _resolve().IsMouLuMengPeachRescueAllowed(actor);
    internal bool IsMouLuMengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouLuMengRecoveryParent(f, recovery);
    internal bool KeepPaidMouLuMeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMouLuMeng(f);
    internal bool MouDujiangRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDujiangRoot(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouDuojingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDuojingChoices(f);
    internal int MouDuojingPhaseSlashBonus(int owner) => _resolve().MouDuojingPhaseSlashBonus(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouDuojingTakeChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDuojingTakeChoices(f);
    internal global::CardGame.Core.CardUseFrame? MouDuojingUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDuojingUse(f);
    internal bool MouKejiCostWasPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouKejiCostWasPaid(f);
    internal int MouKejiPhaseUses(int owner) => _resolve().MouKejiPhaseUses(owner);
    internal bool MouKejiRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouKejiRoot(f);
    internal int MouLuMengArmorHandLimit(global::CardGame.Core.CharacterState owner) => _resolve().MouLuMengArmorHandLimit(owner);
    internal bool MouLuMengAwakened(int owner) => _resolve().MouLuMengAwakened(owner);
    internal bool MouLuMengCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().MouLuMengCardUseForbidden(query);
    internal bool MouLuMengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouLuMengFirstChild(f, child);
    internal bool MouLuMengObserverOrigin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLuMengObserverOrigin(f);
    internal global::CardGame.Core.ProgramSkillFrame? MouLuMengObserverRoot() => _resolve().MouLuMengObserverRoot();
    internal bool MouLuMengObserverSuffix(int index) => _resolve().MouLuMengObserverSuffix(index);
    internal bool MouLuMengOwnDying(int owner) => _resolve().MouLuMengOwnDying(owner);
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey MouLuMengPhase() => _resolve().MouLuMengPhase();
    internal int MouLuMengProjectedPhaseUses(int owner, string skill, int ordinary) => _resolve().MouLuMengProjectedPhaseUses(owner, skill, ordinary);
    internal void OfferMouDuojingTarget(long id) => _resolve().OfferMouDuojingTarget(id);
    internal void PublishMouDuojingChoice(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string message) => _resolve().PublishMouDuojingChoice(f, choices, message);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouDujiang(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMouDujiang(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouKeji(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMouKeji(supplied);
    internal bool ResumeMouDuojing(long id) => _resolve().ResumeMouDuojing(id);
    internal bool ReturnMouDuojingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouDuojingMovement(f);
    internal bool ReturnMouKejiDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouKejiDiscard(f);
    internal global::CardGame.Core.PromptChoice SelectAiMouDuojing(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiMouDuojing(p, f);
    internal bool ValidMouDujiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouDujiang(f);
    internal bool ValidMouDuojing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouDuojing(f);
    internal bool ValidMouKeji(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouKeji(f);
    internal bool HasMouLuMengCapability { get => _resolve().HasMouLuMengCapability; }
}
