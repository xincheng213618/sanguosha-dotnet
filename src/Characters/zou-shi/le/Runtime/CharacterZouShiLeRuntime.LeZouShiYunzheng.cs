#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZouShiLeRuntime
{
    internal bool HasLeZouShiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializeYunzhengInitialEntities) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse);

    internal static CardConversionSource YunzhengSource(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_zou_shi_le_LeZouShiYunzheng.YunzhengSource(frame);
    // The physical mark outlives its original holder, zone and granting instance.
    // Registration only; the query still belongs to legacy GameEngine.LeZouShiYunzheng.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 130)]
    internal void ProjectYunzhengCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HasYunzhengEntityMark(cardId) ? "筝" : null);
    }

    internal bool HasYunzhengEntityMark(int cardId) => HasLeZouShiCapability && (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<YunzhengInitialEntityMarkedEvent>().Any(e => e.CardId == cardId) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuoxinEntityMarkedEvent>().Any(e => e.CardId == cardId));
    internal bool HasYunzhengHandEntityMark(int ownerSeat, int cardId) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Any(c => c.Id == cardId) && HasYunzhengEntityMark(cardId);
    internal bool IsYunzhengHandLimitExempt(CharacterState owner, int cardId) => HasLeZouShiCapability && HasLiveYunzhengSource(owner) && HasYunzhengHandEntityMark(owner.Seat, cardId);
    internal bool YunzhengNonLockedGrantQualified(CharacterState holder, SkillGrant grant) => !HasLeZouShiCapability || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked) || !_corePhysicalOwnedCardQueriesOperations.GetHand(holder).Any(c => HasYunzhengEntityMark(c.Id)) || !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != holder.Seat && HasLiveYunzhengSource(p));
    internal bool YunzhengBaseGrantQualified(CharacterState owner, SkillGrant grant) => _corePrivateGeneralLibrariesOperations.LibraryBaseGrantLive(owner, grant) && (grant.IsModeRule || !owner.IdentityConcealed && _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, grant) && _coreGuanduGeneralsOperations.GuanduGeneralGrantQualified(owner, grant) && _coreLordSkillProjectionsOperations.IsProjectedGrantQualified(owner, grant) && _corePrivateGeneralLibrariesOperations.IsGeneralLibraryGrantQualified(owner, grant) && _coreHpConditionalSkillGrantsOperations.Value.IsHpConditionalGrantQualified(owner, grant) && _characterHeQiOlOperations.IsEquipmentSuitGrantQualified(owner, grant) && _characterDaQiaoXiaoQiaoOlOperations.IsDaQiaoXiaoQiaoLuoyanGrantQualified(owner, grant));
    internal bool HasLiveYunzhengSource(CharacterState owner)
    {
        if (!owner.IsAlive)
            return false;
        return owner.SkillGrants.Grants.Any(g => g.SkillId == "le:yunzheng" && YunzhengBaseGrantQualified(owner, g));
    }

    internal long CaptureYunzhengQualificationStamp(long inner)
    {
        if (!HasLeZouShiCapability)
            return inner;
        var initial = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<YunzhengInitialEntityMarkedEvent>().Count();
        var shown = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuoxinEntityMarkedEvent>().Count();
        if (initial == 0 && shown == 0)
            return inner;
        // Pure cache evidence; a runtime skill lookup here would recurse into shards.
        var inputs = (inner, (long)_coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, initial, shown);
        var changed = inputs != _characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationInputs || _characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationSeats.Length != _coreMatchParticipantsPlayersAccess._players.Count;
        if (_characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationSeats.Length != _coreMatchParticipantsPlayersAccess._players.Count)
            _characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationSeats = new GameEngine.YunzhengQualificationSeat[_coreMatchParticipantsPlayersAccess._players.Count];
        for (var i = 0; i < _coreMatchParticipantsPlayersAccess._players.Count; i++)
        {
            var p = _coreMatchParticipantsPlayersAccess._players[i];
            var next = new GameEngine.YunzhengQualificationSeat(p.SkillGrants.Revision, p.Hp, p.IsAlive, p.IdentityConcealed, p.GeneralSelected, p.GeneralRevealed, p.SecondaryGeneralSelected, p.SecondaryGeneralRevealed, p.Role);
            if (_characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationSeats[i] == next)
                continue;
            _characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationSeats[i] = next;
            changed = true;
        }

        if (changed)
        {
            _characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationInputs = inputs;
            _characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationStamp++;
        }

        return long.MinValue / 8 + _characterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationStamp;
    }

    internal ProgramLifecycleTriggerWindowFrame CaptureYunzhengInitialEntities(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!HasLeZouShiCapability || window.Window != SkillProgramTriggerWindow.GameStarting || window.Continuation != ProgramLifecycleContinuation.CompleteGameStarting)
            return window;
        if (window.YunzhengInitialEntities is not null)
            throw new InvalidOperationException("Yunzheng's final opening Hand can only be frozen once.");
        var owners = window.Candidates.Where(candidate => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is [{ Op: SkillProgramEffectOp.InitializeYunzhengInitialEntities }]).Select(candidate => candidate.OwnerSeat).Distinct().Order();
        var entities = owners.SelectMany(owner => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(card =>
        {
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.To == CardLocation.Hand(owner));
            if (entry is null || entry.CardKind != card.Kind)
                throw new InvalidOperationException("A Yunzheng initial entity requires its actual opening-Hand entry.");
            return new YunzhengInitialEntity(owner, card.Id, card.Kind, entry.Sequence);
        })).ToArray();
        return window with
        {
            YunzhengInitialEntities = entities
        };
    }

    internal bool IsIssuedYunzhengCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => HasLeZouShiCapability && candidate.SkillId == "le:yunzheng" && candidate.BindingId == "mark-initial-entities" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash == candidate.GameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is [{ Op: SkillProgramEffectOp.InitializeYunzhengInitialEntities }] && context.Window == SkillProgramTriggerWindow.GameStarting && context.OwnerSeat == candidate.OwnerSeat && _coreActualTurnProgressPhaseAccess._phase == TurnPhase.NotStarted && _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId)is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting, Continuation: ProgramLifecycleContinuation.CompleteGameStarting, YunzhengInitialEntities: not null } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == candidate && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<YunzhengInitializedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28950, SkillProgramEffectOp.InitializeYunzhengInitialEntities)]
    internal bool CanOfferYunzheng(ProgramTriggerLegalityContext gate) => IsIssuedYunzhengCandidate(gate.Candidate, gate.Context);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28950)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28950)]
    internal bool PermitIssuedYunzheng(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "le:yunzheng" || frame.TriggerId != "mark-initial-entities" || frame.WindowContext is not { } context || frame.InstructionIndex is < 0 or > 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || !_coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(frame, SkillProgramTriggerWindow.GameStarting))
            return false;
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority, context.OccurrenceIndex);
        if (IsIssuedYunzhengCandidate(candidate, context))
            return true;
        // After the only instruction, preserve its completed right while the executor closes it.
        return frame.InstructionIndex == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<YunzhengInitializedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is [var initialized] && initialized.Source == YunzhengSource(frame) && initialized.GameplayHash == frame.GameplayHash;
    }

    SkillProgramStepOutcome ILeZouShiHost.InitializeYunzheng(ProgramSkillFrame frame) => InitializeYunzheng(frame);
    SkillProgramStepOutcome ILeZouShiHost.ResolveHuoxin(ProgramSkillFrame frame) => BeginHuoxin(frame);
    internal SkillProgramStepOutcome InitializeYunzheng(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 1 || !PermitIssuedYunzheng(frame) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<YunzhengInitializedEvent>().Any(e => e.Source.OwnerSeat == frame.OwnerSeat))
            throw new InvalidOperationException("Yunzheng requires its exact frozen opening mother and initializes once.");
        var parent = (ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == frame.WindowContext!.ParentFrameId);
        var entities = parent.YunzhengInitialEntities!.Where(e => e.OwnerSeat == frame.OwnerSeat).ToArray();
        if (entities.Select(e => e.CardId).Distinct().Count() != entities.Length || entities.Any(e => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.EntrySequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.To == CardLocation.Hand(e.OwnerSeat))))
            throw new InvalidOperationException("Yunzheng cannot mark an entity without its exact opening-Hand provenance.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YunzhengInitializedEvent(frame.Id, YunzhengSource(frame), frame.GameplayHash, entities.Length));
        foreach (var entity in entities)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YunzhengInitialEntityMarkedEvent(frame.Id, entity.OwnerSeat, entity.CardId, entity.PrintedKind, entity.EntrySequence));
        return SkillProgramStepOutcome.Continue;
    }
}
