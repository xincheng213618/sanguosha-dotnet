using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool MatchesShanheBadaoDamageModifier(SkillProgramDamageModifier modifier, int sourceSeat, bool chain)
    {
        if (_modeDefinition.Id != ShanheBattleSetup.BattleModeId || _options.ShanheBattle is null ||
            _roundNumber <= 1 || chain || !IsValidPlayerSeat(sourceSeat)) return false;
        var damage = ShanhePreviousRoundDamageAtMostThree(sourceSeat);
        return modifier.Condition switch
        {
            SkillProgramDamageModifierCondition.SourcePreviousRoundNoDamage => damage == 0,
            SkillProgramDamageModifierCondition.SourcePreviousRoundDamageBelowThree => damage < 3,
            _ => false
        };
    }

    // Both rule thresholds are at most three. Read public completed damage
    // facts, including actual skill/reflected/chain damage, without a second
    // mutable ledger or a newly acquired tactic's private usage state.
    private int ShanhePreviousRoundDamageAtMostThree(int sourceSeat)
    {
        var previous = _roundNumber - 1;
        var round = 0;
        var total = 0;
        foreach (var fact in CompleteProgramEventHistory())
        {
            if (fact is RoundStartedEvent started) { round = started.RoundNumber; continue; }
            if (round != previous || fact is not DamageAppliedEvent { SourceLess: false } damage || damage.SourceSeat != sourceSeat)
                continue;
            // Core's applied-damage counters use Amount. Armor is the payment
            // of that same applied damage, rather than a smaller damage fact.
            var paid = Math.Max(0, damage.Amount);
            total += Math.Min(3, paid);
            if (total >= 3) return 3;
        }
        return total;
    }
}
