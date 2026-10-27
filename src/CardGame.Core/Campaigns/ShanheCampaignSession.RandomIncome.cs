namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private int RandomizeBattleIncome(RunState state, int income)
    {
        // Both source rows replace the income range. Locally the wider upper
        // tier takes precedence when both are owned, rather than multiplying
        // two ranges into an undocumented third range.
        var range = OwnedEconomy(state)
            .Where(effect => effect.BattleGoldRandomMinPercent != 100 || effect.BattleGoldRandomMaxPercent != 100)
            .OrderByDescending(effect => effect.BattleGoldRandomMaxPercent)
            .ThenBy(effect => effect.BattleGoldRandomMinPercent).FirstOrDefault();
        if (range is null || income == 0) return income;
        // One integer percentage in the closed interval, isolated from stock,
        // offer and encounter sampling. Reading a quote consumes no randomness.
        var random = new DeterministicRandom(ShanheBattleSetup.DeriveSeed(_options.Seed,
            $"{state.CurrentNodeId}:battle-income", state.EncounterOrdinal));
        var percent = range.BattleGoldRandomMinPercent +
            random.Next(range.BattleGoldRandomMaxPercent - range.BattleGoldRandomMinPercent + 1);
        return checked((int)((long)income * percent / 100));
    }
}
