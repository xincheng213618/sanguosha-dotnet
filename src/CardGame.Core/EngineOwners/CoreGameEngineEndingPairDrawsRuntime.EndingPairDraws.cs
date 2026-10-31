#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEndingPairDrawsRuntime
{
    internal long EndingPairSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal CardConversionSource EndingPairSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static string EndingPairRoundUsage(string state) => RuntimeSkills.RuntimeRules_Core_EndingPairDraws.EndingPairRoundUsage(state);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 770)]
    internal bool CanRunEndingPair(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.DrawEndingPairThenBlockRoundIfUnequal)is not { } e || context.Window == SkillProgramTriggerWindow.TurnEnding && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(c.OwnerSeat, c.SkillId, EndingPairRoundUsage(e.StateId!), SkillUsageScope.Round) == 0;
    internal bool ExactEndingPairParent(ProgramSkillFrame f, long? expected = null) => f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } context && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().LastOrDefault()is { } ending && ending.Id == context.ParentFrameId && (expected is null || ending.Id == expected) && ending.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && ending.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && context.SourceSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && context.TargetSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    internal IReadOnlyList<int>? PublishedEndingPairActorForAi(CharacterState owner, IReadOnlyList<SkillProgramEffect> effects, ProgramSkillWindowContext? context)
    {
        if (!effects.Any(e => e.Op == SkillProgramEffectOp.DrawEndingPairThenBlockRoundIfUnequal))
            return null;
        return context is { Window: SkillProgramTriggerWindow.TurnEnding, SourceSeat: { } actor } && actor == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive ? Array.AsReadOnly(new[] { actor }) : [];
    }

    internal SkillProgramStepOutcome BeginEndingPairDraw(ProgramSkillFrame supplied, string state)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.EndingPairDraw is not null || f.InstructionIndex != 1 || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || !ExactEndingPairParent(f) || f.TriggerId is null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, EndingPairRoundUsage(state), SkillUsageScope.Round) != 0)
            throw new InvalidOperationException("Ending pair drawing lost its original actual Ending/round/current actor or qualified source.");
        var r = new ProgramEndingPairDrawReceipt(f.InstructionIndex, state, EndingPairSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, f.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, 0, false);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { EndingPairDraw = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndingPairDrawStartedEvent(f.Id, state, r.Source, r.GameplayHash, r.ActualTurnNumber, r.ActualTurnOwnerSeat, r.RoundNumber, r.EndingFrameId, r.CurrentActorSeat));
        ContinueEndingPairDraw(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueEndingPairDraw(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.EndingPairDraw!;
        if (!ValidEndingPairDraw(f))
            throw new InvalidOperationException("The issued Ending pair lost its original source/round/typed parent or actual draw invoice.");
        if (r.AwaitingMovement)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, EndingPairDraw = r with { Cursor = r.Cursor + 1, AwaitingMovement = false } });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.EndingPairDraw!;
        }

        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.CurrentActorSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "原参与者死亡或游戏结束，尚未摸牌及比较取消，已发行账单保留。");
            return;
        }

        // The accepted Ending issuance owns both benefits. A child removing the
        // source cannot silently withdraw the already issued second draw.
        if (r.Cursor < 2)
        {
            var seat = r.Cursor == 0 ? f.OwnerSeat : r.CurrentActorSeat;
            var before = EndingPairSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), EndingPairDraw = r with { AwaitingMovement = true } });
            var reason = new CardMoveReason($"skill-program.{(f.SkillId)}.ending-pair.{(r.Cursor)}");
            var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, true, reason).Count;
            var invoice = new ProgramOneCardDrawInvoice(seat, before, EndingPairSequence, count);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { EndingPairDraw = r.Cursor == 0 ? f.EndingPairDraw!with { FirstDraw = invoice } : f.EndingPairDraw!with { SecondDraw = invoice } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndingPairOneDrawIssuedEvent(id, r.Cursor, seat, invoice.SequenceBefore, invoice.SequenceAfter, count));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return;
        }

        var ownerCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
        var actorCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.CurrentActorSeat]).Count;
        var blocked = ownerCount != actorCount;
        if (blocked && !_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(f.OwnerSeat, f.SkillId, EndingPairRoundUsage(r.StateId), SkillUsageScope.Round, 1))
            throw new InvalidOperationException("One exact Ending pair comparison cannot consume the same Round block twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndingPairDrawComparedEvent(id, r.StateId, r.Source, r.RoundNumber, ownerCount, actorCount, blocked));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { EndingPairDraw = null });
        // No unqualified later instruction exists in this exact one-node plan.
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), completed: true);
    }

    internal bool ValidEndingPairDraw(ProgramSkillFrame f)
    {
        if (f.EndingPairDraw is not { } r || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.Source != EndingPairSource(f) || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || string.IsNullOrWhiteSpace(r.Source.BindingId) || r.GameplayHash != f.GameplayHash || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.RoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || r.CurrentActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || r.Cursor is < 0 or > 2 || !ExactEndingPairParent(f, r.EndingFrameId) || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.DrawEndingPairThenBlockRoundIfUnequal, StateId: var state }] || state != r.StateId || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EndingPairDrawStartedEvent>().Count(e => e.ProgramFrameId == f.Id && e.StateId == state && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ActualTurnNumber == r.ActualTurnNumber && e.ActualTurnOwnerSeat == r.ActualTurnOwnerSeat && e.RoundNumber == r.RoundNumber && e.EndingFrameId == r.EndingFrameId && e.CurrentActorSeat == r.CurrentActorSeat) != 1)
            return false;
        if (r.AwaitingMovement && (f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat))
            return false;
        return ValidEndingPairInvoice(f, r.FirstDraw, 0, f.OwnerSeat, r.Cursor > 0 || r.AwaitingMovement && r.Cursor == 0) && ValidEndingPairInvoice(f, r.SecondDraw, 1, r.CurrentActorSeat, r.Cursor > 1 || r.AwaitingMovement && r.Cursor == 1);
    }

    internal bool ValidEndingPairInvoice(ProgramSkillFrame f, ProgramOneCardDrawInvoice? invoice, int cursor, int seat, bool required)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EndingPairOneDrawIssuedEvent>().Where(e => e.ProgramFrameId == f.Id && e.Cursor == cursor).ToArray();
        if (invoice is null)
            return !required && facts.Length == 0;
        return required && invoice.RecipientSeat == seat && invoice.ActualCount is >= 0 and <= 1 && invoice.SequenceBefore >= 0 && invoice.SequenceAfter >= invoice.SequenceBefore && facts is [var fact] && fact.RecipientSeat == seat && fact.SequenceBefore == invoice.SequenceBefore && fact.SequenceAfter == invoice.SequenceAfter && fact.ActualCount == invoice.ActualCount && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > invoice.SequenceBefore && m.Sequence <= invoice.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat) && m.Reason.Value == $"skill-program.{(f.SkillId)}.ending-pair.{(cursor)}") == invoice.ActualCount;
    }
}
