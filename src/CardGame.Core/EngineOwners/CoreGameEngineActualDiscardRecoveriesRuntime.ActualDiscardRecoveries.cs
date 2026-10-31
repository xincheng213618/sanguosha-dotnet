#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualDiscardRecoveriesRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 890)]
    internal bool CanOfferActualDiscardRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        var op = _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.SingleOrDefault(e => e.Op == SkillProgramEffectOp.RestoreActualDiscardBatch);
        if (op is null)
            return true;
        return context is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index, SourceSeat: { } source } && source != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreActualDiscardRecoveryPhasesOperations.MatchingActualDiscardRecoveryIndexes(batch, c).Contains(index);
    }

    internal SkillProgramStepOutcome BeginActualDiscardRecovery(ProgramSkillFrame f, string stateId)
    {
        if (f.ActualDiscardRecovery is not null || f.WindowContext is not { MovementBatch: { } batch, SourceSeat: { } source } context || !CanOfferActualDiscardRecovery(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), context))
            throw new InvalidOperationException("Discard recovery requires its unpaid exact original batch/source opportunity.");
        var phase = batch.DiscardRecoveryPhase!;
        var receipt = new ProgramActualDiscardRecoveryReceipt(f.InstructionIndex, context.ParentFrameId, batch.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, stateId, phase, source, _coreActualDiscardRecoveryPhasesOperations.ActualDiscardRecoveryEntities(batch, source), ActualDiscardRecoveryStage.SelectingReturn);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualDiscardRecovery = receipt });
        PublishActualDiscardRecoveryPrompt(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ActualDiscardRecoveryChoices(ProgramSkillFrame f)
    {
        var r = f.ActualDiscardRecovery!;
        var parameters = new Dictionary<string, string>
        {
            ["program-action"] = "actual-discard-recovery",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        if (r.Stage == ActualDiscardRecoveryStage.SelectingClaim)
            return Array.AsReadOnly(new[] { new PromptChoice(new($"actual-discard-recovery.frame-{(f.Id)}.claim"), "获得该次弃置后仍在弃牌堆的其余牌。", [], [], new Dictionary<string, string>(parameters) { ["option"] = "claim" }), new PromptChoice(new($"actual-discard-recovery.frame-{(f.Id)}.skip"), "不获得其余牌。", [], [], new Dictionary<string, string>(parameters) { ["option"] = "skip" }) });
        return Array.AsReadOnly(r.OriginalEntities.Where(_coreActualDiscardRecoveryPhasesOperations.ActualDiscardEntityStillAvailable).Select(e => new PromptChoice(new($"actual-discard-recovery.frame-{(f.Id)}.return-{(e.CardId)}"), $"将【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).DisplayName)}】交还给 {(_coreMatchParticipantsPlayersAccess._players[r.DiscardOwnerSeat].Name)}。", [e.CardId], [r.DiscardOwnerSeat], new Dictionary<string, string>(parameters) { ["option"] = "return" })).ToArray());
    }

    internal void PublishActualDiscardRecoveryPrompt(ProgramSkillFrame f)
    {
        var choices = ActualDiscardRecoveryChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.ActualDiscardRecovery!.Stage == ActualDiscardRecoveryStage.SelectingReturn ? "选择一张本次真实弃置的牌交还给原角色。" : "是否获得该次弃置后仍在弃牌堆的其余牌？", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("actual-discard-recovery")]
    internal void ResolveActualDiscardRecovery(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Discard recovery lost its owning frame.");
        var r = f.ActualDiscardRecovery!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } p || p.PlayerSeat != f.OwnerSeat || !ActualDiscardRecoveryChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(selected.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Discard recovery must name its exact published original entity or claim choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == ActualDiscardRecoveryStage.SelectingReturn)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.DiscardOwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreActualDiscardRecoveryPhasesOperations.ActualDiscardRecoveryPhaseUsed(f.OwnerSeat, f.SkillId, r.StateId, r.Phase))
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "交还前参与者、来源或阶段机会已失效。");
                return;
            }

            var id = selected.Cards.Single();
            var entity = r.OriginalEntities.Single(e => e.CardId == id);
            if (!_coreActualDiscardRecoveryPhasesOperations.ActualDiscardEntityStillAvailable(entity))
                throw new InvalidOperationException("The original discarded entity left its exact discard provenance.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), ActualDiscardRecovery = r with { Stage = ActualDiscardRecoveryStage.ReturnPaid, ReturnedCardId = id } });
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.DiscardPile, CardLocation.Hand(r.DiscardOwnerSeat), new($"skill-program.{(f.SkillId)}.actual-discard-return"), move =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { ActualDiscardRecovery = active.ActualDiscardRecovery!with { ReturnMovementSequence = move.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualDiscardRecoveryReturnedEvent(f.Id, r.Source, r.GameplayHash, r.StateId, r.Phase, r.BatchId, r.DiscardOwnerSeat, id, move.Sequence));
            });
        }
        else if (r.Stage == ActualDiscardRecoveryStage.SelectingClaim)
        {
            var claim = selected.Parameters["option"] == "claim" && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None;
            var ids = claim ? r.OriginalEntities.Where(e => e.CardId != r.ReturnedCardId && _coreActualDiscardRecoveryPhasesOperations.ActualDiscardEntityStillAvailable(e)).Select(e => e.CardId).ToArray() : [];
            var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), ActualDiscardRecovery = r with { Stage = ActualDiscardRecoveryStage.ClaimPaid, ClaimRemaining = claim, ClaimedCardIds = ids, ClaimSequenceBefore = before } });
            if (ids.Length > 0)
                _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new($"skill-program.{(f.SkillId)}.actual-discard-claim"));
            var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? before;
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { ActualDiscardRecovery = active.ActualDiscardRecovery!with { ClaimSequenceAfter = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualDiscardRecoveryClaimedEvent(f.Id, r.BatchId, ids, before, after));
        }
        else
            throw new InvalidOperationException("Discard recovery cannot repay a completed choice.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainCapturedEquipmentOrDiscardChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 460)]
    internal bool ResumeActualDiscardRecovery(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.ActualDiscardRecovery is not { } r)
            return false;
        if (r.Stage is ActualDiscardRecoveryStage.SelectingReturn or ActualDiscardRecoveryStage.SelectingClaim)
            return true;
        if (DrainCapturedEquipmentOrDiscardChildren(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.ActualDiscardRecovery!;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == ActualDiscardRecoveryStage.ReturnPaid && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.OriginalEntities.Any(e => e.CardId != r.ReturnedCardId && _coreActualDiscardRecoveryPhasesOperations.ActualDiscardEntityStillAvailable(e)))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ActualDiscardRecovery = r with { Stage = ActualDiscardRecoveryStage.SelectingClaim } });
            PublishActualDiscardRecoveryPrompt(f);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ActualDiscardRecovery = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 440)]
    internal bool ReturnCapturedEquipmentOrDiscardMovement(ProgramSkillFrame f)
    {
        if (f.ActualDiscardRecovery is null && f.CapturedEquipmentDraw is null)
            return false;
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("A paid actual equipment/discard return lost its movement continuation.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.RestoreActualDiscardBatch)]
    internal PromptChoice SelectAiActualDiscardRecovery(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.ActualDiscardRecovery!.Stage == ActualDiscardRecoveryStage.SelectingClaim)
            return p.Choices.Single(c => c.Parameters["option"] == "claim");
        return p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }

    SkillProgramStepOutcome IActualDiscardRecoveryProgramHost.PlaceCapturedEquipmentAndDraw(ProgramSkillFrame f, int target, string bind, int draw) => _coreCapturedEquipmentDrawsOperations.BeginCapturedEquipmentAndDraw(f, target, bind, draw);
    SkillProgramStepOutcome IActualDiscardRecoveryProgramHost.RestoreActualDiscardBatch(ProgramSkillFrame f, string stateId) => BeginActualDiscardRecovery(f, stateId);
}
