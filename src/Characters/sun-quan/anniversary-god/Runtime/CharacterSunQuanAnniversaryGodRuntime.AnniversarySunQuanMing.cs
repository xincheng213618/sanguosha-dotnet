#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanAnniversaryGodRuntime
{
    internal static bool IsAnniversaryMingColorTrigger(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingColors }];
    internal int AnniversaryMingBatchColors(CardMovementBatchContext batch)
    {
        var locations = AnniversaryMingPiles().Select(p => p.Location).ToHashSet();
        return batch.Movements.Where(m => m.From != m.To && locations.Contains(m.To) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)).Select(m => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Suit)).Distinct().Count();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4700)]
    internal void AppendAnniversarySunQuanMingMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates)
    {
        if (!HasAnniversaryMing || AnniversaryMingBatchColors(batch) == 0)
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved).Where(c => c.SkillId == AnniversarySunQuanComposition.Cangming && c.BindingId == "sea-colors"))
                if (!candidates.Any(c => c.OwnerSeat == candidate.OwnerSeat && c.SkillId == candidate.SkillId && c.BindingId == candidate.BindingId && c.SkillInstanceId == candidate.SkillInstanceId))
                    candidates.Add(candidate);
    }

    internal ProgramSkillWindowContext CreateAnniversaryMingColorContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, SourceSeat: frame.Batch.OriginOwnerSeat, TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch);
    internal IReadOnlyList<int> DiscardAnniversarySunQuanMingOnDeath(DeathFrame death)
    {
        if (!HasAnniversaryMing)
            return[];
        if (_resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == death.Id)is not { } live || live.VictimSeat != death.VictimSeat || _coreMatchParticipantsPlayersAccess._players[death.VictimSeat].IsAlive)
            throw new InvalidOperationException("Ming cleanup requires its exact dead native holder.");
        var ids = AnniversaryMingCards(death.VictimSeat).Select(c => c.Id).ToArray();
        if (ids.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.DiscardPile, new("skill-program.anniversary-sun-quan.ming-death"), disclosure: CardMovementDisclosure.FaceUp);
        return Array.AsReadOnly(ids);
    }

    [ProgramViewContributor(ProgramViewContribution.NamedAmbushPiles, 40500)]
    internal IReadOnlyList<NamedAmbushPileSnapshot>? IncludeAnniversaryMingViews(IReadOnlyList<NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll)
    {
        var cards = AnniversaryMingCards(holder);
        if (cards.Count == 0)
            return ordinary;
        var entries = revealAll || viewer == holder ? Array.AsReadOnly(cards.Select(c => new NamedAmbushPrivateEntrySnapshot(_coreSnapshotValueProjectionOperations.ToSnapshot(c), c.Kind)).ToArray()) : null;
        return Array.AsReadOnly((ordinary ?? []).Append(new NamedAmbushPileSnapshot(AnniversarySunQuanComposition.Cangming, "溟", cards.Count, entries)).ToArray());
    }

    internal static string AnniversaryMingHolderInstance(int holder) => $"issued:anniversary-cangming:{(holder)}";
    internal ProgramTriggerCandidate AnniversaryMingReturnCandidate(int holder, string binding)
    {
        var p = _contentRegistryOperations._contentRegistry.GetSkill(AnniversarySunQuanComposition.Cangming).Program!;
        var trigger = p.Triggers.Single(t => t.Id == binding);
        return new(holder, p.Id, binding, AnniversaryMingHolderInstance(holder), p.GameplayHash, trigger.Priority);
    }

    internal bool AnniversaryMingHolderHasCards(int holder) => HasAnniversaryMing && _coreTableSeatQueriesOperations.IsValidPlayerSeat(holder) && _coreMatchParticipantsPlayersAccess._players[holder].IsAlive && AnniversaryMingCards(holder).Count > 0;
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 40500)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeAnniversaryMingReturns(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        var binding = window switch
        {
            SkillProgramTriggerWindow.TurnStartBeforeNormalFlow => "return-start",
            SkillProgramTriggerWindow.AfterDamageApplied => "return-damage",
            _ => null
        };
        if (binding is null)
            return ordinary;
        var retained = ordinary.Where(c => c.SkillId != AnniversarySunQuanComposition.Cangming || c.BindingId != binding).ToArray();
        return !AnniversaryMingHolderHasCards(owner.Seat) ? retained : Array.AsReadOnly(retained.Append(AnniversaryMingReturnCandidate(owner.Seat, binding)).ToArray());
    }

    [ProgramPipelineHook(ProgramPipelinePhase.UnissuedTurnStartObligation, 40500)]
    internal bool HasAnniversaryMingStartObligation(int seat) => AnniversaryMingHolderHasCards(seat);
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 40500)]
    internal bool IsIssuedAnniversaryMingReturn(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId == AnniversarySunQuanComposition.Cangming && candidate.BindingId == "sea-colors")
            return IsIssuedAnniversaryMingColors(candidate, context);
        if (candidate.SkillId != AnniversarySunQuanComposition.Cangming || candidate.BindingId is not ("return-start" or "return-damage") || candidate.SkillInstanceId != AnniversaryMingHolderInstance(candidate.OwnerSeat) || candidate.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program!.GameplayHash || context.OwnerSeat != candidate.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive)
            return false;
        // The exact paid frame remains authorized after its last card has left the pile.
        if (!AnniversaryMingHolderHasCards(candidate.OwnerSeat) && !_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.AnniversarySunQuan is { Operation: SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingReturn, Movements.Count: > 0 } && f.WindowContext == context && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate)))
            return false;
        if (candidate.BindingId == "return-start")
            return context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is { } start && start.OwnerSeat == candidate.OwnerSeat && start.CandidateIndex >= 0 && start.CandidateIndex < start.Candidates.Count && start.Candidates[start.CandidateIndex] == candidate;
        return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == candidate.OwnerSeat && context.Amount is> 0 && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is { } damage && damage.TargetSeat == candidate.OwnerSeat && damage.CandidateIndex >= 0 && damage.CandidateIndex < damage.Candidates.Count && damage.Candidates[damage.CandidateIndex].ToProgramCandidate() == candidate;
    }

    internal bool IsIssuedAnniversaryMingColors(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != AnniversarySunQuanComposition.Cangming || candidate.BindingId != "sea-colors" || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) || candidate.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program!.GameplayHash || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != candidate.OwnerSeat || context.TargetSeat != candidate.OwnerSeat || context.MovementBatch is not { } batch || batch.Id != context.ParentFrameId || AnniversaryMingBatchColors(batch) == 0 || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } window || window.Batch.Id != batch.Id || !window.Batch.Movements.SequenceEqual(batch.Movements) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate || batch.ParentFrameId is not { } origin || batch.AwaitingProgramFrameId != origin || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == origin)is not { AnniversarySunQuan: { } paid } producer || paid.Operation is not (SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingSetup or SkillProgramEffectOp.ResolveAnniversarySunQuanJichao) || paid.BatchId != batch.Id || !paid.Movements.SequenceEqual(batch.Movements) || !ValidAnniversarySunQuan(producer))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunQuanMovementPaidEvent>().Count(e => e.FrameId == origin && e.BatchId == batch.Id && e.Before == paid.Before && e.After == paid.After && e.Count == batch.Movements.Count) == 1;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40500, SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingReturn, SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingColors)]
    internal bool AnniversaryMingTriggerEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Effects[0].Op == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingReturn ? IsIssuedAnniversaryMingReturn(c, w) : w.Window == SkillProgramTriggerWindow.CardsMoved && w.MovementBatch is { } batch && AnniversaryMingBatchColors(batch) > 0;
    internal bool ValidAnniversarySunQuan(ProgramSkillFrame f)
    {
        if (f.AnniversarySunQuan is not { } r || !AnniversarySunQuanComposition.Owns(r.Operation) || r.Source != AnniversarySunQuanSource(f) || r.GameplayHash != f.GameplayHash || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || f.InstructionIndex != 1 || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunQuanStartedEvent>().Count(e => e == new AnniversarySunQuanStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.ParentId)) != 1)
            return false;
        if (r.ParentId != 0)
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.ParentId || _resolutionStackOperations._resolutionStack[index - 1] switch
            {
                ProgramLifecycleTriggerWindowFrame p => p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
                DamageTriggerWindowFrame p => p.TargetSeat != f.OwnerSeat || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex].ToProgramCandidate()),
                CardsMovedTriggerWindowFrame p => p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]) || p.Batch.Id != f.WindowContext?.MovementBatch?.Id,
                _ => true
            })
                return false;
        }

        if (r.Stage is AnniversarySunQuanStage.ChoosingBranch or AnniversarySunQuanStage.ChoosingTarget)
            return r.Operation == SkillProgramEffectOp.ResolveAnniversarySunQuanJichao && r.Movements.Count == 0 && r.After == r.Before && r.BatchId == 0 && r.ColorCount == 0;
        if (r.Stage == AnniversarySunQuanStage.Drawing)
            return r.Operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingColors && r.DrawOutcome is null && r.Movements.Count == 0 && r.ColorCount is >= 1 and <= 3 && f.NativeDraw is { } draw && draw.RequestedCount == r.ColorCount && draw.Recipients.SequenceEqual([f.OwnerSeat]) && draw.Operation == r.Operation;
        var records = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == (r.Operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingColors ? AnniversaryMingDraw : AnniversaryMingReason)).ToArray();
        if (!records.SequenceEqual(r.Movements) || r.Movements.Select(m => m.CardId).Distinct().Count() != r.Movements.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversarySunQuanMovementPaidEvent>().Count(e => e == new AnniversarySunQuanMovementPaidEvent(f.Id, r.BatchId, records.Length, r.Before, r.After)) != 1)
            return false;
        if (r.Stage == AnniversarySunQuanStage.Finished && records.Length == 0 && r.DrawOutcome is null && r.Before == r.After && r.BatchId == 0)
            return true;
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingColors)
            return r.ColorCount is >= 1 and <= 3 && r.BatchId == 0 && f.WindowContext?.MovementBatch is { } batch && r.ColorCount == AnniversaryMingBatchColors(batch) && r.DrawOutcome is { } outcome && outcome.RecipientSeat == f.OwnerSeat && outcome.RequestedCount == r.ColorCount && outcome.SequenceBefore == r.Before && outcome.SequenceAfter == r.After && outcome.ActualDrawIds.SequenceEqual(records.Select(m => m.CardId)) && records.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat));
        var locations = AnniversaryMingPiles().Select(p => p.Location).ToHashSet();
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingReturn)
            return records.Length > 0 && records.All(m => locations.Contains(m.From) && m.From.OwnerSeat == f.OwnerSeat && m.To == CardLocation.Hand(f.OwnerSeat));
        return records.Length > 0 && records.All(m => m.From.OwnerSeat is not null && (r.Operation != SkillProgramEffectOp.ResolveAnniversarySunQuanCangmingSetup || m.From.Zone == CardZoneKind.Hand) && (r.Operation != SkillProgramEffectOp.ResolveAnniversarySunQuanJichao || m.From.OwnerSeat != f.OwnerSeat && (r.Branch == 2 || m.From.OwnerSeat == r.Target)) && (m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.From, m.To)) || locations.Contains(m.To) && m.To.OwnerSeat == m.From.OwnerSeat));
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 40500)]
    internal void RequireAnniversarySunQuan(ProgramSkillFrame f)
    {
        if (f.AnniversarySunQuan is not null && !ValidAnniversarySunQuan(f))
            throw new InvalidOperationException("Anniversary Sun Quan lost his exact source, original native parent, private paid allocation or once-only movement receipt.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 40500)]
    internal bool AnniversarySunQuanMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.AnniversarySunQuan is { Stage: AnniversarySunQuanStage.MovementChildren } r && effect?.Op == r.Operation && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidAnniversarySunQuan(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40500)]
    internal bool KeepPaidAnniversarySunQuan(ProgramSkillFrame f) => f.AnniversarySunQuan is { Movements.Count: > 0 } && ValidAnniversarySunQuan(f) || f.AnniversarySunQuan is null && f.SkillId == AnniversarySunQuanComposition.Cangming && f.TriggerId is "return-start" or "return-damage" && f.ActivationId == f.TriggerId && f.InstructionIndex == 0 && f.WindowContext is { } context && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == context.Window) == 1 && IsIssuedAnniversaryMingReturn(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1100)]
    internal ProgramSkillWindowContext? TryCreateAnniversaryMingColorContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsAnniversaryMingColorTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(candidate)))
            return CreateAnniversaryMingColorContext(frame, candidate);
        return null;
    }
}
