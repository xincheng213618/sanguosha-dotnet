#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaManOlRuntime
{
    internal void PaySouyingMutation(ProgramSkillFrame f)
    {
        var r = f.HuaMan!;
        if (r.Stage != HuaManStage.DiscardChildren || !r.Paid || r.Souying is not { } q || r.DiscardedId is not { } discarded || _coreCardUseLifecycleOperations.LifecycleCardUse(q.NativeUseId)is not { Action: { } action } use || use.Action.ActionId != q.ActionId || use.HuaManSouyingMutation is not null)
            throw new InvalidOperationException("Paid Souying lost its native use, unique direction or actual discarded entity.");
        var cancel = f.OwnerSeat == q.TargetSeat;
        var ids = !cancel && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? action.PhysicalCards.Select(c => c.CardId).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use.Id, id)).ToArray() : [];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var mutation = new SouyingNativeMutation(f.Id, r.Source, r.GameplayHash, q, r.Before, r.After, discarded, before, before, cancel)
        {
            ClaimedEntities = ids,
            BeforeTargets = use.TargetSeats
        };
        if (cancel)
        {
            var targets = Array.AsReadOnly(Array.Empty<int>());
            var updated = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(action, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { HuaManSouyingMutation = mutation, TargetSeats = targets, TargetIndex = 0, Action = updated, TargetsAdjusted = true, SlashTargetsCancelled = CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind), SequentialTrick = null, PreparedTargetAttacks = use.PreparedTargetAttacks is null ? null : Array.AsReadOnly(Array.Empty<CardAttackState>()), Continuations = use.Continuations with { FangtianHalberd = use.Continuations.FangtianHalberd is { Active: true } multi ? multi with { TargetSeats = targets } : use.Continuations.FangtianHalberd } }, updated);
        }
        else
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { HuaManSouyingMutation = mutation });
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = r with { Stage = HuaManStage.ClaimChildren, Before = before, After = before, ActualCount = 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (ids.Length > 0)
            _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.Processing, CardLocation.Hand(f.OwnerSeat), new(HuaManClaimReason));
        use = _coreCardUseLifecycleOperations.LifecycleCardUse(q.NativeUseId)!;
        mutation = mutation with
        {
            ClaimAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { HuaManSouyingMutation = mutation });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuaMan = f.HuaMan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = ids.Length } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SouyingNativeMutationEvent(f.Id, use.Id, q.ActionId, r.Source, cancel, ids.Length, r.Before, r.After, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        RecordHuaManMovement(f, f.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

}
