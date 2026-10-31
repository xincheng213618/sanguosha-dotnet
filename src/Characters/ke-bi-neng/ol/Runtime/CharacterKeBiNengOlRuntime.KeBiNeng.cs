#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKeBiNengOlRuntime
{
    internal const string PingduanRewardReason = "skill-program.ke-bi-neng.pingduan.reward", PingduanEquipmentReason = "skill-program.ke-bi-neng.pingduan.equipment";
    internal bool HasKeBiNengCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment);

    internal void ExpireKeBiNengPingduanAlcohol(int actualTurn, int turnOwner)
    {
        if (!HasKeBiNengCapability)
            return;
        var nativeIds = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<KeBiNengPingduanNativeIssuedEvent>().Where(e => e.Origin.ActualTurn == actualTurn && e.Origin.Source.OwnerSeat == turnOwner && e.Origin.CardKind == CardKind.Alcohol).Select(e => e.Origin.CardUseFrameId).ToHashSet();
        foreach (var seat in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AlcoholAppliedEvent>().Where(e => nativeIds.Contains(e.ResolutionId)).Select(e => e.SourceSeat).Distinct().ToArray())
            if (seat != turnOwner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].HasAlcoholEffect)
            {
                _coreMatchParticipantsPlayersAccess._players[seat].HasAlcoholEffect = false;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(seat));
            }
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27040)]
    internal void ObserveKeBiNengAlcoholExpiry(IGameEvent payload)
    {
        if (payload is TurnEndedEvent ended)
            ExpireKeBiNengPingduanAlcohol(ended.TurnNumber, ended.ActorSeat);
    }

    internal static CardConversionSource KeBiNengSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_ke_bi_neng_ol_KeBiNeng.KeBiNengSource(f);
    SkillProgramStepOutcome IKeBiNengProgramHost.Resolve(ProgramSkillFrame frame) => BeginKeBiNeng(frame);
    internal SkillProgramStepOutcome BeginKeBiNeng(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.KeBiNengPingduan is not null || f.SkillId != "ol:pingduan" || f.ActivationId != "ordered-three-items" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var actor] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            throw new InvalidOperationException("Pingduan requires its accepted actual Play activation and one original living recipient.");
        var r = new KeBiNengPingduanReceipt(KeBiNengSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, actor);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KeBiNengPingduan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KeBiNengPingduanStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.PhaseInstance, actor));
        PrepareKeBiNengBasic(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PrepareKeBiNengBasic(ProgramSkillFrame f)
    {
        var actions = BuildKeBiNengPingduanBasicActions(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KeBiNengPingduan = f.KeBiNengPingduan!with { BasicActions = actions } });
        var actor = f.KeBiNengPingduan!.ActorSeat;
        if (actions.Count == 0 && !_characterChengJiChengCuiOlOperations.HasTousuiProgramSlash(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor).Select(p => p.Seat)) && _characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(f, actor, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor).Select(p => p.Seat).ToArray()).Count == 0 && !_characterGuoTuOlOperations.HasWeijieGenericBasic(f) && !_characterLiuFengBoundaryOperations.HasBoundaryLiuFengGenericBasic(f) && !(_characterLiuZhangOlOperations.HasZonghuGenericBasic(f) || _characterHuanShuJunOlOperations.HasHuanpeiGenericBasic(f)))
            FinishKeBiNeng(f, "no-legal-basic-use");
        else
            PublishKeBiNeng(f);
    }

    internal IReadOnlyList<KeBiNengPingduanCard> KeBiNengRecastCards(ProgramSkillFrame f) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.KeBiNengPingduan!.ActorSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(f.KeBiNengPingduan.ActorSeat)).Select(c => new KeBiNengPingduanCard(c.Id, _cardZonesOperations._cardZones.GetLocation(c.Id), _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[f.KeBiNengPingduan.ActorSeat], c))).Where(c => CardCatalog.Get(c.EffectiveKind).CategoryName == "锦囊牌" && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreCardAttackStateOperations.GetAttackCard(c.CardId))).OrderBy(c => c.CardId).ToArray());
    internal IReadOnlyList<KeBiNengPingduanCard> KeBiNengEquipmentCards(ProgramSkillFrame f) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(f.KeBiNengPingduan!.ActorSeat).Where(c => !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c)).OrderBy(c => c.Id).Select(c => new KeBiNengPingduanCard(c.Id, _cardZonesOperations._cardZones.GetLocation(c.Id), c.Kind)).ToArray());
    internal static Dictionary<string, string> KeBiNengArgs(string option) => RuntimeSkills.RuntimeRules_ke_bi_neng_ol_KeBiNeng.KeBiNengArgs(option);
    internal static IReadOnlyList<int> KeBiNengMaterials(LegalAction a) => RuntimeSkills.RuntimeRules_ke_bi_neng_ol_KeBiNeng.KeBiNengMaterials(a);
    internal IReadOnlyList<PromptChoice> KeBiNengChoices(ProgramSkillFrame f)
    {
        var r = f.KeBiNengPingduan!;
        var choices = new List<PromptChoice>();
        if (r.Stage == KeBiNengPingduanStage.BasicChoice)
        {
            choices.AddRange(r.BasicActions.Select((a, i) => new PromptChoice(new($"pingduan.{(f.Id)}.basic.{(i)}"), a.Description, KeBiNengMaterials(a), a.TargetSeats, KeBiNengArgs(i.ToString(CultureInfo.InvariantCulture)))));
            choices.AddRange(_characterMaXiuMaTieOlOperations.KenshangProgramSlashChoices(f, r.ActorSeat, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != r.ActorSeat).Select(p => p.Seat).ToArray()));
            choices.AddRange(_characterChengJiChengCuiOlOperations.TousuiProgramSlashChoices(f, r.ActorSeat, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != r.ActorSeat).Select(p => p.Seat).ToArray()));
        }
        else if (r.Stage is KeBiNengPingduanStage.RecastChoice or KeBiNengPingduanStage.EquipmentChoice)
            choices.AddRange(r.Cards.Select(c => new PromptChoice(new($"pingduan.{(f.Id)}.{(r.Stage)}.{(c.CardId)}"), (r.Stage == KeBiNengPingduanStage.RecastChoice ? "重铸" : "交付") + "【" + CardCatalog.Get(c.EffectiveKind).DisplayName + "】", [c.CardId], [], KeBiNengArgs(c.CardId.ToString(CultureInfo.InvariantCulture)))));
        else
            throw new InvalidOperationException("Pingduan can publish only its current uncommitted item.");
        choices.Add(new(new($"pingduan.{(f.Id)}.{(r.Stage)}.decline"), "不执行此项，结束平端", [], [], KeBiNengArgs("decline")));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishKeBiNeng(ProgramSkillFrame f)
    {
        var r = f.KeBiNengPingduan!;
        var choices = KeBiNengChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.ActorSeat, r.Stage switch
        {
            KeBiNengPingduanStage.BasicChoice => "平端：使用一张合法基本牌。",
            KeBiNengPingduanStage.RecastChoice => "平端：重铸一张锦囊牌。",
            _ => "平端：可以令轲比能获得你装备区的一张牌。"
        }, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = r.ActorSeat,
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ke-bi-neng-pingduan")]
    internal void AnswerKeBiNeng(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { KeBiNengPingduan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.ActorSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], KeBiNengChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Pingduan requires its original recipient's exact published private choice.");
        var option = choice.Parameters["option"];
        if (option == "decline")
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            FinishKeBiNeng(f, "recipient-declined");
            return;
        }

        if (r.Stage == KeBiNengPingduanStage.BasicChoice)
        {
            if (!int.TryParse(option, CultureInfo.InvariantCulture, out var index) || index < 0 || index >= r.BasicActions.Count)
                throw new InvalidOperationException("Pingduan changed its frozen basic option index.");
            var selected = r.BasicActions[index];
            if (!BuildKeBiNengPingduanBasicActions(f).Any(a => SameKeBiNengAction(a, selected)))
                throw new InvalidOperationException("Pingduan basic use is no longer natively legal.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, KeBiNengPingduan = r with { Stage = KeBiNengPingduanStage.BasicIssuing, SelectedBasicAction = selected } });
            BeginKeBiNengPingduanBasicUse(f, selected);
            return;
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var card = r.Cards.Single(c => c.CardId == int.Parse(option, CultureInfo.InvariantCulture));
        if (r.Stage == KeBiNengPingduanStage.RecastChoice && !KeBiNengRecastCards(f).Contains(card) || r.Stage == KeBiNengPingduanStage.EquipmentChoice && !KeBiNengEquipmentCards(f).Contains(card))
            throw new InvalidOperationException("Pingduan changed its frozen native recast or equipment source entity.");
        PayKeBiNengCard(f, card, r.Stage == KeBiNengPingduanStage.RecastChoice);
        ContinueKeBiNeng(f.Id);
    }

    internal void PayKeBiNengCard(ProgramSkillFrame f, KeBiNengPingduanCard card, bool recast)
    {
        var r = f.KeBiNengPingduan!;
        if (recast)
            f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, r.ActorSeat, [card.CardId]);
        var grain = card.From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(card.CardId).Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat]).Select(c => c.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, KeBiNengPingduan = r with { SelectedCard = card, Cards = [], Stage = recast ? KeBiNengPingduanStage.RecastChildren : KeBiNengPingduanStage.EquipmentChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, WoodenOxGrainIds = grain } });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(card.CardId), card.From, recast ? CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat), recast ? CardMoveReasons.RecastDiscard : new(PingduanEquipmentReason), movement =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current = current with { KeBiNengPingduan = current.KeBiNengPingduan!with { After = movement.Sequence, CompletedItems = recast ? 2 : 3, PaidBatchId = _activeCardMovementBatchIdsOperations._activeCardMovementBatchIds.Peek() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KeBiNengPingduanItemPaidEvent(f.Id, recast ? 2 : 3, r.ActorSeat, card.CardId, card.From, current.KeBiNengPingduan!.Before, movement.Sequence));
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { KeBiNengPingduan = f.KeBiNengPingduan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void PayKeBiNengDraw(ProgramSkillFrame f, int item, bool recast)
    {
        var r = f.KeBiNengPingduan!;
        var stage = recast ? KeBiNengPingduanStage.RecastDrawChildren : item switch
        {
            1 => KeBiNengPingduanStage.BasicRewardChildren,
            2 => KeBiNengPingduanStage.RecastRewardChildren,
            _ => KeBiNengPingduanStage.EquipmentRewardChildren
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KeBiNengPingduan = r with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, WoodenOxGrainIds = [] } });
        var drawn = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], 1, true, recast ? CardMoveReasons.RecastDraw : new(PingduanRewardReason)) : [];
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KeBiNengPingduan = f.KeBiNengPingduan!with { DrawCount = drawn.Count, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KeBiNengPingduanDrawPaidEvent(f.Id, item, recast, drawn.Count, f.KeBiNengPingduan!.Before, f.KeBiNengPingduan.After));
        if (recast)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(r.ActorSeat, r.SelectedCard!.CardId, r.SelectedCard.EffectiveKind, drawn.Count));
            _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), drawn.Count);
        }
    }

    internal bool DrainKeBiNeng(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueKeBiNeng(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { KeBiNengPingduan: { } r } f && f.Id == id)
        {
            if (r.Stage is KeBiNengPingduanStage.BasicChoice or KeBiNengPingduanStage.RecastChoice or KeBiNengPingduanStage.EquipmentChoice)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishKeBiNeng(f);
                return;
            }

            if (r.Stage is KeBiNengPingduanStage.BasicIssuing or KeBiNengPingduanStage.BasicNative)
                return;
            if (DrainKeBiNeng(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.KeBiNengPingduan!;
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive)
            {
                FinishKeBiNeng(f, "recipient-dead-or-game-ended");
                return;
            }

            if (r.Stage == KeBiNengPingduanStage.BasicReturned)
            {
                PayKeBiNengDraw(f, 1, false);
                continue;
            }

            if (r.Stage == KeBiNengPingduanStage.RecastChildren)
            {
                PayKeBiNengDraw(f, 2, true);
                continue;
            }

            if (r.Stage == KeBiNengPingduanStage.RecastDrawChildren)
            {
                PayKeBiNengDraw(f, 2, false);
                continue;
            }

            if (r.Stage == KeBiNengPingduanStage.EquipmentChildren)
            {
                PayKeBiNengDraw(f, 3, false);
                continue;
            }

            if (r.Stage == KeBiNengPingduanStage.EquipmentRewardChildren)
            {
                FinishKeBiNeng(f, "three-items-completed");
                return;
            }

            var recast = r.Stage == KeBiNengPingduanStage.BasicRewardChildren;
            if (!recast && !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishKeBiNeng(f, "equipment-recipient-dead");
                return;
            }

            var cards = recast ? KeBiNengRecastCards(f) : KeBiNengEquipmentCards(f);
            if (cards.Count == 0)
            {
                FinishKeBiNeng(f, recast ? "no-trick-to-recast" : "no-equipment-to-give");
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KeBiNengPingduan = r with { Stage = recast ? KeBiNengPingduanStage.RecastChoice : KeBiNengPingduanStage.EquipmentChoice, Cards = cards } });
            PublishKeBiNeng(f);
            return;
        }
    }

    internal void FinishKeBiNeng(ProgramSkillFrame f, string reason)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KeBiNengPingduanCompletedEvent(f.Id, f.KeBiNengPingduan!.CompletedItems, reason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { KeBiNengPingduan = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27040)]
    internal bool ResumeKeBiNeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { KeBiNengPingduan: not null } f || f.Id != id)
            return false;
        ContinueKeBiNeng(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27040)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27040)]
    internal bool ContinuePaidKeBiNeng(ProgramSkillFrame f) => f.KeBiNengPingduan is not null && ValidKeBiNeng(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolvePingduanOrderedBasicRecastEquipment)]
    internal PromptChoice ChooseKeBiNengAi(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.KeBiNengPingduan!;
        var executable = p.Choices.Where(c => c.Parameters.GetValueOrDefault("option") != "decline").ToArray();
        if (executable.Length == 0)
            return p.Choices.Last();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(r.ActorSeat);
        double Hostility(int seat) => _coreAiBrainsAiBrainsAccess._aiBrains[r.ActorSeat].ScoreProgramTarget(view, seat, new(0, 0, 0, 0, 0, 1, false, false));
        if (r.Stage == KeBiNengPingduanStage.EquipmentChoice && f.OwnerSeat != r.ActorSeat && Hostility(f.OwnerSeat) > 1.5 && !executable.Any(c => _coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()).Kind == CardKind.SilverLion && _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].Hp < _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].MaxHp))
            return p.Choices.Last();
        if (r.Stage == KeBiNengPingduanStage.BasicChoice)
            return executable.OrderByDescending(c => c.Parameters.TryGetValue("option", out var basicIndex) && r.BasicActions[int.Parse(basicIndex, CultureInfo.InvariantCulture)].Kind == LegalActionKind.Peach ? 5 : c.Targets.Any(t => t != r.ActorSeat && Hostility(t) > 1.5) ? 3 : 1).First();
        return executable.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[r.ActorSeat])).First();
    }
}
