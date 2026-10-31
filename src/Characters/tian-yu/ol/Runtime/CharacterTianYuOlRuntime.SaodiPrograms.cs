#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuOlRuntime
{
    internal SkillProgramStepOutcome SaodiProgramExpandTargets(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || active.WindowContext?.CardUse is null)
            return SkillProgramStepOutcome.Continue;
        var use = _coreCurrentCardEnhancementsOperations.EnhancementCardUse(active);
        if (use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != active.OwnerSeat || action.EffectiveDesignatedTargetSeats is not [var anchor] || anchor == active.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(anchor) || !_coreMatchParticipantsPlayersAccess._players[anchor].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var additions = SaodiBetweenSeats(active.OwnerSeat, anchor).Where(seat => CanSaodiAddTarget(use, seat)).ToArray();
        if (additions.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var targets = Array.AsReadOnly(use.TargetSeats.Concat(additions).ToArray());
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, old => old with { TargetSeats = targets, Action = _coreProgramCardUseRolesOperations.CloneRoleAction(action, action.ActorSeat, targets), TargetsAdjusted = old.TargetsAdjusted || old.CardKind == CardKind.BorrowedSword });
        _coreTieredRoundActualTargetWindowsOperations.SyncIssuedTieredRoundZeroTrickTargetWindows(use.Id, action);
        var bindingId = global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active);
        foreach (var seat in additions)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSaodiTargetsAddedEvent(active.Id, active.SkillId, bindingId, active.SkillInstanceId, active.OwnerSeat, use.Id, action.ActionId, seat));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，" + string.Join("、", additions.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)) + "也成为此牌的目标。", active.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }

    internal int[] SaodiBetweenSeats(int ownerSeat, int targetSeat) => ((SaodiSeatingRuntime)_closedSaodiSeatingRuntimeSource.Get()).BetweenSeats(ownerSeat, targetSeat);
    internal bool CanSaodiAddTarget(CardUseFrame use, int targetSeat)
    {
        if (use.Action is not { Type: CardActionType.Use } action || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || action.EffectiveDesignatedTargetSeats.Contains(targetSeat))
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[action.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, targetSeat, use.CardKind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, use.CardKind, action.EffectiveSuit ?? Suit.None, global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, targetSeat, action.EffectiveSuit))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, target, new Card(-1, use.CardKind, action.EffectiveSuit ?? Suit.None, action.EffectiveRank ?? 0), effectiveKind: use.CardKind, ignoreDistance: true, existingUseFrameId: use.Id, specificEffectiveRank: action.EffectiveRank);
        return use.CardKind switch
        {
            CardKind.IronChain => true,
            CardKind.BorrowedSword => _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Any(card => _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot == EquipmentSlot.Weapon) && _coreMatchParticipantsPlayersAccess._players.Any(victim => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(target, victim)),
            CardKind.Duel => targetSeat != actor.Seat,
            CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            CardKind.Dismantlement or CardKind.Snatch => targetSeat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(target).Count > 0,
            _ => false
        };
    }
}
