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
        RawSourceAdmissionPreservesSelectionVisibilityAndQualificationStages();
        PrintedLordQualificationsUseExactGrantsForSetupAndPrograms();
        ContentAdmissionLimitsEffectiveSourcesWithoutChangingPossession();
    }

    private static void ContentAdmissionLimitsEffectiveSourcesWithoutChangingPossession()
    {
        const string allowed = "fixture:allowed-content", denied = "fixture:denied-content";
        var definitions = new Dictionary<string, ContentSkillDefinition>
        { [allowed] = new(allowed, "Allowed", ""), [denied] = new(denied, "Denied", "") };
        var owner = Character(new GeneralDefinition("fixture:content-owner", "Owner", "", []));
        owner.SkillGrants.Grant(new("allowed", allowed, "allowed-instance", "acquired:fixture"));
        owner.SkillGrants.Grant(new("denied", denied, "denied-instance", "acquired:fixture"));
        owner.SkillGrants.Grant(new("mode-denied", denied, "mode-instance", "mode:fixture") { IsModeRule = true });
        var ids = new HashSet<string>(StringComparer.Ordinal) { allowed };
        var restricted = new MatchSkillBindingIndex(id => definitions[id], false, allowedEffectiveSkillIds: ids);
        var answer = restricted.GetShard(owner);
        Require(answer.OwnedGrants.Count == 3 && answer.HasOwnedInstance(denied, "denied-instance") &&
            answer.HasOwnedInstance(denied, "mode-instance") && answer.HasInstance(allowed, "allowed-instance") &&
            !answer.HasInstance(denied, "denied-instance") && !answer.HasInstance(denied, "mode-instance"),
            "Immutable content admission affects effectiveness, retains actual possession and applies equally to ordinary and mode-rule sources.");
        ids.Add(denied);
        restricted.Invalidate(owner.Seat);
        Require(!restricted.GetShard(owner).HasSkill(denied) && new MatchSkillBindingIndex(id => definitions[id], false)
            .GetShard(owner).HasInstance(denied, "mode-instance"),
            "The admission set is frozen for the match, while an unrestricted mode retains both sources' ordinary effectiveness.");
    }

    private static void RawSourceAdmissionPreservesSelectionVisibilityAndQualificationStages()
    {
        const string plain = "fixture:raw-source", lord = "fixture:raw-lord";
        var definitions = new Dictionary<string, ContentSkillDefinition>
        { [plain] = new(plain, "Source", ""), [lord] = new(lord, "Lord source", "") { Tags = SkillTag.Lord } };
        var owner = Character(new GeneralDefinition("fixture:raw-owner", "Owner", "", []));
        var primary = new SkillGrant("primary", plain, "primary-instance", CharacterState.PrimarySkillSource);
        owner.SkillGrants.Grant(primary);
        var hidden = new SkillAvailability(id => definitions[id], true, false, null);
        foreach (var selected in new[] { false, true })
        foreach (var revealed in new[] { false, true })
        {
            owner.GeneralSelected = selected; owner.GeneralRevealed = revealed;
            var answer = hidden.Query(owner);
            Require(hidden.IsRawSourceAdmitted(owner, primary) == (selected && revealed) &&
                    answer.HasOwnedInstance(plain, primary.SkillInstanceId) == selected &&
                    answer.HasEffectiveInstance(plain, primary.SkillInstanceId) == (selected && revealed),
                "Hidden primary origins require actual selection and reveal for admission, while possession remains independent of reveal.");
        }
        owner.SecondaryGeneral = new GeneralDefinition("fixture:raw-secondary", "Secondary", "", []);
        var secondary = primary with { GrantId = "secondary", SkillInstanceId = "secondary-instance", SourceId = CharacterState.SecondarySkillSource };
        foreach (var (nationalWar, dualGenerals) in new[] { (false, false), (true, false), (false, true) })
        {
            var availability = new SkillAvailability(id => definitions[id], nationalWar, dualGenerals, null);
            foreach (var selected in new[] { false, true })
            foreach (var revealed in new[] { false, true })
            {
                owner.SecondaryGeneralSelected = selected; owner.SecondaryGeneralRevealed = revealed;
                Require(availability.IsRawSourceAdmitted(owner, secondary) == ((nationalWar || dualGenerals) && selected && revealed),
                    "Secondary origins use the same selection/reveal rule and require a mode with a second general.");
            }
        }
        var ordinary = new SkillAvailability(id => definitions[id], false, false, null);
        owner.GeneralSelected = false; owner.GeneralRevealed = false;
        Require(ordinary.IsRawSourceAdmitted(owner, primary) && !ordinary.IsRawSourceAdmitted(owner, primary with { IsEnabled = false }),
            "Ordinary primary origins retain their established pre-reveal admission, while disabled grants never qualify.");
        owner.IsAlive = false;
        var independent = primary with { SourceId = "equipment:raw-source" };
        var raw = new SkillAvailability(_ => throw new InvalidOperationException("Raw admission resolved a definition."), true, false,
            (_, _) => throw new InvalidOperationException("Raw admission entered derived qualification."));
        Require(raw.IsRawSourceAdmitted(owner, independent) && raw.IsRawSourceAdmitted(owner, independent with { SourceId = "acquired:raw-source" }),
            "Raw origin admission reads no final availability or derived relation; source-owner survival remains a caller rule.");
        owner.IsAlive = true;
        var printedLord = primary with { GrantId = "lord", SkillId = lord, SkillInstanceId = "lord-instance" };
        owner.SkillGrants.Grant(printedLord);
        Require(ordinary.IsRawSourceAdmitted(owner, printedLord) && !ordinary.Query(owner).HasOwnedInstance(lord, printedLord.SkillInstanceId),
            "Raw admission does not replace the existing printed-lord ownership decision or grant its rights to a non-lord.");
        IdentityConcealmentPreservesPossessionAndIndependentEffectiveLimits();
    }

    private static void IdentityConcealmentPreservesPossessionAndIndependentEffectiveLimits()
    {
        const string skill = "fixture:concealed-source", mode = "fixture:visible-mode", deniedMode = "fixture:denied-mode";
        var definitions = new Dictionary<string, ContentSkillDefinition>
        { [skill] = new(skill, "Source", ""), [mode] = new(mode, "Mode", ""), [deniedMode] = new(deniedMode, "Denied mode", "") };
        var owner = Character(new GeneralDefinition("fixture:concealed-owner", "Owner", "", []));
        var printed = new SkillGrant("printed", skill, "printed-instance", CharacterState.PrimarySkillSource);
        var equipment = new SkillGrant("equipment", skill, "equipment-instance", "equipment:fixture");
        var acquired = new SkillGrant("acquired", skill, "acquired-instance", "acquired:fixture");
        var visible = new SkillGrant("visible-mode", mode, "mode-instance", "mode:visible") { IsModeRule = true };
        var denied = new SkillGrant("denied-mode", deniedMode, "denied-mode-instance", "mode:denied") { IsModeRule = true };
        foreach (var grant in new[] { printed, equipment, acquired, visible, denied }) owner.SkillGrants.Grant(grant);
        IReadOnlySet<string> allowed = new HashSet<string>(StringComparer.Ordinal) { skill, mode };
        var index = new MatchSkillBindingIndex(id => definitions[id], false, allowedEffectiveSkillIds: allowed);
        var before = index.GetShard(owner);
        var grants = owner.SkillGrants.Grants;
        var revision = owner.SkillGrants.GrantRevision;
        Require(before.HasInstance(skill, printed.SkillInstanceId) && before.HasInstance(mode, visible.SkillInstanceId) &&
                before.HasOwnedInstance(deniedMode, denied.SkillInstanceId) && !before.HasInstance(deniedMode, denied.SkillInstanceId),
            "An independent effective allow-set preserves possession and also limits real mode-rule grants.");
        owner.IdentityConcealment = new(owner.General.Id, owner.Hp);
        var concealed = index.GetShard(owner);
        Require(!ReferenceEquals(before, concealed) && concealed.OwnedGrants.SequenceEqual(before.OwnedGrants) &&
                new[] { printed, equipment, acquired }.All(grant => concealed.HasOwnedInstance(skill, grant.SkillInstanceId) &&
                    !concealed.HasInstance(skill, grant.SkillInstanceId)) &&
                concealed.HasInstance(mode, visible.SkillInstanceId) && !concealed.HasInstance(deniedMode, denied.SkillInstanceId),
            "Identity concealment disables printed, equipment and acquired effects without losing ownership; mode grants bypass concealment but not the effective allow-set.");
        var raw = new SkillAvailability(_ => throw new InvalidOperationException("Raw concealment resolved a definition."), false, false,
            (_, _) => throw new InvalidOperationException("Raw concealment entered derived qualification."), allowedEffectiveSkillIds: allowed);
        Require(new[] { printed, equipment, acquired }.All(grant => !raw.IsRawSourceAdmitted(owner, grant)) &&
                raw.IsRawSourceAdmitted(owner, visible),
            "Raw concealment and the real mode exemption read no skill definition or derived qualification callback.");
        owner.IdentityConcealment = null;
        var restored = index.GetShard(owner);
        Require(!ReferenceEquals(concealed, restored) &&
                new[] { printed, equipment, acquired }.All(grant => restored.HasInstance(skill, grant.SkillInstanceId)) &&
                restored.HasInstance(mode, visible.SkillInstanceId) && !restored.HasInstance(deniedMode, denied.SkillInstanceId) &&
                owner.SkillGrants.GrantRevision == revision && owner.SkillGrants.Grants.SequenceEqual(grants) &&
                !concealed.HasInstance(skill, printed.SkillInstanceId) && before.HasInstance(skill, printed.SkillInstanceId),
            "Clearing concealment refreshes cached availability with the same grant instances, retains the independent effective limit, and leaves old snapshots frozen.");
    }

    private static void PrintedLordQualificationsUseExactGrantsForSetupAndPrograms()
    {
        const string lord = "fixture:qualified-lord";
        var template = new GeneralDefinition("fixture:qualified-owner", "Owner", "",
            [new("Printed Lord", "") { ContentId = lord, Tags = SkillTag.Lord }]);
        var owner = Character(template);
        var grant = owner.SkillGrants.Grants.Single();
        var index = new MatchSkillBindingIndex(_ => new(lord, "Printed Lord", "") { Tags = SkillTag.Lord }, false);
        var before = index.GetShard(owner);
        Require(!before.HasOwnedSkill(lord), "An ordinary non-Lord printed grant starts unqualified.");
        var qualified = new PrintedLordSkillQualification(owner.Seat, template.Id, grant.SourceId, grant.GrantId,
            grant.SkillId, grant.SkillInstanceId, new PrintedLordQualificationSource.SetupDeclared("fixture:setup"));
        owner.SkillGrants.SetPrintedLordQualification(grant.GrantId, qualified);
        Require(index.GetShard(owner).HasOwnedInstance(lord, grant.SkillInstanceId) &&
            index.GetShard(owner).HasInstance(lord, grant.SkillInstanceId) && owner.Role == Role.Rebel && !before.HasOwnedSkill(lord),
            "A setup declaration qualifies its actual printed grant, refreshes cached possession/effectiveness and preserves the real identity role.");
        var program = qualified with { Source = new PrintedLordQualificationSource.ProgramIssued(17,
            new("fixture:issuer", "paid", 1, "issuer-instance"), "gameplay-hash") };
        owner.SkillGrants.SetPrintedLordQualification(grant.GrantId,
            JsonSerializer.Deserialize<PrintedLordSkillQualification>(JsonSerializer.Serialize(program))!);
        var issued = owner.SkillGrants.Grants.Single();
        Require(PrintedLordSkillQualifications.Matches(owner, issued) && index.GetShard(owner).HasInstance(lord, grant.SkillInstanceId),
            "An original-frame program qualification uses the same grant identity and retains its typed source through serialization.");
        foreach (var wrong in new[] { issued with { GrantId = "another-grant" }, issued with { SkillInstanceId = "another-instance" },
            issued with { SourceId = "acquired:another-source" }, issued with { PrintedLordQualification = qualified with { BeneficiarySeat = 1 } },
            issued with { PrintedLordQualification = qualified with { GeneralId = "fixture:another-general" } } })
            Require(!PrintedLordSkillQualifications.Matches(owner, wrong), "Setup and program provenance cannot authorize another beneficiary, template, grant or instance.");
        Require(!PrintedLordSkillQualifications.Matches(owner, issued with { PrintedLordQualification = program with
            { Source = new PrintedLordQualificationSource.ProgramIssued(0, new("fixture:issuer", "paid", 1, "issuer-instance"), "gameplay-hash") } }),
            "Program-issued qualification still requires its real positive originating frame id.");
        owner.General = template with { Id = "fixture:replacement-owner" };
        Require(owner.SkillGrants.Grants.Single().PrintedLordQualification is null && !index.GetShard(owner).HasOwnedSkill(lord),
            "Replacing the primary general clears stale qualification even when the replacement prints the same skill and keeps its grant id.");
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
