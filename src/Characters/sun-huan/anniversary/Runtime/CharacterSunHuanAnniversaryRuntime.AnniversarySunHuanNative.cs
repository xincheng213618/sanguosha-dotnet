#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHuanAnniversaryRuntime
{
    internal static bool SameSunHuanAction(LegalAction a, LegalAction b) => a.Kind == b.Kind && a.CardId == b.CardId && a.PlayedCardKind == b.PlayedCardKind && a.TargetCardId == b.TargetCardId && a.ProgramActivationId == b.ProgramActivationId && a.ConversionSource == b.ConversionSource && (a.AdditionalConversionSources ?? []).SequenceEqual(b.AdditionalConversionSources ?? []) && a.TargetSeats.SequenceEqual(b.TargetSeats);
    internal IReadOnlyList<int> SunHuanExpectedTargets(AnniversarySunHuanNative n)
    {
        var a = n.Action;
        var actor = _coreMatchParticipantsPlayersAccess._players[n.Actor];
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreCardAttackStateOperations.GetAttackCard(n.CardId), a.ConversionSource);
        if (a.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)is { SingleCardTrickUse: true } rule)
            return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, rule.OutputKind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _coreCardAttackStateOperations.GetAttackCard(n.CardId)), rule.ExcludeOwnerEffects, enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)), hasActualColor: true, physicalCardIds: [n.CardId]).Where(o => o.ActionKind == a.Kind && o.TargetCardId == a.TargetCardId && (o.TargetSeats.SequenceEqual(a.TargetSeats) || a.ProgramActivationId == "red-additional-targets" && o.TargetSeats.SequenceEqual(a.TargetSeats.Take(o.TargetSeats.Count)))).OrderByDescending(o => o.TargetSeats.Count).Select(o => o.TargetSeats).First();
        if (_coreGuanduCardsOperations.GuanduExpectedNativeTargets(actor, card, a)is { } guandu)
            return guandu;
        if (_coreDragonBoatCardsOperations.DragonBoatExpectedNativeTargets(actor, card, a)is { } dragon)
            return dragon;
        if (a.Kind == LegalActionKind.Peach)
            return[actor.Seat];
        if (a.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(a.PlayedCardKind ?? card.Kind, actor.Seat, a.TargetSeats);
        var kind = a.PlayedCardKind ?? card.Kind;
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(i => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, i)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 45400)]
    internal void BindSunHuanNative(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { AnniversarySunHuan: { Stage: AnniversarySunHuanStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        if (!ValidSunHuan(f) || use.CardId != n.CardId || use.SourceSeat != f.OwnerSeat || use.CardKind != (n.Action.PlayedCardKind ?? _coreCardAttackStateOperations.GetAttackCard(n.CardId).Kind) || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || a.PhysicalCards is not [var cost] || cost.CardId != n.CardId || cost.From != CardLocation.Hand(f.OwnerSeat) || !a.ConversionChain.SequenceEqual((n.Action.ConversionSource is { } source ? new[] { source } : []).Concat(n.Action.AdditionalConversionSources ?? [])) || !use.TargetSeats.SequenceEqual(SunHuanExpectedTargets(n)))
            throw new InvalidOperationException("Niji must issue its exact drawn Hand entity, actual native kind, conversion and legal targets.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversarySunHuan = r with { Stage = AnniversarySunHuanStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { AnniversarySunHuanUse = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunHuanUseIssuedEvent(f.Id, use.Id, a.ActionId, n.CardId));
    }

    internal bool IsSunHuanNative(CardUseFrame use) => use.AnniversarySunHuanUse is { } n && use.Id == n.UseId && use.CardId == n.CardId && use.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ActorSeat == n.Actor && a.ProviderSeat == n.Actor && a.PhysicalCards is [var cost] && cost.CardId == n.CardId && cost.From == CardLocation.Hand(n.Actor) && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == n.FrameId)is ProgramSkillFrame { AnniversarySunHuan.Native: { } pending } parent && ValidSunHuan(parent) && SameSunHuanNative(n, pending) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversarySunHuanUseIssuedEvent>().Count(e => e == new AnniversarySunHuanUseIssuedEvent(parent.Id, use.Id, n.ActionId, n.CardId)) == 1;
    internal static bool SameSunHuanNative(AnniversarySunHuanNative a, AnniversarySunHuanNative b) => (a with
    {
        Action = b.Action
    }

    ) == b && SameSunHuanAction(a.Action, b.Action);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 45400)]
    internal bool SunHuanOffTurnSlash(long id, int actor) => actor != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && use.AnniversarySunHuanUse?.Actor == actor && IsSunHuanNative(use);
    [NativeUseValidation(45400)]
    internal void ValidateSunHuanNativeReturn(CardUseFrame use)
    {
        if (use.AnniversarySunHuanUse is not null && (use.Step != ResolutionFrameStep.Completed || !IsSunHuanNative(use)))
            throw new InvalidOperationException("Niji cannot return a different or unfinished native use.");
    }

    [NativeUseReturn(45400)]
    internal void ReturnSunHuanNative(CardUseFrame use)
    {
        if (use.AnniversarySunHuanUse is not { } n)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        var r = f.AnniversarySunHuan!;
        if (!IsSunHuanNative(use) || r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == n.CardId) != 1)
            throw new InvalidOperationException("Niji must wait for this physical use and its children exactly once.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversarySunHuan = r with { Returned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunHuanUseReturnedEvent(f.Id, use.Id, n.ActionId));
        if (use.CardAttack is not { Active: true })
            SettleSunHuanNative(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 45400)]
    internal bool ReturnSunHuanAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversarySunHuan: { Stage: AnniversarySunHuanStage.NativeUse, Returned: true } } f)
            return false;
        SettleSunHuanNative(f);
        return true;
    }

    internal void SettleSunHuanNative(ProgramSkillFrame f)
    {
        var r = f.AnniversarySunHuan!;
        var n = r.Native!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(n.UseId)is not null)
            throw new InvalidOperationException("Niji must retain the native use through its actual completion.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == n.UseId && b.AwaitingProgramFrameId is null && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId).ToArray();
        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversarySunHuanNativeBatchEvent(f.Id, n.UseId, b.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunHuan = r with { Stage = AnniversarySunHuanStage.NativeTail, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueSunHuan(f.Id);
    }

    internal bool MatchesSunHuanDeclaration(ProgramSkillFrame root, CardDeclarationFrame d) => root.AnniversarySunHuan is { Stage: AnniversarySunHuanStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r && ValidSunHuan(root) && d.OwnerSeat == n.Actor && d.TurnNumber == r.Turn && d.Return is { Purpose: CardDeclarationPurpose.Use, UseAction: { } a } ret && ret.ParentFrameId == root.Id && ret.ParentKind == root.Kind && ret.ParentStep == root.Step && ret.ActorSeat == n.Actor && SameSunHuanAction(a, n.Action) && d.Payment is { Claimed: false } p && p.DeclarationId == d.Id && p.OwnerFrameId == d.Id && p.ProviderSeat == n.Actor && p.ActorSeat == n.Actor && p.Cost.CardId == n.CardId && p.Cost.From == CardLocation.Hand(n.Actor) && n.Action.ConversionSource == p.Source && _corePlayCardHistoryOperations.ViewAsRule(p.Source)is { DeclarationValidation: not null } rule && rule.OutputKind == p.DeclaredKind && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardDeclarationCommittedEvent>().Count(e => e.DeclarationId == d.Id && e.OwnerSeat == n.Actor && e.ActorSeat == n.Actor && e.DeclaredKind == p.DeclaredKind && e.TargetSeats.SequenceEqual(a.TargetSeats)) == 1;
    internal bool IsSunHuanDeclarationQuery(CharacterState actor, int? id) => id is { } cardId && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardDeclarationFrame { Stage: CardDeclarationStage.Applying, Succeeded: true } d && _resolutionStackOperations._resolutionStack.Count > 1 && _resolutionStackOperations._resolutionStack[^2] is ProgramSkillFrame parent && MatchesSunHuanDeclaration(parent, d) && d.OwnerSeat == actor.Seat && d.Payment.Cost.CardId == cardId && _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, cardId) == d.Payment;
    internal bool TryReturnAnniversarySunHuanDeclaration(CardDeclarationFrame d, bool success)
    {
        if (success || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversarySunHuan: { Stage: AnniversarySunHuanStage.IssuingUse } r } f || !MatchesSunHuanDeclaration(f, d))
            return false;
        if (_resolutionStackOperations._resolutionStack.Any(x => x.Id == d.Id) || r.Returned || r.Native!.UseId != 0)
            throw new InvalidOperationException("Niji declaration must return once to its original accepted use request.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversarySunHuan = r with { Stage = AnniversarySunHuanStage.Choice, Native = null } });
        DiscardSunHuan(f);
        return true;
    }
}
