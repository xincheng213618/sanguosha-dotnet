#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhenJiHappyGodRuntime
{
    internal const string HappyShenfu = "happy:shenfu", HappyQixian = "happy:qixian", HappyFeifu = "happy:feifu";
    internal bool HasHappyZhenJi => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveHappyZhenJiShenfu) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreHappyZhenJiQixian) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.CollectHappyZhenJiFeifu);

    internal static CardConversionSource HappyZjSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal IEnumerable<HappyZjStartedEvent> HappyShenfuUses(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZjStartedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Op == SkillProgramEffectOp.ResolveHappyZhenJiShenfu && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    internal bool HappyShenfuTarget(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !HappyShenfuUses(owner).Any(e => e.Target == target);
    internal IEnumerable<PublicPersistentPileSource> HappyZjPiles(int owner) => _corePublicPileSourcesOperations.PublicPileSources(owner).Where(p => p.SkillId is HappyQixian or HappyFeifu);
    internal HappyZjEntity[] HappyZjPileEntities(int owner) => HappyZjPiles(owner).SelectMany(p => _corePublicPileSourcesOperations.PublicPileCards(p).Select(c => new HappyZjEntity(c.Id, c.Kind, p.Location))).ToArray();
    SkillProgramStepOutcome IHappyZhenJiHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappyZj(f, op);
    bool IHappyZhenJiQuery.Allows(int owner, int target) => HappyShenfuTarget(owner, target);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41240, SkillProgramEffectOp.StoreHappyZhenJiQixian, SkillProgramEffectOp.CollectHappyZhenJiFeifu, SkillProgramEffectOp.ReturnHappyZhenJiQixian)]
    internal bool HappyZjEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Id switch
    {
        "play-ending-store" => _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Hp > 0 && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Any(card => _corePublicPileCashOutOperations.PublicPileStorageCard(c.OwnerSeat, c.SkillId, c.SkillInstanceId, card, CardLocation.Hand(c.OwnerSeat))),
        "seven-at-turn-end" => HappyZjPileEntities(c.OwnerSeat).Length >= 7,
        "finished-feifu" => _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(p => p.Id == w.ParentFrameId)is { } p && HappyFeifuCost(p, c.OwnerSeat, c.SkillInstanceId)is not null,
        _ => false
    };
    internal CardActionCost? HappyFeifuCost(ProgramCardTriggerWindowFrame w, int owner, string instance)
    {
        if (w.ResponseCompletion is not { CostsDrained: true } || !_coreResponseCompletionOperations.ValidResponseCompletionWindow(w) || w.Action.EffectiveKind != CardKind.Dodge || w.Action.PhysicalCards is not [var card] || card.From.OwnerSeat != owner || card.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !w.Action.ConversionChain.Contains(new CardConversionSource(HappyFeifu, "black-as-dodge", owner, instance)) || !_coreResponseCompletionOperations.IsResponseCompletionCostFinished(w, card) || _cardZonesOperations._cardZones.GetLocation(card.CardId) != CardLocation.DiscardPile || _coreProgramEventHistoryOperations.ProgramEventHistory<HappyZjFeifuClaimedEvent>().Any(e => e.ActionId == w.Action.ActionId && e.CardId == card.CardId))
            return null;
        return card;
    }

    internal SkillProgramStepOutcome BeginHappyZj(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyZhenJi is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Happy Zhen Ji requires her original unexecuted instruction.");
        var r = new HappyZjReceipt(HappyZjSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, f.WindowContext?.ParentFrameId ?? 0)
        {
            Target = f.OwnerSeat
        };
        if (op == SkillProgramEffectOp.ResolveHappyZhenJiShenfu)
        {
            if (f.TriggerId is not null || f.ActivationId != "three-options" || f.SelectedTargetSeats is not [var target] || !HappyShenfuTarget(f.OwnerSeat, target) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, "three-options")) < 1)
                throw new InvalidOperationException("Shenfu lost its exact actual Play debit or unused target.");
            r = r with
            {
                Target = target,
                Stage = HappyZjStage.Branch,
                Ordinal = HappyShenfuUses(f.OwnerSeat).Count() + 1
            };
        }
        else if (op == SkillProgramEffectOp.StoreHappyZhenJiQixian)
            r = r with
            {
                Stage = HappyZjStage.SelectStore,
                Limit = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp),
                Pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue)
            };
        else if (op == SkillProgramEffectOp.ReturnHappyZhenJiQixian)
        {
            var entities = HappyZjPileEntities(f.OwnerSeat);
            if (entities.Length < 7)
                return SkillProgramStepOutcome.Continue;
            r = r with
            {
                Stage = HappyZjStage.ReturnChildren,
                Entities = entities
            };
        }
        else
        {
            var w = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(p => p.Id == r.Parent);
            var cost = HappyFeifuCost(w, f.OwnerSeat, f.SkillInstanceId);
            if (cost is null)
                return SkillProgramStepOutcome.Continue;
            r = r with
            {
                Stage = HappyZjStage.FeifuChildren,
                Action = w.Action.ActionId,
                Pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue),
                Entities = [new(cost.CardId, cost.CardKind, CardLocation.DiscardPile)]
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhenJi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZjStartedEvent(f.Id, r.Source, r.Hash, op, r.Turn, r.TurnOwner, r.Phase, r.Parent, r.Target, r.Ordinal));
        if (r.Stage is HappyZjStage.ReturnChildren or HappyZjStage.FeifuChildren)
            MoveHappyZjEntities(f, r.Stage == HappyZjStage.ReturnChildren ? CardLocation.Hand(f.OwnerSeat) : r.Pile!.Location);
        ResumeHappyZj(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> HappyZjChoices(ProgramSkillFrame f)
    {
        var r = f.HappyZhenJi!;
        PromptChoice C(string option, string text, IReadOnlyList<int>? cards = null, IReadOnlyDictionary<string, string>? more = null) => new(new("happy-zhen-ji." + f.Id + "." + r.Stage + "." + option), text, cards ?? [], [r.Target], new Dictionary<string, string>(more ?? new Dictionary<string, string>()) { ["program-action"] = "happy-zhen-ji", ["option"] = option });
        if (r.Stage == HappyZjStage.Branch)
        {
            var choices = new List<PromptChoice>
            {
                C("thunder", "令其受到1点雷电伤害"),
                C("draw", "令其摸一张牌")
            };
            if (HappyZjDiscards(f).Count > 0)
                choices.Add(C("discard", "弃置其一张牌"));
            return choices;
        }

        if (r.Stage == HappyZjStage.Discard)
            return HappyZjDiscards(f).Select((c, i) => C("card:" + i, c.Description, c.Cards, c.Parameters)).ToArray();
        if (r.Stage == HappyZjStage.SelectStore)
        {
            var choices = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => _corePublicPileCashOutOperations.PublicPileStorageCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, c, CardLocation.Hand(f.OwnerSeat))).Where(c => r.Selected.Contains(c.Id) || r.Selected.Count < r.Limit).Select(c => C("store:" + c.Id, (r.Selected.Contains(c.Id) ? "取消选择" : "选择") + "【" + c.DisplayName + "】", [c.Id])).ToList();
            choices.Add(C("confirm", "将已选的" + r.Selected.Count + "张手牌置于武将牌上"));
            return choices;
        }

        return[];
    }

    internal IReadOnlyList<PromptChoice> HappyZjDiscards(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.HappyZhenJi!.Target].IsAlive ? _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.HappyZhenJi.Target, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard) : [];
    internal void PublishHappyZj(ProgramSkillFrame f)
    {
        var r = f.HappyZhenJi!;
        var choices = HappyZjChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishHappyZj(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.Target], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("happy-zhen-ji")]
    internal void AnswerHappyZj(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Happy Zhen Ji lost her choice owner.");
        var r = f.HappyZhenJi!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HappyZjChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [chosen])))
            throw new InvalidOperationException("Happy Zhen Ji requires the exact current dynamic payment or branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var option = chosen.Parameters["option"];
        if (r.Stage == HappyZjStage.Branch)
        {
            r = r with
            {
                Branch = option,
                Stage = option == "thunder" ? HappyZjStage.Damaging : option == "draw" ? HappyZjStage.TargetDraw : HappyZjStage.Discard
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhenJi = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZjBranchEvent(f.Id, option));
            if (option == "thunder")
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Target, 1, nature: DamageNature.Thunder);
            else if (option == "draw")
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Target, 1, new("skill.happy-shenfu.target-draw"));
            else
                PublishHappyZj(f);
            return;
        }

        if (r.Stage == HappyZjStage.SelectStore)
        {
            if (option != "confirm")
            {
                var id = chosen.Cards.Single();
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhenJi = r with { Selected = r.Selected.Contains(id) ? r.Selected.Where(c => c != id).ToArray() : r.Selected.Append(id).ToArray() } });
                PublishHappyZj(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            }

            if (r.Selected.Count == 0)
            {
                FinishHappyZj(f);
                return;
            }

            var entities = r.Selected.Select(id => new HappyZjEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, CardLocation.Hand(f.OwnerSeat))).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhenJi = r with { Stage = HappyZjStage.StoreChildren, Entities = entities } });
            MoveHappyZjEntities(f, r.Pile!.Location);
            ResumeHappyZj(f.Id);
            return;
        }

        var zone = Enum.Parse<CardZoneKind>(chosen.Parameters["source-zone"]);
        var index = int.Parse(chosen.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = _cardZonesOperations._cardZones.CardsAt(new(zone, r.Target))[index];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhenJi = r with { Stage = HappyZjStage.EffectChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new("skill.happy-shenfu.discard"), (_, _) => SaveHappyZjPayment(f.Id, before));
        ResumeHappyZj(f.Id);
    }

    internal void MoveHappyZjEntities(ProgramSkillFrame f, CardLocation to)
    {
        var r = f.HappyZhenJi!;
        if (r.Entities.Count == 0 || r.Entities.Any(e => _cardZonesOperations._cardZones.GetLocation(e.Id) != e.From))
            throw new InvalidOperationException("Happy Zhen Ji lost her exact unpaid entity locations.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var reason = r.Stage == HappyZjStage.ReturnChildren ? "skill.happy-qixian.return" : r.Stage == HappyZjStage.FeifuChildren ? "skill.happy-feifu.store" : "skill.happy-qixian.store";
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Entities.Select(e => e.Id).ToArray(), to, new(reason), (_, moves) =>
        {
            SaveHappyZjPayment(f.Id, before);
            if (r.Stage == HappyZjStage.FeifuChildren)
            {
                var m = moves.Single();
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZjFeifuClaimedEvent(f.Id, r.Action!.Value, r.Source, m.CardId, m.From, m.To, m.Sequence));
            }
        });
    }

    internal void SaveHappyZjPayment(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.HappyZhenJi!;
        var ordinal = r.Payments.Count + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhenJi = r with { Payments = r.Payments.Append(new HappyZjPayment(ordinal, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) { Moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray() }).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZjPaidEvent(id, ordinal, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41240)]
    internal bool ResumeHappyZj(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyZhenJi: { } r } f || f.Id != id)
            return false;
        if (r.Stage is HappyZjStage.TargetDraw or HappyZjStage.RewardDraw || f.AttackAttempt is not null)
            return true;
        if (r.Stage is HappyZjStage.Branch or HappyZjStage.Discard or HappyZjStage.SelectStore)
        {
            PublishHappyZj(f);
            return true;
        }

        if (_characterZhenJiOlGodOperations.DrainOlShenfu(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.HappyZhenJi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage is HappyZjStage.Damaging or HappyZjStage.EffectChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Count != _coreMatchParticipantsPlayersAccess._players[r.Target].Hp)
            {
                FinishHappyZj(f);
                return true;
            }

            var key = (f.OwnerSeat, f.SkillId, "three-options");
            var before = _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key);
            if (r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.Phase != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || before < 1)
                throw new InvalidOperationException("Shenfu cannot refund another actual Play's debit.");
            _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = before - 1;
            var reward = Math.Min(5, r.Ordinal + 1);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhenJi = r = r with { Refunded = true, Reward = reward, Draw = null, Stage = HappyZjStage.RewardDraw } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZjRefundedEvent(f.Id, r.Turn, r.Phase, r.Target, r.Ordinal, reward, before, before - 1));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, reward, new("skill.happy-shenfu.reward"));
            return true;
        }

        FinishHappyZj(f);
        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyZhenJiShenfu)]
    internal void ReturnHappyZjDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.HappyZhenJi is not { } r || r.Stage is not (HappyZjStage.TargetDraw or HappyZjStage.RewardDraw) || r.Draw is not null || outcome.RecipientSeat != (r.Stage == HappyZjStage.TargetDraw ? r.Target : f.OwnerSeat) || outcome.RequestedCount != (r.Stage == HappyZjStage.TargetDraw ? 1 : r.Reward) || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Shenfu lost its original one-card/reward native producer return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyZhenJi = r with { Draw = outcome, Stage = r.Stage == HappyZjStage.TargetDraw ? HappyZjStage.EffectChildren : HappyZjStage.RewardChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        SaveHappyZjPayment(f.Id, outcome.SequenceBefore);
        ResumeHappyZj(f.Id);
    }

    internal void FinishHappyZj(ProgramSkillFrame f)
    {
        if (f.PendingMovementContinuation is not null || f.AttackAttempt is not null)
            throw new InvalidOperationException("Happy Zhen Ji must await her original paid child.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyZhenJi = f.HappyZhenJi!with { Stage = HappyZjStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyZjCompletedEvent(f.Id));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 41240)]
    internal bool ReturnHappyZjMovement(ProgramSkillFrame f) => f.HappyZhenJi is not null && ResumeHappyZj(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyZhenJiShenfu)]
    internal PromptChoice HappyZjAiShenfu(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.StoreHappyZhenJiQixian)]
    internal PromptChoice HappyZjAiStore(PendingDecision p, ProgramSkillFrame f) => p.Choices.First(c => c.Parameters.GetValueOrDefault("option") == "confirm");
    [ProgramAiChoice(SkillProgramEffectOp.CollectHappyZhenJiFeifu)]
    internal PromptChoice HappyZjAiFeifu(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ReturnHappyZhenJiQixian)]
    internal PromptChoice HappyZjAiReturn(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
