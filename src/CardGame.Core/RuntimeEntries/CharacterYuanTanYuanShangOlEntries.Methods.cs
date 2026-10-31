#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanTanYuanShangOlEntries
{
    internal void AssertNeifaFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertNeifaFrame(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginNeifa(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginNeifa(supplied, effect);
    internal bool CanContinueNeifa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueNeifa(f);
    internal bool CanOfferNeifa(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferNeifa(c, t, w);
    internal void CaptureNeifaPaidEquipmentUse(long useId, global::CardGame.Core.CardActionContext action) => _resolve().CaptureNeifaPaidEquipmentUse(useId, action);
    internal void CaptureNeifaPaymentBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureNeifaPaymentBatch(batch);
    internal void CollectIssuedNeifaCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedNeifaCandidates(action, window, result);
    internal global::CardGame.Core.NeifaTurnState? CurrentNeifa(int seat) => _resolve().CurrentNeifa(seat);
    internal bool DrainNeifa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainNeifa(f);
    internal bool ExactNeifaCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().ExactNeifaCandidate(c, context, out use);
    internal void ExpireNeifaTurn() => _resolve().ExpireNeifaTurn();
    internal void FinishNeifa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishNeifa(f);
    internal bool HasIssuedNeifaCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HasIssuedNeifaCandidate(c, w);
    internal bool HasNeifaBeforeDamageObserver(long id) => _resolve().HasNeifaBeforeDamageObserver(id);
    internal bool HasNeifaDamageObserver(long id) => _resolve().HasNeifaDamageObserver(id);
    internal bool HasNeifaTrickTargetCancellation(global::CardGame.Core.CardUseFrame use) => _resolve().HasNeifaTrickTargetCancellation(use);
    internal bool IsNeifaDying() => _resolve().IsNeifaDying();
    internal bool IsNeifaMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsNeifaMovement(f, e, p);
    internal bool IsNeifaTargetsResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.NeifaTargetsResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original) => _resolve().IsNeifaTargetsResolvedFact(use, fact, original);
    internal bool IsNeifaUseForbidden(int actor, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardActionType type) => _resolve().IsNeifaUseForbidden(actor, kind, type);
    internal global::CardGame.Core.PromptChoice NeifaAi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NeifaAi(f);
    internal global::CardGame.Core.PromptChoice NeifaChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().NeifaChoice(f, branch, label, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NeifaChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NeifaChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NeifaFieldCards(int actor) => _resolve().NeifaFieldCards(actor);
    internal bool NeifaFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().NeifaFinalizedAfterCategoryTargetWindow(query);
    internal bool NeifaFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().NeifaFirstChild(root, child);
    internal bool NeifaNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().NeifaNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.ProgramSkillFrame? NeifaObserverRoot() => _resolve().NeifaObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<int> NeifaPrimaryShape(global::CardGame.Core.CardUseFrame use) => _resolve().NeifaPrimaryShape(use);
    internal int NeifaSlashLimitBonus(int seat) => _resolve().NeifaSlashLimitBonus(seat);
    internal int NeifaSlashTargetBonus(int seat, global::CardGame.Core.CardKind kind) => _resolve().NeifaSlashTargetBonus(seat, kind);
    internal bool NeifaSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NeifaSourceCurrent(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> NeifaTargetChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NeifaTargetChoices(f);
    internal global::CardGame.Core.RuleQueryContribution? NeifaTargetCountContribution(global::CardGame.Core.ProgramTargetCountQuery query) => _resolve().NeifaTargetCountContribution(query);
    internal int NeifaX(int seat, bool basicBranch) => _resolve().NeifaX(seat, basicBranch);
    internal void ObserveNeifaTurnBoundary(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveNeifaTurnBoundary(payload);
    internal void PayNeifaDraw(global::CardGame.Core.ProgramSkillFrame f, int requested, bool equipment) => _resolve().PayNeifaDraw(f, requested, equipment);
    internal void PublishNeifa(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishNeifa(f);
    internal void RecordNeifaSegment(long frameId, global::CardGame.Core.NeifaPaidSegment s) => _resolve().RecordNeifaSegment(frameId, s);
    internal void ResolveNeifaChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveNeifaChoice(choice);
    internal void ResolveNeifaTargets(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveNeifaTargets(f, choice);
    internal bool ResumeNeifa(long id) => _resolve().ResumeNeifa(id);
    internal bool ReturnNeifaMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnNeifaMovement(f);
    internal bool TryAdvanceNeifaSubtree() => _resolve().TryAdvanceNeifaSubtree();
    internal bool ValidNeifaEquipment(global::CardGame.Core.NeifaEquipmentUseProof q, global::CardGame.Core.CardUseFrame use) => _resolve().ValidNeifaEquipment(q, use);
    internal bool HasNeifaCapability { get => _resolve().HasNeifaCapability; }
}
