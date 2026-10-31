#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouDunMouMobileRuntime
{
    internal const string MouXiahouDunGanglie = "mou-mobile:ganglie", MouXiahouDunQingjian = "mou-mobile:qingjian";
    internal const string MouXiahouDunCollectReason = "skill-program.mou-mobile:qingjian.collect", MouXiahouDunGiftReason = "skill-program.mou-mobile:qingjian.give";
    internal bool HasMouXiahouDunCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.CollectMouXiahouDunQingjian) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMouXiahouDunGanglie);

    internal static CardConversionSource MouXiahouDunSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_xiahou_dun_mou_mobile_MouXiahouDunMobile.MouXiahouDunSource(f);
    internal SkillProgramStepOutcome StartMouXiahouDunMobile(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouXiahouDunMobile is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            throw new InvalidOperationException("Mou Xiahou Dun requires its live owning entry.");
        ActualDiscardRecoveryPhaseKey? phase = null;
        if (op == SkillProgramEffectOp.ResolveMouXiahouDunGanglie)
        {
            if (f.SkillId != MouXiahouDunGanglie || f.ActivationId != "retaliate-all" || f.TriggerId is not null || f.WindowContext is not null || !CanStartMouXiahouDunGanglie(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]))
                throw new InvalidOperationException("Ganglie requires a genuine own Play and an unspent historical attacker.");
            phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        }
        else if (f.SkillId != MouXiahouDunQingjian || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat || (op == SkillProgramEffectOp.CollectMouXiahouDunQingjian ? context.Window != SkillProgramTriggerWindow.DiscardPileReceived || f.TriggerId != "collect-non-use" : op != SkillProgramEffectOp.DistributeMouXiahouDunQingjian || context.Window != SkillProgramTriggerWindow.PlayEnding || f.TriggerId != "distribute-qingjian"))
            throw new InvalidOperationException("Qingjian requires its exact native discard or own Play-end window.");
        var r = new MouXiahouDunMobileReceipt(MouXiahouDunSource(f), f.GameplayHash, op)
        {
            Phase = phase,
            Stage = op == SkillProgramEffectOp.ResolveMouXiahouDunGanglie ? MouXiahouDunMobileStage.SelectTargets : op == SkillProgramEffectOp.CollectMouXiahouDunQingjian ? MouXiahouDunMobileStage.MovementChildren : MouXiahouDunMobileStage.SelectRecipient,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileStartedEvent(f.Id, r.Source, r.GameplayHash, op, phase));
        if (op == SkillProgramEffectOp.CollectMouXiahouDunQingjian)
        {
            var context = f.WindowContext!;
            var batch = context.MovementBatch!;
            var move = batch.Movements[context.MovementIndex!.Value];
            if (!MatchingMouXiahouDunDiscardIndexes(batch).Contains(context.MovementIndex.Value) || _cardZonesOperations._cardZones.GetLocation(move.CardId) != CardLocation.DiscardPile || MouXiahouDunQingjianCount(f.OwnerSeat) >= MouXiahouDunQingjianCapacity(f.OwnerSeat))
            {
                FinishMouXiahouDun(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            var cap = MouXiahouDunQingjianCapacity(f.OwnerSeat);
            var pile = _corePublicPileSourcesOperations.EnsurePublicPileSource(f, int.MaxValue);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r with { Pile = pile, Cards = [move.CardId], Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(move.CardId), CardLocation.DiscardPile, pile.Location, new(MouXiahouDunCollectReason), disclosure: CardMovementDisclosure.FaceUp);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileCollectedEvent(f.Id, r.Source, move.CardId, move.Sequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, cap));
            SealMouXiahouDunMovements(f.Id, before);
            ContinueMouXiahouDunMovement(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.DistributeMouXiahouDunQingjian)
        {
            var pile = MouXiahouDunPile(f.OwnerSeat, f.SkillInstanceId);
            if (pile is null || _corePublicPileSourcesOperations.PublicPileCards(pile).Count == 0)
            {
                FinishMouXiahouDun(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            var ids = _corePublicPileSourcesOperations.PublicPileCards(pile).Select(c => c.Id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r with { Pile = pile, Cards = ids } });
            for (var i = 0; i < ids.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileRosterEvent(f.Id, i, ids[i], pile.Location));
        }

        PublishMouXiahouDun(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MouXiahouDunChoice(ProgramSkillFrame f, string branch, string label, int? target = null, int? card = null) => new(new($"mou-xiahou-dun.{(f.Id)}.{(f.MouXiahouDunMobile!.Index)}.{(branch)}.{(target ?? card ?? 0)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mou-xiahou-dun-mobile", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    internal IReadOnlyList<PromptChoice> MouXiahouDunChoices(ProgramSkillFrame f)
    {
        var r = f.MouXiahouDunMobile!;
        if (r.Stage == MouXiahouDunMobileStage.SelectTargets)
        {
            var choices = MouXiahouDunGanglieTargets(f.OwnerSeat).Select(s => MouXiahouDunChoice(f, "toggle", (r.Targets.Contains(s) ? "取消：" : "选择：") + _coreMatchParticipantsPlayersAccess._players[s].Name, target: s)).ToList();
            if (r.Targets.Count > 0)
                choices.Add(MouXiahouDunChoice(f, "confirm", $"确定对{(r.Targets.Count)}名角色各造成2点伤害"));
            return Array.AsReadOnly(choices.ToArray());
        }

        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.Cards[r.Index]);
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => MouXiahouDunChoice(f, "give", $"将清俭【{(card.DisplayName)}】交给{(p.Name)}", p.Seat, card.Id)).ToArray());
    }

    internal void PublishMouXiahouDun(ProgramSkillFrame f)
    {
        var choices = MouXiahouDunChoices(f);
        if (choices.Count == 0)
        {
            FinishMouXiahouDun(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-xiahou-dun-mobile")]
    internal void AnswerMouXiahouDun(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.MouXiahouDunMobile!;
        if (!ValidMouXiahouDun(f) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouXiahouDunChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Xiahou Dun requires its exact current choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == MouXiahouDunMobileStage.SelectTargets)
        {
            if (selected.Parameters["branch"] == "toggle")
            {
                var target = selected.Targets.Single();
                var targets = r.Targets.Contains(target) ? r.Targets.Where(s => s != target).ToArray() : r.Targets.Append(target).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r with { Targets = targets } });
                PublishMouXiahouDun(f);
                return;
            }

            var roster = r.Targets.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, s)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r with { Targets = roster, Stage = MouXiahouDunMobileStage.Damaging } });
            for (var i = 0; i < roster.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileGanglieSpentEvent(f.Id, f.OwnerSeat, roster[i], i));
            IssueMouXiahouDunDamage(f);
            return;
        }

        var id = r.Cards[r.Index];
        var recipient = selected.Targets.Single();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (_cardZonesOperations._cardZones.GetLocation(id) != r.Pile!.Location)
            throw new InvalidOperationException("Qingjian distribution lost its current exact pile entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = r with { Stage = MouXiahouDunMobileStage.MovementChildren, RecipientSeat = recipient, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), r.Pile.Location, CardLocation.Hand(recipient), new(MouXiahouDunGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, recipient, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceUp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouDunMobileGiftEvent(f.Id, r.Index, id, recipient, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        SealMouXiahouDunMovements(f.Id, before);
        ContinueMouXiahouDunMovement(f.Id);
    }

    internal void ContinueMouXiahouDunMovement(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouXiahouDunMobile: { Stage: MouXiahouDunMobileStage.MovementChildren } r } f || f.Id != id)
            return;
        if (!ValidMouXiahouDun(f))
            throw new InvalidOperationException("Qingjian lost its paid movement cursor.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouXiahouDunMobile!;
        if (r.Op == SkillProgramEffectOp.CollectMouXiahouDunQingjian || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishMouXiahouDun(f);
            return;
        }

        var index = r.Index + 1;
        while (index < r.Cards.Count && _cardZonesOperations._cardZones.GetLocation(r.Cards[index]) != r.Pile!.Location)
            index++;
        if (index >= r.Cards.Count)
        {
            FinishMouXiahouDun(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, MouXiahouDunMobile = r with { Index = index, Stage = MouXiahouDunMobileStage.SelectRecipient } });
        PublishMouXiahouDun(f);
    }

    internal void FinishMouXiahouDun(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouDunMobile = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouXiahouDunGanglie, SkillProgramEffectOp.DistributeMouXiahouDunQingjian)]
    internal PromptChoice AiMouXiahouDun(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MouXiahouDunMobile!;
        if (r.Stage == MouXiahouDunMobileStage.SelectTargets)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "toggle" && !r.Targets.Contains(c.Targets.Single()) && !CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])) ?? p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "confirm") ?? p.Choices.First();
        return p.Choices.OrderByDescending(c => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])).ThenBy(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count).First();
    }

    SkillProgramStepOutcome IMouXiahouDunMobileHost.Start(ProgramSkillFrame f, SkillProgramEffectOp op) => StartMouXiahouDunMobile(f, op);
}
