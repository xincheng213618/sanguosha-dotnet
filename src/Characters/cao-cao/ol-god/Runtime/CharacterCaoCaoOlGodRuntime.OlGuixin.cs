#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoOlGodRuntime
{
    internal const string OlGuixinReason = "skill-program.ol:guixin.obtain";
    internal static CardConversionSource OlGuixinSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    internal static IReadOnlyList<CardZoneKind> OlGuixinPriority(CardZoneKind preferred) => preferred switch
    {
        CardZoneKind.Hand => [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment],
        CardZoneKind.Equipment => [CardZoneKind.Equipment, CardZoneKind.Hand, CardZoneKind.Judgment],
        CardZoneKind.Judgment => [CardZoneKind.Judgment, CardZoneKind.Hand, CardZoneKind.Equipment],
        _ => throw new InvalidOperationException("OL Guixin requires one HEJ priority region.")};
    SkillProgramStepOutcome IOlGuixinHost.Resolve(ProgramSkillFrame f) => BeginOlGuixin(f);
    internal SkillProgramStepOutcome BeginOlGuixin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.OlGuixin is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out _) || damage.TargetSeat != f.OwnerSeat || f.WindowContext!.OccurrenceIndex < 0 || f.WindowContext.OccurrenceIndex >= damage.Amount)
            throw new InvalidOperationException("OL Guixin requires its exact original positive damage-point candidate.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var r = new OlGuixinReceipt(OlGuixinSource(f), f.GameplayHash, window.Id, damage.Id, f.WindowContext.OccurrenceIndex, damage.Amount)
        {
            OtherSeats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != f.OwnerSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlGuixin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, window.Id, damage.Id, r.OccurrenceIndex, damage.Amount, string.Join(",", r.OtherSeats)));
        ResumeOlGuixin(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal static IReadOnlyList<PromptChoice> OlGuixinPriorityChoices(long id) => Array.AsReadOnly(CharacterCaoCaoMobileGodDefinitions.MobileGuixinZones.Select(zone => new PromptChoice(new("ol-guixin." + id + "." + zone), zone switch
    {
        CardZoneKind.Hand => "优先手牌区",
        CardZoneKind.Equipment => "优先装备区",
        _ => "优先判定区"
    }, [], [], new Dictionary<string, string> { ["program-action"] = "ol-guixin-priority", ["frame-id"] = id.ToString(CultureInfo.InvariantCulture), ["priority-zone"] = zone.ToString() })).ToArray());
    [ProgramContinuation(ProgramContinuationKind.Resume, 40700)]
    internal bool ResumeOlGuixin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { OlGuixin: not null } initial || initial.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            var r = f.OlGuixin!;
            if (r.Stage is OlGuixinStage.MovementChildren or OlGuixinStage.TurnoverChildren)
            {
                if (_characterCaoCaoMobileGodOperations.DrainMobileGuixinChildren(f))
                    return true;
                if (r.Stage == OlGuixinStage.TurnoverChildren)
                {
                    FinishOlGuixin(f, false);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, OlGuixin = r = r with { Stage = OlGuixinStage.Visiting, Cursor = r.Cursor + 1 } });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Payments.Count == 0 && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                FinishOlGuixin(f, true);
                return true;
            }

            if (r.Stage == OlGuixinStage.PriorityChoice)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
                var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
                _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "归心：统一选择本次获得牌的优先区域。", [], [], f.OwnerSeat)
                {
                    PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                    Choices = OlGuixinPriorityChoices(id),
                    SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
                };
                _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return true;
            }

            if (r.Cursor == r.OtherSeats.Count)
            {
                var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlGuixin = r with { Stage = OlGuixinStage.TurnoverChildren, TurnoverIssued = true, FaceDownBefore = before, FaceDownAfter = _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) ? before : !before } });
                _coreProgramLifecycleOperations.TurnOverProgramTarget(id, f.OwnerSeat, f.OwnerSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlGuixinTurnoverIssuedEvent(id, before, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown));
                continue;
            }

            var target = r.OtherSeats[r.Cursor];
            var candidates = _coreMatchParticipantsPlayersAccess._players[target].IsAlive ? OlGuixinCandidates(f, target) : [];
            if (candidates.Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlGuixin = r with { Cursor = r.Cursor + 1 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlGuixinSeatSkippedEvent(id, r.Cursor, target));
                continue;
            }

            var zone = OlGuixinPriority(r.PreferredZone!.Value).First(z => candidates.Any(c => c.From.Zone == z));
            var selectedZone = candidates.Where(c => c.From.Zone == zone).ToArray();
            var randomBefore = _randomOperations._random.State;
            var selected = selectedZone[_randomOperations._random.Next(selectedZone.Length)];
            var randomAfter = _randomOperations._random.State;
            var beforeMove = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { OlGuixin = r with { Stage = OlGuixinStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([selected.CardId], CardLocation.Hand(f.OwnerSeat), new(OlGuixinReason), (batch, records) =>
            {
                var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                var payment = new OlGuixinPayment(new(r.Cursor, target, selected.CardId, selected.From, batch, beforeMove, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) { Movements = records }, randomBefore, randomAfter)
                {
                    Candidates = candidates
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { OlGuixin = parent.OlGuixin!with { Payments = parent.OlGuixin.Payments.Append(payment).ToArray() } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlGuixinMovementPaidEvent(id, r.Cursor, target, zone, batch, beforeMove, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            }, disclosure: zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        }

        return true;
    }

    internal IReadOnlyList<OlGuixinCandidate> OlGuixinCandidates(ProgramSkillFrame f, int target) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, CharacterCaoCaoMobileGodDefinitions.MobileGuixinZones, OwnedCardMoveIntent.Obtain).Select(c =>
    {
        var zone = Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]);
        var slot = int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var cards = zone switch
        {
            CardZoneKind.Hand => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]),
            CardZoneKind.Equipment => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]),
            CardZoneKind.Judgment => _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[target]),
            _ => throw new InvalidOperationException("Unsupported OL Guixin source.")};
        return new OlGuixinCandidate(cards[slot].Id, new(zone, target));
    }).Where(c => c.From.Zone != CardZoneKind.Judgment || !_resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.TargetSeat == target && j.CardId == c.CardId)).OrderBy(c => c.CardId).ToArray());
    [ProgramAnswer("ol-guixin-priority")]
    internal void AnswerOlGuixinPriority(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("OL Guixin lost its original frame.");
        var r = f.OlGuixin!;
        if (r.Stage != OlGuixinStage.PriorityChoice || !OlGuixinPriorityChoices(f.Id).Any(c => c.Id == chosen.Id && c.Cards.SequenceEqual(chosen.Cards) && c.Targets.SequenceEqual(chosen.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(chosen.Parameters.OrderBy(p => p.Key))))
            throw new InvalidOperationException("OL Guixin needs one published priority-area choice for this actual damage point.");
        var zone = Enum.Parse<CardZoneKind>(chosen.Parameters["priority-zone"]);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.ResolvingEffect, OlGuixin = r with { PreferredZone = zone, Stage = OlGuixinStage.Visiting } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlGuixinPriorityChosenEvent(f.Id, zone));
        ResumeOlGuixin(f.Id);
    }

    internal void FinishOlGuixin(ProgramSkillFrame f, bool cancelled)
    {
        var r = f.OlGuixin!;
        if (f.PendingMovementContinuation is not null || r.Stage == OlGuixinStage.MovementChildren)
            throw new InvalidOperationException("OL Guixin cannot retire a paid gain before its children return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OlGuixin = r with { Stage = OlGuixinStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OlGuixinCompletedEvent(f.Id, r.Payments.Count, r.TurnoverIssued, cancelled));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40700)]
    internal bool ReturnOlGuixinMovement(ProgramSkillFrame f) => f.OlGuixin is not null && ResumeOlGuixin(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40700)]
    internal bool RetainOlGuixinPaid(ProgramSkillFrame f) => f.OlGuixin is { } r && (r.Payments.Count > 0 || r.TurnoverIssued) && ValidOlGuixin(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveOlShenCaoCaoGuixin)]
    internal PromptChoice AiOlGuixin(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
