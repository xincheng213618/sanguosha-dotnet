#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLeiBoAnniversaryRuntime
{
    internal const string LeiBoAction = "anniversary-lei-bo";
    internal static CardConversionSource LeiBoSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int LeiBoMarkedSeat(int owner)
    {
        var holders = _coreMatchParticipantsPlayersAccess._players.Where(p => p.MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.AnniversaryLeiBoSilei, owner)) > 0).ToArray();
        return holders is [var holder] && holder.MarkerSourceCounts[(PlayerMarkerKind.AnniversaryLeiBoSilei, owner)] == 1 ? holder.Seat : -1;
    }

    internal void MoveLeiBoMark(ProgramSkillFrame f, int target)
    {
        var previous = LeiBoMarkedSeat(f.OwnerSeat);
        var marker = PlayerMarkerKind.AnniversaryLeiBoSilei;
        // Preserve independent Lei Bo owners' attributed marks, including on a
        // dead former holder. The ordinary aggregate marker projection is shared.
        foreach (var holder in _coreMatchParticipantsPlayersAccess._players)
        {
            var key = (marker, f.OwnerSeat);
            var before = holder.MarkerSourceCounts.GetValueOrDefault(key);
            var after = holder.Seat == target ? 1 : 0;
            if (before == after)
                continue;
            if (after == 0)
                holder.MarkerSourceCounts.Remove(key);
            else
                holder.MarkerSourceCounts[key] = after;
            var aggregate = checked(holder.Markers.GetValueOrDefault(marker) + after - before);
            if (aggregate == 0)
                holder.Markers.Remove(marker);
            else
                holder.Markers[marker] = aggregate;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, holder.Seat, marker, after - before, aggregate, f.OwnerSeat, "program.lei-bo.mark"));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoMarkMovedEvent(f.Id, LeiBoSource(f), previous, target));
    }

    internal bool LeiBoDamage(ProgramSkillWindowContext c) => c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } victim, DamageFrameId: { } id } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } w && w.ParentFrameId == id && w.TargetSeat == victim && w.SourceSeat == c.SourceSeat && w.TriggerWindow == c.Window && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == id)is { } damage && damage.TargetSeat == victim && damage.SourceSeat == c.SourceSeat && damage.Amount == c.Amount && _coreDamageZoneAcquisitionOperations.DamageAcquisitionHasExactOwningAttack(damage, w);
    internal bool LeiBoDamageParent(ProgramSkillFrame f) => f.WindowContext is { } c && LeiBoDamage(c) && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Single(w => w.Id == c.ParentFrameId)is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].ToProgramCandidate());
    internal bool LeiBoLoot(int owner, ProgramSkillWindowContext c) => LeiBoDamage(c) && c.SourceSeat is { } source && c.TargetSeat is { } victim && source != victim && LeiBoMarkedSeat(owner) == source && _coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[victim]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[victim]).Count > 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLeiBoLootInvokedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Turn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Victim == victim);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49800)]
    internal bool CanOfferLeiBo(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => candidate.SkillId != AnniversaryLeiBoComposition.Silei || (trigger.Id switch
    {
        "opening-mark" => LeiBoMarkedSeat(candidate.OwnerSeat) < 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLeiBoMarkMovedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat),
        "marked-source-loot" => LeiBoLoot(candidate.OwnerSeat, c),
        "marked-victim-reply" => LeiBoDamage(c) && c.TargetSeat == LeiBoMarkedSeat(candidate.OwnerSeat),
        _ => false
    });
    internal bool CanLeiBoShuaijie(int owner) => LeiBoMarkedSeat(owner)is var marked && marked >= 0 && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && (!_coreMatchParticipantsPlayersAccess._players[marked].IsAlive || _coreMatchParticipantsPlayersAccess._players[owner].Hp > _coreMatchParticipantsPlayersAccess._players[marked].Hp && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner]).Count > _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[marked]).Count);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 49800, SkillProgramEffectOp.BeginAnniversaryLeiBoShuaijie)]
    internal bool CanActivateLeiBo(ProgramActivationLegalityContext c) => CanLeiBoShuaijie(c.OwnerSeat);
    SkillProgramStepOutcome IAnniversaryLeiBoHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.InitializeAnniversaryLeiBoSilei => InitializeLeiBo(f),
        SkillProgramEffectOp.RecordAnniversaryLeiBoSileiLoot => RecordLeiBoLoot(f),
        SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash => RequestLeiBoSlash(f),
        SkillProgramEffectOp.SettleAnniversaryLeiBoSileiPenalty => SettleLeiBoPenalty(f),
        SkillProgramEffectOp.BeginAnniversaryLeiBoShuaijie => BeginLeiBoShuaijie(f),
        SkillProgramEffectOp.FinishAnniversaryLeiBoShuaijie => FinishLeiBoShuaijie(f),
        _ => throw new InvalidOperationException("Unknown Lei Bo operation.")};
    internal SkillProgramStepOutcome InitializeLeiBo(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != AnniversaryLeiBoComposition.Silei || f.TriggerId != "opening-mark" || f.InstructionIndex != 2 || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || LeiBoMarkedSeat(f.OwnerSeat) >= 0)
            throw new InvalidOperationException("Silue requires one original other living opening target.");
        MoveLeiBoMark(f, target);
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome RecordLeiBoLoot(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.TriggerId != "marked-source-loot" || f.InstructionIndex != 1 || f.WindowContext is not { } c || !LeiBoDamageParent(f) || !LeiBoLoot(f.OwnerSeat, c))
            throw new InvalidOperationException("Silue loot requires its actual marked source and unused actual-turn victim.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoLootInvokedEvent(f.Id, LeiBoSource(f), f.GameplayHash, c.ParentFrameId, c.DamageFrameId!.Value, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, c.TargetSeat!.Value));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsAnniversaryLeiBoSelfLoot(ProgramSkillFrame f, SkillProgramEffect e) => f.SkillId == AnniversaryLeiBoComposition.Silei && f.TriggerId == "marked-source-loot" && f.ActivationId == f.TriggerId && f.InstructionIndex == 2 && e is { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.OwnerHand, Amount: 1 } && e.CardOwnerRef?.Kind == ProgramParticipantRef.EventTarget && e.ChooserRef?.Kind == ProgramParticipantRef.Owner && f.WindowContext is { TargetSeat: { } target, SourceSeat: { } source } c && target == f.OwnerSeat && source != target && LeiBoDamageParent(f) && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryLeiBoLootInvokedEvent>().Count(x => x.FrameId == f.Id && x.Source == LeiBoSource(f) && x.Hash == f.GameplayHash && x.WindowId == c.ParentFrameId && x.DamageId == c.DamageFrameId && x.Victim == target && x.Turn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && x.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) == 1;
    internal SkillProgramStepOutcome RequestLeiBoSlash(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryLeiBo is not null || f.InstructionIndex != 1 || f.TriggerId != "marked-victim-reply" || f.WindowContext is not { } c || !LeiBoDamageParent(f) || c.TargetSeat != LeiBoMarkedSeat(f.OwnerSeat))
            throw new InvalidOperationException("Silue reply requires its original marked damage victim.");
        var r = new AnniversaryLeiBoReceipt(LeiBoSource(f), f.GameplayHash, c.TargetSeat!.Value, c.ParentFrameId, c.DamageFrameId!.Value, c.SourceSeat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, AnniversaryLeiBoStage.SlashRequest);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoReplyStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.DamageId, r.Marked, r.DamageSource, r.Turn, r.TurnOwner));
        if (c.SourceSeat is not { } target || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FixedTargetSlash = new(target, false) });
        _coreFixedSlashAndDeclaredDeckProgramsOperations.PublishFixedSlashAndDeclaredDeckPrompt(f, _coreFixedSlashAndDeclaredDeckProgramsOperations.FixedTargetSlashChoices(f), "私掠：对伤害来源使用杀，否则弃置一张手牌。", [target]);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool IsAnniversaryLeiBoFixedSlash(ProgramSkillFrame f) => f is { SkillId: AnniversaryLeiBoComposition.Silei, TriggerId: "marked-victim-reply", InstructionIndex: 1, FixedTargetSlash: { IgnoreDistance: false } slash, AnniversaryLeiBo: { Stage: AnniversaryLeiBoStage.SlashRequest } r } && r.DamageSource == slash.TargetSeat && slash.TargetSeat != f.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(slash.TargetSeat) && ValidLeiBo(f);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 49800)]
    internal void ObserveLeiBoSlash(CardUseDeclaredEvent fact)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use || !CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind))
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { AnniversaryLeiBo: { Stage: AnniversaryLeiBoStage.SlashRequest } r } f || !IsAnniversaryLeiBoFixedSlash(f))
            return;
        if (r.UseId != 0 || a.ActorSeat != f.OwnerSeat || r.DamageSource is not { } target || !a.TargetSeats.Contains(target) || fact.SourceSeat != a.ActorSeat || fact.CardKind != a.EffectiveKind)
            throw new InvalidOperationException("Silue reply substituted its original real Slash actor or damage source.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryLeiBo = r with { UseId = use.Id, ActionId = a.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoSlashUsedEvent(f.Id, use.Id, a.ActionId, target));
    }

    internal SkillProgramStepOutcome SettleLeiBoPenalty(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryLeiBo is not { Stage: AnniversaryLeiBoStage.SlashRequest } r || !ValidLeiBo(f) || f.FixedTargetSlash is not null || f.InstructionIndex != 2)
            throw new InvalidOperationException("Silue penalty lost its exact returned native Slash opportunity.");
        if (r.UseId != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            CompleteLeiBo(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r with { Stage = AnniversaryLeiBoStage.PenaltyChoice } });
        PublishLeiBo(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome BeginLeiBoShuaijie(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryLeiBo is not null || f.SkillId != AnniversaryLeiBoComposition.Shuaijie || f.ActivationId != "seize-and-return-mark" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanLeiBoShuaijie(f.OwnerSeat))
            throw new InvalidOperationException("Shuaijie requires its limited actual play activation and marked participant qualification before payment.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var r = new AnniversaryLeiBoReceipt(LeiBoSource(f), f.GameplayHash, LeiBoMarkedSeat(f.OwnerSeat), 0, 0, null, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, AnniversaryLeiBoStage.BeforeMaximum)
        {
            MaximumBefore = owner.MaxHp,
            HpBefore = owner.Hp
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoShuaijieStartedEvent(f.Id, r.Source, r.Hash, r.Marked, r.Turn, r.TurnOwner, r.MaximumBefore, r.HpBefore));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome FinishLeiBoShuaijie(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryLeiBo is not { Stage: AnniversaryLeiBoStage.BeforeMaximum } r || !ValidLeiBo(f) || f.InstructionIndex != 3)
            throw new InvalidOperationException("Shuaijie cannot bypass or repeat its native maximum-HP payment.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r with { Stage = AnniversaryLeiBoStage.RewardChoice } });
        PublishLeiBo(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> LeiBoChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryLeiBo!;
        PromptChoice C(string branch, string text) => new(new($"lei-bo.{(f.Id)}.{(branch)}"), text, [], [], new Dictionary<string, string> { ["program-action"] = LeiBoAction, ["branch"] = branch });
        if (r.Stage == AnniversaryLeiBoStage.RewardChoice)
            return[C("marked", "获得私掠角色至多三张牌"), C("deck", "从牌堆获得类型各异的牌")];
        var penalty = r.Stage == AnniversaryLeiBoStage.PenaltyChoice;
        if (!penalty && r.Stage != AnniversaryLeiBoStage.PickReward)
            return[];
        var seat = penalty ? f.OwnerSeat : r.Marked;
        var list = new List<PromptChoice>();
        if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            foreach (var choice in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, seat, penalty ? [CardZoneKind.Hand] : [CardZoneKind.Hand, CardZoneKind.Equipment], penalty ? OwnedCardMoveIntent.Discard : OwnedCardMoveIntent.Obtain))
            {
                var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
                var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
                var card = _cardZonesOperations._cardZones.CardsAt(new(zone, seat))[slot];
                if (r.Selected.Contains(card.Id))
                    continue;
                var p = choice.Parameters.ToDictionary(kv => kv.Key, kv => kv.Value);
                p["program-action"] = LeiBoAction;
                p["branch"] = "card";
                p["selected-count"] = r.Selected.Count.ToString(CultureInfo.InvariantCulture);
                list.Add(choice with { Id = new($"lei-bo.{(f.Id)}.{(r.Selected.Count)}.{(zone)}.{(slot)}"), Parameters = p });
            }

        if (!penalty)
            list.Add(C("finish", $"获得已选择的 {(r.Selected.Count)} 张牌"));
        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishLeiBo(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            CompleteLeiBo(f);
            return;
        }

        var choices = LeiBoChoices(f);
        if (choices.Count == 0)
        {
            CompleteLeiBo(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.AnniversaryLeiBo!.Stage == AnniversaryLeiBoStage.PenaltyChoice ? "私掠：弃置一张手牌。" : "衰劫：选择取牌方式与原实体。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(LeiBoAction)]
    internal void AnswerLeiBo(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLeiBo: { } r } f || !ValidLeiBo(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !LeiBoChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [answer])))
            throw new InvalidOperationException("Lei Bo choice changed its original owner, source or canonical entity slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = answer.Parameters["branch"];
        if (r.Stage == AnniversaryLeiBoStage.RewardChoice)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoBranchChosenEvent(f.Id, branch));
            if (branch == "marked")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r with { Stage = AnniversaryLeiBoStage.PickReward, Branch = branch } });
                PublishLeiBo(f);
                return;
            }

            _corePhysicalDrawFacadesOperations.EnsureDrawPile();
            var ids = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().GroupBy(c => CardCatalog.Get(c.Kind).CategoryName).Select(g => g.First().Id).Take(3).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r with { Branch = branch, Selected = ids } });
            IssueLeiBoMovement(f, false);
            return;
        }

        if (branch == "finish")
        {
            IssueLeiBoMovement(f, false);
            return;
        }

        var seat = r.Stage == AnniversaryLeiBoStage.PenaltyChoice ? f.OwnerSeat : r.Marked;
        var zone = Enum.Parse<CardZoneKind>(answer.Parameters["source-zone"]);
        var slot = int.Parse(answer.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = _cardZonesOperations._cardZones.CardsAt(new(zone, seat))[slot];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r with { Selected = r.Selected.Append(card.Id).ToArray(), Branch = r.Stage == AnniversaryLeiBoStage.PenaltyChoice ? "discard" : r.Branch } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoCardSelectedEvent(f.Id, f.AnniversaryLeiBo!.Selected.Count));
        if (r.Stage == AnniversaryLeiBoStage.PenaltyChoice || f.AnniversaryLeiBo!.Selected.Count == 3)
            IssueLeiBoMovement(f, r.Stage == AnniversaryLeiBoStage.PenaltyChoice);
        else
            PublishLeiBo(f);
    }

    internal void IssueLeiBoMovement(ProgramSkillFrame f, bool discard)
    {
        var r = f.AnniversaryLeiBo!;
        var ids = r.Selected.ToArray();
        if (ids.Length == 0)
        {
            CompleteLeiBo(f);
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = r with { Stage = AnniversaryLeiBoStage.MovementChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var reason = new CardMoveReason("program.lei-bo." + r.Branch);
        var logicalDiscard = discard ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, ids, reason, f.Id) : null;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, discard ? CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat), reason, afterRequestSettled: settlement =>
        {
            if (!discard)
                _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, f.Id, ids, CardLocation.Hand(f.OwnerSeat), before);
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { AnniversaryLeiBo = current.AnniversaryLeiBo!with { Gain = settlement, Moves = settlement.Movements, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoMovementSettledEvent(f.Id, r.Branch!, ids.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }, producerFrameId: f.Id);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        ContinueLeiBo(f.Id);
    }

    internal bool DrainLeiBo(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueLeiBo(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (!ValidLeiBo(f))
            throw new InvalidOperationException("Lei Bo lost its original paid instruction or native child receipt.");
        if (f.AnniversaryLeiBo!.Stage == AnniversaryLeiBoStage.MovementChildren)
        {
            if (DrainLeiBo(f))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            CompleteLeiBo(f);
            return;
        }

        if (f.AnniversaryLeiBo.Stage is AnniversaryLeiBoStage.PenaltyChoice or AnniversaryLeiBoStage.RewardChoice or AnniversaryLeiBoStage.PickReward)
            PublishLeiBo(f);
    }

    internal void CompleteLeiBo(ProgramSkillFrame f)
    {
        if (f.SkillId == AnniversaryLeiBoComposition.Shuaijie)
            MoveLeiBoMark(f, f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryLeiBoCompletedEvent(f.Id, f.AnniversaryLeiBo?.UseId > 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryLeiBo = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 49800)]
    internal bool ResumeLeiBo(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryLeiBo: { Stage: not (AnniversaryLeiBoStage.SlashRequest or AnniversaryLeiBoStage.BeforeMaximum) } } f || f.Id != id)
            return false;
        ContinueLeiBo(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49800)]
    internal bool ReturnLeiBoMovement(ProgramSkillFrame f)
    {
        if (f.AnniversaryLeiBo is not { Stage: AnniversaryLeiBoStage.MovementChildren })
            return false;
        if (!LeiBoMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), f.PendingMovementContinuation!))
            throw new InvalidOperationException("Lei Bo cannot consume a foreign movement return.");
        ContinueLeiBo(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.RequestAnniversaryLeiBoSileiSlash, SkillProgramEffectOp.SettleAnniversaryLeiBoSileiPenalty, SkillProgramEffectOp.FinishAnniversaryLeiBoShuaijie)]
    internal PromptChoice AiLeiBo(PendingDecision p, ProgramSkillFrame f) => f.FixedTargetSlash is not null ? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("request-option") == "use") ?? p.Choices.Last() : p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "deck") ?? p.Choices.First();
}
