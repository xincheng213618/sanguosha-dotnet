using System.Text.Json;
using CardGame.Core;

internal static class CharacterSkillSetChecks
{
    public static void ObservationsPreservePaidBaselineAndConsumeChangesOnce()
    {
        var owner = Character(new GeneralDefinition("fixture:owner", "Owner", "", []));
        var observations = new SkillGrantRevisionStore();
        var otherMatch = new SkillGrantRevisionStore();
        Require(!observations.Observe(owner), "The first observation establishes a baseline without inventing a change.");
        observations.Capture([owner]);
        owner.SkillGrants.Grant(new SkillGrant("paid", "fixture:skill", "instance", "source"));
        observations.Capture([owner]);
        Require(observations.Observe(owner) && !observations.Observe(owner),
            "Repeated capture during a paid child retains the pre-change revision; its change is consumed exactly once.");
        Require(!otherMatch.Observe(owner), "A different match has its own first-observation baseline.");
        owner.SkillGrants.RemoveGrant("paid");
        Require(observations.Observe(owner) && otherMatch.Observe(owner),
            "Each match consumes a genuine later grant revision independently.");
        owner.IsAlive = false;
        observations.Capture([owner]);
        owner.SkillGrants.Grant(new SkillGrant("dead", "fixture:skill", "instance:dead", "source"));
        observations.Capture([owner]);
        Require(observations.Observe(owner), "Capture includes the original dead participants without replacing an existing revision.");
    }

    public static void GrantsRetainSourcesAndStableOwnership()
    {
        var skills = new CharacterSkillSet();
        var first = new SkillGrant("default", "fixture:skill", "instance:default", "template:primary");
        var second = new SkillGrant("equipment", "fixture:skill", "instance:equipment", "equipment:one");
        Require(skills.Grant(first) && skills.Grant(second) && skills.Revision == 2 &&
                skills.EffectiveSkillIds.SequenceEqual(["fixture:skill"]),
            "Two sources must retain separate grants and one effective skill identity.");
        var frozen = skills.Grants;
        Require(!skills.Grant(second) && skills.Revision == 2, "Identical grant retries must be idempotent.");
        Require(skills.RemoveGrant("default") && skills.EffectiveSkillIds.SequenceEqual(["fixture:skill"]) &&
                frozen.Count == 2 && skills.Revision == 3,
            "Removing one source must retain the other and must not mutate old snapshots.");
        Require(skills.SetEnabled("equipment", false) && skills.EffectiveSkillIds.Count == 0 &&
                !skills.SetEnabled("equipment", false) && skills.Revision == 4,
            "Disabled grants must remain owned without repeatedly invalidating the revision.");
        Require(skills.SetEnabled("equipment", true) && skills.Grants.Single() == second &&
                skills.Revision == 5, "Re-enabling must preserve source and instance identity.");
        Require(!skills.RemoveGrant("missing") && skills.Revision == 5,
            "Removing a missing grant must not change state.");
        Require(new CharacterSkillSet().EffectiveSkillIds.Count == 0,
            "Character skill sets must not share mutable registration state.");
    }

    public static void InvalidGrantsAndConflictsLeaveStateUnchanged()
    {
        var skills = new CharacterSkillSet();
        var grant = new SkillGrant("grant", "fixture:skill", "instance", "source");
        skills.Grant(grant);
        Throws<InvalidOperationException>(() => skills.Grant(grant with { SkillId = "fixture:other" }));
        Throws<ArgumentException>(() => skills.Grant(grant with { GrantId = "new", SkillId = "invalid" }));
        Throws<ArgumentException>(() => skills.Grant(grant with { GrantId = "new", SourceId = " " }));
        Throws<KeyNotFoundException>(() => skills.SetEnabled("unknown", false));
        Require(skills.Revision == 1 && skills.Grants.SequenceEqual([grant]),
            "Rejected grant mutations must preserve the complete previous state.");
        Throws<NotSupportedException>(() => ((ICollection<SkillGrant>)skills.Grants).Clear());
        skills.Grant(new SkillGrant("a", "fixture:alpha", "another", "source"));
        Require(skills.Grants[0].GrantId == "a" &&
                skills.EffectiveSkillIds.SequenceEqual(["fixture:alpha", "fixture:skill"]),
            "Grant snapshots and effective skill identities must have deterministic ordering.");
    }

