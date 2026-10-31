#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiMouOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32902, SkillProgramEffectOp.ResolveMouLuZhiLiance)]
    internal bool CanOfferMouLzLiance(ProgramTriggerLegalityContext g)
    {
        if (g.WindowContext?.MovementBatch is not { } b || !_coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn || !_coreMatchParticipantsPlayersAccess._players[g.OwnerSeat].IsAlive || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlStartedEvent>().Any(e => e.Source.OwnerSeat == g.OwnerSeat && e.Op == SkillProgramEffectOp.ResolveMouLuZhiLiance && e.BatchId == b.Id))
            return false;
        var loc = CardLocation.Hand(g.OwnerSeat);
        var changes = b.SourceCounts.Concat(b.DestinationCounts ?? []).Where(c => c.Location == loc).ToArray();
        if (changes.Length == 0 || changes.All(c => c.CountBefore == c.CountAfter))
            return false;
        var count = _corePhysicalOwnedCardQueriesOperations.GetHand(g.Owner).Count;
        return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count >= count);
    }

    internal void StartMouLzDraw(ProgramSkillFrame f)
    {
        var r = f.MouLuZhiOl!;
        var amount = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuZhiOl = r with { Requested = amount, Paid = true } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, amount, new(MouLzDraw));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouLuZhiLiance)]
    internal void ReturnMouLzDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MouLuZhiOl is not { Stage: MouLuZhiOlStage.Drawing, Paid: true, Draw: null } r || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.Requested || o.SequenceBefore < 0 || o.SequenceAfter < o.SequenceBefore || o.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || o.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == f.Id && e.RecipientSeat == f.OwnerSeat && e.RequestedCount == r.Requested && e.Replaced == (o.Replacement is not null)) != 1)
            throw new InvalidOperationException("Liance must return its exact requested native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuZhiOl = r with { Stage = MouLuZhiOlStage.DrawChildren, Draw = o, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlDrawReturnedEvent(f.Id, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.AttemptFrameId));
        ContinueMouLz(f.Id);
    }

    internal int MouLzChainCursor(GameEngine.IDamageAttempt a) => a is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex;
    internal void SetMouLzChainAmount(GameEngine.IDamageAttempt a, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(a, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !a.IsChainPropagation)
            throw new InvalidOperationException("Liance chain return requires the exact current damage instance.");
        if (a is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(a.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(a.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 32902)]
    internal int MouLzLianceBonus(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsSourceLess || !_coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn)
            return 0;
        var grants = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlLianceArmedEvent>().Where(e => e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlLianceConsumedEvent>().Any(c => c.FrameId == e.FrameId)).ToArray();
        foreach (var e in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlLianceConsumedEvent(e.FrameId, attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, attack.IsChainPropagation, attack.IsChainPropagation ? MouLzChainCursor(attack) : 0));
        return grants.Length;
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 32902)]
    internal void BeforeMouLzChain(GameEngine.IDamageAttempt attack)
    {
        if (!attack.IsChainPropagation || attack.IsSourceLess)
            return;
        var cursor = MouLzChainCursor(attack);
        var remaining = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats;
        if (!remaining.Skip(cursor).Any(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlChainIssuedEvent>().SingleOrDefault(e => e.AttackId == attack.ResolutionId && e.Cursor == cursor)is not { } issued || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlLianceConsumedEvent>().Any(e => e.AttackId == attack.ResolutionId && e.Chain && e.Cursor == cursor) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlChainRebasedEvent>().Any(e => e.AttackId == attack.ResolutionId && e.Cursor == cursor))
            return;
        SetMouLzChainAmount(attack, issued.Inherited);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlChainRebasedEvent(attack.ResolutionId, cursor));
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 32902)]
    internal void AfterMouLzChain(GameEngine.IDamageAttempt attack)
    {
        if (!attack.IsChainPropagation || attack.IsSourceLess || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlLianceArmedEvent>().Any(e => e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLuZhiOlLianceConsumedEvent>().Any(c => c.FrameId == e.FrameId)))
            return;
        var cursor = MouLzChainCursor(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuZhiOlChainIssuedEvent(attack.ResolutionId, cursor, attack.DamageAmount));
        if (attack.DamageAmountFinalized)
            SetMouLzChainAmount(attack, checked(attack.DamageAmount + MouLzLianceBonus(attack)));
    }
}
