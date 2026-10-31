#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellKingProgramsEntries
{
    internal void AnswerHellExtreme(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHellExtreme(choice);
    internal void AssertHellKingSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertHellKingSelection(frame, plan);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHellPreparationAlcohol(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginHellPreparationAlcohol(frame);
    internal void BindHellChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, int? target, string option, bool selectsTarget) => _resolve().BindHellChoice(frame, effect, target, option, selectsTarget);
    internal void BindHellParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, int? seat) => _resolve().BindHellParticipant(frame, effect, seat);
    internal bool CanRunHellZhiwangCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunHellZhiwangCandidate(candidate, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ChooseHellExtremeParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ChooseHellExtremeParticipant(frame, effect);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectHellZhiwangCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectHellZhiwangCandidates(batch);
    internal void CollectWindow_CollectHellZhiwangCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectHellZhiwangCandidates(batch, result);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateHellZhiwangContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateHellZhiwangContext(parent, candidate);
    internal bool ExcludeOrdinaryMovement_IsHellZhiwangTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsHellZhiwangTrigger(candidate, trigger);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteHellKing(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteHellKing(supplied, effect);
    internal bool HasExactHellDeathOriginalCursor(global::CardGame.Core.ProgramSkillFrame root, scoped out global::CardGame.Core.DyingFrame? originalDying, scoped out long? originalAttack, int directDepth = 0) => _resolve().HasExactHellDeathOriginalCursor(root, out originalDying, out originalAttack, directDepth);
    internal bool HasExactHellKingDeathPrefix(global::CardGame.Core.ProgramSkillFrame root, scoped out global::CardGame.Core.DyingFrame? originalDying, scoped out long? originalAttack) => _resolve().HasExactHellKingDeathPrefix(root, out originalDying, out originalAttack);
    internal bool HasExactHellKingParticipantSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasExactHellKingParticipantSelection(frame);
    internal bool HasExactHellPreparationParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasExactHellPreparationParent(frame);
    internal bool HasExactHellWuhunDirectDeath(global::CardGame.Core.ProgramSkillFrame causer, global::CardGame.Core.DeathFrame death) => _resolve().HasExactHellWuhunDirectDeath(causer, death);
    internal int[] HellExtremeParticipants(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().HellExtremeParticipants(frame, op);
    internal int[] HellOtherSeats(int owner) => _resolve().HellOtherSeats(owner);
    internal bool IsExactHellPreparationAlcoholDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramVirtualBasicDraft draft) => _resolve().IsExactHellPreparationAlcoholDraft(frame, draft);
    internal bool IsHellKingChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => _resolve().IsHellKingChoiceResult(frame, binding);
    internal bool IsHellRandomAssaultBranch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().IsHellRandomAssaultBranch(frame, effect);
    internal bool IsHellZhiwangGain(global::CardGame.Core.CardMovementBatchContext? batch, int owner, int gainer) => _resolve().IsHellZhiwangGain(batch, owner, gainer);
    internal bool IsHellZhiwangMovement(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardMovementRecord move, int gainer) => _resolve().IsHellZhiwangMovement(batch, move, gainer);
    internal bool IsOriginalDyingSuspendedByHellKing(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByHellKing(dying);
    internal bool? ProveHellZhiwangMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveHellZhiwangMovement(trigger, candidate, context);
    internal int? RandomHellSeat(global::System.Collections.Generic.IReadOnlyList<int> seats) => _resolve().RandomHellSeat(seats);
    internal void ReturnHellNativeDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHellNativeDraw(frame, outcome);
    internal global::CardGame.Core.PromptChoice SelectAiHellExtreme(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiHellExtreme(decision);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateHellZhiwangContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateHellZhiwangContext(frame, candidate);
    internal bool HasHellZhiwangHolder { get => _resolve().HasHellZhiwangHolder; }
}
