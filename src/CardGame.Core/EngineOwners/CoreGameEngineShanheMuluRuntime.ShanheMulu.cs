#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheMuluRuntime
{
    internal const string MuluSummonSkill = "shanhe:mulu-summon", ElephantYangshengSkill = "shanhe:elephant-yangsheng";
    internal static bool IsShanheMuluFollower(CharacterState player) => player.General.Id is "shanhe:mulu-rage-elephant-2026" or "shanhe:mulu-angry-elephant-2026";
    SkillProgramStepOutcome IShanheMuluProgramHost.ReviveFollowers(ProgramSkillFrame frame) => BeginShanheMuluRevival(frame);
    void IShanheMuluProgramHost.FreezeRoundDamage(ProgramSkillFrame frame) => FreezeShanheElephantRoundDamage(frame);
    internal SkillProgramStepOutcome BeginShanheMuluRevival(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MuluSummonSkill || frame.ShanheMuluRevival is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Mulu revival requires its original living summoner's real turn start.");
        var targets = _coreMatchParticipantsPlayersAccess._players.Where(player => !player.IsAlive && player.MaxHp > 0 && IsShanheMuluFollower(player) && _coreTeamActivitiesOperations.SamePublicTeam(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], player)).Select(player => player.Seat).ToArray();
        if (targets.Length > 4)
            throw new InvalidOperationException("Mulu may own at most its four authored elephant followers.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = new(frame.InstructionIndex, 0, ShanheMuluRevivalStage.Cleanup) { TargetSeats = targets } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMuluRevivalIssuedEvent(frame.Id, frame.InstructionIndex, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), string.Join(",", targets)));
        ContinueShanheMuluRevival(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueShanheMuluRevival(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        while (frame.ShanheMuluRevival is { } receipt)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = null });
                _coreSkillProgramsOperations.FinishProgramSkill(frame, false);
                return;
            }

            if (receipt.TargetIndex >= receipt.TargetSeats.Count)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = null });
                _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
                return;
            }

            var target = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeats[receipt.TargetIndex]];
            if (receipt.Stage == ShanheMuluRevivalStage.StateChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with { TargetIndex = receipt.TargetIndex + 1, Stage = ShanheMuluRevivalStage.Cleanup } });
                continue;
            }

            if (receipt.Stage == ShanheMuluRevivalStage.CleanupChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with { Stage = ShanheMuluRevivalStage.StateChildren } });
                if (!target.IsAlive && target.MaxHp > 0 && target.HasHpValue)
                {
                    var wasFaceDown = _coreProgramLifeRestorationOperations.RestoreProgramCharacterLife(frame, target, target.MaxHp);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheMuluFollowerRevivedEvent(frame.Id, frame.OwnerSeat, target.Seat, target.Hp));
                    _coreProgramLifeRestorationOperations.PublishProgramRevivalFaceState(target, wasFaceDown);
                }

                continue;
            }

            if (target.IsAlive || target.MaxHp <= 0 || !target.HasHpValue)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with { TargetIndex = receipt.TargetIndex + 1 } });
                continue;
            }

            // Death normally already cleaned these zones. Any surviving owned
            // entities are really discarded; revival never creates starting cards.
            var cards = _cardZonesOperations._cardZones.CreateDiagnostics().Where(card => card.Location.OwnerSeat == target.Seat).GroupBy(card => card.Location).Select(group => (Location: group.Key, Cards: group.Select(card => card.CardId).ToArray())).ToArray();
            foreach (var group in cards)
                _corePhysicalMovementFacadesOperations.MoveCards(group.Cards.Select(id => _coreCardAttackStateOperations.GetAttackCard(id)).ToArray(), group.Location, CardLocation.DiscardPile, new("shanhe.mulu.revival-cleanup"));
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with { Stage = ShanheMuluRevivalStage.CleanupChildren } });
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 5420)]
    internal bool ResumeShanheMuluRevival(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheMuluRevival: not null } frame || frame.Id != id)
            return false;
        ContinueShanheMuluRevival(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 5420)]
    internal bool ReturnShanheMuluRevivalMovement(ProgramSkillFrame frame) => frame.ShanheMuluRevival is not null && ResumeShanheMuluRevival(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5420)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5420)]
    internal bool CanCompleteShanheMuluRevival(ProgramSkillFrame frame) => frame.ShanheMuluRevival is not null;

    internal void FreezeShanheElephantRoundDamage(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ElephantYangshengSkill || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)
            throw new InvalidOperationException("Yangsheng requires its own actual turn start.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var boundary = Array.FindLastIndex(history, item => item is RoundStartedEvent);
        var amount = Math.Min(3, history.Skip(boundary + 1).OfType<DamageAppliedEvent>().Count(item => item.TargetSeat == frame.OwnerSeat && item.Amount > 0));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheElephantDamageFrozenEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, amount));
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 5421)]
    internal int ShanheElephantFrozenDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsSourceLess || attack.IsChainPropagation || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.FreezeShanheElephantRoundDamage))
            return 0;
        var owner = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var instances = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(instance => instance.SkillId == ElephantYangshengSkill).Select(instance => instance.SkillInstanceId).ToHashSet(StringComparer.Ordinal);
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheElephantDamageFrozenEvent>().Where(item => item.Source.OwnerSeat == owner.Seat && instances.Contains(item.Source.SkillInstanceId)).GroupBy(item => item.Source.SkillInstanceId).Select(group => group.Last().Amount).Sum();
    }
}
