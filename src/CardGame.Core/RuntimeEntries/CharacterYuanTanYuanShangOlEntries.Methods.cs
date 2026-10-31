#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanTanYuanShangOlEntries
{
    internal SkillProgramStepOutcome BeginNeifa(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginNeifa(supplied, effect);
    internal bool CanContinueNeifa(ProgramSkillFrame f) => _resolve().CanContinueNeifa(f);
    internal bool CanOfferNeifa(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferNeifa(c, t, w);
    internal void CaptureNeifaPaidEquipmentUse(long useId, CardActionContext action) => _resolve().CaptureNeifaPaidEquipmentUse(useId, action);
    internal void CaptureNeifaPaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureNeifaPaymentBatch(batch);
    internal void CollectIssuedNeifaCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedNeifaCandidates(action, window, result);
    internal NeifaTurnState? CurrentNeifa(int seat) => _resolve().CurrentNeifa(seat);
    internal bool DrainNeifa(ProgramSkillFrame f) => _resolve().DrainNeifa(f);
    internal bool ExactNeifaCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context, scoped out CardUseFrame use) => _resolve().ExactNeifaCandidate(c, context, out use);
    internal void ExpireNeifaTurn() => _resolve().ExpireNeifaTurn();
    internal void FinishNeifa(ProgramSkillFrame f) => _resolve().FinishNeifa(f);
    internal bool HasIssuedNeifaCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().HasIssuedNeifaCandidate(c, w);
    internal bool HasNeifaTrickTargetCancellation(CardUseFrame use) => _resolve().HasNeifaTrickTargetCancellation(use);
    internal bool IsNeifaTargetsResolvedFact(CardUseFrame use, NeifaTargetsResolvedEvent fact, IReadOnlyList<int> original) => _resolve().IsNeifaTargetsResolvedFact(use, fact, original);
    internal bool IsNeifaUseForbidden(int actor, CardKind kind, CardActionType type) => _resolve().IsNeifaUseForbidden(actor, kind, type);
    internal PromptChoice NeifaAi(ProgramSkillFrame f) => _resolve().NeifaAi(f);
    internal PromptChoice NeifaChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().NeifaChoice(f, branch, label, cards, targets);
    internal IReadOnlyList<PromptChoice> NeifaChoices(ProgramSkillFrame f) => _resolve().NeifaChoices(f);
    internal IReadOnlyList<Card> NeifaFieldCards(int actor) => _resolve().NeifaFieldCards(actor);
    internal bool NeifaFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().NeifaFinalizedAfterCategoryTargetWindow(query);
    internal bool NeifaFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().NeifaFirstChild(root, child);
    internal bool NeifaNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().NeifaNormalizeImplicitDrawTwoTargetWindow(query);
    internal ProgramSkillFrame? NeifaObserverRoot() => _resolve().NeifaObserverRoot();
    internal IReadOnlyList<int> NeifaPrimaryShape(CardUseFrame use) => _resolve().NeifaPrimaryShape(use);
    internal int NeifaSlashLimitBonus(int seat) => _resolve().NeifaSlashLimitBonus(seat);
    internal int NeifaSlashTargetBonus(int seat, CardKind kind) => _resolve().NeifaSlashTargetBonus(seat, kind);
    internal bool NeifaSourceCurrent(ProgramSkillFrame f) => _resolve().NeifaSourceCurrent(f);
    internal IReadOnlyList<PromptChoice> NeifaTargetChoices(ProgramSkillFrame f) => _resolve().NeifaTargetChoices(f);
    internal RuleQueryContribution? NeifaTargetCountContribution(ProgramTargetCountQuery query) => _resolve().NeifaTargetCountContribution(query);
    internal int NeifaX(int seat, bool basicBranch) => _resolve().NeifaX(seat, basicBranch);
    internal void ObserveNeifaTurnBoundary(IGameEvent payload) => _resolve().ObserveNeifaTurnBoundary(payload);
    internal void PayNeifaDraw(ProgramSkillFrame f, int requested, bool equipment) => _resolve().PayNeifaDraw(f, requested, equipment);
    internal void PublishNeifa(ProgramSkillFrame f) => _resolve().PublishNeifa(f);
    internal void RecordNeifaSegment(long frameId, NeifaPaidSegment s) => _resolve().RecordNeifaSegment(frameId, s);
    internal void ResolveNeifaChoice(PromptChoice choice) => _resolve().ResolveNeifaChoice(choice);
    internal void ResolveNeifaTargets(ProgramSkillFrame f, PromptChoice choice) => _resolve().ResolveNeifaTargets(f, choice);
    internal bool ResumeNeifa(long id) => _resolve().ResumeNeifa(id);
    internal bool ReturnNeifaMovement(ProgramSkillFrame f) => _resolve().ReturnNeifaMovement(f);
    internal bool TryAdvanceNeifaSubtree() => _resolve().TryAdvanceNeifaSubtree();
    internal bool ValidNeifaEquipment(NeifaEquipmentUseProof q, CardUseFrame use) => _resolve().ValidNeifaEquipment(q, use);
    internal bool HasNeifaCapability { get => _resolve().HasNeifaCapability; }
}