    public static void AvailabilitySeparatesPossessionFromEffectiveSources()
    {
        var definitions = new Dictionary<string, ContentSkillDefinition>
        {
            ["fixture:plain"] = new("fixture:plain", "Plain", ""),
            ["fixture:locked"] = new("fixture:locked", "Locked", "") { Tags = SkillTag.Locked },
            ["fixture:mode"] = new("fixture:mode", "Mode", "")
        };
        var owner = Character(new GeneralDefinition("fixture:owner", "Owner", "", []));
        owner.SkillGrants.Grant(new("printed", "fixture:plain", "printed-instance", CharacterState.PrimarySkillSource));
        owner.SkillGrants.Grant(new("equipment", "fixture:plain", "equipment-instance", "equipment:fixture"));
        owner.SkillGrants.Grant(new("locked", "fixture:locked", "locked-instance", "fixture:grant"));
        owner.SkillGrants.Grant(new("mode", "fixture:mode", "mode-instance", "mode:fixture") { IsModeRule = true });
        var index = new MatchSkillBindingIndex(id => definitions[id], isNationalWarMode: true);
        var original = index.GetShard(owner);
        var observations = new SkillGrantRevisionStore();
        observations.Capture([owner]);
        var grants = owner.SkillGrants.Grants;
        var grantRevision = owner.SkillGrants.GrantRevision;
        var revision = owner.SkillGrants.Revision;
        owner.SkillGrants.SetRestriction(Restriction("suppress", SkillStateAction.Suppress, SkillStateMatch.NonLockedCharacters, owner.Seat));
        var suppressed = index.GetShard(owner);
        Require(owner.SkillGrants.Grants.SequenceEqual(grants) && owner.SkillGrants.GrantRevision == grantRevision &&
                owner.SkillGrants.Revision > revision && !observations.Observe(owner) && !ReferenceEquals(original, suppressed),
            "Restrictions occupy a separate table, refresh availability, and do not invent a real skill-grant observation.");
        Require(suppressed.HasOwnedInstance("fixture:plain", "printed-instance") &&
                !suppressed.HasInstance("fixture:plain", "printed-instance") &&
                suppressed.HasInstance("fixture:plain", "equipment-instance") &&
                suppressed.HasInstance("fixture:locked", "locked-instance") && suppressed.HasInstance("fixture:mode", "mode-instance"),
            "Suppression preserves possession, protects excluded sources, and never substitutes a different instance.");
        owner.SkillGrants.SetRestriction(Restriction("lose", SkillStateAction.Lose,
            new(["fixture:plain"], Expression("[\"==\",[\"number\",\"skill.character\"],1]")), owner.Seat));
        Require(!index.GetShard(owner).HasOwnedInstance("fixture:plain", "printed-instance") &&
                index.GetShard(owner).HasOwnedInstance("fixture:plain", "equipment-instance"),
            "Temporary loss removes possession for its matching source scope while equipment retains its independent grant.");
        owner.SkillGrants.SetRestriction(Restriction("equipment-suppress", SkillStateAction.Suppress,
            new(["fixture:plain"]), owner.Seat));
        Require(index.GetShard(owner).HasOwnedSkill("fixture:plain") && !index.GetShard(owner).HasSkill("fixture:plain"),
            "An explicitly inclusive restriction can suppress equipment without losing that source's possession.");
        owner.SkillGrants.RemoveRestriction("suppress");
        owner.SkillGrants.RemoveRestriction("lose");
        owner.SkillGrants.RemoveRestriction("equipment-suppress");
        Require(owner.SkillGrants.GrantRevision == grantRevision && !observations.Observe(owner) &&
                owner.SkillGrants.Grants.SequenceEqual(grants) && index.GetShard(owner).HasSkill("fixture:plain"),
            "Removing restrictions restores cached availability without changing real grants or producing a SkillsChanged observation.");
        owner.GeneralRevealed = false;
        Require(index.GetShard(owner).HasOwnedInstance("fixture:plain", "printed-instance") &&
                !index.GetShard(owner).HasInstance("fixture:plain", "printed-instance"),
            "An unrevealed selected national-war general retains possession without making its printed skill effective.");
        owner.GeneralRevealed = true;
        Require(index.GetShard(owner).HasInstance("fixture:plain", "printed-instance") &&
                original.HasInstance("fixture:plain", "printed-instance") && suppressed.OwnedGrants.Count == 4,
            "Expiry and reveal restore the same instance while earlier availability snapshots remain frozen.");
        owner.SkillGrants.SetRestriction(Restriction("empty", SkillStateAction.Suppress, SkillStateMatch.All.Freeze([]), owner.Seat));
        Require(index.GetShard(owner).HasInstance("fixture:plain", "printed-instance"), "An explicitly empty skill selection must not become an all-skills restriction.");
        owner.SkillGrants.RemoveRestriction("empty");
    }

    private static SkillStateRestriction Restriction(string id, SkillStateAction action, SkillStateMatch match, int seat) =>
        new(id, "fixture:restriction", action, match, new(seat, seat),
            new(new SkillStateOrigin(1, 0, new("fixture:plain", "audit", seat, "printed-instance")),
                [new(SkillStateBoundaryEvent.TurnCleanup, seat, SkillStateBoundaryTiming.Before, 1, false)]));

    private static ProgramExpression Expression(string json)
    { using var document = JsonDocument.Parse(json); return ProgramExpressions.Parse(document.RootElement, "fixture"); }

    private static CharacterState Character(GeneralDefinition template) => new()
    {
        Seat = 0, Name = "Fixture", IsHuman = true, Role = Role.Rebel,
        RoleRevealed = true, General = template, GeneralSelected = true,
        GeneralRevealed = true, MaxHp = 4, Hp = 4
    };

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Throws<TException>(Action action) where TException : Exception
    {
        try { action(); }
        catch (TException) { return; }
        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }
}
