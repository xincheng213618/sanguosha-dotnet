#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangAuditCombatRuntime
{
    internal const string WushuangJieying = "wushuang:skill-3230", WushuangWangong = "wushuang:skill-3263";
    internal const string ShanheWangong = "shanhe:wangong-3263";
    internal const string WushuangCampClaimReason = "skill-program.wushuang.jieying.ended-hand";
    SkillProgramStepOutcome IWushuangAuditCombatHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ClaimWushuangEndedCampHand ? ClaimWushuangCampHand(frame) : CaptureWushuangWangong(frame);
    internal bool IsWushuangCampClaimContext(ProgramSkillFrame frame) => _coreWushuangModeProjectionOperations.IsWushuangMode && frame.SkillId == WushuangJieying && frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } context && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is { } parent && parent.OwnerSeat != frame.OwnerSeat && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, frame, context);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 63200)]
    internal bool AllowsWushuangCampClaim(ProgramTriggerLegalityContext gate) => gate.SkillId != WushuangJieying || gate.WindowContext?.Window != SkillProgramTriggerWindow.AfterTurnEnded || _coreWushuangModeProjectionOperations.IsWushuangMode && gate.WindowContext.SourceSeat is { } seat && seat != gate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[seat].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Camp, gate.OwnerSeat)) > 0;
    internal SkillProgramStepOutcome ClaimWushuangCampHand(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsWushuangCampClaimContext(frame) || frame.WushuangCampClaim is not null || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.ClaimWushuangEndedCampHand)
            throw new InvalidOperationException("Camp collection requires its exact native completed-turn child.");
        var seat = frame.WindowContext!.SourceSeat!.Value;
        if (_coreMatchParticipantsPlayersAccess._players[seat].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Camp, frame.OwnerSeat)) <= 0)
            return SkillProgramStepOutcome.Continue;
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(c => c.Id).ToArray();
        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var receipt = new WushuangCampClaimReceipt(source, frame.GameplayHash, frame.InstructionIndex, frame.WindowContext.ParentFrameId, seat, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            Cards = cards
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { WushuangCampClaim = receipt });
        void Complete(long batch, IReadOnlyList<CardMovementRecord> moves)
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var paid = receipt with
            {
                SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
                BatchId = batch
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active = active with { WushuangCampClaim = paid });
            _coreStrategicProgramsOperations.SetStrategicMarker(seat, PlayerMarkerKind.Camp, frame.OwnerSeat, 0, active);
            _coreStrategicProgramsOperations.SetStrategicMarker(frame.OwnerSeat, PlayerMarkerKind.Camp, frame.OwnerSeat, 1, active);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangCampClaimedEvent(frame.Id, source, frame.GameplayHash, receipt.EndedFrameId, seat, receipt.ActualTurn, cards.Length, paid.SequenceBefore, paid.SequenceAfter, batch));
        }

        if (cards.Length == 0)
        {
            Complete(0, []);
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { WushuangCampClaim = null });
            return SkillProgramStepOutcome.Continue;
        }

        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(frame.OwnerSeat), new(WushuangCampClaimReason), Complete);
        var outcome = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
        if (outcome == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { WushuangCampClaim = null });
        return outcome;
    }

    internal bool ValidWushuangCampClaim(ProgramSkillFrame frame)
    {
        if (frame.WushuangCampClaim is not { } receipt || receipt.Source != new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || receipt.GameplayHash != frame.GameplayHash || receipt.InstructionIndex != frame.InstructionIndex || receipt.EndedFrameId != frame.WindowContext?.ParentFrameId || receipt.EndedSeat != frame.WindowContext?.SourceSeat || receipt.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || receipt.Cards.Count == 0 || receipt.Cards.Distinct().Count() != receipt.Cards.Count || receipt.BatchId <= 0 || receipt.SequenceAfter <= receipt.SequenceBefore || !IsWushuangCampClaimContext(frame))
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > receipt.SequenceBefore && m.Sequence <= receipt.SequenceAfter).ToArray();
        return moves.Length == receipt.Cards.Count && moves.Select(m => m.CardId).SequenceEqual(receipt.Cards) && moves.All(m => m.From == CardLocation.Hand(receipt.EndedSeat) && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == WushuangCampClaimReason) && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCampClaimedEvent>().Count(e => e.ProgramFrameId == frame.Id && e.Source == receipt.Source && e.GameplayHash == receipt.GameplayHash && e.EndedFrameId == receipt.EndedFrameId && e.EndedSeat == receipt.EndedSeat && e.ActualTurn == receipt.ActualTurn && e.Count == receipt.Cards.Count && e.BatchId == receipt.BatchId && e.SequenceBefore == receipt.SequenceBefore && e.SequenceAfter == receipt.SequenceAfter) == 1;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 63200)]
    internal bool ReturnWushuangCampClaim(ProgramSkillFrame frame)
    {
        if (frame.WushuangCampClaim is null)
            return false;
        if (!ValidWushuangCampClaim(frame))
            throw new InvalidOperationException("Camp collection lost its paid movement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { WushuangCampClaim = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 63200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 63200)]
    internal bool RetainWushuangCampClaim(ProgramSkillFrame frame) => frame.WushuangCampClaim is not null;
    internal CurrentTurnCardUseKindsRecordedEvent? PreviousWushuangActualUse(int actor, CardUseFrame? current = null)
    {
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>();
        if (current is not null)
            facts = facts.TakeWhile(f => f.ActorSeat != actor || f.OriginFrameId != current.Id || current.Action is not { } action || f.CardActionId != action.ActionId).ToArray();
        return facts.LastOrDefault(f => f.ActorSeat == actor);
    }

    internal bool IsWangongModeSkill(string skillId) => _coreWushuangModeProjectionOperations.IsWushuangMode && skillId == WushuangWangong || _coreMatchDefinitionOptionsAccess._options.ShanheBattle is not null && skillId == ShanheWangong;
    internal bool HasWushuangWangongPrivilege(CharacterState actor, long? existingUseId = null)
    {
        var current = existingUseId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id) : null;
        if (current?.WushuangWangong is { } receipt)
            return receipt.Source.OwnerSeat == actor.Seat && ValidWushuangWangongUse(current);
        return (IsWangongModeSkill(WushuangWangong) && _coreNativeSkillContextQueriesOperations.HasProgramSkill(actor, WushuangWangong) || IsWangongModeSkill(ShanheWangong) && _coreNativeSkillContextQueriesOperations.HasProgramSkill(actor, ShanheWangong)) && PreviousWushuangActualUse(actor.Seat, current)?.CardCategoryMask == 1 << (int)SkillProgramCardCategory.Basic;
    }

    internal int WangongPublicProposedBonus(GameSnapshot view, int sourceSeat, CardKind? kind)
    {
        if (kind is not { } card || !CoreNativeCardClassificationsRuntime.IsSlashCard(card) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat))
            return 0;
        var source = view.Players.Single(player => player.Seat == sourceSeat);
        var visible = CoreGameEnginePublicDamagePredictionRuntime.VisiblePredictionSkillIds(source);
        return source.IsAlive && (visible.Contains(WushuangWangong) && IsWangongModeSkill(WushuangWangong) || visible.Contains(ShanheWangong) && IsWangongModeSkill(ShanheWangong)) && HasWushuangWangongPrivilege(_coreMatchParticipantsPlayersAccess._players[sourceSeat]) ? 1 : 0;
    }

    internal SkillProgramStepOutcome CaptureWushuangWangong(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsWangongModeSkill(frame.SkillId) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } context } || context.ActorSeat != frame.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(context.ParentCardUseFrameId)is not { Action: { } action, WushuangWangong: null } use || action.ActionId != context.CardActionId || action.ActorSeat != frame.OwnerSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            throw new InvalidOperationException("Wangong requires its exact accepted Slash and skill-source identity.");
        var previous = PreviousWushuangActualUse(frame.OwnerSeat, use);
        if (previous?.CardCategoryMask != 1 << (int)SkillProgramCardCategory.Basic)
            return SkillProgramStepOutcome.Continue;
        var receipt = new WushuangWangongUseReceipt(new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, use.Id, action.ActionId, previous.OriginFrameId, previous.CardActionId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { WushuangWangong = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangWangongUseCapturedEvent(receipt));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidWushuangWangongUse(CardUseFrame use) => use.WushuangWangong is { } receipt && IsWangongModeSkill(receipt.Source.SkillId) && receipt.Source.OwnerSeat == use.SourceSeat && receipt.UseFrameId == use.Id && use.Action is { } action && action.ActionId == receipt.ActionId && action.ActorSeat == use.SourceSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && _contentRegistryOperations._contentRegistry.GetSkill(receipt.Source.SkillId).Program?.GameplayHash == receipt.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangWangongUseCapturedEvent>().Count(e => e.Receipt == receipt) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.ActorSeat == use.SourceSeat && e.OriginFrameId == receipt.PreviousOriginFrameId && e.CardActionId == receipt.PreviousActionId && e.CardCategoryMask == 1 << (int)SkillProgramCardCategory.Basic);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 63201)]
    internal bool WushuangWangongQuota(ProgramSlashQuotaQuery query) => CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) && HasWushuangWangongPrivilege(query.Actor);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 63201)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 63201)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 63201)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 63201)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 63201)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 63201)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 63201)]
    internal bool WushuangWangongDistance(ProgramSlashDistanceQuery query) => CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) && HasWushuangWangongPrivilege(query.Actor, query.ExistingUseFrameId);
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 63201)]
    internal int WushuangWangongDamage(GameEngine.IDamageAttempt attack) => !attack.IsSourceLess && !attack.IsChainPropagation && attack.EffectiveCardKind is { } kind && CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { } use && use.SourceSeat == attack.SourceSeat && ValidWushuangWangongUse(use) ? 1 : 0;
}
