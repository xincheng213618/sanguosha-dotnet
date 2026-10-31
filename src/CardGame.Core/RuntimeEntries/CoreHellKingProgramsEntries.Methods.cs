#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellKingProgramsEntries
{
    internal void AnswerHellExtreme(PromptChoice choice) => _resolve().AnswerHellExtreme(choice);
    internal SkillProgramStepOutcome BeginHellPreparationAlcohol(ProgramSkillFrame frame) => _resolve().BeginHellPreparationAlcohol(frame);
    internal void BindHellChoice(ProgramSkillFrame frame, SkillProgramEffect effect, int? target, string option, bool selectsTarget) => _resolve().BindHellChoice(frame, effect, target, option, selectsTarget);
    internal void BindHellParticipant(ProgramSkillFrame frame, SkillProgramEffect effect, int? seat) => _resolve().BindHellParticipant(frame, effect, seat);
    internal bool CanRunHellZhiwangCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunHellZhiwangCandidate(candidate, context);
    internal SkillProgramStepOutcome ChooseHellExtremeParticipant(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ChooseHellExtremeParticipant(frame, effect);
    internal IEnumerable<ProgramTriggerCandidate> CollectHellZhiwangCandidates(CardMovementBatchContext batch) => _resolve().CollectHellZhiwangCandidates(batch);
    internal void CollectWindow_CollectHellZhiwangCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectHellZhiwangCandidates(batch, result);
    internal ProgramSkillWindowContext CreateHellZhiwangContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate) => _resolve().CreateHellZhiwangContext(parent, candidate);
    internal bool ExcludeOrdinaryMovement_IsHellZhiwangTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsHellZhiwangTrigger(candidate, trigger);
    internal SkillProgramStepOutcome ExecuteHellKing(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteHellKing(supplied, effect);
    internal bool HasExactHellDeathOriginalCursor(ProgramSkillFrame root, scoped out DyingFrame? originalDying, scoped out long? originalAttack, int directDepth = 0) => _resolve().HasExactHellDeathOriginalCursor(root, out originalDying, out originalAttack, directDepth);
    internal bool HasExactHellKingDeathPrefix(ProgramSkillFrame root, scoped out DyingFrame? originalDying, scoped out long? originalAttack) => _resolve().HasExactHellKingDeathPrefix(root, out originalDying, out originalAttack);
    internal bool HasExactHellPreparationParent(ProgramSkillFrame frame) => _resolve().HasExactHellPreparationParent(frame);
    internal bool HasExactHellWuhunDirectDeath(ProgramSkillFrame causer, DeathFrame death) => _resolve().HasExactHellWuhunDirectDeath(causer, death);
    internal int[] HellExtremeParticipants(ProgramSkillFrame frame, SkillProgramEffectOp op) => _resolve().HellExtremeParticipants(frame, op);
    internal int[] HellOtherSeats(int owner) => _resolve().HellOtherSeats(owner);
    internal bool IsHellKingChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) => _resolve().IsHellKingChoiceResult(frame, binding);
    internal bool IsHellRandomAssaultBranch(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().IsHellRandomAssaultBranch(frame, effect);
    internal bool IsHellZhiwangGain(CardMovementBatchContext? batch, int owner, int gainer) => _resolve().IsHellZhiwangGain(batch, owner, gainer);
    internal bool IsHellZhiwangMovement(CardMovementBatchContext batch, CardMovementRecord move, int gainer) => _resolve().IsHellZhiwangMovement(batch, move, gainer);
    internal bool IsOriginalDyingSuspendedByHellKing(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByHellKing(dying);
    internal bool? ProveHellZhiwangMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveHellZhiwangMovement(trigger, candidate, context);
    internal int? RandomHellSeat(IReadOnlyList<int> seats) => _resolve().RandomHellSeat(seats);
    internal void ReturnHellNativeDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnHellNativeDraw(frame, outcome);
    internal PromptChoice SelectAiHellExtreme(PendingDecision decision) => _resolve().SelectAiHellExtreme(decision);
    internal ProgramSkillWindowContext? TryCreateHellZhiwangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateHellZhiwangContext(frame, candidate);
    internal bool HasHellZhiwangHolder { get => _resolve().HasHellZhiwangHolder; }
}
