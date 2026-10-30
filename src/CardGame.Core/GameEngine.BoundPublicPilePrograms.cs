namespace CardGame.Core;

public sealed record ProgramPublicPileSelectedEvent(long FrameId, string SkillId, string GameplayHash,
    int InstructionIndex, string Bind, string SourceSkillId, string? SourceSkillInstanceId,
    CardLocation? SourceLocation, IReadOnlyList<int> CardIds, IReadOnlyList<Suit> PrintedSuits) : IGameEvent;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class BoundPublicPileProgramCapability(GameEngine engine) : IBoundPublicPileProgramHost
    {
        public SkillProgramStepOutcome SelectPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) =>
            engine.SelectProgramPublicPile(effect, frame);
        public SkillProgramStepOutcome DistributeBoundCards(SkillProgramEffect effect, ProgramSkillFrame frame) =>
            engine.DistributeProgramBoundPublicPile(effect, frame);
    }

    private SkillProgramStepOutcome SelectProgramPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var sources = ReferencedPublicPileSources(frame.OwnerSeat, effect.SkillIds.Single(), frame.SkillInstanceId)
            .Where(source => source.Capacity == effect.Amount && PublicPileCards(source).Count > 0 &&
                PublicPileSourceSatisfiesFrozenTrigger(frame, source)).ToArray();
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
        ReplaceRuntimeTop(frame = frame with { PublicPileDraft = new(frame.OwnerSeat,
            effect.SkillIds.Single(), "source", -1, [], [], [], []) });
        PublishPublicPileChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void BindSelectedPublicPile(ProgramSkillFrame frame, SkillProgramEffect effect, PublicPersistentPileSource? source)
    {
        var cards = source is null ? Array.Empty<Card>() : PublicPileCards(source);
        var locations = source is null ? Array.Empty<CardLocation>() : cards.Select(_ => source.Location).ToArray();
        SetProgramCardSet(frame.Id, effect.ResultBind!, cards.Select(card => card.Id).ToArray(),
            SkillProgramCardSetVisibility.Public, locations);
        var current = GetActiveProgramFrame(frame.Id);
        ReplaceRuntimeTop(current with { CardSetBindings = Array.AsReadOnly(current.CardSetBindings.Select(binding =>
            binding.Name == effect.ResultBind ? binding with { FrozenSelectedSuits = cards.Select(card => card.Suit).ToArray() } : binding).ToArray()) });
        AdvanceEventRulesAndQueueFact(new ProgramPublicPileSelectedEvent(frame.Id, frame.SkillId, frame.GameplayHash,
            frame.InstructionIndex, effect.ResultBind!, effect.SkillIds.Single(), source?.SkillInstanceId,
            source?.Location, Array.AsReadOnly(cards.Select(card => card.Id).ToArray()), Array.AsReadOnly(cards.Select(card => card.Suit).ToArray())));
    }

    private SkillProgramStepOutcome DistributeProgramBoundPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var binding = GetProgramCardSet(frame, effect.SourceBind!);
        if (binding.CardIds.Count == 0) return SkillProgramStepOutcome.Continue;
        if (frame.PublicPileDraft is not null || binding.Visibility != SkillProgramCardSetVisibility.Public ||
            binding.SourceLocations.Count != binding.CardIds.Count ||
            binding.SourceLocations.Distinct().ToArray() is not [var location] ||
            location.Zone != CardZoneKind.PublicPersistentPile || location.OwnerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("A bound distribution requires one exact public pile source.");
        var source = PublicPileSources(frame.OwnerSeat).SingleOrDefault(source => source.Location == location);
        // Losing the source skill cleans its pile through the existing observer;
        // the already selected binding cannot recreate those departed entities.
        if (source is null) return SkillProgramStepOutcome.Continue;
        if (binding.CardIds.Any(id => _cardZones.GetLocation(id) != location))
            throw new InvalidOperationException("A selected public-pile entity moved before distribution.");
        ReplaceRuntimeTop(frame = frame with { PublicPileDraft = new(frame.OwnerSeat, source.SkillId, "distribute", source.Capacity,
            [], [], [], Array.AsReadOnly(binding.CardIds.ToArray()),
            SupportsMultiplePublicPiles ? source.SkillInstanceId : null, SupportsMultiplePublicPiles ? source.Location : null) });
        if (PublicPileDistributionTargets(frame).Count == 0)
        { ReplaceRuntimeTop(frame with { PublicPileDraft = null }); return SkillProgramStepOutcome.Continue; }
        PublishPublicPileChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private IReadOnlyList<int> PublicPileDistributionTargets(ProgramSkillFrame frame)
    {
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!)
            .GetPausedInstruction(frame.InstructionIndex).Effect;
        return GetProgramTargetSeats(frame.OwnerSeat, paused.TargetKind ?? SkillProgramTargetKind.OtherLiving);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 900003)]
    private void AssertSelectedPublicPileBindings(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        for (var index = 0; index < Math.Min(frame.InstructionIndex, plan.Instructions.Count); index++)
        {
            var effect = plan.Instructions[index];
            if (effect.Op != SkillProgramEffectOp.SelectPublicPile) continue;
            var binding = frame.CardSetBindings.SingleOrDefault(binding => binding.Name == effect.ResultBind);
            // A source-choice prompt has not issued its binding yet.
            if (binding is null)
            {
                if (frame.InstructionIndex > index + 1)
                    throw new InvalidOperationException("A completed public-pile selection lost its issued binding.");
                continue;
            }
            var receipts = ProgramEventHistory<ProgramPublicPileSelectedEvent>().Where(issued => issued.FrameId == frame.Id &&
                issued.Bind == binding.Name && issued.InstructionIndex == index + 1).ToArray();
            if (receipts is not [var receipt] || receipt.SkillId != frame.SkillId || receipt.GameplayHash != frame.GameplayHash ||
                receipt.SourceSkillId != effect.SkillIds.Single() || binding.Visibility != SkillProgramCardSetVisibility.Public ||
                !binding.CardIds.SequenceEqual(receipt.CardIds) || binding.FrozenSelectedSuits is not { } suits ||
                !suits.SequenceEqual(receipt.PrintedSuits) || binding.CardIds.Count != binding.SourceLocations.Count ||
                binding.CardIds.Count != suits.Count || binding.CardIds.Distinct().Count() != binding.CardIds.Count ||
                (binding.CardIds.Count == 0 ? receipt.SourceLocation is not null || receipt.SourceSkillInstanceId is not null :
                    receipt.SourceLocation is not { Zone: CardZoneKind.PublicPersistentPile } location || location.OwnerSeat != frame.OwnerSeat ||
                    receipt.SourceSkillInstanceId is null || binding.SourceLocations.Any(source => source != location)))
                throw new InvalidOperationException("A selected public pile lost its exact issued entity and printed-suit receipt.");
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 900001)]
    private bool ContinueBoundPublicPileDistribution(ProgramSkillFrame frame)
    {
        if (frame.PublicPileDraft is not { Stage: "distribute" }) return false;
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!)
            .GetPausedInstruction(frame.InstructionIndex).Effect;
        if (paused.Op != SkillProgramEffectOp.DistributeBoundCards) return false;
        if (_pendingDecision is not null) return true;
        if (new ProgramSkillHost(this).TryStartPostInstructionWindow(frame.Id)) return true;
        frame = GetActiveProgramFrame(frame.Id);
        if (frame.PublicPileDraft is not { Stage: "distribute" } draft) return false;
        if (frame.PendingMovementContinuation is not null)
        { ReturnRuntimeProgramMovement(frame.Id); return true; }
        if (_winner != Winner.None)
        { FinishProgramSkill(frame, completed: false); return true; }
        if (!_players[frame.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        { CancelProgramBindingAndCleanup(frame, "公开牌分配的持有人或技能实例已失效。"); return true; }
        if (draft.RemainingIds.Count == 0 || PublicPileDistributionTargets(frame).Count == 0)
        {
            ReplaceRuntimeTop(frame with { PublicPileDraft = null });
            AdvanceRuntimeProgram(frame.Id);
        }
        else PublishPublicPileChoice(frame);
        return true;
    }
}
