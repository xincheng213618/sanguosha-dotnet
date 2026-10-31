#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouRuntime
{
    internal bool IsMouZyExactCardQuery(CharacterState actor, int? id)
    {
        if (id is not { } card || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhouYuAnniversary: { Yin: true, Stage: MouZhouYuStage.OfferUse or MouZhouYuStage.IssuingUse } r } f || r.Actor != actor.Seat || !actor.IsAlive || !r.Remaining.Contains(card) || _cardZonesOperations._cardZones.GetLocation(card) != CardLocation.Hand(actor.Seat) || !MouZyAnniversary.Valid(f))
            return false;
        return r.Stage == MouZhouYuStage.OfferUse || r.Native is { UseId: 0, ActionId: 0 } n && n.Actor == actor.Seat && n.CardId == card && n.FrameId == f.Id;
    }

    internal IReadOnlyList<int> MouZyExpectedTargets(int actor, LegalAction action)
    {
        if (action.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(action.PlayedCardKind ?? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[actor], _coreAdvancedLifecycleOperations.GetAdvancedCard(action.CardId!.Value)), actor, action.TargetSeats);
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(_coreMatchParticipantsPlayersAccess._players[actor], _coreAdvancedLifecycleOperations.GetAdvancedCard(action.CardId!.Value), action.ConversionSource);
        var kind = action.PlayedCardKind ?? card.Kind;
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actor], card);
        return Array.AsReadOnly(Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor, offset)]).Where(p => p.IsAlive && !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(suit)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor, p.Seat, suit) && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(actor, p.Seat, kind))).Select(p => p.Seat).ToArray());
    }

    internal IReadOnlyList<LegalAction> MouZyUseOptions(ProgramSkillFrame f)
    {
        if (f.MouZhouYuAnniversary is not { Yin: true, Stage: MouZhouYuStage.OfferUse } r || !_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        return Array.AsReadOnly(r.Remaining.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(r.Actor)).SelectMany(id => _coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[r.Actor], includeProgramActions: false, selectedPhysicalCardId: id)).Where(a => a.CardId is { } id && r.Remaining.Contains(id) && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && (a.PlayedCardKind ?? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.Actor], _coreAdvancedLifecycleOperations.GetAdvancedCard(id)))is var kind && (CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) == SkillProgramCardCategory.Trick && (CoreGameEngineMouLuMengRuntime.IsDamageCardKind(kind) || CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(kind) || kind == CardKind.Lightning)) && a.Kind is not (LegalActionKind.Recast or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect) && MouZyExpectedTargets(r.Actor, a).Contains(r.Target)).Select(a => a with { TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()), AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()), SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()), SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()), SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()), SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(x => (IReadOnlyList<int>)Array.AsReadOnly(x.ToArray())).ToArray()) }).ToArray());
    }

    internal static IEnumerable<IReadOnlyList<int>> MouZyExtraGroups(IReadOnlyList<int> seats, int maximum)
    {
        yield return Array.Empty<int>();
        for (var size = 1; size <= Math.Min(maximum, seats.Count); size++)
            foreach (var group in Build(0, size, []))
                yield return group;
        IEnumerable<IReadOnlyList<int>> Build(int start, int left, IReadOnlyList<int> chosen)
        {
            if (left == 0)
            {
                yield return Array.AsReadOnly(chosen.ToArray());
                yield break;
            }

            for (var i = start; i <= seats.Count - left; i++)
                foreach (var group in Build(i + 1, left - 1, [..chosen, seats[i]]))
                    yield return group;
        }
    }

    internal IReadOnlyList<IReadOnlyList<int>> MouZyFireOptions(ProgramSkillFrame f)
    {
        if (f.MouZhouYuAnniversary is not { Yin: false, Stage: MouZhouYuStage.OfferUse, Target: >= 0 } r || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        var allowed = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.FireAttack, physicalSuit: Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []).SelectMany(o => o.TargetSeats).Distinct().ToArray();
        if (!allowed.Contains(r.Target))
            return[];
        return Array.AsReadOnly(MouZyExtraGroups(allowed.Where(t => t != r.Target).ToArray(), MouZyAnniversary.Budget(f.OwnerSeat)).Select(extra => (IReadOnlyList<int>)Array.AsReadOnly(new[] { r.Target }.Concat(extra).ToArray())).ToArray());
    }

    internal void IssueMouZyUse(ProgramSkillFrame f)
    {
        if (!MouZyAnniversary.Valid(f) || f.MouZhouYuAnniversary is not { Stage: MouZhouYuStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r)
            throw new InvalidOperationException("Yingmou must issue from its current accepted native request.");
        if (r.Yin)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouZhouYuAnniversary = r with { Stage = MouZhouYuStage.OfferUse } });
            var canonical = MouZyUseOptions(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)).SingleOrDefault(a => a.CardId == n.CardId && a.Kind == n.Action!.Kind && a.PlayedCardKind == n.Action.PlayedCardKind && a.TargetCardId == n.Action.TargetCardId && a.ConversionSource == n.Action.ConversionSource && a.TargetSeats.SequenceEqual(n.Action.TargetSeats));
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f);
            if (canonical is null)
                throw new InvalidOperationException("Yingmou's accepted exact Hand card or native legal targets expired.");
            _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[n.Actor], canonical);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouZhouYuAnniversary = r with { Stage = MouZhouYuStage.OfferUse } });
        var valid = MouZyFireOptions(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)).Any(t => t.SequenceEqual(n.Targets));
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f);
        if (!valid)
            throw new InvalidOperationException("Yingmou's zero-material FireAttack targets expired.");
        var card = new Card(0, CardKind.FireAttack, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, n.Actor, n.Targets, CardKind.FireAttack, physicalCardIds: []);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, n.Actor, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, LegalActionKind.FireAttack, null, null, CardKind.FireAttack);
    }

    internal void BindMouZyNativeUse(CardUseFrame use)
    {
        if (use.Action is not { Type: CardActionType.Use } a)
            return;
        MouZyAnniversary.Consume(use.Id, a.ActionId, a.ActorSeat, use.CardKind);
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { MouZhouYuAnniversary: { Stage: MouZhouYuStage.IssuingUse, Native: { UseId: 0, ActionId: 0 } n } r } f)
            return;
        var expected = r.Yin ? MouZyExpectedTargets(n.Actor, n.Action!) : n.Targets;
        if (!MouZyAnniversary.Valid(f) || a.ActorSeat != n.Actor || a.ProviderSeat != n.Actor || a.RequesterSeat is not null || use.SourceSeat != n.Actor || use.CardId != n.CardId || use.CardKind != n.Kind || !use.TargetSeats.SequenceEqual(expected) || r.Yin && (a.PhysicalCards is not [{ CardId: var id, From: var from }] || id != n.CardId || from != CardLocation.Hand(n.Actor) || use.PhysicalCardIds is not [var physical] || physical != n.CardId) || !r.Yin && (use.CardKind != CardKind.FireAttack || a.PhysicalCards.Count != 0 || use.PhysicalCardIds.Count != 0))
            throw new InvalidOperationException("Yingmou changed its native actor/provider, exact Hand entity, zero-material FireAttack or original target.");
        var bound = n with
        {
            UseId = use.Id,
            ActionId = a.ActionId,
            Targets = expected
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouZhouYuAnniversary = r with { Stage = MouZhouYuStage.NativeUse, Native = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouZhouYuYingmou = bound, UnlimitedUse = r.Yin || u.UnlimitedUse == true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuUseIssuedEvent(f.Id, use.Id, a.ActionId, n.Actor, n.CardId, r.Target));
    }

    internal bool MouZyNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => MouZyAnniversary.Valid(f) && u.MouZhouYuYingmou is { } n && f.MouZhouYuAnniversary is { Stage: MouZhouYuStage.NativeUse, Native: { } p } && MouZhouYuRuntime.SameUse(n, p) && n.FrameId == f.Id && n.Instruction == f.InstructionIndex && n.Source == f.MouZhouYuAnniversary.Source && n.GameplayHash == f.GameplayHash && n.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber && u.Id == n.UseId && u.SourceSeat == n.Actor && u.CardKind == n.Kind && u.CardId == n.CardId && u.TargetSeats.SequenceEqual(n.Targets) && u.Action is { Type: CardActionType.Use } a && a.ActionId == n.ActionId && a.ActorSeat == n.Actor && a.ProviderSeat == n.Actor && (n.CardId == 0 ? a.PhysicalCards.Count == 0 : a.PhysicalCards is [{ CardId: var id, From: var from }] && id == n.CardId && from == CardLocation.Hand(n.Actor)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuUseIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == u.Id && e.ActionId == a.ActionId && e.Actor == n.Actor && e.CardId == n.CardId) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 36000)]
    internal bool MouZyOffTurnUse(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouZhouYuYingmou: { } n } u && n.Actor == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { } parent && MouZyNativeRidesOn(u, parent);
    [NativeUseValidation(36000)]
    internal void AssertMouZyUse(CardUseFrame u)
    {
        if (u.MouZhouYuYingmou is { } n && (u.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != u.Id || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is not { } f || !MouZyNativeRidesOn(u, f)))
            throw new InvalidOperationException("Yingmou can only retire its exact issued completed native child.");
    }

    [NativeUseReturn(36000)]
    internal bool ReturnMouZyUse(CardUseFrame u)
    {
        if (u.MouZhouYuYingmou is not { } n)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(u.Id)is not null || !MouZyNativeRidesOn(u, f) || u.Step != ResolutionFrameStep.Completed || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhouYuUseReturnedEvent>().Any(e => e.UseId == u.Id))
            throw new InvalidOperationException("Yingmou cannot return an unrelated or already completed native use.");
        MouZyAnniversary.Returned(n, u.CardAttack is { Active: true });
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 36000)]
    internal bool ResumeMouZyUse()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhouYuAnniversary: { Stage: MouZhouYuStage.ReturnedUse } } f)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
