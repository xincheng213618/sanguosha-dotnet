#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLingOlRuntime
{
    internal SkillProgramStepOutcome UseHuqiSlash(ProgramSkillFrame supplied, string bind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var binding = frame.CardSetBindings.Single(b => b.Name == bind);
        // A final judgment face is retained even if another real judgment effect
        // has already claimed the card. Only an entity still in Processing is
        // our cleanup cost; a claimed entity must never be taken back from hand.
        if (binding.CardIds is [var cardId] && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Processing)
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var movement = new CategoryTalismanMovementReceipt(SkillProgramEffectOp.UseRedJudgmentSlashAtDamageSource, frame.InstructionIndex, cardId, CardLocation.Processing, CardLocation.DiscardPile, before, before, bind);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CategoryTalismanMovement = movement, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(cardId), movement.From, movement.To, new("program.huqi.judgment-finished"));
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { CategoryTalismanMovement = movement with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(frame))
                ReturnCategoryTalismanMovement(frame);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return IssueHuqiSlash(frame, bind);
    }

    internal SkillProgramStepOutcome IssueHuqiSlash(ProgramSkillFrame frame, string bind)
    {
        var binding = frame.CardSetBindings.Single(b => b.Name == bind);
        if (binding.CardIds.Count == 0 && binding.FrozenRevealedSuit is null)
            return SkillProgramStepOutcome.Continue;
        var suit = binding.FrozenRevealedSuit ?? throw new InvalidOperationException("Huqi requires its frozen actual final judgment suit.");
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangLingBindingIssuedEvent>().Single(e => e.FrameId == frame.Id);
        if (suit is not (Suit.Heart or Suit.Diamond) || issued.DamageSourceSeat is not { } source || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || source == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[source], new Card(0, CardKind.Slash, Suit.None, 0), new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), CardKind.Slash, noEffectiveRank: true, physicalCardIds: []))
            return SkillProgramStepOutcome.Continue;
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId;
        var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel?.OwnerFrameId;
        var childId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HuqiSlash = new(frame.InstructionIndex, source, suit, attack, duel, childId) });
        if (attack is { } attackId)
            _coreCardAttackStateOperations.UpdateCardAttackState(attackId, state => state!with { Active = false });
        if (duel is { } duelId)
            _coreCardAttackStateOperations.UpdateCardContinuations(duelId, state => state with { Duel = state.Duel!with { Active = false } });
        return _coreTriggeredCardProgramsOperations.Value.UseTriggeredProgramVirtualSlash(frame, source);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 26050)]
    internal bool RestoreHuqiSuspendedAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.ProgramFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuqiSlash: { } receipt } parent || parent.Id != parentId)
            return false;
        if (receipt.ChildUseId != completion.ResolutionId || receipt.InstructionIndex != parent.InstructionIndex || _resolutionStackOperations._resolutionStack.Any(f => f.Id == receipt.ChildUseId))
            throw new InvalidOperationException("Huqi lost its exact retired native Slash child.");
        if (receipt.SuspendedAttackId is { } attackId)
            _coreCardAttackStateOperations.UpdateCardAttackState(attackId, state => state!with { Active = true });
        if (receipt.SuspendedDuelId is { } duelId)
            _coreCardAttackStateOperations.UpdateCardContinuations(duelId, state => state with { Duel = state.Duel!with { Active = true } });
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { HuqiSlash = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
        return true;
    }
}
