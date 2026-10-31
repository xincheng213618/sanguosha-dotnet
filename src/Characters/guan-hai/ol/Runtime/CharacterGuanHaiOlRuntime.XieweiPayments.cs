#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanHaiOlRuntime
{
    internal const string XieweiStoreReason = "skill-program.guan-hai.xiewei.store";
    internal bool HasGuanHaiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveXieweiTwoHandBaitBasicUse) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveYouqueMinimumBaitPindian);

    internal bool IsXieweiSource(CardConversionSource source) => _corePlayCardHistoryOperations.ViewAsRule(source)?.XieweiBaitBasicUse is not null;
    internal bool IsXieweiSelection(ProgramMultiCardViewAsSelection selection) => IsXieweiSource(selection.Source);
    internal CardActionContext PrepareXieweiAction(CardActionContext action) => !action.ConversionChain.Any(IsXieweiSource) ? action : new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, Suit.None, 0, null, action.FactionOrigin);
    internal bool XieweiNativeDirection(CharacterState owner, CardKind kind, bool forResponse)
    {
        if (!forResponse)
            return kind == CardKind.Slash;
        if (kind != CardKind.Dodge)
            return false;
        var attack = _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } defense && defense.CurrentCandidateSeat == owner.Seat ? defense.Attack : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack;
        return attack is not null && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge) && (attack.TargetSeat == owner.Seat || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } d && d.CurrentCandidateSeat == owner.Seat && global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(d.Attack, attack));
    }

    internal bool TryCompleteXieweiFactionDefense(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, CharacterState provider, ProgramMultiCardViewAsSelection selection)
    {
        if (!IsXieweiSelection(selection))
            return false;
        var attack = pending.Attack;
        if (selection.OutputKind != CardKind.Dodge || selection.Source.OwnerSeat != provider.Seat || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense, pending) || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || pending.CurrentCandidateSeat != provider.Seat || provider.Seat == pending.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].IsAlive || !provider.IsAlive || attack.TargetSeat != pending.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge) || pending.Source.FactionId != "magic-any" && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) != pending.Source.FactionId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ResponseWindowFrame response || response.ParentFrameId != attack.ResolutionId || response.ResponderSeat != pending.OwnerSeat || response.RequiredCardKind != CardKind.Dodge || selection.Cards.Count != 2 || selection.Cards.Select(c => c.Id).Distinct().Count() != 2 || selection.Cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id) != CardLocation.Hand(provider.Seat)) || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(provider, selection.Cards.Select(c => c.Id).ToArray(), CardKind.Dodge, true, selection.Source)is null)
            throw new InvalidOperationException("Xiewei's faction Dodge requires its current original Slash, living requester/provider, exact two Hand cards and issued faction candidate.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt)
        {
            if (prompt.Kind != DecisionKind.RespondDodge || !prompt.IsPrivate || prompt.PlayerSeat != provider.Seat || prompt.TargetSeat != pending.OwnerSeat || prompt.RequiredCardKind != CardKind.Dodge || !prompt.Choices.Any(c => c.Cards.Order().SequenceEqual(selection.Cards.Select(card => card.Id).Order()) && global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(c.Parameters, out var source) && source == selection.Source))
                throw new InvalidOperationException("Xiewei cannot answer a different or unpublished private faction request.");
        }
        else if (provider.IsHuman)
            throw new InvalidOperationException("A human Xiewei faction response must retain its actual private published pair.");
        // The ordinary AI candidate dispatcher has already cleared its prompt.
        // Its still-active faction cursor and original ResponseWindow above own
        // this exact selection until both real Hand costs have been deposited.
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        var action = _coreCardConversionsOperations.MoveProgramMultiCardResponse(provider, selection, attack.ResolutionId, attack.SourceSeat, pending.OwnerSeat);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        _coreMatchLogOperations.AddLog("CardResponded", $"{(provider.Name)} 发动【卸尾】，将两张手牌置为饵，替 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 响应【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】。", provider.Seat, pending.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionDefenseResolvedEvent(attack.ResolutionId, pending.OwnerSeat, true, provider.Seat, selection.Cards[0].Id));
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardResponseAccepted, [attack.SourceSeat], ProgramCardContinuation.FactionDefenseDodge))
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, action, ProgramCardContinuation.FactionDefenseDodge);
        return true;
    }

    internal PublicPersistentPileSource EnsureXieweiPile(CardConversionSource source)
    {
        var key = (source.OwnerSeat, source.SkillId, source.SkillInstanceId);
        if (_coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.TryGetValue(key, out var existing))
            return existing;
        var pileId = _corePublicPileSourcesOperations.PublicPileSources(source.OwnerSeat).Any(s => s.PublicPileId is null) ? global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(source.SkillId, source.SkillInstanceId) : null;
        var pile = new PublicPersistentPileSource(source.OwnerSeat, source.SkillId, source.SkillInstanceId, int.MaxValue, pileId);
        _cardZonesOperations._cardZones.EnsurePublicPersistentPile(pile.Location);
        _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Add(key, pile);
        return pile;
    }

    internal XieweiNativePayment PrepareXieweiPayment(CardUseFrame use, int provider, int actor, int? requester, CardConversionSource source, CardKind kind, bool response, IReadOnlyList<CardActionCost> costs)
    {
        if (!IsXieweiSource(source) || source.OwnerSeat != provider || !_coreMatchParticipantsPlayersAccess._players[provider].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[provider], source.SkillId, source.SkillInstanceId) || costs.Count != 2 || costs.Select(c => c.CardId).Distinct().Count() != 2 || costs.Any(c => c.From != CardLocation.Hand(provider) || _cardZonesOperations._cardZones.GetLocation(c.CardId) != c.From) || !XieweiNativeDirection(_coreMatchParticipantsPlayersAccess._players[provider], kind, response))
            throw new InvalidOperationException("Xiewei requires its original two genuine Hand cards and current true basic-use direction.");
        var p = new XieweiNativePayment(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, use.Id, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, provider, actor, requester, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, response, kind, EnsureXieweiPile(source), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Materials = costs.Select(c => new GuanHaiMaterial(c.CardId, c.CardKind, c.From)).ToArray()
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieweiPaymentPreparedEvent(p.Id, p.UseId, p.Source, p.GameplayHash, provider, actor, requester, p.ActualTurn, p.TurnOwner, response, kind, p.Pile, p.Before));
        for (var i = 0; i < p.Materials.Count; i++)
        {
            var m = p.Materials[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieweiMaterialFrozenEvent(p.Id, i, m.CardId, m.Kind, m.From));
        }

        return p;
    }

    internal void PrepareXieweiNativeUse(CardUseFrame use)
    {
        if (!HasGuanHaiCapability || use.Action is not { Type: CardActionType.Use } action || action.ConversionChain.SingleOrDefault(IsXieweiSource)is not { } source)
            return;
        if (use.XieweiPayments?.Any(p => !p.ResponseUse) == true)
            throw new InvalidOperationException("Xiewei cannot prepare one native Use twice.");
        var paid = PrepareXieweiPayment(use, action.ProviderSeat, action.ActorSeat, action.RequesterSeat, source, action.EffectiveKind, false, action.PhysicalCards)with
        {
            ActionId = action.ActionId
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { XieweiPayments = (u.XieweiPayments ?? []).Append(paid).ToArray() });
        if (use.TargetSeats.Count > 1)
        {
            var maximum = XieweiTargetMaximum(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]);
            if (use.TargetSeats.Count > maximum || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count)
                throw new InvalidOperationException("Xiewei exceeded its genuine native Slash target allowance.");
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { XieweiTargetMaximum = maximum, XieweiOriginalTargets = use.TargetSeats });
            for (var i = 0; i < use.TargetSeats.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieweiTargetFrozenEvent(use.Id, action.ActionId, i, use.TargetSeats[i], maximum));
        }
    }

    internal void PrepareXieweiResponsePayment(CharacterState owner, ProgramMultiCardViewAsSelection selection, long nativeUseId, int? actorSeat = null)
    {
        if (!IsXieweiSelection(selection))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(nativeUseId)is not { } use || use.XieweiPayments?.Any(p => p.ResponseUse && p.ActionId == 0) == true)
            throw new InvalidOperationException("A Xiewei Dodge requires exactly one current native attack payment.");
        var actor = actorSeat ?? owner.Seat;
        var costs = selection.Cards.Select(c => new CardActionCost(c.Id, c.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(owner, c), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(owner.Seat, c))).ToArray();
        var paid = PrepareXieweiPayment(use, owner.Seat, actor, actor == owner.Seat ? null : actor, selection.Source, CardKind.Dodge, true, costs);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { XieweiPayments = (u.XieweiPayments ?? []).Append(paid).ToArray() });
    }

    internal void UpdateXieweiPayment(long useId, XieweiNativePayment payment) => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { XieweiPayments = (u.XieweiPayments ?? []).Select(p => p.Id == payment.Id ? payment : p).ToArray() });
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MaterialPayment, 10)]
    internal bool TryPayXieweiMaterials(IReadOnlyList<Card> cards, int providerSeat, CardLocation? sourceLocation, CardMoveReason reason)
    {
        if (!HasGuanHaiCapability || cards.Count != 2)
            return false;
        var ids = cards.Select(c => c.Id).Order().ToArray();
        var p = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SelectMany(u => u.XieweiPayments ?? []).SingleOrDefault(p => !p.Paid && p.OwnerSeat == providerSeat && p.Materials.Select(c => c.CardId).Order().SequenceEqual(ids));
        if (p is null)
            return false;
        if (reason != (p.ResponseUse ? CardMoveReasons.Respond : CardMoveReasons.Use) || sourceLocation is not null && sourceLocation != CardLocation.Hand(providerSeat) || cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id) != CardLocation.Hand(providerSeat)))
            throw new InvalidOperationException("Xiewei changed its issued Hand cost before the public deposit.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, p.Pile.Location, new(XieweiStoreReason), disclosure: CardMovementDisclosure.FaceUp);
        return true;
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 20)]
    internal void CaptureXieweiPaymentBatch(CardMovementBatchContext batch)
    {
        if (!HasGuanHaiCapability || batch.ParentFrameId is not { } id || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { XieweiPayments: { } payments })
            return;
        foreach (var original in payments.Where(p => !p.Paid).ToArray())
        {
            var records = batch.Movements.Where(m => original.Materials.Any(c => c.CardId == m.CardId) && m.Reason.Value == XieweiStoreReason).ToArray();
            if (records.Length == 0)
                continue;
            if (records.Length != 2 || records.Any(m => m.From != CardLocation.Hand(original.OwnerSeat) || m.To != original.Pile.Location || m.Sequence <= original.Before || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || !original.Materials.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind)) || records.Select(m => m.CardId).Distinct().Count() != 2)
                throw new InvalidOperationException("Xiewei requires one exact atomic two-Hand public-pile deposit, never a Processing invoice.");
            var p = original with
            {
                Paid = true,
                After = records.Max(m => m.Sequence),
                Movements = records.OrderBy(m => m.Sequence).ToArray()
            };
            UpdateXieweiPayment(id, p);
            for (var i = 0; i < p.Materials.Count; i++)
            {
                var m = records.Single(m => m.CardId == p.Materials[i].CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieweiBaitStoredEvent(p.Id, i, m.CardId, m.From, m.To, m.Sequence, batch.Id));
            }

            if (!p.ResponseUse && p.ActionId > 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieweiOutputBoundEvent(p.Id, p.UseId, p.ActionId, p.ActorSeat, p.OwnerSeat, p.RequesterSeat, p.Kind, false));
            if (!p.ResponseUse && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { PaymentMovementReceipt: null, Action: { } a })
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { PaymentMovementReceipt = new(id, a.ActionId, records[0].Sequence - 1, records[^1].Sequence) { Batch = batch } });
        }
    }

    internal void BindXieweiResponsePayment(long nativeUseId, CardActionContext action)
    {
        if (!HasGuanHaiCapability || !action.ConversionChain.Any(IsXieweiSource))
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(nativeUseId)is not { } use || use.XieweiPayments?.Where(p => p.ResponseUse && p.ActionId == 0 && p.OwnerSeat == action.ProviderSeat).ToArray()is not [var p] || !p.Paid || action.Type != CardActionType.Response || action.EffectiveKind != CardKind.Dodge || action.ActorSeat != p.ActorSeat || action.RequesterSeat != p.RequesterSeat || action.ResponderSeat != p.OwnerSeat || !XieweiActionCosts(p, action))
            throw new InvalidOperationException("A Xiewei Dodge lost its real paid bait costs or native actor/provider/requester.");
        p = p with
        {
            ActionId = action.ActionId
        };
        UpdateXieweiPayment(nativeUseId, p);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XieweiOutputBoundEvent(p.Id, p.UseId, p.ActionId, p.ActorSeat, p.OwnerSeat, p.RequesterSeat, p.Kind, true));
    }

    internal static bool XieweiActionCosts(XieweiNativePayment p, CardActionContext a) => global::CardGame.Core.RuntimeSkills.RuntimeRules_guan_hai_ol_XieweiPayments.XieweiActionCosts(p, a);
    internal bool ValidXieweiPayment(XieweiNativePayment p)
    {
        if (p.Materials.Count != 2 || p.Materials.Select(c => c.CardId).Distinct().Count() != 2 || p.Materials.Any(c => c.From != CardLocation.Hand(p.OwnerSeat)) || p.Source.OwnerSeat != p.OwnerSeat || p.Pile.OwnerSeat != p.OwnerSeat || p.Pile.SkillId != p.Source.SkillId || p.Pile.SkillInstanceId != p.Source.SkillInstanceId || _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.GameplayHash != p.GameplayHash || !IsXieweiSource(p.Source) || _coreProgramEventHistoryOperations.ProgramEventHistory<XieweiPaymentPreparedEvent>().Count(e => e == new XieweiPaymentPreparedEvent(p.Id, p.UseId, p.Source, p.GameplayHash, p.OwnerSeat, p.ActorSeat, p.RequesterSeat, p.ActualTurn, p.TurnOwner, p.ResponseUse, p.Kind, p.Pile, p.Before)) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<XieweiMaterialFrozenEvent>().Where(e => e.PaymentId == p.Id).OrderBy(e => e.Index).Select(e => new GuanHaiMaterial(e.CardId, e.Kind, e.From)).SequenceEqual(p.Materials) || p.Paid != (p.Movements.Count == 2))
            return false;
        return p.Movements.All(m => m.Sequence > p.Before && m.Sequence <= p.After && m.From == CardLocation.Hand(p.OwnerSeat) && m.To == p.Pile.Location && m.Reason.Value == XieweiStoreReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && p.Materials.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind) && _coreProgramEventHistoryOperations.ProgramEventHistory<XieweiBaitStoredEvent>().Count(e => e.PaymentId == p.Id && e.CardId == m.CardId && e.Sequence == m.Sequence && e.Pile == m.To) == 1) && (!p.Paid || p.ActionId == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<XieweiOutputBoundEvent>().Count(e => e == new XieweiOutputBoundEvent(p.Id, p.UseId, p.ActionId, p.ActorSeat, p.OwnerSeat, p.RequesterSeat, p.Kind, p.ResponseUse)) == 1);
    }

    internal bool IsXieweiRemovedMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.XieweiPayments?.Any(p => !p.ResponseUse && p.Paid && p.ActionId > 0 && p.Materials.Any(c => c.CardId == cardId) && ValidXieweiPayment(p)) == true;
    internal bool IsXieweiResponseRemovedMaterial(ProgramMultiCardViewAsSelection selection, int cardId) => IsXieweiSelection(selection) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SelectMany(u => u.XieweiPayments ?? []).Any(p => p.ResponseUse && p.Paid && p.ActionId > 0 && p.Source == selection.Source && p.Materials.Select(c => c.CardId).Order().SequenceEqual(selection.Cards.Select(c => c.Id).Order()) && p.Materials.Any(c => c.CardId == cardId) && ValidXieweiPayment(p));
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MatchBatch, 30)]
    internal bool IsXieweiPaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => use.XieweiPayments?.Any(p => !p.ResponseUse && p.Paid && p.UseId == use.Id && p.ActionId == use.Action?.ActionId && batch.ParentFrameId == use.Id && batch.Movements.SequenceEqual(p.Movements) && ValidXieweiPayment(p)) == true;
    internal void AssertXieweiPayments(CardUseFrame use)
    {
        if (use.XieweiPayments is not null && (use.XieweiPayments is not System.Collections.IList { IsReadOnly: true } || use.XieweiPayments.Any(p => p.UseId != use.Id || p.Materials is not System.Collections.IList { IsReadOnly: true } || p.Movements is not System.Collections.IList { IsReadOnly: true } || !ValidXieweiPayment(p))))
            throw new InvalidOperationException("Xiewei lost its immutable native cost/virtual-output separation.");
    }

    internal bool IsXieweiMultiTargetUse(CardUseFrame use) => use.XieweiTargetMaximum is >= 2 && use.XieweiOriginalTargets is { Count: >= 2 } targets && targets is System.Collections.IList { IsReadOnly: true } && targets.Count <= use.XieweiTargetMaximum && targets.Distinct().Count() == targets.Count && use.XieweiPayments?.Any(p => !p.ResponseUse && p.Paid && p.ActionId == use.Action?.ActionId && ValidXieweiPayment(p)) == true && _coreProgramEventHistoryOperations.ProgramEventHistory<XieweiTargetFrozenEvent>().Where(e => e.UseId == use.Id).OrderBy(e => e.Index).Select(e => (e.Index, e.TargetSeat, e.Maximum, e.ActionId)).SequenceEqual(targets.Select((t, i) => (i, t, use.XieweiTargetMaximum.Value, use.Action!.ActionId)));
}
