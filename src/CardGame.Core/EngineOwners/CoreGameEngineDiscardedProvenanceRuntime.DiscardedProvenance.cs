#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDiscardedProvenanceRuntime
{
    internal bool TracksDiscardedEntityProvenance => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ClaimDiscardedEntityWithProvenance);

    internal void CaptureProvenanceJudgmentOrigin(JudgmentFrame judgment, Card card, Suit effectiveSuit)
    {
        if (!TracksDiscardedEntityProvenance)
            return;
        var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id);
        if (entry is null || entry.To != _cardZonesOperations._cardZones.GetLocation(card.Id) || entry.To != CardLocation.Processing && entry.To != CardLocation.Judgment(judgment.TargetSeat))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentEntityOriginEvent(judgment.Id, judgment.TargetSeat, card.Id, effectiveSuit, entry.Sequence));
    }

    internal void CaptureDiscardedEntityOrigin(Card card, CardMovementRecord movement)
    {
        if (!TracksDiscardedEntityProvenance || movement.To != CardLocation.DiscardPile)
            return;
        var previous = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.Sequence < movement.Sequence);
        var judged = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramJudgmentEntityOriginEvent>().LastOrDefault(e => e.CardId == card.Id);
        if (judged is not null && previous is not null && previous.To == movement.From && (previous.Sequence == judged.EntryMovementSequence && (movement.From == CardLocation.Processing || movement.From == CardLocation.Judgment(judged.SubjectSeat)) || IsProvenanceJudgmentResultHandoff(card.Id, judged, previous, movement.From)))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDiscardedEntityOriginEvent(movement.Sequence, judged.SubjectSeat, judged.EffectiveSuit, judged.JudgmentFrameId));
            return;
        }

        if (_coreProgramDiscardOriginsOperations.GetProgramDiscardSource(movement)?.OwnerSeat is not { } source)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDiscardedEntityOriginEvent(movement.Sequence, source, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[source], card), null));
    }

    internal bool IsProvenanceJudgmentResultHandoff(int cardId, ProgramJudgmentEntityOriginEvent judged, CardMovementRecord previous, CardLocation currentFrom)
    {
        // A program's final judgment result has exactly one typed handoff to
        // Processing before its configured disposition. No other Processing
        // movement inherits a judgment origin.
        if (currentFrom != CardLocation.Processing || previous.To != CardLocation.Processing || previous.From != CardLocation.Judgment(judged.SubjectSeat) || previous.Reason.Value != "skill-program.judgment.result")
            return false;
        var entry = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == cardId && m.Sequence < previous.Sequence);
        return entry is not null && entry.Sequence == judged.EntryMovementSequence && entry.To == previous.From && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<JudgmentResolvedEvent>().Any(e => e.ResolutionId == judged.JudgmentFrameId && e.TargetSeat == judged.SubjectSeat && e.CardId == cardId && e.Suit == judged.EffectiveSuit);
    }

    internal ProgramDiscardedEntityOriginEvent? DiscardedProvenanceOrigin(CardMovementRecord movement) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDiscardedEntityOriginEvent>().SingleOrDefault(e => e.MovementSequence == movement.Sequence);
    internal int[] MatchingProvenanceDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var current = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).ToDictionary(c => c.Id);
        return batch.Movements.Select((m, i) => (m, i, origin: DiscardedProvenanceOrigin(m))).Where(x => x.origin is not null && x.m.To == CardLocation.DiscardPile && x.origin.SourceSeat != candidate.OwnerSeat && current.ContainsKey(x.m.CardId) && (trigger.Suits.Count == 0 || trigger.Suits.Contains(x.origin.EffectiveSuit))).Select(x => x.i).ToArray();
    }

    SkillProgramStepOutcome IDiscardedProvenanceProgramHost.ClaimDiscardedEntityWithProvenance(ProgramSkillFrame frame, string provenanceId) => BeginDiscardedProvenanceClaim(frame, provenanceId);
    SkillProgramStepOutcome IDiscardedProvenanceProgramHost.OfferFaceUpForOutsideClaims(ProgramSkillFrame frame, string provenanceId) => OfferProvenanceFaceUp(frame, provenanceId);
    SkillProgramStepOutcome IDiscardedProvenanceProgramHost.UseVirtualAlcohol(ProgramSkillFrame frame) => _coreProvenanceVirtualAlcoholOperations.BeginProvenanceVirtualAlcohol(frame);
    internal SkillProgramStepOutcome BeginDiscardedProvenanceClaim(ProgramSkillFrame input, string tag)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ProvenanceClaim is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count)
            throw new InvalidOperationException("A provenance claim lost its exact per-entity discard window.");
        var movement = batch.Movements[index];
        var origin = DiscardedProvenanceOrigin(movement);
        if (origin is null || origin.SourceSeat == f.OwnerSeat || movement.To != CardLocation.DiscardPile || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(movement))
            throw new InvalidOperationException("A provenance claim lost its captured discard/judgment origin.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _cardZonesOperations._cardZones.GetLocation(movement.CardId) != CardLocation.DiscardPile)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "落英实体或来源已失效，未领取牌。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == movement.CardId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceClaim = new(f.InstructionIndex, tag, card.Id, movement.Sequence, 0, ProgramProvenanceClaimStage.Claiming) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new($"skill-program.{(f.SkillId)}.provenance-claim"), beforeFact: claimed =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { ProvenanceClaim = active.ProvenanceClaim!with { ClaimMovementSequence = claimed.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDiscardedEntityClaimedEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), tag, card.Id, movement.Sequence, claimed.Sequence, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 630)]
    internal bool ResumeProvenanceClaim(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.ProvenanceClaim is not { } receipt)
            return false;
        AssertProvenanceClaim(f);
        if (receipt.Stage is ProgramProvenanceClaimStage.Claiming or ProgramProvenanceClaimStage.Flipping)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return true;
            if (receipt.Stage == ProgramProvenanceClaimStage.Flipping)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceClaim = null });
                return false;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceClaim = receipt with { Stage = ProgramProvenanceClaimStage.Claimed } });
            return false;
        }

        return receipt.Stage == ProgramProvenanceClaimStage.Choosing;
    }

    internal bool ProvenanceThresholdQualified(ProgramSkillFrame f, ProgramProvenanceClaimReceipt receipt) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsFaceDown && _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != f.OwnerSeat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], SkillProgramCardPolicyKind.ClaimedEntitiesFaceDownUse).Any(p => p.Policy.Id == receipt.ProvenanceId) && GetOutsideProvenanceCount(f.OwnerSeat, f.SkillId, f.SkillInstanceId, receipt.ProvenanceId) >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp;
    internal int GetOutsideProvenanceCount(int seat, string skill, string instance, string tag)
    {
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var cutoff = Array.FindLastIndex(facts, e => e is TurnStartedEvent t && t.ActorSeat == seat || e is ProgramProvenanceFaceUpResetEvent r && r.OwnerSeat == seat);
        return facts.Skip(cutoff + 1).OfType<ProgramDiscardedEntityClaimedEvent>().Count(e => e.Origin.OwnerSeat == seat && e.Origin.SkillId == skill && e.Origin.SkillInstanceId == instance && e.ProvenanceId == tag && e.ActualTurnOwnerSeat != seat);
    }

    internal SkillProgramStepOutcome OfferProvenanceFaceUp(ProgramSkillFrame input, string tag)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ProvenanceClaim is not { Stage: ProgramProvenanceClaimStage.Claimed } receipt || receipt.ProvenanceId != tag || f.InstructionIndex != receipt.InstructionIndex + 1)
            throw new InvalidOperationException("The provenance threshold lost its completed one-entity claim.");
        if (!ProvenanceThresholdQualified(f, receipt))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceClaim = null });
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceClaim = receipt with { Stage = ProgramProvenanceClaimStage.Choosing } });
        PromptChoice Choice(string option, string label) => new(new($"provenance-face.{(f.Id)}.{(option)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = "provenance-face-up", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option"] = option });
        var policy = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], SkillProgramCardPolicyKind.ClaimedEntitiesFaceDownUse).First(p => p.Policy.Id == tag);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(policy.Source.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "回合外累计获得落英牌已达到体力上限，可以翻至正面。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(new[] { Choice("flip", "翻至正面"), Choice("pass", "保持背面") }),
            SkillPrompt = new(policy.Source.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("provenance-face-up")]
    internal void ResolveProvenanceFaceUp(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.ProvenanceClaim is not { Stage: ProgramProvenanceClaimStage.Choosing } receipt || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || choice.Cards.Count != 0 || choice.Targets.Count != 0 || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Parameters.GetValueOrDefault("option")is not ("flip" or "pass"))
            throw new InvalidOperationException("A provenance face-up choice lost its exact unpaid owner.");
        var flip = choice.Parameters["option"] == "flip" && ProvenanceThresholdQualified(f, receipt);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!flip)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceClaim = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ProvenanceClaim = receipt with { Stage = ProgramProvenanceClaimStage.Flipping } });
        _coreProgramLifecycleOperations.SetProgramTargetFaceState(f.Id, f.OwnerSeat, f.OwnerSeat, false);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void RecordProvenanceFaceUpReset(int owner, bool wasFaceDown)
    {
        if (TracksDiscardedEntityProvenance && wasFaceDown && !_coreMatchParticipantsPlayersAccess._players[owner].IsFaceDown)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramProvenanceFaceUpResetEvent(owner, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
    }

    internal void AssertProvenanceClaim(ProgramSkillFrame f)
    {
        if (f.ProvenanceClaim is not { } r)
            return;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions;
        if (r.InstructionIndex < 1 || r.InstructionIndex >= plan.Count || plan[r.InstructionIndex - 1].Op != SkillProgramEffectOp.ClaimDiscardedEntityWithProvenance || plan[r.InstructionIndex - 1].SourceBind != r.ProvenanceId || plan[r.InstructionIndex].Op != SkillProgramEffectOp.OfferFaceUpForOutsideClaims || f.InstructionIndex != (r.Stage is ProgramProvenanceClaimStage.Claiming or ProgramProvenanceClaimStage.Claimed ? r.InstructionIndex : r.InstructionIndex + 1) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDiscardedEntityClaimedEvent>().Any(e => e.FrameId == f.Id && e.CardId == r.CardId && e.DiscardMovementSequence == r.DiscardMovementSequence && e.ClaimMovementSequence == r.ClaimMovementSequence && e.Origin == new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && e.ProvenanceId == r.ProvenanceId) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.ClaimMovementSequence && m.CardId == r.CardId && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("A provenance claim lost its immutable exact entity receipt.");
        if (r.Stage == ProgramProvenanceClaimStage.Choosing && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != f.OwnerSeat || prompt.Choices.Count != 2 || prompt.Choices.Any(c => c.Cards.Count != 0 || c.Targets.Count != 0 || c.Parameters.GetValueOrDefault("program-action") != "provenance-face-up")))
            throw new InvalidOperationException("A provenance threshold lost its exact optional chooser.");
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 900)]
    internal int[]? SelectWindowIndexes_MatchingProvenanceDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.ClaimDiscardedEntityWithProvenance))
            return MatchingProvenanceDiscardIndexes(batch, candidate, trigger);
        return null;
    }
}
