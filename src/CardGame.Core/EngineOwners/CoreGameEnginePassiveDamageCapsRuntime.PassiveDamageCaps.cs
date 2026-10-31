#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePassiveDamageCapsRuntime
{
    internal (CardUseEffectSource Source, int Cap)? GetPassiveProgramDamageCap(GameEngine.IDamageAttempt attack)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive)
            return null;
        var sourceSeat = !attack.IsSourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) ? attack.SourceSeat : (int? )null;
        var programCap = FindPassiveProgramDamageCap(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], sourceSeat);
        var activityCap = _coreTeamActivitiesOperations.JiangeDamageCap(attack);
        return activityCap is { } activity && (programCap is null || activity.Cap <= programCap.Value.Cap) ? activity : programCap;
    }

    internal (CardUseEffectSource Source, int Cap)? FindPassiveProgramDamageCap(CharacterState target, int? sourceSeat, IReadOnlySet<string>? visibleSkillIds = null)
    {
        if (!target.IsAlive)
            return null;
        (CardUseEffectSource Source, int Cap)? result = null;
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(target).ProgramInstances)
        {
            if (visibleSkillIds is not null && !visibleSkillIds.Contains(instance.SkillId))
                continue;
            foreach (var modifier in instance.Program.DamageModifiers)
            {
                if (!modifier.CapsToAmount || modifier.SourceScope != SkillProgramDamageModifierSourceScope.DamageParticipant || modifier.CardKinds.Count != 0)
                    continue;
                var eligible = modifier.Condition switch
                {
                    SkillProgramDamageModifierCondition.Always => true,
                    SkillProgramDamageModifierCondition.DamageSourceAttackRangeBelowThree => sourceSeat is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreNativeRuleQueriesOperations.GetAttackRange(seat) < 3,
                    _ => false
                };
                if (!eligible)
                    continue;
                if (result is null || modifier.Amount < result.Value.Cap)
                    result = (new CardUseEffectSource(instance.SkillId, modifier.Id, target.Seat, instance.SkillInstanceId), modifier.Amount);
            }
        }

        return result;
    }

    internal int? GetIncomingFixedDamageCap(CharacterState target, IReadOnlySet<string>? visibleSkillIds = null) => FindPassiveProgramDamageCap(target, null, visibleSkillIds)?.Cap;
    internal bool HasNextRecipientPassiveDamageCap(GameEngine.IDamageAttempt attack)
    {
        var state = attack is NativeOwners.ProgramAttackHandle ? (_coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats, _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex) : (_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats, _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex);
        var next = (state.ChainedTargetSeats ?? []).Skip(state.ChainedTargetIndex).FirstOrDefault(seat => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, -1);
        return next >= 0 && GetIncomingFixedDamageCap(_coreMatchParticipantsPlayersAccess._players[next])is not null;
    }
}
