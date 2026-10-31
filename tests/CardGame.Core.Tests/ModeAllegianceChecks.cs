using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Modes.Standard;

internal static class ModeAllegianceChecks
{
    private const string ModeId = "identity:standard-5";

    public static void StandardPoliciesKeepPublicIdentityFactionAndDuelBoundaries()
    {
        var identity = Policy("identity:test", ContentModeKind.Identity);
        var national = Policy("national:test", ContentModeKind.NationalWarLite);
        var team = Policy("team:test", ContentModeKind.Team);
        var survival = Policy("survival:test", ContentModeKind.Survival);
        var duel = Policy(WushuangMode.ModeId, ContentModeKind.Team);
        Require(identity.NativeFactionChoices.SequenceEqual(["wei", "shu", "wu", "qun", "jin"]) &&
            identity.NativeFactionChoices is IList<string> { IsReadOnly: true } && national.NativeFactionChoices.Count == 0 &&
            identity.AllowsAvatarFactionChanges && !national.AllowsAvatarFactionChanges &&
            identity.AllowsIdentityConcealment && !team.AllowsIdentityConcealment && !national.AllowsIdentityConcealment &&
            identity.AllowsLordSkillProjection && survival.AllowsLordSkillProjection &&
            !team.AllowsLordSkillProjection && !national.AllowsLordSkillProjection,
            "Identity appearance, native faction choices and projected Lord sources retain their separate mode qualifications.");
        Require(!national.IsPublicFactionVisible(new(true, false)) && national.IsPublicFactionVisible(new(false, true)) &&
            identity.IsPublicFactionVisible(new(true, false)) && !identity.IsPublicFactionVisible(new(false, true)),
            "Public faction counts require the actual faction reveal in National War and the general reveal elsewhere.");
        Require(identity.PublicIdentityDeathRole(new(Role.Rebel, false, true)) == Role.Rebel &&
            identity.PublicIdentityDeathRole(new(Role.Lord, false, true)) is null &&
            identity.PublicIdentityDeathRole(new(Role.Rebel, true, true)) is null &&
            identity.PublicIdentityDeathRole(new(Role.Rebel, false, false)) is null &&
            national.PublicIdentityDeathRole(new(Role.Rebel, false, true)) is null &&
            team.PublicIdentityDeathRole(new(Role.Rebel, false, true)) is null,
            "A compatibility role or an unrevealed/living identity cannot enter the public dead-role payment mask.");
        var owner = new ModeAllegianceSeatFacts(0, true, "team:a");
        var other = new ModeAllegianceSeatFacts(1, true, "team:b");
        Require(team.PublicTeamEnemy(owner, other) && !team.SamePublicTeam(owner, other) &&
            team.SamePublicTeam(owner, other with { TeamId = "team:a" }) &&
            !team.SamePublicTeam(owner with { TeamId = null }, other with { TeamId = null }) &&
            duel.PublicTeamEnemy(owner, other with { TeamId = "team:a" }) &&
            !duel.PublicTeamEnemy(owner, other with { IsAlive = false }) &&
            !duel.PublicTeamEnemy(owner, owner) && team.AllowsPublicTeamSkillSynergy && !identity.AllowsPublicTeamSkillSynergy,
            "Team equality remains distinct from the duel's living-opponent relation and public-team-only skill synergy.");
    }

    private static IMatchModeAllegiancePolicy Policy(string id, ContentModeKind kind)
    {
        var mode = new ContentModeDefinition(id, id, 5, 5, new Dictionary<string, int>(), ModeKind: kind);
        var hosts = new ModeHostRegistry().Register<IModeSetupRandom>(() => new NoRandom());
        return (IMatchModeAllegiancePolicy)new StandardModeProfileFactory().Create(new ModeFeatureCreationContext(
            new GameOptions { PlayerCount = 5 }, mode, ContentRegistry.Build(), hosts));
    }
    private sealed class NoRandom : IModeSetupRandom
    {
        public int Next(int exclusiveMax) => throw new InvalidOperationException("An allegiance policy cannot consume gameplay random during construction.");
        public void Shuffle<T>(IList<T> values) => throw new InvalidOperationException("An allegiance policy cannot shuffle during construction.");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
