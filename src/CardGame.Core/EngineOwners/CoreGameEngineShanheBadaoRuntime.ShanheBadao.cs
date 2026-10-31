#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheBadaoRuntime
{
    internal bool MatchesShanheBadaoDamageModifier(SkillProgramDamageModifier modifier, int sourceSeat, bool chain)
    {
        if (_coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || _coreActualRoundProgressRoundNumberAccess._roundNumber <= 1 || chain || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat))
            return false;
        var damage = ShanhePreviousRoundDamageAtMostThree(sourceSeat);
        return modifier.Condition switch
        {
            SkillProgramDamageModifierCondition.SourcePreviousRoundNoDamage => damage == 0,
            SkillProgramDamageModifierCondition.SourcePreviousRoundDamageBelowThree => damage < 3,
            _ => false
        };
    }

    internal int ShanhePreviousRoundDamageAtMostThree(int sourceSeat)
    {
        var previous = _coreActualRoundProgressRoundNumberAccess._roundNumber - 1;
        var round = 0;
        var total = 0;
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (fact is RoundStartedEvent started)
            {
                round = started.RoundNumber;
                continue;
            }

            if (round != previous || fact is not DamageAppliedEvent { SourceLess: false } damage || damage.SourceSeat != sourceSeat)
                continue;
            // Core's applied-damage counters use Amount. Armor is the payment
            // of that same applied damage, rather than a smaller damage fact.
            var paid = Math.Max(0, damage.Amount);
            total += Math.Min(3, paid);
            if (total >= 3)
                return 3;
        }

        return total;
    }
}
