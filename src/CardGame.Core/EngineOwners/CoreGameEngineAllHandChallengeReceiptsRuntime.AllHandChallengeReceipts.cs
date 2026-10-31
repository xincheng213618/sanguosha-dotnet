#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAllHandChallengeReceiptsRuntime
{
    internal bool ValidAllHandChallenge(ProgramSkillFrame f)
    {
        if (f.SequentialDiscard is not { } d || !CoreGameEngineAllHandChallengesRuntime.IsAllHandChallengeKind(d.Kind) || f.TriggerId is not null || f.WindowContext is not null || f.ActivationId is null || !f.ReexecuteParticipantInstruction || f.InstructionIndex != 1 || d.InstructionIndex != 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(f.OwnerSeat) || f.OwnerSeat != d.ActualTurnOwnerSeat || d.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || d.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || d.Source != _coreSequentialDiscardProgramsOperations.SequentialDiscardSource(f) || d.GameplayHash != f.GameplayHash || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var start] || start != d.StartSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(start) || start == f.OwnerSeat || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(f.SkillId)?.Program is not { } program || program.GameplayHash != f.GameplayHash || program.Activations.SingleOrDefault(a => a.Id == f.ActivationId)is not { Effects.Count: 1 } activation || d.Kind != (activation.Effects[0].Op == SkillProgramEffectOp.ChooseTrickOrAllHandDiscard ? ProgramSequentialDiscardKind.CategoryOrAllHand : ProgramSequentialDiscardKind.SelectedStartAllHand) || !CoreGameEngineAllHandChallengesRuntime.IsAllHandChallengeOp(activation.Effects[0].Op) || !d.Order.SequenceEqual(_coreSequentialDiscardProgramsOperations.SequentialDiscardOrder(f.OwnerSeat, start, CoreGameEngineAllHandChallengesRuntime.AllHandChallengeRing(d))) || d.Cursor < 0 || d.Cursor >= d.Order.Count || d.ChooserSeat != d.Order[d.Cursor] || d.PreviousCount != 0 || d.SelectedCardIds.Count != 0 || d.SelectedLocations.Count != 0 || f.SequentialDiscardTopPayment is not null || d.Stage is not (ProgramSequentialDiscardStage.BranchChoice or ProgramSequentialDiscardStage.CardChoice or ProgramSequentialDiscardStage.AwaitingMovement or ProgramSequentialDiscardStage.AwaitingDamage))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) != 1 || history.OfType<ProgramSequentialDiscardStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued != new ProgramSequentialDiscardStartedEvent(f.Id, d.Kind, d.Source, d.GameplayHash, d.ActualTurnNumber, d.ActualTurnOwnerSeat, start))
            return false;
        if (CoreGameEngineAllHandChallengesRuntime.AllHandChallengeRing(d))
        {
            if (history.OfType<ProgramAllHandChallengeQuotaPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var quota] || quota != new ProgramAllHandChallengeQuotaPaidEvent(f.Id, d.Source, d.GameplayHash, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat) || quota.RoundNumber < 1 || history.OfType<ProgramAllHandChallengeQuotaPaidEvent>().Any(e => e.FrameId < f.Id && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.Source.BindingId == f.ActivationId && quota.RoundNumber - e.RoundNumber < 2))
                return false;
        }
        else if (_coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, activation.UsageGroup)) != 1)
            return false;
        var branch = history.OfType<ProgramSequentialDiscardBranchEvent>().Where(e => e.FrameId == f.Id && e.Cursor == d.Cursor).ToArray();
        if (d.Stage == ProgramSequentialDiscardStage.BranchChoice)
            return d.PrimaryBranch is null && d.Payment is null && d.Remaining == 0 && f.AttackAttempt is null && f.AttackReturn is null && f.PendingMovementContinuation is null && branch.Length == 0;
        if (d.Stage == ProgramSequentialDiscardStage.AwaitingDamage)
        {
            if (!CoreGameEngineAllHandChallengesRuntime.AllHandChallengeRing(d) || d.PrimaryBranch is not null || d.Payment is not null || d.Remaining != 0 || branch.Length != 0 || f.PendingMovementContinuation is not null || history.OfType<ProgramSequentialDiscardDamageChosenEvent>().Where(e => e.FrameId == f.Id && e.Cursor == d.Cursor).ToArray()is not [var damage] || damage.SourceSeat != f.OwnerSeat || damage.TargetSeat != d.ChooserSeat || damage.Amount < 0 || damage.Nature != DamageNature.Fire)
                return false;
            return f.AttackAttempt is not { } attempt || attempt.SourceSeat == f.OwnerSeat && attempt.Nature == DamageNature.Fire && f.AttackReturn is not null;
        }

        if (branch is not [var chosen] || chosen.ChooserSeat != d.ChooserSeat || chosen.PrimaryBranch != d.PrimaryBranch || chosen.RequiredCount < 0 || d.PrimaryBranch == true && (CoreGameEngineAllHandChallengesRuntime.AllHandChallengeRing(d) || chosen.RequiredCount != 1) || f.AttackAttempt is not null || f.AttackReturn is not null)
            return false;
        if (d.Stage == ProgramSequentialDiscardStage.CardChoice)
            return !CoreGameEngineAllHandChallengesRuntime.AllHandChallengeRing(d) && d.PrimaryBranch == true && d.Remaining == 1 && d.Payment is null && f.PendingMovementContinuation is null;
        if (d.Stage != ProgramSequentialDiscardStage.AwaitingMovement || d.Remaining != 0 || d.Payment is not { } payment || payment.SourceLocations.Any(l => l != CardLocation.Hand(d.ChooserSeat)) || payment.CardIds.Count != chosen.RequiredCount || d.PrimaryBranch == true && payment.CardIds.Any(id => !_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Any(c => c.Id == id && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == SkillProgramCardCategory.Trick)) || !_coreSequentialDiscardReceiptsOperations.ValidSequentialDiscardPayment(f, d, payment))
            return false;
        return f.PendingMovementContinuation is not null || ReferenceEquals(f, _resolutionStackOperations._resolutionStack.LastOrDefault());
    }
}
