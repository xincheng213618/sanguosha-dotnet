#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXingDaoRongAnniversaryRuntime
{
    internal const string XingDaoRongAction = "anniversary-xing-dao-rong";
    internal static CardMoveReason XingDaoRongDiscardReason => new("skill-program.anniversary:xuhe.discard");
    internal static CardMoveReason XingDaoRongDrawReason => new("skill-program.anniversary:xuhe.nearby-draw");

    internal static CardConversionSource XingDaoRongSource(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IAnniversaryXingDaoRongHost.Begin(ProgramSkillFrame f) => BeginXingDaoRong(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 45700)]
    internal bool QualifyXingDaoRong(ProgramTriggerLegalityContext g) => g.SkillId != XingDaoRongComposition.Skill || g.Trigger.Id != XingDaoRongComposition.Ending || g.WindowContext is { Window: SkillProgramTriggerWindow.PlayEnding, SourceSeat: { } actor } && actor == g.OwnerSeat && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.MaxHp > g.Owner.MaxHp);
    internal SkillProgramStepOutcome BeginXingDaoRong(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.XingDaoRongXuHe is not null || f.InstructionIndex != 2 || !XingDaoRongOriginal(f))
            throw new InvalidOperationException("Xuhe requires its exact own Play-start frame after the real maximum-HP payment.");
        var started = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(started, e => e is ProgramBindingStartedEvent b && b.FrameId == f.Id);
        var paid = started.Skip(start + 1).OfType<MaximumHpChangedEvent>().Where(e => e.PlayerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.Delta == -1).ToArray();
        if (start < 0 || paid is not [var cost])
            throw new InvalidOperationException("Xuhe has no unique native maximum-HP payment in its original binding.");
        var r = new XingDaoRongXuHeReceipt(XingDaoRongSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, cost.MaximumHp);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XingDaoRongXuHe = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingDaoRongXuHeStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.ActualTurn, r.PaidMaximum));
        PublishXingDaoRong(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int[] XingDaoRongNearby(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (p.Seat == owner || _coreNativeRuleQueriesOperations.GetCombatDistance(owner, p.Seat) <= 1)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, p.Seat)).Select(p => p.Seat).ToArray();
    internal IReadOnlyList<PromptChoice> XingDaoRongCards(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.XingDaoRongXuHe!.Targets[f.XingDaoRongXuHe.Index], [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard);
    internal IReadOnlyList<PromptChoice> XingDaoRongChoices(ProgramSkillFrame f)
    {
        var r = f.XingDaoRongXuHe!;
        if (r.Stage == XingDaoRongXuHeStage.Mode)
            return Array.AsReadOnly(new[] { new PromptChoice(new($"xu-he.{(f.Id)}.discard"), "弃置距离1以内每名角色各一张牌（包括自己）", [], [], new Dictionary<string, string> { ["program-action"] = XingDaoRongAction, ["mode"] = "discard" }), new PromptChoice(new($"xu-he.{(f.Id)}.draw"), "距离1以内每名角色各摸一张牌（包括自己）", [], [], new Dictionary<string, string> { ["program-action"] = XingDaoRongAction, ["mode"] = "draw" }) });
        return r.Stage != XingDaoRongXuHeStage.Card ? [] : Array.AsReadOnly(XingDaoRongCards(f).Select(c => c with { Id = new($"xu-he.{(f.Id)}.{(r.Index)}.{(c.Id.Value)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = XingDaoRongAction, ["mode"] = "card" } }).ToArray());
    }

    internal void PublishXingDaoRong(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, XingDaoRongChoices(f), "虚猲：统一选择弃牌或摸牌，按实际距离依次结算。", f.XingDaoRongXuHe!.Stage == XingDaoRongXuHeStage.Card ? f.XingDaoRongXuHe.Targets[f.XingDaoRongXuHe.Index] : f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(XingDaoRongAction)]
    internal void AnswerXingDaoRong(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XingDaoRongXuHe: { } r } f || !ValidXingDaoRong(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], XingDaoRongChoices(f).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Xuhe lost its current original group choice or opaque discard slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == XingDaoRongXuHeStage.Mode)
        {
            var mode = answer.Parameters["mode"];
            var targets = XingDaoRongNearby(f.OwnerSeat);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XingDaoRongXuHe = r with { Mode = mode, Targets = targets, Stage = XingDaoRongXuHeStage.Next } });
            foreach (var(seat, ordinal)in targets.Select((seat, i) => (seat, i)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingDaoRongXuHeTargetEvent(f.Id, ordinal, seat));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingDaoRongXuHeModeEvent(f.Id, mode, targets.Length));
            ContinueXingDaoRong(f.Id);
            return;
        }

        if (r.Stage != XingDaoRongXuHeStage.Card || answer.Parameters["mode"] != "card")
            throw new InvalidOperationException("Xuhe cannot repeat an already paid discard.");
        var target = r.Targets[r.Index];
        var from = new CardLocation(Enum.Parse<CardZoneKind>(answer.Parameters["source-zone"]), target);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(answer.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XingDaoRongXuHe = r with { Stage = XingDaoRongXuHeStage.DiscardChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Discard = null, Draw = null }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], XingDaoRongDiscardReason, (batch, records) =>
        {
            var m = records.Single();
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { XingDaoRongXuHe = active.XingDaoRongXuHe!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Discard = m } });
            _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(m, f.OwnerSeat, f.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingDaoRongXuHeDiscardedEvent(f.Id, r.Index, target, m.CardId, m.From, m.To, m.Sequence, batch));
        });
        ContinueXingDaoRong(f.Id);
    }

    internal bool DrainXingDaoRong(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 45700)]
    internal bool ContinueXingDaoRong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XingDaoRongXuHe.Stage: not XingDaoRongXuHeStage.Completed } top || top.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { XingDaoRongXuHe: { } r } f && f.Id == id)
        {
            if (!ValidXingDaoRong(f))
                throw new InvalidOperationException("Xuhe lost its original paid nearby group cursor.");
            if (r.Stage is XingDaoRongXuHeStage.DiscardChildren or XingDaoRongXuHeStage.DrawChildren)
            {
                if (DrainXingDaoRong(f))
                    return true;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.XingDaoRongXuHe!;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, XingDaoRongXuHe = r = r with { Index = r.Index + 1, Stage = XingDaoRongXuHeStage.Next, Draw = null, Discard = null } });
            }

            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || r.Index >= r.Targets.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, XingDaoRongXuHe = r with { Stage = XingDaoRongXuHeStage.Completed } });
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
                return true;
            }

            if (r.Stage == XingDaoRongXuHeStage.Next)
            {
                var target = r.Targets[r.Index];
                if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive || r.Mode == "discard" && XingDaoRongCards(f).Count == 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { XingDaoRongXuHe = r with { Index = r.Index + 1 } });
                    continue;
                }

                if (r.Mode == "draw")
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XingDaoRongXuHe = r with { Stage = XingDaoRongXuHeStage.Drawing } });
                    _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, 1, XingDaoRongDrawReason);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XingDaoRongXuHe = r with { Stage = XingDaoRongXuHeStage.Card } });
            }

            PublishXingDaoRong(f);
            return true;
        }

        return true;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryXingDaoRongXuHe)]
    internal void ReturnXingDaoRongDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.XingDaoRongXuHe!;
        if (!ValidXingDaoRong(f) || r.Stage != XingDaoRongXuHeStage.Drawing || f.NativeDraw is not null || outcome.RecipientSeat != r.Targets[r.Index] || outcome.RequestedCount != 1 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Xuhe lost its exact current recipient's native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { XingDaoRongXuHe = r with { Stage = XingDaoRongXuHeStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Draw = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XingDaoRongXuHeDrawnEvent(f.Id, r.Index, outcome.RecipientSeat, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueXingDaoRong(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 45700)]
    internal bool ReturnXingDaoRongMovement(ProgramSkillFrame f) => f.XingDaoRongXuHe is not null && ContinueXingDaoRong(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 45700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 45700)]
    internal bool KeepPaidXingDaoRong(ProgramSkillFrame f) => f.XingDaoRongXuHe is not null && ValidXingDaoRong(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryXingDaoRongXuHe)]
    internal PromptChoice XingDaoRongAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("mode") == "draw") ?? p.Choices[0];
}
