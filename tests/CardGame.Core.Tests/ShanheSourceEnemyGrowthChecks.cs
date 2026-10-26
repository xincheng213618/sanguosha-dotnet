using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

internal static class ShanheSourceEnemyGrowthChecks
{
    public static void ExactSourceCompositionRoundingAndSeedOrder()
    {
        var calculator = StandardShanheSourceEnemyGrowth.Season5;
        var basis = new ShanheSourceEnemyAttributes(Hp: 4, Draw: 2, StartingCards: 4);
        var one = calculator.Calculate(basis, new(25, 20, 3, 1, ShanheSourceEncounterCategory.Boss));
        Need(one.Attributes == new ShanheSourceEnemyAttributes(Hp: 11, Draw: 10, ExtraSlash: 4, ArmorGain: 2,
                StartingCards: 15, Armor: 9, ExtraDamage: 2) &&
            one.Steps.Single(step => step.Attribute == ShanheSourceEnemyAttribute.Hp) is
                { Base: 4, DayGrowth: 3, DifficultyGrowth: 2, ChapterGrowth: 2, EnemyCountPercentage: 100, RoundedGrowth: 7 },
            "Source day, small-tier and chapter growth compose without treating hprate/drawrate as an additional percentage.");
        var three = calculator.Calculate(basis, new(25, 20, 3, 3, ShanheSourceEncounterCategory.Boss),
            [new(3, new(StartingCards: 1)), new(4, new(StartingCards: 1))],
            [new(1, 1, 4, true, 3), new(2, 1, 5, false, 100)]);
        Need(three.Attributes == new ShanheSourceEnemyAttributes(Hp: 7, Draw: 5, ExtraSlash: 1, ArmorGain: 1,
                StartingCards: 7, Armor: 3, ExtraDamage: 2) &&
            three.Steps.Single(step => step.Attribute == ShanheSourceEnemyAttribute.ArmorGain) is
                { ScaledGrowth: 0.8, RoundedGrowth: 1 } &&
            three.Steps.Single(step => step.Attribute == ShanheSourceEnemyAttribute.StartingCards) is
                { RoundedGrowth: 4, GeneralTierGrowth: 2, BeforeSeed: 10, SeedGrowth: -3 },
            "Count scaling rounds a positive fraction below one upward, floors larger additions, then adds cumulative General attributes and only matching seeds.");
        var absentDay = calculator.Calculate(basis, new(25, 31, 3, 1, ShanheSourceEncounterCategory.Boss));
        Need(absentDay.Attributes.Hp == 8 && absentDay.Steps.All(step => step.DayGrowth == 0) &&
            calculator.Calculate(basis, new(11, 0, 3, 1, ShanheSourceEncounterCategory.Ordinary)).Attributes.ArmorGain == 1 &&
            calculator.AppearanceRows.Count == 0 && calculator.DayRows.Count == 806 && calculator.ChapterRows.Count == 15 && calculator.CountRows.Count == 21,
            "Exact source lookup does not stretch day thirty into later days; the hard-tier armor reply comes from the chapter table, while the absent EnemyShow table stays empty.");
        var friend = calculator.Calculate(basis, new(25, 20, 3, 3, ShanheSourceEncounterCategory.Boss, IsFriend: true),
            [new(3, new(StartingCards: 1)), new(4, new(StartingCards: 1))],
            [new(3, 1, 1, false, 100), new(10, 1, 2, true, 1)]);
        Need(friend.Attributes == new ShanheSourceEnemyAttributes(Hp: 4, Draw: 2, StartingCards: 6, Armor: -1) &&
            friend.Steps.All(step => step.RoundedGrowth == 0),
            "Friend preview skips enemy growth, retains General attributes, and matches the client's seed-after-clamp order without inventing a second clamp.");
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
