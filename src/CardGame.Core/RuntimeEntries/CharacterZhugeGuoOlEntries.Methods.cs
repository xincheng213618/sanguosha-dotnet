#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhugeGuo(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect e) => _resolve().BeginZhugeGuo(supplied, e);
    internal bool CanContinueZhugeGuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueZhugeGuo(f);
    internal bool CanOfferZhugeGuo(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferZhugeGuo(candidate, trigger, context);
    internal void CaptureQirangDeclaredUse(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureQirangDeclaredUse(payload);
    internal void FinishZhugeGuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhugeGuo(f);
    internal global::CardGame.Core.CardSnapshot[] GetZhugeGuoPrivatelyViewedCards(int viewer) => _resolve().GetZhugeGuoPrivatelyViewedCards(viewer);
    internal bool HasZhugeGuoBeforeDamageObserver(long id) => _resolve().HasZhugeGuoBeforeDamageObserver(id);
    internal bool HasZhugeGuoDamageObserver(long id) => _resolve().HasZhugeGuoDamageObserver(id);
    internal bool IsZhugeGuoMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsZhugeGuoMovement(f, e, p);
    internal void IssueQirangAcquiredBenefit(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ZhugeGuoReceipt r, global::CardGame.Core.CardMovementRecord movement) => _resolve().IssueQirangAcquiredBenefit(f, r, movement);
    internal void MarkYuhuaTopPlacement(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.ProgramTopReorder state) => _resolve().MarkYuhuaTopPlacement(supplied, state);
    internal void ObserveMovementBatchZhugeGuo(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementRecord movement) => _resolve().ObserveMovementBatchZhugeGuo(batch, movement);
    internal void ObserveZhugeGuoMovement(global::CardGame.Core.CardMovementRecord movement) => _resolve().ObserveZhugeGuoMovement(movement);
    internal void ObserveZhugeGuoTurnAndDeath(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveZhugeGuoTurnAndDeath(payload);
    internal void PublishQirangExtraChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishQirangExtraChoice(f);
    internal global::CardGame.Core.PromptChoice QirangExtraChoice(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().QirangExtraChoice(f, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QirangExtraChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QirangExtraChoices(f);
    internal bool QirangTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().QirangTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal void RequireZhugeGuo(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireZhugeGuo(f);
    internal void ResolveQirangExtraChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveQirangExtraChoice(choice);
    internal bool ResumeZhugeGuo(long id) => _resolve().ResumeZhugeGuo(id);
    internal bool ReturnZhugeGuoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhugeGuoMovement(f);
    internal bool TryAdvanceZhugeGuoSubtree() => _resolve().TryAdvanceZhugeGuoSubtree();
    internal bool TryQirangEquipmentParent(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().TryQirangEquipmentParent(owner, context, out use);
    internal bool TryQirangExtraParent(int owner, string skill, string instance, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().TryQirangExtraParent(owner, skill, instance, context, out use);
    internal bool ValidQirangQualification(global::CardGame.Core.QirangUseQualification q) => _resolve().ValidQirangQualification(q);
    internal bool ZhugeGuoFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhugeGuoFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? ZhugeGuoObserverRoot() => _resolve().ZhugeGuoObserverRoot();
}
