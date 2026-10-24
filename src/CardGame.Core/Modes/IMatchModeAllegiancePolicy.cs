namespace CardGame.Core;

/// <summary>Native scalar facts; no hidden faction identity or mutable player crosses the policy boundary.</summary>
public readonly record struct ModeFactionVisibilityFacts(bool GeneralRevealed, bool FactionRevealed);
public readonly record struct ModeIdentityDeathFacts(Role Role, bool IsAlive, bool RoleRevealed);
public readonly record struct ModeAllegianceSeatFacts(int Seat, bool IsAlive, string? TeamId);

/// <summary>
/// Defines how this match interprets identity, public teams and declared allegiance.
/// Native code still proves the exact skill grant, hidden slot, owner and paid frame.
/// </summary>
public interface IMatchModeAllegiancePolicy : IGameModeFeature
{
    IReadOnlyList<string> NativeFactionChoices { get; }
    bool AllowsAvatarFactionChanges { get; }
    bool AllowsLordSkillProjection { get; }
    bool AllowsIdentityConcealment { get; }
    bool AllowsPublicTeamSkillSynergy { get; }
    bool IsPublicFactionVisible(ModeFactionVisibilityFacts facts);
    Role? PublicIdentityDeathRole(ModeIdentityDeathFacts facts);
    bool SamePublicTeam(ModeAllegianceSeatFacts first, ModeAllegianceSeatFacts second);
    bool PublicTeamEnemy(ModeAllegianceSeatFacts owner, ModeAllegianceSeatFacts target);
}
