#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanOlRuntime
{
    internal SkillProgramStepOutcome DrawAndOfferJianjiExactCard(ProgramSkillFrame supplied, int target)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SelectedTargetSeats is not [var selected] || target != selected || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || f.TriggerId is not null || f.SkillId == "dragonboat:jianji" && !_coreDragonBoatGeneralsOperations.DragonBoatAllies(f.OwnerSeat, target))
            throw new InvalidOperationException("Jianji requires its accepted actual own-Play other-living target.");
        return IssueHuangQuanDraw(f, target, "jianji", null);
    }

    internal bool IsJianjiExactCardQuery(CharacterState actor, int? selectedId)
    {
        if (_coreJianjiDeclarationsOperations.IsJianjiDeclarationQuery(actor, selectedId))
            return true;
        if (selectedId is not { } id || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuangQuan: { Kind: "jianji", Stage: "offer" or "issuing" } p } f || p.RecipientSeat != actor.Seat || !actor.IsAlive || p.CardIds is not [var drawn] || id != drawn || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(actor.Seat) || !ValidHuangQuanDraw(f))
            return false;
        return p.Stage == "offer" || p.NativeUse is { UseId: 0, ActionId: 0 } r && r.ActorSeat == actor.Seat && r.CardId == id && r.ProgramFrameId == f.Id;
    }

    internal IReadOnlyList<LegalAction> JianjiActions(ProgramSkillFrame f)
    {
        if (f.HuangQuan is not { Kind: "jianji", Stage: "offer", CardIds: [var id] } p || !_coreMatchParticipantsPlayersAccess._players[p.RecipientSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(p.RecipientSeat))
            return[];
        return Array.AsReadOnly(_coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[p.RecipientSeat], includeProgramActions: false, selectedPhysicalCardId: id).Where(a => a.CardId == id && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && (a.ConversionSource is null || _corePlayCardHistoryOperations.ViewAsRule(a.ConversionSource)is not { } rule || rule.InputCount == 1) && a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.Recast or LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill or LegalActionKind.RevealGeneral)).Select(a => a with { TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()), AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()), SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()), SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()), SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()), SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(t => (IReadOnlyList<int>)Array.AsReadOnly(t.ToArray())).ToArray()) }).ToArray());
    }

    internal IReadOnlyList<PromptChoice> JianjiChoices(ProgramSkillFrame f)
    {
        var result = JianjiActions(f).Select((a, i) => new PromptChoice(new($"jianji.{(f.Id)}.{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats, new Dictionary<string, string> { { "program-action", "jianji-use" }, { "option", i.ToString(CultureInfo.InvariantCulture) } })).ToList();
        result.Add(new(new($"jianji.{(f.Id)}.decline"), "不使用此牌", [], [], new Dictionary<string, string> { { "program-action", "jianji-use" }, { "option", "decline" } }));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishJianjiChoice(ProgramSkillFrame f)
    {
        var p = f.HuangQuan!;
        if (JianjiActions(f).Count == 0)
        {
            FinishHuangQuan(f);
            return;
        }

        var choices = JianjiChoices(f);
        var definition = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, p.RecipientSeat, "可以使用谏计刚摸到的这张牌，或不使用。", p.CardIds, choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = p.RecipientSeat,
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, definition.Name, definition.Name, definition.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[p.RecipientSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswerBefore(0)]
    internal bool TryResolveJianjiChoice(PromptChoice choice)
    {
        if (choice.Parameters.GetValueOrDefault("program-action") != "jianji-use")
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuangQuan: { Stage: "offer" } p } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || prompt.PlayerSeat != p.RecipientSeat || !JianjiChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || !ValidHuangQuanDraw(f))
            throw new InvalidOperationException("Jianji requires its actual recipient's published exact-card use choice.");
        var value = choice.Parameters["option"];
        if (value == "decline")
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishHuangQuan(f);
            return true;
        }

        var action = JianjiActions(f)[int.Parse(value, CultureInfo.InvariantCulture)];
        var receipt = new JianjiNativeReceipt(f.Id, f.InstructionIndex, p.Source, f.GameplayHash, p.ActualTurn, p.TurnOwner, p.ActualPhase, p.RecipientSeat, p.CardIds.Single(), action, 0, 0);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuangQuan = p with { Stage = "issuing", NativeUse = receipt }, PendingMovementContinuation = null });
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[p.RecipientSeat], action);
        return true;
    }

    internal bool ValidJianjiReceipt(JianjiNativeReceipt r, ProgramSkillFrame parent, bool bound)
    {
        if (parent.HuangQuan is not { Kind: "jianji" } p || r.ProgramFrameId != parent.Id || r.Source != p.Source || r.GameplayHash != parent.GameplayHash || r.InstructionIndex != parent.InstructionIndex || r.ActorSeat != p.RecipientSeat || p.CardIds is not [var id] || r.CardId != id || r.ActualTurn != p.ActualTurn || r.TurnOwner != p.TurnOwner || r.ActualPhase != p.ActualPhase || r.Action.CardId != id || !ValidHuangQuanDraw(parent))
            return false;
        return !bound || r.UseId > 0 && r.ActionId > 0 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JianjiUseIssuedEvent>().Count(e => SameJianjiReceipt(e.Receipt, r)) == 1;
    }

    internal bool SameJianjiReceipt(JianjiNativeReceipt a, JianjiNativeReceipt b) => (a with
    {
        Action = b.Action
    }

    ) == b && a.Action.Kind == b.Action.Kind && a.Action.CardId == b.Action.CardId && a.Action.PlayedCardKind == b.Action.PlayedCardKind && a.Action.TargetCardId == b.Action.TargetCardId && a.Action.ConversionSource == b.Action.ConversionSource && a.Action.TargetSeats.SequenceEqual(b.Action.TargetSeats) && (a.Action.AdditionalConversionSources ?? []).SequenceEqual(b.Action.AdditionalConversionSources ?? []);
    internal void BindJianjiNativeUse(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramSkillFrame { HuangQuan: { Stage: "issuing", NativeUse: { UseId: 0, ActionId: 0 } r } p } parent)
            return;
        var action = use.Action ?? throw new InvalidOperationException("Jianji native use lost its real action.");
        var targets = JianjiExpectedNativeTargets(r);
        if (!ValidJianjiReceipt(r, parent, false) || use.SourceSeat != r.ActorSeat || use.CardId != r.CardId || use.CardKind != (r.Action.PlayedCardKind ?? _coreCardAttackStateOperations.GetAttackCard(r.CardId).Kind) || use.PhysicalCardIds is not [var physical] || physical != r.CardId || action.Type != CardActionType.Use || action.ActorSeat != r.ActorSeat || action.ProviderSeat != r.ActorSeat || action.RequesterSeat is not null || action.PhysicalCards is not [var cost] || cost.CardId != r.CardId || !action.ConversionChain.SequenceEqual((r.Action.ConversionSource is { } primary ? new[] { primary } : Array.Empty<CardConversionSource>()).Concat(r.Action.AdditionalConversionSources ?? [])) || !use.TargetSeats.SequenceEqual(targets))
            throw new InvalidOperationException("Jianji changed its actual drawn material, actor/provider, native action or accepted target list.");
        var bound = r with
        {
            UseId = use.Id,
            ActionId = action.ActionId
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { HuangQuan = p with { Stage = "native-use", NativeUse = bound } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { JianjiUse = bound });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JianjiUseIssuedEvent(bound));
    }

    internal IReadOnlyList<int> JianjiExpectedNativeTargets(JianjiNativeReceipt r)
    {
        var a = r.Action;
        var actor = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat];
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, _coreCardAttackStateOperations.GetAttackCard(r.CardId), a.ConversionSource);
        if (a.ConversionSource is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)is { SingleCardTrickUse: true } rule)
            return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, rule.OutputKind, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _coreCardAttackStateOperations.GetAttackCard(r.CardId)), rule.ExcludeOwnerEffects, enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)), hasActualColor: true, physicalCardIds: [r.CardId]).Where(o => o.ActionKind == a.Kind && o.TargetCardId == a.TargetCardId && (o.TargetSeats.SequenceEqual(a.TargetSeats) || a.ProgramActivationId == "red-additional-targets" && o.TargetSeats.SequenceEqual(a.TargetSeats.Take(o.TargetSeats.Count)))).OrderByDescending(o => o.TargetSeats.Count).Select(o => o.TargetSeats).First();
        if (_coreGuanduCardsOperations.GuanduExpectedNativeTargets(actor, card, a)is { } guanduTargets)
            return guanduTargets;
        if (_coreDragonBoatCardsOperations.DragonBoatExpectedNativeTargets(actor, card, a)is { } dragonTargets)
            return dragonTargets;
        var effectiveKind = a.PlayedCardKind ?? card.Kind;
        if (a.Kind == LegalActionKind.Peach)
            return[actor.Seat];
        if (a.Kind is not (LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains))
            return _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(a.PlayedCardKind ?? card.Kind, actor.Seat, a.TargetSeats);
        var offensive = a.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage;
        var seats = Enumerable.Range(offensive ? 1 : 0, _coreMatchDefinitionPlayerCountAccess._playerCount - (offensive ? 1 : 0)).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(actor.Seat, offset)]).Where(p => p.IsAlive && (!offensive || !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ExcludeGlobalTarget, effectiveKind)) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, effectiveKind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(p => p.Seat).ToArray();
        if (!offensive)
            seats = _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, seats).ToArray();
        if (a.Kind == LegalActionKind.FiveGrains)
            seats = seats.Take(_cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile)).ToArray();
        return Array.AsReadOnly(seats);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 40)]
    internal bool IsJianjiOffTurnNativeUse(long id, int actor) => actor != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { JianjiUse: { } r } use && r.ActorSeat == actor && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramFrameId)is ProgramSkillFrame parent && JianjiNativeUseRidesOn(use, parent);
    internal bool JianjiNativeUseRidesOn(CardUseFrame use, ProgramSkillFrame parent) => use.JianjiUse is { } r && parent.HuangQuan is { NativeUse: { } pending } && SameJianjiReceipt(r, pending) && ValidJianjiReceipt(r, parent, true) && use.Action is { Type: CardActionType.Use } a && a.ActionId == r.ActionId && a.ActorSeat == r.ActorSeat && a.ProviderSeat == r.ActorSeat && a.PhysicalCards is [var cost] && cost.CardId == r.CardId;
    [NativeUseValidation(50)]
    internal void AssertJianjiNativeUseCompletion(CardUseFrame use)
    {
        if (use.JianjiUse is not { } r)
            return;
        if (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramFrameId)is not ProgramSkillFrame parent || !JianjiNativeUseRidesOn(use, parent))
            throw new InvalidOperationException("Jianji must return through the exact issued native use after its children.");
    }

    [NativeUseReturn(60)]
    internal bool CompleteJianjiUseReturn(CardUseFrame use)
    {
        if (use.JianjiUse is null)
            return false;
        ReturnJianjiNativeUse(use);
        return true;
    }

    internal void ReturnJianjiNativeUse(CardUseFrame use)
    {
        if (use.JianjiUse is not { } r)
            return;
        if (_resolutionStackOperations._resolutionStack.Any(f => f.Id == use.Id) || use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.ProgramFrameId)is not ProgramSkillFrame parent || !JianjiNativeUseRidesOn(use, parent) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JianjiUseReturnedEvent>().Any(e => SameJianjiReceipt(e.Receipt, r)))
            throw new InvalidOperationException("Jianji cannot return or repay another native use.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { HuangQuan = parent.HuangQuan!with { Stage = "returned" } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JianjiUseReturnedEvent(r));
        if (use.CardAttack is not { Active: true })
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 10)]
    internal bool TryResumeJianjiAfterNativeAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuangQuan: { Stage: "returned", NativeUse: { } } } p)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(p.Id);
        return true;
    }

    internal void RecordJianjiOrdinaryAlcoholUse(long id)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.AdditionalAlcoholUses) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.DrawAndOfferJianjiExactCard) && !_characterKeBiNengOlOperations.HasKeBiNengCapability && !_characterGuoTuOlOperations.HasWeijieCapability && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQingyaBetweenHandDiscards) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveHuiyunCompletedFireAttack) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouGuoYouZhiZhongchun) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouJiangJiShiju) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouJiaXuFumou) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryBaiLingYunLinghui) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversarySunHuanNijiEnding) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiGuoYuanQingdao))
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id) ?? throw new InvalidOperationException("Ordinary alcohol requires its native use owner.");
        if (use.DyingResponse is not null || use.Action is not { Type: CardActionType.Use } action || !(use.CardKind == CardKind.Alcohol && action.EffectiveKind == CardKind.Alcohol || _coreDragonBoatCardsOperations.IsDragonBoatRealgarWineQuotaUse(use, action)))
            throw new InvalidOperationException("Jianji ordinary alcohol quota cannot consume a dying recovery or unrelated card.");
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JianjiOrdinaryAlcoholPaidEvent>().Any(e => e.NativeUseId == id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JianjiOrdinaryAlcoholPaidEvent(id, action.ActionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, action.ActorSeat, !_coreProgramCardUsePaymentsOperations.CardUsePayments.ExemptsAlcoholDebit(id, action.ActorSeat) && !_characterMiHengLeOperations.HasIssuedSiruiUse(id, action.ActorSeat) && !_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(id, action.ActorSeat) && (!_characterKeBiNengOlOperations.HasKeBiNengCapability || !_characterFuRongOlOperations.HasIssuedFuRongUnlimitedUse(id, action.ActorSeat)), use.JianjiUse?.ProgramFrameId ?? use.MouJiangJiShiju?.FrameId ?? use.MouJiaXuFumou?.FrameId ?? use.AnniversaryBlyUse?.FrameId ?? use.AnniversarySunHuanUse?.FrameId ?? use.ShiGuoYuanUse?.FrameId));
    }

    internal bool CanUseJianjiOrdinaryAlcohol(CharacterState actor, int? selectedId) => _coreProgramAlcoholQuotaOperations.HasAdditionalAlcoholUse(actor) || _characterWeiYanMobileShiOperations.HasMobileShiZhuangshiQuota(actor.Seat) || ((IsJianjiExactCardQuery(actor, selectedId) || _characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) || _characterYaDanOlOperations.IsQingyaDiscardQuery(actor, selectedId) || _characterHuBanOlOperations.IsHuiyunExactHandQuery(actor, selectedId) || _characterGuoTuOlOperations.IsWeijieOrdinaryAlcoholQuery(actor) || _characterGuoYouZhiMouMobileOperations.IsMouGyzExactCardQuery(actor, selectedId) || _characterJiangJiMouOperations.IsMouJjExactCardQuery(actor, selectedId) || _characterJiaXuMouOperations.IsMouJxExactCardQuery(actor, selectedId) || _characterBaiLingYunAnniversaryOperations.IsAnniversaryBaiLingYunExactCardQuery(actor, selectedId) || _characterSunHuanAnniversaryOperations.IsAnniversarySunHuanExactCardQuery(actor, selectedId) || _characterGuoYuanMobileShiOperations.IsShiGyHandQuery(actor, selectedId)) ? !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JianjiOrdinaryAlcoholPaidEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActorSeat == actor.Seat && e.CountsQuota) : !actor.UsedPlayPhaseAlcoholThisTurn);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 560)]
    internal void ObserveJianjiAlcoholExpiry(IGameEvent payload)
    {
        if (payload is not TurnEndedEvent ended || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.DrawAndOfferJianjiExactCard) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouJiangJiShiju) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouJiaXuFumou) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryBaiLingYunLinghui) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversarySunHuanNijiEnding) && !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiGuoYuanQingdao))
            return;
        foreach (var actor in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JianjiOrdinaryAlcoholPaidEvent>().Where(e => e.ActualTurn == ended.TurnNumber && e.JianjiProgramFrameId is not null && e.ActorSeat != ended.ActorSeat).Select(e => e.ActorSeat).Distinct().ToArray())
            if (_coreMatchParticipantsPlayersAccess._players[actor].HasAlcoholEffect)
            {
                _coreMatchParticipantsPlayersAccess._players[actor].HasAlcoholEffect = false;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(actor));
            }
    }
}
