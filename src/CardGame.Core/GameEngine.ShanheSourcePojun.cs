using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShanheSourcePojunSkill = "shanhe:source-pojun-14112";
    private const string ShanheSourcePojunBind = "source-pojun-held";
    private bool IsShanheSourcePojunMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;

    [ProgramCapability]
    private sealed class ShanheSourcePojunCapability(GameEngine engine) : IShanheSourcePojunHost
    {
        public SkillProgramStepOutcome HoldAllTargetHand(ProgramSkillFrame frame) => engine.HoldShanheSourcePojunTargetHand(frame);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62049, SkillProgramEffectOp.HoldShanheSourcePojunHand)]
    private bool CanHoldShanheSourcePojunHand(ProgramTriggerLegalityContext context) =>
        IsShanheSourcePojunMode && context.WindowContext is
        { Window: SkillProgramTriggerWindow.SlashBeforeResponse, TargetSeat: { } target, CardUse: { } use } &&
        use.ActorSeat == context.OwnerSeat && target != context.OwnerSeat && IsValidPlayerSeat(target) &&
        _players[target].IsAlive && GetHand(_players[target]).Count > 0;

    private SkillProgramStepOutcome HoldShanheSourcePojunTargetHand(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (!IsShanheSourcePojunMode || frame.SkillId != ShanheSourcePojunSkill ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.SlashBeforeResponse, TargetSeat: { } target, CardUse: { } action } context ||
            frame.OwnerSeat != action.ActorSeat || target == frame.OwnerSeat ||
            _resolutionStack.Count < 2 || _resolutionStack[^2] is not ProgramCardTriggerWindowFrame window || window.Id != context.ParentFrameId ||
            window.AttackOwnerFrameId is not { } attackId ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(frame, ToSharedCandidate(window.Candidates[window.CandidateIndex])) ||
            _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(candidateUse => candidateUse.Id == attackId) is not { } use ||
            use.Id != action.ParentCardUseFrameId || use.Action?.ActionId != action.CardActionId || !IsSlashCard(use.CardKind) ||
            !SameAttackOwner(ActiveCardAttack, new CardAttackHandle(this, attackId)) ||
            new CardAttackHandle(this, attackId).TargetSeat != target ||
            frame.CardSetBindings.Any(set => set.Name == ShanheSourcePojunBind) || frame.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Source Pojun lost its original current Slash, final target or unpaid instruction.");
        if (!_players[frame.OwnerSeat].IsAlive || !_players[target].IsAlive ||
            !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var cards = GetHand(_players[target]).ToArray();
        if (cards.Length == 0) return SkillProgramStepOutcome.Continue;
        var before = _cardMovements.LastOrDefault()?.Sequence ?? 0;
        // Native PojunHold is an outside-game zone. Its existing turn-end return
        // and death cleanup own the cards after this paid program has completed.
        MoveCards(cards, CardLocation.Hand(target), CardLocation.PojunHold(target), CardMoveReasons.PojunHold);
        SetProgramCardSet(frame.Id, ShanheSourcePojunBind, cards.Select(card => card.Id).ToArray(),
            SkillProgramCardSetVisibility.Private, cards.Select(_ => CardLocation.PojunHold(target)).ToArray());
        var after = _cardMovements.LastOrDefault()?.Sequence ?? before;
        AdvanceEventRulesAndQueueFact(new ShanheSourcePojunHeldEvent(frame.Id, use.Id, action.CardActionId,
            frame.OwnerSeat, target, cards.Length, before, after));
        return AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 62049)]
    private bool IsShanheSourcePojunMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.HoldShanheSourcePojunHand || frame.SkillId != ShanheSourcePojunSkill ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.SlashBeforeResponse, TargetSeat: { } target, CardUse: { } use } ||
            pending.SubjectSeat != frame.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null ||
            frame.CardSetBindings.SingleOrDefault(set => set.Name == ShanheSourcePojunBind) is not { } cards || cards.CardIds.Count == 0)
            return false;
        var facts = CompleteProgramEventHistory().OfType<ShanheSourcePojunHeldEvent>().Where(fact => fact.ProgramFrameId == frame.Id).ToArray();
        if (facts is not [var paid] || paid.CardUseFrameId != use.ParentCardUseFrameId || paid.ActionId != use.CardActionId ||
            paid.OwnerSeat != frame.OwnerSeat || paid.TargetSeat != target || paid.Count != cards.CardIds.Count ||
            paid.SequenceAfter - paid.SequenceBefore != paid.Count || cards.SourceLocations is null ||
            cards.SourceLocations.Any(location => location != CardLocation.PojunHold(target))) return false;
        var moves = _cardMovements.Where(move => move.Sequence > paid.SequenceBefore && move.Sequence <= paid.SequenceAfter).ToArray();
        return moves.Select(move => move.CardId).SequenceEqual(cards.CardIds) && moves.All(move =>
            move.From == CardLocation.Hand(target) && move.To == CardLocation.PojunHold(target) && move.Reason == CardMoveReasons.PojunHold);
    }
}
