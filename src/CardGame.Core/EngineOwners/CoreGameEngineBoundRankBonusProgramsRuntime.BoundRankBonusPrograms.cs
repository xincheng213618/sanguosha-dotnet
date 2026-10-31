#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBoundRankBonusProgramsRuntime
{
    internal void RevealWithNextBooleanBonus(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var bonus = _coreProgramPersistentStateOperations.GetProgramBooleanState(frame, effect.StateId!) ? effect.MaximumCards : 0;
        // The accepted reveal consumes the exact source-instance state once.
        // A declined trigger never reaches this instruction.
        _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, effect.StateId!, false);
        _coreProgramLifecycleOperations.RevealProgramTopCards(frame.Id, frame.OwnerSeat, checked(effect.Amount + bonus), null, effect.ResultBind!, effect.Visibility);
    }

    internal SkillProgramStepOutcome ObtainAndArmNextRevealBonus(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var source = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
        if (source.CardIds.Count != source.SourceLocations.Count || source.SourceLocations.Any(location => location != CardLocation.Processing))
            throw new InvalidOperationException("A rank-bonus gain requires exact revealed Processing entities.");
        var cards = source.CardIds.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id)).ToArray();
        var sum = cards.Sum(card => card.Rank);
        if (sum > effect.MaximumRankSum)
            throw new InvalidOperationException("The selected revealed subset exceeds the issuing rank threshold.");
        // MoveProgramBoundCards performs the atomic gain and queues its facts;
        // it does not begin own-hand gain observers. Arm before those children.
        _ = _coreProgramLifecycleOperations.MoveProgramBoundCards(frame.Id, frame.OwnerSeat, effect.SourceBind!, null, SkillProgramCardDestination.OwnerHand, null, new($"skill-program.{(frame.SkillId)}.rank-bonus-obtain"));
        _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, effect.StateId!, cards.Length > 0 && sum == effect.MaximumRankSum);
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    internal bool TryOtherDyingVictim(ProgramSkillWindowContext context, int ownerSeat, out DyingFrame dying)
    {
        dying = null !;
        if (context.Window != SkillProgramTriggerWindow.DyingEntering || context.TargetSeat is not { } victim || victim == ownerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) || !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive || _coreMatchParticipantsPlayersAccess._players[victim].Hp > 0 || _coreDyingCursorRuntimeOperations.ActiveDying is not { } active || active.VictimSeat != victim || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault()is not { } entry || entry.Id != context.ParentFrameId || entry.OwnerSeat != victim || entry.Window != SkillProgramTriggerWindow.DyingEntering || entry.Continuation != ProgramLifecycleContinuation.ResumeDyingEntry || entry.ResumeDyingFrameId != active.FrameId || _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(f => f.Id == active.FrameId)is not { } current)
            return false;
        dying = current;
        return true;
    }

    // Hook only when the trigger contains the new op, before any equipment cost.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 930)]
    internal bool CanRunOtherDyingVictimRecovery(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RecoverOtherDyingVictimTo) || TryOtherDyingVictim(context, ownerSeat, out _);
    internal void RecoverOtherDyingVictim(ProgramSkillFrame frame, int hp)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.WindowContext is not { } context || !TryOtherDyingVictim(context, frame.OwnerSeat, out var dying))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "原濒死进入窗口或他人目标已失效，已付装备不重复支付。");
            return;
        }

        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame entry || entry.Id != context.ParentFrameId || entry.CandidateIndex < 0 || entry.CandidateIndex >= entry.Candidates.Count || entry.Candidates[entry.CandidateIndex] is not { } candidate || candidate.OwnerSeat != frame.OwnerSeat || candidate.SkillId != frame.SkillId || candidate.BindingId != frame.TriggerId || candidate.SkillInstanceId != frame.SkillInstanceId || candidate.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("A dying recovery lost its exact current program candidate and source instance.");
        _coreProgramLifecycleOperations.RecoverProgramTargetTo(frame.Id, frame.OwnerSeat, dying.VictimSeat, SkillProgramNumberExpression.IntegerConstant, hp, true);
    }

    void IBoundRankBonusProgramHost.RevealTopCardsWithNextBooleanBonus(ProgramSkillFrame f, SkillProgramEffect e) => RevealWithNextBooleanBonus(f, e);
    SkillProgramStepOutcome IBoundRankBonusProgramHost.ObtainBoundCardsAndArmNextRevealBonus(ProgramSkillFrame f, SkillProgramEffect e) => ObtainAndArmNextRevealBonus(f, e);
    void IBoundRankBonusProgramHost.RecoverOtherDyingVictimTo(ProgramSkillFrame f, int hp) => RecoverOtherDyingVictim(f, hp);
}
