namespace CardGame.Core;

/// <summary>Mode-owned visibility choices; Core still applies viewer permissions and freezes the final snapshot.</summary>
public sealed record ModeVisibilityRules(bool ShowsPublicTeams = false, bool ConcealsRoleCompletely = false,
    bool UsesHiddenFaction = false, bool PublishesActivityFaction = false, bool UsesSecondaryGeneral = false,
    bool AllowsGeneralReveal = false, bool UsesSurvivalRanking = false, bool PublishesIdentityComposition = false);
