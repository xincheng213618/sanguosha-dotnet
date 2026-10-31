#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoOlEntries
{
    internal SkillProgramStepOutcome BeginZhugeGuo(ProgramSkillFrame supplied, SkillProgramEffect e) => _resolve().BeginZhugeGuo(supplied, e);
    internal bool CanContinueZhugeGuo(ProgramSkillFrame f) => _resolve().CanContinueZhugeGuo(f);
    internal bool CanOfferZhugeGuo(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferZhugeGuo(candidate, trigger, context);
    internal void CaptureQirangDeclaredUse(IGameEvent payload) => _resolve().CaptureQirangDeclaredUse(payload);
    internal void FinishZhugeGuo(ProgramSkillFrame f) => _resolve().FinishZhugeGuo(f);
    internal CardSnapshot[] GetZhugeGuoPrivatelyViewedCards(int viewer) => _resolve().GetZhugeGuoPrivatelyViewedCards(viewer);
    internal void IssueQirangAcquiredBenefit(ProgramSkillFrame f, ZhugeGuoReceipt r, CardMovementRecord movement) => _resolve().IssueQirangAcquiredBenefit(f, r, movement);
    internal void MarkYuhuaTopPlacement(ProgramSkillFrame supplied, ProgramTopReorder state) => _resolve().MarkYuhuaTopPlacement(supplied, state);
    internal void ObserveMovementBatchZhugeGuo(CardMovementBatchBuilder batch, CardMovementRecord movement) => _resolve().ObserveMovementBatchZhugeGuo(batch, movement);
    internal void ObserveZhugeGuoMovement(CardMovementRecord movement) => _resolve().ObserveZhugeGuoMovement(movement);
    internal void ObserveZhugeGuoTurnAndDeath(IGameEvent payload) => _resolve().ObserveZhugeGuoTurnAndDeath(payload);
    internal void PublishQirangExtraChoice(ProgramSkillFrame f) => _resolve().PublishQirangExtraChoice(f);
    internal PromptChoice QirangExtraChoice(ProgramSkillFrame f, IReadOnlyList<int> targets) => _resolve().QirangExtraChoice(f, targets);
    internal IReadOnlyList<PromptChoice> QirangExtraChoices(ProgramSkillFrame f) => _resolve().QirangExtraChoices(f);
    internal bool QirangTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().QirangTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal void RequireZhugeGuo(ProgramSkillFrame f) => _resolve().RequireZhugeGuo(f);
    internal void ResolveQirangExtraChoice(PromptChoice choice) => _resolve().ResolveQirangExtraChoice(choice);
    internal bool ResumeZhugeGuo(long id) => _resolve().ResumeZhugeGuo(id);
    internal bool ReturnZhugeGuoMovement(ProgramSkillFrame f) => _resolve().ReturnZhugeGuoMovement(f);
    internal bool TryAdvanceZhugeGuoSubtree() => _resolve().TryAdvanceZhugeGuoSubtree();
    internal bool TryQirangEquipmentParent(int owner, ProgramSkillWindowContext context, scoped out CardUseFrame use) => _resolve().TryQirangEquipmentParent(owner, context, out use);
    internal bool TryQirangExtraParent(int owner, string skill, string instance, ProgramSkillWindowContext context, scoped out CardUseFrame use) => _resolve().TryQirangExtraParent(owner, skill, instance, context, out use);
    internal bool ValidQirangQualification(QirangUseQualification q) => _resolve().ValidQirangQualification(q);
    internal bool ZhugeGuoFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().ZhugeGuoFirstChild(root, child);
    internal ProgramSkillFrame? ZhugeGuoObserverRoot() => _resolve().ZhugeGuoObserverRoot();
}
