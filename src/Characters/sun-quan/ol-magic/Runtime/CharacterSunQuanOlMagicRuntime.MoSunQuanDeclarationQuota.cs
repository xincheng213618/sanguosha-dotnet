#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal static bool IsMoQuanyuTrueSlashDeclaration(DeclaredCardPayment paid, CardDeclarationParentReturn parent) => RuntimeSkills.RuntimeRules_sun_quan_ol_magic_MoSunQuanDeclarationQuota.IsMoQuanyuTrueSlashDeclaration(paid, parent);
    internal DeclaredCardPayment CaptureMoQuanyuDeclarationQuota(DeclaredCardPayment paid, CardDeclarationParentReturn parent, IReadOnlyList<int> targets)
    {
        if (!HasMoQuanyuSlash || !IsMoQuanyuTrueSlashDeclaration(paid, parent) || targets is not [var target])
            return paid;
        if (paid.MoQuanyuDeclarationQuota is not null || paid.Claimed || paid.OwnerFrameId != paid.DeclarationId || paid.ActorSeat != parent.ActorSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(parent.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || paid.Cost.From != CardLocation.Hand(paid.ProviderSeat) || _cardZonesOperations._cardZones.GetLocation(paid.Cost.CardId) != paid.Cost.From || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDeclarationQuotaCapturedEvent>().Any(e => e.DeclarationId == paid.DeclarationId))
            throw new InvalidOperationException("Meteor must freeze the original unpaid declaration and its exact initial singleton once.");
        var kind = parent.FinalEffectiveKind ?? paid.DeclaredKind;
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return paid;
        CardConversionSource? source = null;
        string? hash = null;
        var mask = 0;
        var entered = false;
        var actor = _coreMatchParticipantsPlayersAccess._players[parent.ActorSeat];
        var hasSource = TryGetMoQuanyuSource(actor.Seat, out var liveSource, out var liveHash);
        var eligible = _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == actor.Seat && actor.IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && actor.Seat != target && hasSource && (MoQuanyuSlashMask(actor.Seat, target) & MoQuanyuBit(MoQuanyuEffect.Liuxing)) != 0;
        if (eligible)
        {
            source = liveSource;
            hash = liveHash;
            mask = MoQuanyuSlashMask(actor.Seat, target);
            entered = MoQiangangEntered(actor.Seat);
        }

        var q = new MoQuanyuDeclarationQuota(paid.DeclarationId, actor.Seat, paid.ProviderSeat, paid.Cost.CardId, paid.Cost.From, paid.Source, paid.DeclaredKind, kind, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, eligible, source, hash, mask, entered);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuDeclarationQuotaCapturedEvent(paid.DeclarationId, paid.OwnerFrameId, q));
        return paid with
        {
            MoQuanyuDeclarationQuota = q
        };
    }

    internal void RecordMoQuanyuDeclarationPayment(long declarationId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardDeclarationFrame { Stage: CardDeclarationStage.Paying } f || f.Id != declarationId || f.Payment.MoQuanyuDeclarationQuota is not { } q)
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDeclarationPaymentRecordedEvent>().Any(e => e.DeclarationId == declarationId) || q.DeclarationId != declarationId || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDeclarationQuotaCapturedEvent>().Count(e => e.DeclarationId == declarationId && e.OwnerFrameId == declarationId && e.Receipt == q) != 1)
            throw new InvalidOperationException("Meteor cannot replace a declaration's original physical payment invoice.");
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > q.SequenceBefore && m.CardId == q.CardId && m.CardKind == f.Payment.Cost.CardKind && m.From == q.From && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay").ToArray();
        if (moves is not [var paid])
            throw new InvalidOperationException("Meteor requires the one genuine native declaration payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuDeclarationPaymentRecordedEvent(declarationId, paid.Sequence));
    }

    internal bool ValidMoQuanyuDeclarationQuota(DeclaredCardPayment paid, MoQuanyuDeclarationQuota q)
    {
        if (paid.MoQuanyuDeclarationQuota != q || paid.DeclarationId != q.DeclarationId || q.DeclarationId <= 0 || paid.ActorSeat != q.ActorSeat || paid.ProviderSeat != q.ProviderSeat || paid.Cost.CardId != q.CardId || paid.Cost.From != q.From || paid.Source != q.DeclarationSource || paid.DeclaredKind != q.DeclaredKind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(q.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(q.ProviderSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(q.TargetSeat) || q.From != CardLocation.Hand(q.ProviderSeat) || !CoreNativeCardClassificationsRuntime.IsSlashCard(q.DeclaredKind) || !CoreNativeCardClassificationsRuntime.IsSlashCard(q.OutputKind) || q.ActualTurn < 1 || q.ActualTurn > _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(q.TurnOwner) || q.SequenceBefore < 0 || q.SequenceBefore > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.OwnerFrameId)is not { } owner || !(owner.AcceptedDeclarationPayment == paid || owner is CardDeclarationFrame d && d.Payment == paid) || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDeclarationQuotaCapturedEvent>().Where(e => e.DeclarationId == q.DeclarationId).ToArray()is not [var captured] || captured.OwnerFrameId != q.DeclarationId || captured.Receipt != q || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDeclarationPaymentRecordedEvent>().Where(e => e.DeclarationId == q.DeclarationId).ToArray()is not [var invoice] || invoice.MovementSequence <= q.SequenceBefore || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == invoice.MovementSequence && m.CardId == q.CardId && m.CardKind == paid.Cost.CardKind && m.From == q.From && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay") != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<CardDeclarationCommittedEvent>().Count(e => e.DeclarationId == q.DeclarationId && e.OwnerSeat == q.ProviderSeat && e.ActorSeat == q.ActorSeat && e.DeclaredKind == q.DeclaredKind && e.TargetSeats.SequenceEqual([q.TargetSeat])) != 1)
            return false;
        if (q.Eligible)
        {
            if (q.Source is not { SkillId: "ol:mo-quanyu", BindingId: "target-only-choices" } source || source.OwnerSeat != q.ActorSeat || string.IsNullOrEmpty(source.SkillInstanceId) || q.GameplayHash is null || q.ChoiceMask is < 1 or > 63 || (q.ChoiceMask & MoQuanyuBit(MoQuanyuEffect.Liuxing)) == 0 || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash != q.GameplayHash)
                return false;
        }
        else if (q.Source is not null || q.GameplayHash is not null || q.ChoiceMask != 0 || q.Entered)
            return false;
        return owner is not CardDeclarationFrame declaration || declaration.Return.ActorSeat == q.ActorSeat && IsMoQuanyuTrueSlashDeclaration(paid, declaration.Return) && declaration.TargetSeats.SequenceEqual([q.TargetSeat]) && (declaration.Return.FinalEffectiveKind ?? paid.DeclaredKind) == q.OutputKind;
    }

    internal bool TryGetMoQuanyuDeclarationQuota(CharacterState actor, CharacterState target, CardKind kind, out bool allowed)
    {
        allowed = false;
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        DeclaredCardPayment? paid;
        if (top is CardDeclarationFrame { Stage: CardDeclarationStage.Applying } declaration && declaration.Return.ActorSeat == actor.Seat && CoreNativeCardClassificationsRuntime.IsSlashCard(declaration.Payment.DeclaredKind))
            paid = declaration.Payment;
        else
        {
            if (top is ResponseWindowFrame response)
                top = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == response.ParentFrameId);
            paid = top?.AcceptedDeclarationPayment;
            if (paid is not { Claimed: false } || paid.ActorSeat != actor.Seat || !CoreNativeCardClassificationsRuntime.IsSlashCard(paid.DeclaredKind) || _cardZonesOperations._cardZones.GetLocation(paid.Cost.CardId) != CardLocation.Processing)
                return false;
        }

        allowed = paid.MoQuanyuDeclarationQuota is { Eligible: true } q && q.TargetSeat == target.Seat && q.OutputKind == kind && ValidMoQuanyuDeclarationQuota(paid, q);
        return true;
    }

    internal bool TryInheritMoQuanyuDeclarationQuota(CardUseFrame use)
    {
        if (!HasMoQuanyuSlash || use.Action is not { Type: CardActionType.Use } action || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return false;
        var paid = use.AcceptedDeclarationPayment;
        if (paid is null && action.PhysicalCards is [var material])
            paid = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(action.ProviderSeat, material.CardId);
        if (paid is null || !CoreNativeCardClassificationsRuntime.IsSlashCard(paid.DeclaredKind))
            return false;
        // No initial-singleton declaration ticket means no Meteor privilege,
        // rather than permission to recapture the current live source.
        if (paid.MoQuanyuDeclarationQuota is not { } q)
            return true;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || !ValidMoQuanyuDeclarationQuota(paid, q) || use.SourceSeat != q.ActorSeat || action.ActorSeat != q.ActorSeat || action.ProviderSeat != q.ProviderSeat || use.CardKind != q.OutputKind || !use.TargetSeats.SequenceEqual([q.TargetSeat]) || action.PhysicalCards is not [var cost] || cost.CardId != q.CardId || cost.CardKind != paid.Cost.CardKind || cost.From != q.From || use.MoQuanyuQuota is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDeclarationQuotaBoundEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Meteor's accepted Use must inherit its exact initial declaration, actor and original paid material.");
        if (q.Eligible)
        {
            var accepted = new MoQuanyuQuotaReceipt(use.Id, action.ActionId, q.ActorSeat, q.TargetSeat, q.OutputKind, q.Source!, q.GameplayHash!, q.ChoiceMask, q.Entered);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MoQuanyuQuota = accepted });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuQuotaAcceptedEvent(accepted));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuDeclarationQuotaBoundEvent(q.DeclarationId, use.Id, action.ActionId, q.ActorSeat, q.TargetSeat, q.OutputKind, q.Eligible));
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 28911)]
    internal void AssertMoQuanyuDeclarationQuotas()
    {
        foreach (var frame in _resolutionStackOperations._resolutionStack)
        {
            var paid = frame is CardDeclarationFrame declaration ? declaration.Payment : frame.AcceptedDeclarationPayment;
            if (paid?.MoQuanyuDeclarationQuota is not { } q)
                continue;
            if (!ValidMoQuanyuDeclarationQuota(paid, q))
                throw new InvalidOperationException("Meteor lost its original declaration admission or exact native payment.");
            if (frame is not CardUseFrame use || use.Action is not { Type: CardActionType.Use } action)
                continue;
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuDeclarationQuotaBoundEvent>().Where(e => e.UseId == use.Id).ToArray()is not [var bound] || bound != new MoQuanyuDeclarationQuotaBoundEvent(q.DeclarationId, use.Id, action.ActionId, q.ActorSeat, q.TargetSeat, q.OutputKind, q.Eligible) || q.Eligible && use.MoQuanyuQuota != new MoQuanyuQuotaReceipt(use.Id, action.ActionId, q.ActorSeat, q.TargetSeat, q.OutputKind, q.Source!, q.GameplayHash!, q.ChoiceMask, q.Entered) || !q.Eligible && use.MoQuanyuQuota is not null)
                throw new InvalidOperationException("Meteor cannot recapture a declaration's quota from a later source or another native Use.");
        }
    }
}
