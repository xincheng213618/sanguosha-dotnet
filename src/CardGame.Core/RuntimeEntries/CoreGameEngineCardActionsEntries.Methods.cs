#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardActionsEntries
{
    internal void AdvanceProgramCardCandidate(ProgramCardTriggerWindowFrame frame) => _resolve().AdvanceProgramCardCandidate(frame);
    internal CardActionContext? CaptureCardUseAction(Card card, int actorSeat, IReadOnlyList<int> targets, CardKind effectiveKind, IReadOnlyList<int> physicalIds, CardConversionSource? explicitConversion = default, IReadOnlyList<CardConversionSource>? additionalConversions = default, IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false) => _resolve().CaptureCardUseAction(card, actorSeat, targets, effectiveKind, physicalIds, explicitConversion, additionalConversions, designatedTargetSeats, isTrueZhangbaSlash);
    internal IReadOnlyList<ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(CardActionContext action, SkillProgramTriggerWindow window, IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, ProgramCardContinuation? continuation = default) => _resolve().CollectSharedCardActionCandidates(action, window, eventTargets, cardUseCausedDamage, continuation);
    internal ProgramTriggerWindowFlow CompleteProgramCardWindow(ProgramCardTriggerWindowFrame frame) => _resolve().CompleteProgramCardWindow(frame);
    internal void ContinueAcceptedDelayedCardUse(ProgramCardTriggerWindowFrame frame) => _resolve().ContinueAcceptedDelayedCardUse(frame);
    internal void ContinueCommittedSlashAfterPrograms(NativeOwners.CardAttackHandle attack) => _resolve().ContinueCommittedSlashAfterPrograms(attack);
    internal void ContinueCompletedSlashAfterPrograms(NativeOwners.CardAttackHandle attack) => _resolve().ContinueCompletedSlashAfterPrograms(attack);
    internal void ContinueProgramCardWindowCore() => _resolve().ContinueProgramCardWindowCore();
    internal void ContinueTrickAfterProgramTargetEffects(ProgramCardTriggerWindowFrame frame) => _resolve().ContinueTrickAfterProgramTargetEffects(frame);
    internal ProgramSkillWindowContext CreateCardActionProgramContext(CardActionContext action, SkillProgramTriggerWindow window, long parentFrameId, int ownerSeat, int eventTargetSeat, SkillProgramTriggerFacts facts) => _resolve().CreateCardActionProgramContext(action, window, parentFrameId, ownerSeat, eventTargetSeat, facts);
    internal ProgramSkillWindowContext CreateCardActionProgramContext2(ProgramCardTriggerWindowFrame frame, ProgramCardTriggerCandidate candidate) => _resolve().CreateCardActionProgramContext(frame, candidate);
    internal int GetFangtianEffectiveTarget(NativeOwners.FangtianHalberdHandle pending, int index) => _resolve().GetFangtianEffectiveTarget(pending, index);
    internal ProgramTriggerWindowPreparation<ProgramCardTriggerWindowFrame> PrepareProgramCardWindow(ProgramCardTriggerWindowFrame frame) => _resolve().PrepareProgramCardWindow(frame);
    internal bool PrepareProgramSlashTargets(NativeOwners.CardAttackHandle attack) => _resolve().PrepareProgramSlashTargets(attack);
    internal IReadOnlyList<CardAttackState>? PreparedTargetAttacks(long id) => _resolve().PreparedTargetAttacks(id);
    internal void ResolveRecast(CharacterState actor, Card card, CardKind? playedCardKind = default) => _resolve().ResolveRecast(actor, card, playedCardKind);
    internal CommandResult SubmitRecast(RecastCardCommand command) => _resolve().SubmitRecast(command);
    internal bool TryBeginCardResponsePrograms(NativeOwners.CardAttackHandle attack, CharacterState actor, CharacterState provider, int? requesterSeat, int opponentSeat, CardKind effectiveKind, IReadOnlyList<CardActionCost> costs, ProgramCardContinuation continuation, CardConversionSource? conversionSource = default, Suit? completedResponseUseSuit = default) => _resolve().TryBeginCardResponsePrograms(attack, actor, provider, requesterSeat, opponentSeat, effectiveKind, costs, continuation, conversionSource, completedResponseUseSuit);
    internal bool TryBeginDelayedCardUsePrograms(long resolutionId) => _resolve().TryBeginDelayedCardUsePrograms(resolutionId);
    internal bool TryBeginFinalizedTrickPrograms(NativeOwners.JizhiResolution pending) => _resolve().TryBeginFinalizedTrickPrograms(pending);
    internal bool TryBeginProgramCardUseBeforeTargetEffects(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginProgramCardUseBeforeTargetEffects(attack);
    internal bool TryBeginProgramCardUseBeforeTargetEffects2(NativeOwners.JizhiResolution pending) => _resolve().TryBeginProgramCardUseBeforeTargetEffects(pending);
    internal bool TryBeginProgramCardWindow(NativeOwners.CardAttackHandle? attack, CardActionContext action, SkillProgramTriggerWindow window, IReadOnlyList<int> opponents, ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, ProgramTrickContinuation? trickContinuation = default, ProgramSimpleCardContinuation? simpleContinuation = default, ProgramCompletedResponseReturn? completedResponseReturn = default, CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => _resolve().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
    internal bool TryBeginProgramSlashStage(NativeOwners.CardAttackHandle attack, SkillProgramTriggerWindow window, ProgramCardContinuation continuation) => _resolve().TryBeginProgramSlashStage(attack, window, continuation);
    internal NativeOwners.CardAttackHandle? ProgramCardAttack { get => _resolve().ProgramCardAttack; }
}
