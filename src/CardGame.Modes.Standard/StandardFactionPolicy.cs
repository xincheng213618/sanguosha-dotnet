using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature : IMatchModeFactionPolicy
{
    public string? EffectiveFaction(ModeFactionFacts facts) => _mode.DragonBoatActivity is not null
        ? facts.TeamId?[5..]
        : Rules.Visibility.UsesHiddenFaction ? facts.NationalFactionId
        : facts.NativeOverride ?? facts.LibraryFaction ?? facts.ChosenFaction ??
            ScenarioGodFaction(facts.PrintedFaction) ?? facts.PrintedFaction;

    public string? ScenarioGodFaction(string? printedFaction) =>
        string.Equals(printedFaction, "god", StringComparison.Ordinal) ? _options.ShanheBattle?.DefaultGodFactionId : null;
}
