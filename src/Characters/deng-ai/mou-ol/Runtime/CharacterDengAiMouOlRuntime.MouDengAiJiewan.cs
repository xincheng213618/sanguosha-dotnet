#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiMouOlRuntime
{
    internal PromptChoice MouDengAiChoice(ProgramSkillFrame f, string branch, string label, int? card = null, GameEngine.ProgramOrdinaryTrickUseOption? option = null) => new(new($"mou-deng-ai.{(f.Id)}.{(f.MouDengAiOl!.Stage)}.{(branch)}.{(card)}.{(option?.Id.Value)}"), label, card is { } id ? [id] : [], option?.TargetSeats ?? [], new Dictionary<string, string> { ["program-action"] = "mou-deng-ai-ol", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["option-id"] = option?.Id.Value ?? "" });
    internal IReadOnlyList<PromptChoice> MouDengAiChoices(ProgramSkillFrame f)
    {
        var r = f.MouDengAiOl!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var gu = MouDengAiGu(f.OwnerSeat);
        var list = new List<PromptChoice>();
        if (r.Stage == MouDengAiOlStage.ExchangeHand)
        {
            if (r.HandCards.Count < gu.Length)
                list.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => !r.HandCards.Contains(c.Id)).Select(c => MouDengAiChoice(f, "hand", $"选择手牌【{(c.DisplayName)}】", c.Id)));
            list.Add(MouDengAiChoice(f, "confirm", r.HandCards.Count == 0 ? "不交换" : $"交换{(r.HandCards.Count)}张"));
            return Array.AsReadOnly(list.ToArray());
        }

        if (r.Stage is MouDengAiOlStage.ExchangeGu or MouDengAiOlStage.RemoveGu)
            return Array.AsReadOnly(gu.Where(c => !r.GuCards.Contains(c.Id)).Select(c => MouDengAiChoice(f, "gu", r.Stage == MouDengAiOlStage.RemoveGu ? $"移出游戏【{(c.DisplayName)}】" : $"取回谷【{(c.DisplayName)}】", c.Id)).ToArray());
        if (r.Stage == MouDengAiOlStage.Cost)
        {
            if (owner.MaxHp > 1)
                list.Add(MouDengAiChoice(f, "maxhp", "减少1点体力上限，使用顺手牵羊"));
            if (gu.Length >= 2)
                list.Add(MouDengAiChoice(f, "remove", "移出游戏两张谷，使用顺手牵羊"));
            list.Add(MouDengAiChoice(f, "decline", "不发动解腕"));
            return Array.AsReadOnly(list.ToArray());
        }

        if (r.Stage == MouDengAiOlStage.Material)
            return Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => MouDengAiSnatchOptions(f, c.Id).Count > 0).Select(c => MouDengAiChoice(f, "material", $"将【{(c.DisplayName)}】当顺手牵羊", c.Id)).ToArray());
        if (r.Stage == MouDengAiOlStage.Snatch)
            return Array.AsReadOnly(MouDengAiSnatchOptions(f, r.CardId).Select(o => MouDengAiChoice(f, "snatch", o.Description, option: o)).ToArray());
        return[];
    }

    internal void PublishMouDengAiOl(ProgramSkillFrame f)
    {
        var choices = MouDengAiChoices(f);
        if (choices.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishMouDengAiOl(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-deng-ai-ol")]
    internal void AnswerMouDengAiOl(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDengAiOl: { } r } f || !ValidMouDengAiOl(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MouDengAiChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mou Deng Ai requires its current owner and exact live option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "decline" || branch == "confirm" && r.HandCards.Count == 0)
        {
            FinishMouDengAiOl(f);
            return;
        }

        if (branch == "hand")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { HandCards = r.HandCards.Append(choice.Cards.Single()).ToArray() } });
            PublishMouDengAiOl(f);
            return;
        }

        if (branch == "confirm")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Stage = MouDengAiOlStage.ExchangeGu } });
            PublishMouDengAiOl(f);
            return;
        }

        if (branch == "gu")
        {
            var ids = r.GuCards.Append(choice.Cards.Single()).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { GuCards = ids } });
            if (ids.Length < (r.Stage == MouDengAiOlStage.RemoveGu ? 2 : r.HandCards.Count))
            {
                PublishMouDengAiOl(f);
                return;
            }

            if (r.Stage == MouDengAiOlStage.ExchangeGu)
                ConfirmMouDengAiExchange(f);
            else
                PayMouDengAiGu(f);
            return;
        }

        if (branch == "remove")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Stage = MouDengAiOlStage.RemoveGu, GuCards = [] } });
            PublishMouDengAiOl(f);
            return;
        }

        if (branch == "maxhp")
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp <= 1 || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count == 0)
                throw new InvalidOperationException("Jiewan must pay a real reducible maximum HP with a current Hand material.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Stage = MouDengAiOlStage.CostChildren, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, -1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiCostPaidEvent(f.Id, "maximum-hp", 0, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp));
            SealMouDengAiMovement(f.Id);
            ContinueMouDengAiOl(f.Id);
            return;
        }

        if (branch == "material")
        {
            var id = choice.Cards.Single();
            if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat) || MouDengAiSnatchOptions(f, id).Count == 0)
                throw new InvalidOperationException("Jiewan lost its legal current Hand conversion.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Stage = MouDengAiOlStage.Snatch, CardId = id } });
            PublishMouDengAiOl(f);
            return;
        }

        if (branch == "snatch")
        {
            IssueMouDengAiSnatch(f, MouDengAiSnatchOptions(f, r.CardId).Single(o => o.Id.Value == choice.Parameters["option-id"]));
            return;
        }

        throw new InvalidOperationException("Unknown Mou Deng Ai choice.");
    }

    internal void PayMouDengAiGu(ProgramSkillFrame f)
    {
        var r = f.MouDengAiOl!;
        if (r.GuCards.Count != 2 || r.GuCards.Distinct().Count() != 2 || r.GuCards.Any(id => !MouDengAiIsGu(f.OwnerSeat, id)) || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count == 0)
            throw new InvalidOperationException("Jiewan must remove exactly two current Gu entities.");
        var groups = r.GuCards.GroupBy(id => _cardZonesOperations._cardZones.GetLocation(id)).Select(g => (Location: g.Key, Cards: g.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray())).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Paid = true, Stage = MouDengAiOlStage.CostChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var g in groups)
            _corePhysicalMovementFacadesOperations.MoveCards(g.Cards, g.Location, CardLocation.OutsideGame, new(MouDengAiRemoveReason), disclosure: CardMovementDisclosure.FaceUp);
        foreach (var id in r.GuCards)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiCostPaidEvent(f.Id, "remove-gu", id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == id).Sequence, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp));
        SealMouDengAiMovement(f.Id);
        ContinueMouDengAiOl(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.CollectMouDengAiJigu, SkillProgramEffectOp.ExchangeMouDengAiJigu, SkillProgramEffectOp.UseMouDengAiJiewan, SkillProgramEffectOp.GrowMouDengAiJiewan)]
    internal PromptChoice AiMouDengAiOl(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.MouDengAiOl!;
        var choices = decision.Choices!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (r.Stage == MouDengAiOlStage.ExchangeHand)
        {
            var card = choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), owner)).FirstOrDefault();
            return r.HandCards.Count == 0 && card is not null && MouDengAiGu(f.OwnerSeat).Any(g => CoreNativeAiDecisionsRuntime.GetKeepValue(g, owner) > CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(card.Cards[0]), owner)) ? card : choices.Single(c => c.Parameters["branch"] == "confirm");
        }

        if (r.Stage == MouDengAiOlStage.Cost)
            return choices.FirstOrDefault(c => c.Parameters["branch"] == "remove") ?? choices.First();
        if (r.Stage == MouDengAiOlStage.ExchangeGu)
            return choices.OrderByDescending(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), owner)).First();
        if (r.Stage is MouDengAiOlStage.RemoveGu or MouDengAiOlStage.Material)
            return choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), owner)).First();
        return choices.OrderBy(c => c.Targets.Count > 0 && CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(owner, _coreMatchParticipantsPlayersAccess._players[c.Targets[0]])).First();
    }

    internal bool DrainMouDengAiOl(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void SealMouDengAiMovement(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDengAiOl = f.MouDengAiOl!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void ContinueMouDengAiOl(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDengAiOl: { } r } f || f.Id != id)
            return;
        if (!ValidMouDengAiOl(f))
            throw new InvalidOperationException("Mou Deng Ai lost its signed source or native continuation.");
        if (r.Stage is MouDengAiOlStage.Issuing or MouDengAiOlStage.NativeUse)
            return;
        if (r.Stage is MouDengAiOlStage.CostChildren or MouDengAiOlStage.Children)
        {
            if (DrainMouDengAiOl(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MouDengAiOl!;
            if (r.Stage == MouDengAiOlStage.Children || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMouDengAiOl(f);
                return;
            }

            var count = MouDengAiGu(f.OwnerSeat).Length;
            var x = Math.Max(1, count);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = r with { Distance = x, Stage = MouDengAiOlStage.Material }, PendingMovementContinuation = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiDistanceFrozenEvent(f.Id, count, x));
        }

        PublishMouDengAiOl(f);
    }

    internal void FinishMouDengAiOl(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDengAiCompletedEvent(f.Id, f.MouDengAiOl!.Operation));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDengAiOl = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 33400)]
    internal bool ResumeMouDengAiOl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDengAiOl: not null } f || f.Id != id)
            return false;
        ContinueMouDengAiOl(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 33400)]
    internal bool ReturnMouDengAiMovement(ProgramSkillFrame f) => f.MouDengAiOl is not null && ResumeMouDengAiOl(f.Id);
}
