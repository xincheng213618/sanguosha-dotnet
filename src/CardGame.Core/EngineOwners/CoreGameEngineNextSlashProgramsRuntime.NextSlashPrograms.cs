#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNextSlashProgramsRuntime
{
    internal bool HasNextSlashDamageCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ReserveNextSlashDamage);

    void global::CardGame.Core.INextSlashProgramHost.RecoverToMaximum(ProgramSkillFrame frame) => RecoverProgramToMaximum(frame);
    void global::CardGame.Core.INextSlashProgramHost.DrawRecoveryReceipt(ProgramSkillFrame frame) => DrawProgramRecoveryReceipt(frame);
    void global::CardGame.Core.INextSlashProgramHost.ReserveNextSlashDamage(ProgramSkillFrame frame) => ReserveProgramNextSlashDamage(frame);
    internal void RecoverProgramToMaximum(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || active.RecoveryReceipt is not null)
            throw new InvalidOperationException("Maximum recovery requires one fresh preparation receipt.");
        var target = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var recovered = target.IsAlive ? Math.Max(0, target.MaxHp - target.Hp) : 0;
        var recoveryId = 0L;
        if (recovered > 0 && _coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(active.Id, active.OwnerSeat, active.OwnerSeat, recovered, new(RecoveryAttemptProducer.ProgramMaximum, active.InstructionIndex)))
            return;
        if (recovered > 0)
        {
            recoveryId = _coreNativeRecoveryFramesOperations.BeginRecovery(active.Id, active.OwnerSeat, active.OwnerSeat, recovered);
            target.Hp += recovered;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(active.OwnerSeat, active.OwnerSeat, recovered, target.Hp));
            _coreRuntimeOperations.PopResolutionFrame(recoveryId, ResolutionFrameKind.Recovery);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { RecoveryReceipt = new(recoveryId, active.InstructionIndex, active.OwnerSeat, recovered) });
    }

    internal void DrawProgramRecoveryReceipt(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var receipt = active.RecoveryReceipt ?? throw new InvalidOperationException("Recovery draw requires the owner's completed recovery receipt.");
        if (receipt.Drawn || receipt.TargetSeat != active.OwnerSeat || receipt.InstructionIndex != active.InstructionIndex - 1)
            throw new InvalidOperationException("Recovery receipt must be drawn once by its next instruction.");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { RecoveryReceipt = receipt with { Drawn = true } });
        if (receipt.Recovered > 0 && _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
            _coreProgramLifecycleOperations.DrawProgramCards(active.Id, active.OwnerSeat, receipt.Recovered, null, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw);
    }

    internal void ReserveProgramNextSlashDamage(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { Window: SkillProgramTriggerWindow.SlashFullyDodged, CardUse: { } context } || _coreCardUseLifecycleOperations.LifecycleCardUse(context.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } use || action.ActorSeat != active.OwnerSeat || use.CardAttack is not { } attack || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } actualAttack || actualAttack.ResolutionId != use.Id || attack.SuccessfulDodgeResponses < attack.RequiredDodgeResponses)
            throw new InvalidOperationException("Next Slash damage requires the actual user's fully dodged Slash owner.");
        if (_coreSlashSuitDiscardOperations.IsSlashDodgeCancellationPrevented(actualAttack))
            return;
        var cancellation = new NamedSlashCancellation(active.SkillId, active.OwnerSeat, attack.TargetSeat);
        if (use.NamedSlashCancellations?.Contains(cancellation) == true)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { NamedSlashCancellations = Array.AsReadOnly((use.NamedSlashCancellations ?? []).Append(cancellation).ToArray()) });
        var index = _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.FindIndex(item => item.OwnerSeat == active.OwnerSeat && item.SkillId == active.SkillId && item.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && item.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        if (index < 0)
            _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.Add(new(active.OwnerSeat, active.SkillId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, 1));
        else
            _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves[index] = _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves[index] with
            {
                Amount = checked(_coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves[index].Amount + 1)
            };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NextSlashDamageReservedEvent(active.OwnerSeat, active.SkillId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, use.Id, attack.TargetSeat, 1));
    }

    internal void FreezeNextSlashDamage(long frameId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(frameId) ?? throw new InvalidOperationException("Next Slash damage lost its card owner.");
        if (use.NextSlashDamage is not null || use.Action is not { Type: CardActionType.Use } action || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind))
            return;
        var matching = _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.Where(item => item.OwnerSeat == action.ActorSeat && item.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && item.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat).ToArray();
        var amount = matching.Sum(item => item.Amount);
        // Preserve old nullable/canonical output when this registry has never issued this capability.
        if (amount == 0 && !HasNextSlashDamageCapability)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frameId, use with { NextSlashDamage = amount });
        if (amount == 0)
            return;
        foreach (var item in matching)
            _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.Remove(item);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NextSlashDamageConsumedEvent(action.ActorSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frameId, amount));
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 50)]
    internal int FrozenNextSlashDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack.EffectiveCardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash ? _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.NextSlashDamage ?? 0 : 0;
    internal void ExpireNextSlashDamage(int turnNumber, int turnSeat)
    {
        foreach (var item in _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.Where(item => item.TurnNumber == turnNumber && item.TurnSeat == turnSeat).ToArray())
        {
            _coreGameEngineNextSlashProgramsNamedNextSlashReservesAccess._namedNextSlashReserves.Remove(item);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NextSlashDamageExpiredEvent(item.OwnerSeat, item.SkillId, turnNumber, turnSeat, item.Amount));
        }
    }
}
