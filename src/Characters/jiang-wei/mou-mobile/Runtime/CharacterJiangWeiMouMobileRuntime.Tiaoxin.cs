#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMouMobileRuntime
{
    internal const string MouJwGiftReason = "skill-program.mou-mobile:tiaoxin.give";
    internal static CardConversionSource MouJwSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int MouJwCharge(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].Markers.GetValueOrDefault(PlayerMarkerKind.MouJiangWeiCharge);
    internal int MouJwAvailableCharge(int owner) => MouJwCharge(owner) - _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == owner && f.MouJiangWei is { Op: SkillProgramEffectOp.ResolveMouJiangWeiTiaoxin, Accepted: true }).Sum(f => f.MouJiangWei!.Targets.Count);
    internal int MouJwSelectedCount(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiSelectionClosedEvent>().Where(e => e.OwnerSeat == owner).Sum(e => e.Count);
    internal void ChangeMouJwCharge(long frameId, int owner, int amount, long invocation = 0)
    {
        var before = MouJwCharge(owner);
        var after = Math.Clamp(checked(before + amount), 0, 4);
        if (before == after)
            return;
        var player = _coreMatchParticipantsPlayersAccess._players[owner];
        var key = (PlayerMarkerKind.MouJiangWeiCharge, owner);
        if (after == 0)
        {
            player.Markers.Remove(PlayerMarkerKind.MouJiangWeiCharge);
            player.MarkerSourceCounts.Remove(key);
        }
        else
        {
            player.Markers[PlayerMarkerKind.MouJiangWeiCharge] = after;
            player.MarkerSourceCounts[key] = after;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frameId, owner, PlayerMarkerKind.MouJiangWeiCharge, after - before, after, owner, MouJiangWeiComposition.Tiaoxin));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiChargeChangedEvent(frameId, owner, invocation, before, after));
    }

    SkillProgramStepOutcome global::CardGame.Core.IMouJiangWeiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMouJw(f, op);
    internal SkillProgramStepOutcome BeginMouJw(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (op == SkillProgramEffectOp.InitializeMouJiangWei)
        {
            if (f.WindowContext is not { Window: SkillProgramTriggerWindow.GameStarting } context || context.OwnerSeat != f.OwnerSeat || f.SkillId != MouJiangWeiComposition.Tiaoxin || f.TriggerId != "initial-charge" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting } window || context.SourceSeat != window.OwnerSeat || context.TargetSeat != window.OwnerSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) || window.ParticipantFacts?.GetValueOrDefault(f.OwnerSeat)is not { } ownerFacts || System.Text.Json.JsonSerializer.Serialize(context.Facts) != System.Text.Json.JsonSerializer.Serialize(ownerFacts) || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.OwnerSeat == f.OwnerSeat && e.Window == SkillProgramTriggerWindow.GameStarting) != 1)
                throw new InvalidOperationException("Jiang Wei's initial charge needs its exact game-start owner, participant facts and current original candidate; the shared window source is not the skill owner.");
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiInitializedEvent>().Any(e => e.OwnerSeat == f.OwnerSeat))
            {
                ChangeMouJwCharge(f.Id, f.OwnerSeat, 4);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiInitializedEvent(f.Id, f.OwnerSeat, MouJwSource(f), 4));
            }

            return SkillProgramStepOutcome.Continue;
        }
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        var wake = op == SkillProgramEffectOp.ResolveMouJiangWeiZhiji;
        if (wake ? f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: var seat } || seat != f.OwnerSeat || phase is not { Kind: ActualDiscardRecoveryPhaseKind.Preparation } || phase.ActorSeat != f.OwnerSeat || MouJwSelectedCount(f.OwnerSeat) < 4 && !_characterGuoJiaMobileGodOperations.HasShenGuoJiaAwakeningCondition(f.OwnerSeat, f.SkillId, f.TriggerId) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiAwakenedEvent>().Any(e => e.OwnerSeat == f.OwnerSeat) : op != SkillProgramEffectOp.ResolveMouJiangWeiTiaoxin || f.ActivationId != "taunt" || phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } || phase.ActorSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatAccess._currentSeat != f.OwnerSeat)
            throw new InvalidOperationException("Jiang Wei needs its actual own Play or qualified preparation.");
        var r = new MouJiangWeiReceipt(MouJwSource(f), f.GameplayHash, op, f.WindowContext?.ParentFrameId ?? 0, phase)
        {
            Stage = wake ? MouJiangWeiStage.AwakeningChildren : MouJiangWeiStage.Targets,
            Budget = wake ? _coreMatchDefinitionPlayerCountAccess._playerCount : Math.Max(0, MouJwAvailableCharge(f.OwnerSeat)),
            TurnOrdinal = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Count(),
            Accepted = wake,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, phase));
        if (wake)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiAwakenedEvent(f.Id, f.OwnerSeat, r.Source, r.GameplayHash, MouJwSelectedCount(f.OwnerSeat)));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().ChangeMaximumHp(f, -1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillAwakenedEvent(f.OwnerSeat, f.SkillId, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, Array.Empty<string>()));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), MouJiangWei = f.MouJiangWei!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            ContinueMouJw(f.Id);
        }
        else
            PublishMouJw(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice MouJwChoice(ProgramSkillFrame f, string branch, string text, int? card = null, int? target = null, int option = -1) => new(new($"mou-jiang-wei.{(f.Id)}.{(f.MouJiangWei!.Cursor)}.{(branch)}.{(card ?? target ?? option)}"), text, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mou-jiang-wei", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["option"] = option.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> MouJwChoices(ProgramSkillFrame f)
    {
        var r = f.MouJiangWei!;
        var choices = new List<PromptChoice>();
        if (r.Stage is MouJiangWeiStage.Targets or MouJiangWeiStage.MarkTargets)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (r.Stage == MouJiangWeiStage.MarkTargets || p.Seat != f.OwnerSeat) && (r.Targets.Contains(p.Seat) || r.Targets.Count < r.Budget)))
                choices.Add(MouJwChoice(f, "target", (r.Targets.Contains(p.Seat) ? "取消：" : "选择：") + p.Name, target: p.Seat));
            choices.Add(MouJwChoice(f, "confirm", r.Stage == MouJiangWeiStage.Targets ? $"挑衅{(r.Targets.Count)}人" : $"令{(r.Targets.Count)}人获得北伐"));
        }
        else if (r.Stage == MouJiangWeiStage.Responding)
        {
            var actor = r.Targets[r.Cursor];
            var plans = MouJwSlashPlans(f);
            for (var i = 0; i < plans.Count; i++)
                choices.Add(new(new($"mou-jiang-wei.{(f.Id)}.{(r.Cursor)}.slash.{(i)}"), plans[i].Action.Description, plans[i].Materials, plans[i].Action.TargetSeats, new Dictionary<string, string> { ["program-action"] = "mou-jiang-wei", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "slash", ["option"] = i.ToString(CultureInfo.InvariantCulture) }));
            if (CanRequestMouJwFaction(f))
                choices.Add(MouJwChoice(f, "faction", "发动自己的势力供杀能力，对其使用杀"));
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[actor])))
                choices.Add(MouJwChoice(f, "give", "交给其【" + card.DisplayName + "】", card.Id));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouJw(ProgramSkillFrame f)
    {
        var r = f.MouJiangWei!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishMouJw(f);
            return;
        }

        var chooser = r.Stage == MouJiangWeiStage.Responding ? r.Targets[r.Cursor] : f.OwnerSeat;
        var choices = MouJwChoices(f);
        if (choices.Count == 0)
        {
            NextMouJwRecipient(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouJiangWeiStage.Responding ? "挑衅：对其使用一张无距离限制的杀，或交给其一张手牌/装备。" : r.Stage == MouJiangWeiStage.MarkTargets ? "志继：选择任意名角色获得北伐（可为零人）。" : $"挑衅：选择至多{(r.Budget)}名其他角色（按选择顺序结算）。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-jiang-wei")]
    internal void AnswerMouJw(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiangWei: { } r } f || !ValidMouJw(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouJiangWeiStage.Responding ? r.Targets[r.Cursor] : f.OwnerSeat) || !MouJwChoices(f).Any(x => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([x], [c])))
            throw new InvalidOperationException("Jiang Wei's response changed its original private chooser or legal plan.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = c.Parameters["branch"];
        if (branch == "target")
        {
            var seat = c.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r with { Targets = r.Targets.Contains(seat) ? r.Targets.Where(x => x != seat).ToArray() : r.Targets.Append(seat).ToArray() } });
            PublishMouJw(f);
            return;
        }

        if (branch == "confirm")
        {
            if (r.Stage == MouJiangWeiStage.MarkTargets)
            {
                GrantMouJwBeifa(f);
                FinishMouJw(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            }

            if (r.Targets.Count > MouJwAvailableCharge(f.OwnerSeat))
                throw new InvalidOperationException("Tiaoxin cannot reserve already committed charge.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r with { Accepted = true, Stage = MouJiangWeiStage.Responding } });
            for (var i = 0; i < r.Targets.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiTargetAcceptedEvent(f.Id, f.OwnerSeat, i, r.Targets[i]));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiSelectionClosedEvent(f.Id, f.OwnerSeat, r.Targets.Count));
            ContinueMouJw(f.Id);
            return;
        }

        if (branch == "slash")
        {
            IssueMouJwSlash(f, int.Parse(c.Parameters["option"], CultureInfo.InvariantCulture));
            return;
        }

        if (branch == "faction")
        {
            BeginMouJwFaction(f);
            return;
        }

        if (branch != "give")
            throw new InvalidOperationException("Unknown mobile Jiang Wei choice.");
        var actor = r.Targets[r.Cursor];
        var id = c.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        if (from.OwnerSeat != actor || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("Tiaoxin needs the responder's real HE entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r with { Stage = MouJiangWeiStage.GiftChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new(MouJwGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, actor, f.OwnerSeat, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.Unspecified);
        var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(x => x.Sequence > r.After && x.CardId == id && x.From == from && x.Reason.Value == MouJwGiftReason);
        var paid = new MouJiangWeiPayment(id, card.Kind, from, moved.To, moved.Sequence);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouJiangWei = f.MouJiangWei!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Payment = paid } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiGiftPaidEvent(f.Id, r.Cursor, actor, paid));
        ContinueMouJw(f.Id);
    }

    internal bool DrainMouJw(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouJw(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouJiangWei!;
        if (r.Stage is MouJiangWeiStage.GiftChildren or MouJiangWeiStage.AwakeningChildren)
        {
            if (DrainMouJw(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == MouJiangWeiStage.GiftChildren)
            {
                NextMouJwRecipient(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMouJw(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r with { Stage = MouJiangWeiStage.MarkTargets } });
            PublishMouJw(f);
            return;
        }

        if (r.Stage == MouJiangWeiStage.NativeReturned)
        {
            NextMouJwRecipient(f);
            return;
        }

        if (r.Stage == MouJiangWeiStage.Responding)
        {
            if (r.Cursor >= r.Targets.Count || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishMouJw(f);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]].IsAlive)
            {
                NextMouJwRecipient(f);
                return;
            }

            PublishMouJw(f);
            return;
        }

        if (r.Stage is MouJiangWeiStage.Targets or MouJiangWeiStage.MarkTargets)
            PublishMouJw(f);
    }

    internal void NextMouJwRecipient(ProgramSkillFrame f)
    {
        var r = f.MouJiangWei!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = r with { Cursor = r.Cursor + 1, Stage = MouJiangWeiStage.Responding, Payment = null, UseId = 0, ActionId = 0, NativeAction = null, Materials = [], FactionPolicy = null, FactionTried = false, ProviderSeat = -1, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = null });
        ContinueMouJw(f.Id);
    }

    internal void FinishMouJw(ProgramSkillFrame f)
    {
        var r = f.MouJiangWei!;
        if (r.Op == SkillProgramEffectOp.ResolveMouJiangWeiTiaoxin && r.Accepted)
            ChangeMouJwCharge(f.Id, f.OwnerSeat, -r.Targets.Count);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouJiangWeiCompletedEvent(f.Id, r.Op, r.Targets.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouJiangWei = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouJiangWeiTiaoxin, SkillProgramEffectOp.ResolveMouJiangWeiZhiji)]
    internal PromptChoice ChooseMouJw(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.MouJiangWei!;
        if (r.Stage is MouJiangWeiStage.Targets or MouJiangWeiStage.MarkTargets)
        {
            var target = p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "target" && !r.Targets.Contains(c.Targets.Single()) && c.Targets.Single() != f.OwnerSeat && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]));
            return target ?? p.Choices.Single(c => c.Parameters["branch"] == "confirm");
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[r.Targets[r.Cursor]];
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(actor, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) && p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "faction")is { } faction)
            return faction;
        if (!global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(actor, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) && p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "slash")is { } slash)
            return slash;
        return p.Choices.Where(c => c.Parameters["branch"] == "give").OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), actor)).FirstOrDefault() ?? p.Choices.First();
    }
}
