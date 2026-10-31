#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHongOlRuntime
{
    internal const string ZenrunTakeReason = "skill-program.sun-hong.zenrun.obtain";
    internal const string ZenrunCompensationReason = "skill-program.sun-hong.zenrun.compensation";
    internal CardConversionSource ZenrunSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasPrintedGeneralSkillTargetBan(CardConversionSource source, int target) => HasPrintedGeneralSkillTargetBan(source.OwnerSeat, source.SkillId, target, source.SkillInstanceId);
    internal bool HasPrintedGeneralSkillTargetBan(int owner, string skillId, int target, string? skillInstanceId = null)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !SunHongTargetBlocked(owner, target))
            return false;
        // Inspect raw source identities, never the HP-conditional binding shard. All
        // native execution/disable eligibility remains with the admitting caller.
        return _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.Grants.Any(g => g.SkillId == skillId && (skillInstanceId is null || g.SkillInstanceId == skillInstanceId) && g.EquipmentSuitSource is null && !g.SourceId.StartsWith("equipment:", StringComparison.Ordinal));
    }

    internal bool ZenrunCanTake(int owner, int source, CardZoneKind zone, Card card) => !(zone == CardZoneKind.Equipment && card.IsGeneralWeapon) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, new(zone, source), OwnedCardMoveIntent.Obtain);
    internal bool ZenrunSourceLegal(int owner, int source, int count) => count > 0 && source != owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && !HasPrintedGeneralSkillTargetBan(owner, "ol:zenrun", source) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[source]).Count(c => ZenrunCanTake(owner, source, CardZoneKind.Hand, c)) + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[source]).Count(c => ZenrunCanTake(owner, source, CardZoneKind.Equipment, c)) >= count;
    internal bool ZenrunPhaseAvailable(int owner, ActualDiscardRecoveryPhaseKey? phase) => phase is not null && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZenrunPaidEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == "ol:zenrun" && e.Phase == phase);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27571)]
    internal bool QualifyZenrun(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveZenrunNativeDrawReplacement))
            return true;
        return candidate.SkillId == "ol:zenrun" && candidate.BindingId == "replace-native-draw" && context.NativeDraw is { } draw && draw.RecipientSeat == candidate.OwnerSeat && _coreNativeDrawAttemptsOperations.ExactNativeDrawAttempt(context)is { Replacement: null } && ZenrunPhaseAvailable(candidate.OwnerSeat, draw.ActualPhase) && _coreMatchParticipantsPlayersAccess._players.Any(p => ZenrunSourceLegal(candidate.OwnerSeat, p.Seat, draw.RequestedCount));
    }

    SkillProgramStepOutcome ISunHongZenrunHost.Resolve(ProgramSkillFrame f) => BeginZenrun(f);
    internal SkillProgramStepOutcome BeginZenrun(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Zenrun is not null || f.InstructionIndex != 1 || f.SkillId != "ol:zenrun" || CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) != "replace-native-draw" || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { NativeDraw: { } draw } context || !_coreNativeDrawAttemptsOperations.IsExactNativeDrawCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context) || !ZenrunPhaseAvailable(f.OwnerSeat, draw.ActualPhase))
            throw new InvalidOperationException("Zenrun needs the genuine unconsumed native draw attempt in its actual phase.");
        var r = new ZenrunReceipt(ZenrunSource(f), f.GameplayHash, context.ParentFrameId, draw, ZenrunStage.Target)
        {
            SourceSeat = f.OwnerSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, draw));
        PublishZenrun(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> ZenrunChoices(ProgramSkillFrame f)
    {
        var r = f.Zenrun!;
        var result = new List<PromptChoice>();
        PromptChoice C(string branch, string label, IReadOnlyList<int>? cards = null, int? target = null, CardZoneKind? zone = null, int? slot = null)
        {
            var parameters = new Dictionary<string, string>
            {
                ["program-action"] = "sun-hong-zenrun",
                ["branch"] = branch
            };
            if (zone is { } z)
                parameters["source-zone"] = z.ToString();
            if (slot is { } index)
                parameters["slot-index"] = index.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return new(new($"zenrun.{(f.Id)}.{(r.Stage)}.{(r.Revision)}.{(branch)}.{(target)}.{(zone)}.{(slot)}"), label, cards ?? [], target is { } seat ? [seat] : [], parameters);
        }

        if (r.Stage == ZenrunStage.Target && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => ZenrunSourceLegal(f.OwnerSeat, p.Seat, r.Draw.RequestedCount)))
                result.Add(C("target", player.Name, target: player.Seat));
        if (r.Stage == ZenrunStage.Cards && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.SourceSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            foreach (var choice in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.SourceSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Obtain, canSelect: (zone, card) => ZenrunCanTake(f.OwnerSeat, r.SourceSeat, zone, card)))
            {
                var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
                var slot = int.Parse(choice.Parameters["slot-index"]);
                var selected = r.Selected.Any(c => c.Zone == zone && c.Index == slot);
                if (selected || r.Selected.Count < r.Draw.RequestedCount)
                    result.Add(C(selected ? "unpick" : "pick", (selected ? "撤回：" : "选择获得：") + choice.Description, choice.Cards, zone: zone, slot: slot));
            }

            if (r.Selected.Count == r.Draw.RequestedCount)
                result.Add(C("take", $"以这{(r.Selected.Count)}张原牌替代摸牌"));
        }

        if (r.Stage == ZenrunStage.VictimChoice && _coreMatchParticipantsPlayersAccess._players[r.SourceSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            result.Add(C("draw", $"摸{(r.Draw.RequestedCount)}张牌"));
            result.Add(C("block", "令其不能再对你发动武将牌上的技能"));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishZenrun(ProgramSkillFrame f)
    {
        var r = f.Zenrun!;
        var choices = ZenrunChoices(f);
        if (choices.Count == 0)
        {
            FinishZenrun(f);
            return;
        }

        var chooser = r.Stage == ZenrunStage.VictimChoice ? r.SourceSeat : f.OwnerSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == ZenrunStage.VictimChoice ? "谮润：被获得牌者自行选择。" : "谮润：选择其他角色及其原区域牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), r.SourceSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("sun-hong-zenrun")]
    internal void AnswerZenrun(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zenrun: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == ZenrunStage.VictimChoice ? r.SourceSeat : f.OwnerSeat) || !ZenrunChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Zenrun lost the exact opaque chooser or original native choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == ZenrunStage.Target)
        {
            var seat = choice.Targets.Single();
            if (branch != "target" || !ZenrunSourceLegal(f.OwnerSeat, seat, r.Draw.RequestedCount))
                throw new InvalidOperationException("Zenrun's source cannot pay the requested quantity.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r with { SourceSeat = seat, Stage = ZenrunStage.Cards } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunSourceSelectedEvent(f.Id, seat));
            PublishZenrun(f);
            return;
        }

        if (r.Stage == ZenrunStage.Cards)
        {
            if (branch is "pick" or "unpick")
            {
                var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
                var index = int.Parse(choice.Parameters["slot-index"]);
                var id = _cardZonesOperations._cardZones.CardsAt(new(zone, r.SourceSeat))[index].Id;
                var selected = branch == "pick" ? r.Selected.Append(new ZenrunSlot(zone, index, id)).ToArray() : r.Selected.Where(s => s.Zone != zone || s.Index != index).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r with { Selected = selected, Revision = r.Revision + 1 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunSlotSelectedEvent(f.Id, r.Revision + 1, zone, index, branch == "pick"));
                PublishZenrun(f);
                return;
            }

            if (branch != "take" || !ZenrunPhaseAvailable(f.OwnerSeat, r.Draw.ActualPhase) || r.Selected.Count != r.Draw.RequestedCount || !ZenrunSourceLegal(f.OwnerSeat, r.SourceSeat, r.Draw.RequestedCount) || r.Selected.Any(s => s.Index < 0 || s.Index >= _cardZonesOperations._cardZones.CardsAt(new(s.Zone, r.SourceSeat)).Count || _cardZonesOperations._cardZones.CardsAt(new(s.Zone, r.SourceSeat))[s.Index].Id != s.CardId || !ZenrunCanTake(f.OwnerSeat, r.SourceSeat, s.Zone, _coreAdvancedLifecycleOperations.GetAdvancedCard(s.CardId))))
                throw new InvalidOperationException("Zenrun cannot change or repay its original equal-count HE slots.");
            var materials = r.Selected.Select(s => new ZenrunMaterial(s.CardId, _coreAdvancedLifecycleOperations.GetAdvancedCard(s.CardId).Kind, new(s.Zone, r.SourceSeat))).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r = r with { Paid = true, Materials = materials, Stage = ZenrunStage.TakeChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Step = r.Step + 1 }, PendingMovementContinuation = new(r.SourceSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunPaidEvent(f.Id, r.Source, r.GameplayHash, r.Draw.AttemptFrameId, r.Draw.ActualPhase!, r.SourceSeat, r.Draw.RequestedCount));
            for (var i = 0; i < materials.Length; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunMaterialCommittedEvent(f.Id, i, materials[i].CardId, materials[i].Kind, materials[i].From));
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
            var frameId = f.Id;
            var step = r.Step;
            var before = r.Before;
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials.Select(m => m.CardId).ToArray(), CardLocation.Hand(f.OwnerSeat), new(ZenrunTakeReason), (batch, moves) =>
            {
                foreach (var m in moves)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunEntityMovedEvent(frameId, step, batch, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
            });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = f.Zenrun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _coreNativeDrawAttemptsOperations.CompleteNativeDrawReplacement(f, r.SourceSeat, materials.Select(m => m.CardId).ToArray(), new(ZenrunTakeReason), before);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunMovementIssuedEvent(frameId, step, ZenrunStage.TakeChildren, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, materials.Length));
            ContinueZenrun(frameId);
            return;
        }

        if (r.Stage != ZenrunStage.VictimChoice || branch is not ("draw" or "block"))
            throw new InvalidOperationException("Zenrun requires the real victim's compensation choice.");
        var option = branch == "draw" ? 1 : 2;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r = r with { Choice = option } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunVictimChoiceEvent(f.Id, r.SourceSeat, option));
        if (option == 2)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunTargetBlockedEvent(f.Id, r.Source, r.GameplayHash, f.OwnerSeat, r.SourceSeat));
            FinishZenrun(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r with { Stage = ZenrunStage.CompensationRequest, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunCompensationRequestedEvent(f.Id, r.SourceSeat, r.Draw.RequestedCount, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.SourceSeat, r.Draw.RequestedCount, new(ZenrunCompensationReason));
    }

    internal void ContinueZenrun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zenrun: { } r } f || f.Id != id)
            return;
        if (f.NativeDraw is not null || r.Stage == ZenrunStage.CompensationRequest)
            return;
        if (r.Stage is ZenrunStage.Target or ZenrunStage.Cards or ZenrunStage.VictimChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishZenrun(f);
            return;
        }

        if (DrainXianbi(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.Zenrun!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == ZenrunStage.TakeChildren && _coreMatchParticipantsPlayersAccess._players[r.SourceSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r with { Stage = ZenrunStage.VictimChoice } });
            PublishZenrun(f);
            return;
        }

        FinishZenrun(f);
    }

    internal void ReturnZenrunCompensation(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.Zenrun is not { Paid: true, Stage: ZenrunStage.CompensationRequest, Choice: 1 } r || f.NativeDraw is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || outcome.RecipientSeat != r.SourceSeat || outcome.RequestedCount != r.Draw.RequestedCount || outcome.SequenceBefore < r.Before || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.ActualDrawIds.Count > r.Draw.RequestedCount || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZenrunCompensationRequestedEvent>().Count(e => e.FrameId == f.Id && e.RecipientSeat == r.SourceSeat && e.RequestedCount == r.Draw.RequestedCount && e.SequenceBefore == r.Before) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZenrunCompensationReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Zenrun compensation lost its exact native draw request return.");
        if (outcome.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == f.Id && e.RecipientSeat == r.SourceSeat && e.RequestedCount == r.Draw.RequestedCount && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("Zenrun compensation cannot borrow another native draw invoice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = r with { Stage = ZenrunStage.CompensationChildren, Step = r.Step + 1, Before = checked((int)outcome.SequenceBefore), After = checked((int)outcome.SequenceAfter) }, PendingMovementContinuation = new(r.SourceSeat, 0, null) });
        for (var i = 0; i < outcome.ActualDrawIds.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunCompensationEntityEvent(f.Id, i, outcome.ActualDrawIds[i]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunCompensationReturnedEvent(f.Id, outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunMovementIssuedEvent(f.Id, f.Zenrun!.Step, ZenrunStage.CompensationChildren, f.Zenrun.Before, f.Zenrun.After, outcome.ActualDrawIds.Count));
        ContinueZenrun(f.Id);
    }

    internal void FinishZenrun(ProgramSkillFrame f)
    {
        var r = f.Zenrun!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZenrunCompletedEvent(f.Id, r.Paid, r.Materials.Count, r.Choice));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Zenrun = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Paid);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27571)]
    internal bool ResumeZenrun(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Zenrun: not null, NativeDraw: null } f || f.Id != id)
            return false;
        ContinueZenrun(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27571)]
    internal bool ReturnZenrunMovement(ProgramSkillFrame f)
    {
        if (f.Zenrun is null || f.NativeDraw is not null)
            return false;
        ContinueZenrun(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27571)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27571)]
    internal bool ContinuePaidZenrun(ProgramSkillFrame f) => f.Zenrun is { Paid: true } && ValidZenrun(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveZenrunNativeDrawReplacement)]
    internal PromptChoice ChooseZenrunAi(PendingDecision prompt, ProgramSkillFrame f) => prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch")is "draw" or "take") ?? prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pick") ?? prompt.Choices[0];
}
