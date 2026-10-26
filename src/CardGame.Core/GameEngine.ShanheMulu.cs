namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MuluSummonSkill = "shanhe:mulu-summon", ElephantYangshengSkill = "shanhe:elephant-yangsheng";
    private static bool IsShanheMuluFollower(CharacterState player) => player.General.Id is
        "shanhe:mulu-rage-elephant-2026" or "shanhe:mulu-angry-elephant-2026";
    [ProgramCapability]
    private sealed class ShanheMuluProgramHost(GameEngine engine) : IShanheMuluProgramHost
    {
        public SkillProgramStepOutcome ReviveFollowers(ProgramSkillFrame frame) => engine.BeginShanheMuluRevival(frame);
        public void FreezeRoundDamage(ProgramSkillFrame frame) => engine.FreezeShanheElephantRoundDamage(frame);
    }

    private SkillProgramStepOutcome BeginShanheMuluRevival(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MuluSummonSkill || frame.ShanheMuluRevival is not null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } ||
            frame.OwnerSeat != _currentSeat || !_players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Mulu revival requires its original living summoner's real turn start.");
        var targets = _players.Where(player => !player.IsAlive && player.MaxHp > 0 && IsShanheMuluFollower(player) &&
            SamePublicTeam(_players[frame.OwnerSeat], player)).Select(player => player.Seat).ToArray();
        if (targets.Length > 4) throw new InvalidOperationException("Mulu may own at most its four authored elephant followers.");
        ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = new(frame.InstructionIndex, 0, ShanheMuluRevivalStage.Cleanup)
            { TargetSeats = targets } });
        AdvanceEventRulesAndQueueFact(new ShanheMuluRevivalIssuedEvent(frame.Id, frame.InstructionIndex,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), string.Join(",", targets)));
        ContinueShanheMuluRevival(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    private void ContinueShanheMuluRevival(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        while (frame.ShanheMuluRevival is { } receipt)
        {
            if (TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) ||
                TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) ||
                TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) ||
                TryBeginCardsMovedProgramWindow(frame.Id) || TryBeginAdvancedSkillsChanged(frame.Id)) return;
            frame = GetActiveProgramFrame(frame.Id);
            if (_winner != Winner.None || _status == EngineStatus.Completed)
            { ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = null }); FinishProgramSkill(frame, false); return; }
            if (receipt.TargetIndex >= receipt.TargetSeats.Count)
            { ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = null }); FinishProgramSkill(frame, true); return; }
            var target = _players[receipt.TargetSeats[receipt.TargetIndex]];
            if (receipt.Stage == ShanheMuluRevivalStage.StateChildren)
            {
                ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with
                    { TargetIndex = receipt.TargetIndex + 1, Stage = ShanheMuluRevivalStage.Cleanup } });
                continue;
            }
            if (receipt.Stage == ShanheMuluRevivalStage.CleanupChildren)
            {
                ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with { Stage = ShanheMuluRevivalStage.StateChildren } });
                if (!target.IsAlive && target.MaxHp > 0 && target.HasHpValue)
                {
                    var wasFaceDown = RestoreProgramCharacterLife(frame, target, target.MaxHp);
                    AdvanceEventRulesAndQueueFact(new ShanheMuluFollowerRevivedEvent(frame.Id, frame.OwnerSeat, target.Seat, target.Hp));
                    PublishProgramRevivalFaceState(target, wasFaceDown);
                }
                continue;
            }
            if (target.IsAlive || target.MaxHp <= 0 || !target.HasHpValue)
            {
                ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with
                    { TargetIndex = receipt.TargetIndex + 1 } });
                continue;
            }
            // Death normally already cleaned these zones. Any surviving owned
            // entities are really discarded; revival never creates starting cards.
            var cards = _cardZones.CreateDiagnostics().Where(card => card.Location.OwnerSeat == target.Seat)
                .GroupBy(card => card.Location).Select(group => (Location: group.Key, Cards: group.Select(card => card.CardId).ToArray())).ToArray();
            foreach (var group in cards)
                MoveCards(group.Cards.Select(id => GetAttackCard(id)).ToArray(), group.Location, CardLocation.DiscardPile, new("shanhe.mulu.revival-cleanup"));
            ReplaceRuntimeTop(frame = frame with { ShanheMuluRevival = receipt with { Stage = ShanheMuluRevivalStage.CleanupChildren } });
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 5420)]
    private bool ResumeShanheMuluRevival(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheMuluRevival: not null } frame || frame.Id != id) return false;
        ContinueShanheMuluRevival(frame); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 5420)]
    private bool ReturnShanheMuluRevivalMovement(ProgramSkillFrame frame) => frame.ShanheMuluRevival is not null && ResumeShanheMuluRevival(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 5420)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 5420)]
    private bool CanCompleteShanheMuluRevival(ProgramSkillFrame frame) => frame.ShanheMuluRevival is not null;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 5420)]
    private void AssertShanheMuluRevival(ProgramSkillFrame frame)
    {
        if (frame.ShanheMuluRevival is not { } receipt) return;
        if (frame.SkillId != MuluSummonSkill || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow ||
            receipt.InstructionIndex != frame.InstructionIndex || receipt.TargetSeats.Count > 4 ||
            receipt.TargetSeats.Distinct().Count() != receipt.TargetSeats.Count || receipt.TargetIndex < 0 ||
            receipt.TargetIndex > receipt.TargetSeats.Count || !Enum.IsDefined(receipt.Stage) ||
            receipt.TargetSeats.Any(seat => !IsValidPlayerSeat(seat) || !IsShanheMuluFollower(_players[seat])) ||
            ProgramEventHistory<ShanheMuluRevivalIssuedEvent>().Where(item => item.FrameId == frame.Id).ToArray() is not [var issued] ||
            issued.InstructionIndex != receipt.InstructionIndex || issued.Source != new CardConversionSource(frame.SkillId,
                GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || issued.TargetSeatsKey != string.Join(",", receipt.TargetSeats))
            throw new InvalidOperationException("Mulu revival lost its issued follower roster or owning continuation.");
    }

    private void FreezeShanheElephantRoundDamage(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ElephantYangshengSkill || frame.OwnerSeat != _currentSeat || !_players[frame.OwnerSeat].IsAlive ||
            frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)
            throw new InvalidOperationException("Yangsheng requires its own actual turn start.");
        var history = CompleteProgramEventHistory().ToArray();
        var boundary = Array.FindLastIndex(history, item => item is RoundStartedEvent);
        var amount = Math.Min(3, history.Skip(boundary + 1).OfType<DamageAppliedEvent>()
            .Count(item => item.TargetSeat == frame.OwnerSeat && item.Amount > 0));
        AdvanceEventRulesAndQueueFact(new ShanheElephantDamageFrozenEvent(frame.Id,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), _roundNumber, _turnNumber, amount));
    }
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 5421)]
    private int ShanheElephantFrozenDamageBonus(IDamageAttempt attack)
    {
        if (attack.IsSourceLess || attack.IsChainPropagation || !IsValidPlayerSeat(attack.SourceSeat) ||
            !_players[attack.SourceSeat].IsAlive || !_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.FreezeShanheElephantRoundDamage)) return 0;
        var owner = _players[attack.SourceSeat];
        var instances = GetSkillBindingShard(owner).ProgramInstances.Where(instance => instance.SkillId == ElephantYangshengSkill)
            .Select(instance => instance.SkillInstanceId).ToHashSet(StringComparer.Ordinal);
        return ProgramEventHistory<ShanheElephantDamageFrozenEvent>()
            .Where(item => item.Source.OwnerSeat == owner.Seat && instances.Contains(item.Source.SkillInstanceId))
            .GroupBy(item => item.Source.SkillInstanceId).Select(group => group.Last().Amount).Sum();
    }
}
