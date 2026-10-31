#nullable enable

using System.Collections.ObjectModel;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoCaoMobileGodRuntime
{
    internal const string MobileGuixinReason = "skill-program.mobile:guixin.obtain";
    internal static CardConversionSource MobileGuixinSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IMobileGuixinHost.Resolve(ProgramSkillFrame f) => BeginMobileGuixin(f);
    internal SkillProgramStepOutcome BeginMobileGuixin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MobileGuixin is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreOrderedPrintedSkillLossObserversOperations.ExactOrderedPrintedSkillLossParent(f, out var window, out var damage, out _) || damage.TargetSeat != f.OwnerSeat || f.WindowContext!.OccurrenceIndex < 0 || f.WindowContext.OccurrenceIndex >= damage.Amount)
            throw new InvalidOperationException("Mobile Guixin requires its actual positive original damage-point owner candidate.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var r = new MobileGuixinReceipt(MobileGuixinSource(f), f.GameplayHash, window.Id, damage.Id, f.WindowContext.OccurrenceIndex, damage.Amount)
        {
            OtherSeats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.Seat != f.OwnerSeat).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileGuixin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileGuixinStartedEvent(f.Id, r.Source, r.GameplayHash, window.Id, damage.Id, r.OccurrenceIndex, damage.Amount, string.Join(",", r.OtherSeats)));
        ResumeMobileGuixin(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40300)]
    internal bool ResumeMobileGuixin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileGuixin: not null } initial || initial.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            var r = f.MobileGuixin!;
            if (r.Stage is MobileGuixinStage.MovementChildren or MobileGuixinStage.TurnoverChildren)
            {
                if (DrainMobileGuixinChildren(f))
                    return true;
                if (r.Stage == MobileGuixinStage.TurnoverChildren)
                {
                    FinishMobileGuixin(f, false);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, MobileGuixin = r = r with { Stage = MobileGuixinStage.Choosing, Cursor = r.Cursor + 1 } });
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Payments.Count == 0 && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                FinishMobileGuixin(f, true);
                return true;
            }

            if (r.Cursor == r.OtherSeats.Count)
            {
                var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown;
                var after = _coreShenwuNpcProgramsOperations.ShenwuCannotTurnOver(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) ? before : !before;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileGuixin = r with { Stage = MobileGuixinStage.TurnoverChildren, TurnoverIssued = true, FaceDownBefore = before, FaceDownAfter = after } });
                _coreProgramLifecycleOperations.TurnOverProgramTarget(id, f.OwnerSeat, f.OwnerSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileGuixinTurnoverIssuedEvent(id, before, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown));
                continue;
            }

            var target = r.OtherSeats[r.Cursor];
            var choices = _coreMatchParticipantsPlayersAccess._players[target].IsAlive ? MobileGuixinChoices(f) : [];
            if (choices.Count == 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileGuixin = r with { Cursor = r.Cursor + 1 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileGuixinSeatSkippedEvent(id, r.Cursor, target));
                continue;
            }

            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "归心：获得该角色区域里的一张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [target], f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                TargetSeat = target,
                Choices = choices,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        }

        return true;
    }

    internal IReadOnlyList<PromptChoice> MobileGuixinChoices(ProgramSkillFrame f)
    {
        var r = f.MobileGuixin!;
        if (r.Stage != MobileGuixinStage.Choosing || r.Cursor >= r.OtherSeats.Count)
            return[];
        var target = r.OtherSeats[r.Cursor];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, CharacterCaoCaoMobileGodDefinitions.MobileGuixinZones, OwnedCardMoveIntent.Obtain).Where(c => c.Parameters.GetValueOrDefault("source-zone") != "Judgment" || !c.Cards.Any(id => _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().Any(j => j.TargetSeat == target && j.CardId == id))).Select(c => c with { Id = new("mobile-guixin." + f.Id + "." + r.Cursor + "." + c.Id.Value), Parameters = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mobile-guixin", ["cursor"] = r.Cursor.ToString(CultureInfo.InvariantCulture) }) }).ToArray());
    }

    [ProgramAnswer("mobile-guixin")]
    internal void AnswerMobileGuixin(PromptChoice chosen)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Mobile Guixin lost its active original frame.");
        var r = f.MobileGuixin!;
        var exact = MobileGuixinChoices(f).SingleOrDefault(c => c.Id == chosen.Id && c.Cards.SequenceEqual(chosen.Cards) && c.Targets.SequenceEqual(chosen.Targets) && c.Parameters.OrderBy(p => p.Key).SequenceEqual(chosen.Parameters.OrderBy(p => p.Key))) ?? throw new InvalidOperationException("Mobile Guixin requires its exact current public card or opaque hand slot.");
        var target = r.OtherSeats[r.Cursor];
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || r.Payments.Count == 0 && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Mobile Guixin lost an unpaid live participant or its original instance.");
        var zone = Enum.Parse<CardZoneKind>(exact.Parameters["source-zone"]);
        var slot = int.Parse(exact.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var from = new CardLocation(zone, target);
        var cards = zone switch
        {
            CardZoneKind.Hand => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]),
            CardZoneKind.Equipment => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]),
            CardZoneKind.Judgment => _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[target]),
            _ => throw new InvalidOperationException("Unsupported Guixin area.")};
        var card = cards[slot];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, MobileGuixin = r with { Stage = MobileGuixinStage.MovementChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(f.OwnerSeat), new(MobileGuixinReason), (batch, movements) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var payment = new MobileGuixinPayment(r.Cursor, target, card.Id, from, batch, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            {
                Movements = movements
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { MobileGuixin = parent.MobileGuixin!with { Payments = parent.MobileGuixin.Payments.Append(payment).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileGuixinMovementPaidEvent(f.Id, r.Cursor, target, batch, movements.Count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }, disclosure: zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        ResumeMobileGuixin(f.Id);
    }

    internal bool DrainMobileGuixinChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void FinishMobileGuixin(ProgramSkillFrame f, bool cancelled)
    {
        var r = f.MobileGuixin!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileGuixin = r with { Stage = MobileGuixinStage.Finished } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileGuixinCompletedEvent(f.Id, r.Payments.Count, r.TurnoverIssued, cancelled));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40300)]
    internal bool ReturnMobileGuixinMovement(ProgramSkillFrame f) => f.MobileGuixin is not null && ResumeMobileGuixin(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40300)]
    internal bool RetainMobileGuixinPaid(ProgramSkillFrame f) => f.MobileGuixin is { } r && (r.Payments.Count > 0 || r.TurnoverIssued) && ValidMobileGuixin(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShenCaoCaoGuixin)]
    internal PromptChoice AiMobileGuixin(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
