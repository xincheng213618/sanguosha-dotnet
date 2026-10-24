using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature : IMatchModeAllegiancePolicy
{
    private static readonly IReadOnlyList<string> DeclaredFactionChoices =
        Array.AsReadOnly(new[] { "wei", "shu", "wu", "qun", "jin" });

    public IReadOnlyList<string> NativeFactionChoices => Rules.Visibility.UsesHiddenFaction ? [] : DeclaredFactionChoices;
    public bool AllowsAvatarFactionChanges => !Rules.Visibility.UsesHiddenFaction;
    public bool AllowsLordSkillProjection => !Rules.Visibility.ShowsPublicTeams && !Rules.Visibility.UsesHiddenFaction;
    public bool AllowsIdentityConcealment => _mode.ModeKind == ContentModeKind.Identity;
    public bool AllowsPublicTeamSkillSynergy => _mode.ModeKind == ContentModeKind.Team;

    public bool IsPublicFactionVisible(ModeFactionVisibilityFacts facts) =>
        Rules.Visibility.UsesHiddenFaction ? facts.FactionRevealed : facts.GeneralRevealed;

    public Role? PublicIdentityDeathRole(ModeIdentityDeathFacts facts) =>
        _mode.ModeKind == ContentModeKind.Identity && !facts.IsAlive && facts.RoleRevealed &&
        facts.Role is Role.Loyalist or Role.Rebel or Role.Renegade ? facts.Role : null;

    // These established public-team queries do not imply identity friendship or
    // use the national-war compatibility TeamId as a faction relationship.
    public bool SamePublicTeam(ModeAllegianceSeatFacts first, ModeAllegianceSeatFacts second) =>
        first.TeamId is not null && first.TeamId == second.TeamId;

    public bool PublicTeamEnemy(ModeAllegianceSeatFacts owner, ModeAllegianceSeatFacts target) =>
        _mode.Id == WushuangMode.ModeId ? target.IsAlive && target.Seat != owner.Seat :
            owner.TeamId is { } team && target.TeamId is { } other && team != other;
}
