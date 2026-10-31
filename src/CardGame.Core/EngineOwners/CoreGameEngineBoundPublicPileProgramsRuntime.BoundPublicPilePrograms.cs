#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBoundPublicPileProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IBoundPublicPileProgramHost.SelectPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => SelectProgramPublicPile(effect, frame);
    SkillProgramStepOutcome global::CardGame.Core.IBoundPublicPileProgramHost.DistributeBoundCards(SkillProgramEffect effect, ProgramSkillFrame frame) => DistributeProgramBoundPublicPile(effect, frame);
    internal SkillProgramStepOutcome SelectProgramPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var sources = _corePublicPileSourcesOperations.ReferencedPublicPileSources(frame.OwnerSeat, effect.SkillIds.Single(), frame.SkillInstanceId).Where(source => source.Capacity == effect.Amount && _corePublicPileSourcesOperations.PublicPileCards(source).Count > 0 && _corePublicPersistentPilesOperations.PublicPileSourceSatisfiesFrozenTrigger(frame, source)).ToArray();
        if (sources.Length == 0)
        {
            BindSelectedPublicPile(frame, effect, null);
            return SkillProgramStepOutcome.Continue;
        }

        if (sources.Length == 1)
        {
            BindSelectedPublicPile(frame, effect, sources[0]);
            return SkillProgramStepOutcome.Continue;
        }

        if (frame.PublicPileDraft is not null)
            throw new InvalidOperationException("A public pile source is already being selected.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicPileDraft = new(frame.OwnerSeat, effect.SkillIds.Single(), "source", -1, [], [], [], []) });
        _corePublicPersistentPilesOperations.PublishPublicPileChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void BindSelectedPublicPile(ProgramSkillFrame frame, SkillProgramEffect effect, PublicPersistentPileSource? source)
    {
        var cards = source is null ? Array.Empty<Card>() : _corePublicPileSourcesOperations.PublicPileCards(source);
        var locations = source is null ? Array.Empty<CardLocation>() : cards.Select(_ => source.Location).ToArray();
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, effect.ResultBind!, cards.Select(card => card.Id).ToArray(), SkillProgramCardSetVisibility.Public, locations);
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { CardSetBindings = Array.AsReadOnly(current.CardSetBindings.Select(binding => binding.Name == effect.ResultBind ? binding with { FrozenSelectedSuits = cards.Select(card => card.Suit).ToArray() } : binding).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPublicPileSelectedEvent(frame.Id, frame.SkillId, frame.GameplayHash, frame.InstructionIndex, effect.ResultBind!, effect.SkillIds.Single(), source?.SkillInstanceId, source?.Location, Array.AsReadOnly(cards.Select(card => card.Id).ToArray()), Array.AsReadOnly(cards.Select(card => card.Suit).ToArray())));
    }

    internal SkillProgramStepOutcome DistributeProgramBoundPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
        if (binding.CardIds.Count == 0)
            return SkillProgramStepOutcome.Continue;
        if (frame.PublicPileDraft is not null || binding.Visibility != SkillProgramCardSetVisibility.Public || binding.SourceLocations.Count != binding.CardIds.Count || binding.SourceLocations.Distinct().ToArray()is not [var location] || location.Zone != CardZoneKind.PublicPersistentPile || location.OwnerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("A bound distribution requires one exact public pile source.");
        var source = _corePublicPileSourcesOperations.PublicPileSources(frame.OwnerSeat).SingleOrDefault(source => source.Location == location);
        // Losing the source skill cleans its pile through the existing observer;
        // the already selected binding cannot recreate those departed entities.
        if (source is null)
            return SkillProgramStepOutcome.Continue;
        if (binding.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != location))
            throw new InvalidOperationException("A selected public-pile entity moved before distribution.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicPileDraft = new(frame.OwnerSeat, source.SkillId, "distribute", source.Capacity, [], [], [], Array.AsReadOnly(binding.CardIds.ToArray()), _corePublicPileSourcesOperations.SupportsMultiplePublicPiles ? source.SkillInstanceId : null, _corePublicPileSourcesOperations.SupportsMultiplePublicPiles ? source.Location : null) });
        if (PublicPileDistributionTargets(frame).Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicPileDraft = null });
            return SkillProgramStepOutcome.Continue;
        }

        _corePublicPersistentPilesOperations.PublishPublicPileChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<int> PublicPileDistributionTargets(ProgramSkillFrame frame)
    {
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        return _coreProgramLifecycleOperations.GetProgramTargetSeats(frame.OwnerSeat, paused.TargetKind ?? SkillProgramTargetKind.OtherLiving);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 900003)]
    internal void AssertSelectedPublicPileBindings(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        for (var index = 0; index < Math.Min(frame.InstructionIndex, plan.Instructions.Count); index++)
        {
            var effect = plan.Instructions[index];
            if (effect.Op != SkillProgramEffectOp.SelectPublicPile)
                continue;
            var binding = frame.CardSetBindings.SingleOrDefault(binding => binding.Name == effect.ResultBind);
            // A source-choice prompt has not issued its binding yet.
            if (binding is null)
            {
                if (frame.InstructionIndex > index + 1)
                    throw new InvalidOperationException("A completed public-pile selection lost its issued binding.");
                continue;
            }

            var receipts = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramPublicPileSelectedEvent>().Where(issued => issued.FrameId == frame.Id && issued.Bind == binding.Name && issued.InstructionIndex == index + 1).ToArray();
            if (receipts is not [var receipt] || receipt.SkillId != frame.SkillId || receipt.GameplayHash != frame.GameplayHash || receipt.SourceSkillId != effect.SkillIds.Single() || binding.Visibility != SkillProgramCardSetVisibility.Public || !binding.CardIds.SequenceEqual(receipt.CardIds) || binding.FrozenSelectedSuits is not { } suits || !suits.SequenceEqual(receipt.PrintedSuits) || binding.CardIds.Count != binding.SourceLocations.Count || binding.CardIds.Count != suits.Count || binding.CardIds.Distinct().Count() != binding.CardIds.Count || (binding.CardIds.Count == 0 ? receipt.SourceLocation is not null || receipt.SourceSkillInstanceId is not null : receipt.SourceLocation is not { Zone: CardZoneKind.PublicPersistentPile } location || location.OwnerSeat != frame.OwnerSeat || receipt.SourceSkillInstanceId is null || binding.SourceLocations.Any(source => source != location)))
                throw new InvalidOperationException("A selected public pile lost its exact issued entity and printed-suit receipt.");
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 900001)]
    internal bool ContinueBoundPublicPileDistribution(ProgramSkillFrame frame)
    {
        if (frame.PublicPileDraft is not { Stage: "distribute" })
            return false;
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (paused.Op != SkillProgramEffectOp.DistributeBoundCards)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return true;
        if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().TryStartPostInstructionWindow(frame.Id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.PublicPileDraft is not { Stage: "distribute" } draft)
            return false;
        if (frame.PendingMovementContinuation is not null)
        {
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
            return true;
        }

        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(frame, completed: false);
            return true;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "公开牌分配的持有人或技能实例已失效。");
            return true;
        }

        if (draft.RemainingIds.Count == 0 || PublicPileDistributionTargets(frame).Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicPileDraft = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        }
        else
            _corePublicPersistentPilesOperations.PublishPublicPileChoice(frame);
        return true;
    }
}
