#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduExtraEffectsRuntime
{
    internal bool GuanduRepeatTargetsRemainLegal(CardUseFrame use)
    {
        if (use.Action is not { } action || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive || use.TargetSeats.Count == 0)
            return false;
        bool IsTrickTargetAllowed(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(use.SourceSeat, seat, use.CardKind, use.Id) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], use.CardKind, action.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(use.SourceSeat, seat, action.EffectiveSuit) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(use.SourceSeat, seat, use.CardKind);
        // Borrowed Sword designates weapon owners. Each following seat is that
        // owner's Slash victim, not another target of the original trick.
        if (use.CardKind == CardKind.BorrowedSword)
        {
            if (use.TargetSeats.Count % 2 != 0)
                return false;
            for (var i = 0; i < use.TargetSeats.Count; i += 2)
            {
                var owner = use.TargetSeats[i];
                var victim = use.TargetSeats[i + 1];
                if (!IsTrickTargetAllowed(owner) || owner == use.SourceSeat || _coreNativeBorrowedSwordOperations.GetWeapon(_coreMatchParticipantsPlayersAccess._players[owner])is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) || !_coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[victim]))
                    return false;
            }

            return true;
        }

        foreach (var seat in use.TargetSeats)
        {
            if (!IsTrickTargetAllowed(seat))
                return false;
            var target = _coreMatchParticipantsPlayersAccess._players[seat];
            var legal = use.CardKind switch
            {
                CardKind.Peach or CardKind.Zong => _coreTreatedWoundedOperations.IsRuleWounded(target),
                CardKind.Reinforcements => seat != use.SourceSeat && _coreTreatedWoundedOperations.IsRuleWounded(target),
                CardKind.Alcohol or CardKind.RealgarWine or CardKind.DrawTwo or CardKind.IronChain => true,
                CardKind.Intoxication => !_coreGuanduActivitiesOperations.GuanduAllies(use.SourceSeat, seat),
                CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
                CardKind.StoreGrain or CardKind.UnitedEffort or CardKind.PeachGarden or CardKind.FiveGrains or CardKind.BarbarianAssault or CardKind.ArrowBarrage => true,
                _ => _coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, target)};
            if (!legal)
                return false;
        }

        return true;
    }
}
