using CardGame.Modes.Standard;
using CardGame.Core;

internal static class ModeRuleSetChecks
{
    internal static void FrozenOpeningDependenciesAndDeterministicInitiative()
    {
        ModePlayerLife[] players =
        [
            new(3, Role.TeamB, true, "team:red", "faction:shu"),
            new(1, Role.Loyalist, true, "team:blue", "faction:wei"),
            new(2, Role.Lord, true, "team:blue", "faction:wei"),
            new(0, Role.Rebel, true, "team:red", "faction:shu")
        ];
        var options = new GameOptions { PlayerCount = 4, HumanSeat = 3 };
        var identity = StandardModeRuleProfiles.Create(Mode("identity:classic-4", ContentModeKind.Identity), options);
        Require(identity.Opening.GetStartingSeat(players, NoRandom) == 2 && identity.UsesGeneralBaseHp &&
            identity.SupportsGodFactionSelection && !identity.ReducesFirstActorInitialHand && identity.IsClassicIdentityContext &&
            identity.Visibility.PublishesIdentityComposition,
            "Identity initiative belongs to its Lord, without consuming the deterministic random sequence.");

        var teams = new Dictionary<string, int> { ["team:red"] = 2, ["team:blue"] = 2 };
        var team = StandardModeRuleProfiles.Create(Mode("team:test", ContentModeKind.Team) with { TeamCounts = teams }, options);
        teams.Clear();
        Require(team.Opening.GetStartingSeat(players, NoRandom) == 1 && team.ReducesFirstActorInitialHand &&
            !team.SupportsGodFactionSelection && !team.UsesGeneralBaseHp && !team.IsClassicIdentityContext &&
            !team.Visibility.PublishesIdentityComposition,
            "The public-team rule freezes ordinal team order, then selects its lowest seat and initiative compensation.");

        var national = StandardModeRuleProfiles.Create(Mode("national:test", ContentModeKind.NationalWarLite), options);
        Require(national.Opening.GetStartingSeat(players, NoRandom) == 0 && !national.ReducesFirstActorInitialHand,
            "National War starts at zero independently of identity or team compatibility fields.");

        var calls = 0;
        var duel = StandardModeRuleProfiles.Create(Mode(WushuangMode.ModeId, ContentModeKind.Team),
            options with { PlayerCount = 2 });
        Require(duel.Opening.GetStartingSeat(players, count =>
            { calls++; Require(count == 2, "Duel initiative requests its actual player count."); return 1; }) == 1 && calls == 1 &&
            duel.UsesClassicBattleRules && !duel.IsClassicIdentityContext && !duel.Visibility.PublishesIdentityComposition &&
            duel.RequiresRoundTracking && duel.RequiresRecipientDamageReevaluation,
            "Random initiative consumes exactly one bounded draw instead of evaluating every installed mode.");

        var bossMode = Mode("activity:boss", ContentModeKind.Team) with
        {
            BossActivity = new([], [], 5, [])
        };
        var boss = StandardModeRuleProfiles.Create(bossMode, options);
        Require(boss.Opening.GetStartingSeat(players, NoRandom) == 3 && !boss.ReducesFirstActorInitialHand,
            "A cooperative encounter uses its first allied seat and does not inherit 2v2 initiative compensation.");
        Require(StandardModeRuleProfiles.Create(bossMode, options with { HumanSeat = -1 }).Opening.GetStartingSeat(players, NoRandom) == 0,
            "An all-AI cooperative encounter retains the existing zero-seat fallback.");

        var guandu = StandardModeRuleProfiles.Create(Mode("activity:guandu", ContentModeKind.Team) with
        {
            GuanduActivity = new(new Dictionary<string, IReadOnlyList<string>>(),
                new Dictionary<string, string>(), [], [])
        }, options);
        var configured = new ModeSetupPlan([], [], [], initialActorSeat: 3);
        Require(guandu.Opening.GetStartingSeat(players, NoRandom, initialActorSeat: configured.InitialActorSeat) == 3 &&
            guandu.RequiresRoundTracking && guandu.RequiresRecipientDamageReevaluation,
            "Guandu takes the setup plan's generic initial actor and does not consume another random draw.");
        var hell = StandardModeRuleProfiles.Create(Mode("activity:hell", ContentModeKind.Team) with
            { HellActivity = new(5, "fixture:auxiliary", "fixture:hidden") }, options);
        var wenhe = StandardModeRuleProfiles.Create(Mode("activity:wenhe", ContentModeKind.Survival) with
            { WenheActivity = new() }, options);
        var nian = StandardModeRuleProfiles.Create(Mode("activity:nian", ContentModeKind.Team) with
            { NianActivity = new([]) }, options);
        Require(!hell.RequiresRecipientDamageReevaluation && !wenhe.RequiresRecipientDamageReevaluation &&
            nian.RequiresRoundTracking && !nian.RequiresRecipientDamageReevaluation,
            "Installing a damage provider does not itself opt Hell or Wenhe into Guandu and duel recipient resets; Nian independently requires rounds.");
        var dragon = StandardModeRuleProfiles.Create(Mode("activity:dragon", ContentModeKind.Team) with
            { DragonBoatActivity = new(new Dictionary<string, IReadOnlyList<string>>()) }, options);
        Require(identity.RuntimeRequirements == new ModeRuntimeRequirements() &&
            guandu.RuntimeRequirements == new ModeRuntimeRequirements(TracksActualDiscardRecoveryPhases: true,
                TracksActualPhaseEnds: true, CapturesRecipientDamageBase: true, TracksFinalizedCardTargets: true) &&
            duel.RuntimeRequirements == new ModeRuntimeRequirements(TracksNativeDrawInvocations: true) &&
            dragon.RuntimeRequirements == new ModeRuntimeRequirements(TracksRoundEnding: true) &&
            hell.RuntimeRequirements == new ModeRuntimeRequirements() && wenhe.RuntimeRequirements == new ModeRuntimeRequirements(),
            "Each profile opts into only its original tracking mechanisms; duel recipient reevaluation does not imply a Guandu recipient base.");
    }

    internal static void DeathObjectivesPreserveEncounterAndRankingBoundaries()
    {
        var options = new GameOptions { PlayerCount = 6, HumanSeat = 1 };
        ModePlayerLife[] encounter =
        [
            new(0, Role.TeamB, true, "team:allies", null), // NPC: its apparent team is not an allied player slot.
            new(1, Role.TeamA, false, "team:allies", null),
            new(2, Role.TeamA, false, "team:allies", null),
            new(3, Role.TeamA, false, "team:allies", null),
            new(4, Role.TeamB, true, "team:beasts", null),
            new(5, Role.TeamB, false, "team:beasts", null)
        ];
        int[] participantOffsets = [0, 1, 2];
        var shenwu = StandardModeRuleProfiles.Create(Mode("activity:shenwu", ContentModeKind.Team) with
        {
            ShenwuActivity = new([], [], [], [], participantOffsets, 4, [3, 5], 5, [],
                new Dictionary<string, string>())
        }, options);
        participantOffsets[0] = 5;
        Require(shenwu.Death.Evaluate(encounter) is { Kind: ModeDeathOutcomeKind.Team, WinningGroupId: "team:beasts" },
            "Shenwu freezes actual participant slots: a living NPC with an allied-looking team cannot prevent defeat.");
        encounter[1] = encounter[1] with { IsAlive = true };
        Require(shenwu.Death.Evaluate(encounter).WinningGroupId is null,
            "A dead Shenwu leader does not grant victory before its killing action and death children finish.");

        var boss = StandardModeRuleProfiles.Create(Mode("activity:boss", ContentModeKind.Team) with { BossActivity = new([], [], 5, []) }, options);
        Require(boss.Death.Evaluate(encounter).WinningGroupId is null,
            "Successive bosses leave progression to the stable encounter boundary rather than declaring victory on death.");
        var wiped = encounter.Select(player => player with { IsAlive = false }).ToArray();
        Require(boss.Death.Evaluate(wiped).WinningGroupId == "team:boss",
            "Allied defeat remains immediate even when the killing sequence also killed the last enemy.");

        var teams = new Dictionary<string, int> { ["team:blue"] = 4, ["team:red"] = 4 };
        var jiange = StandardModeRuleProfiles.Create(Mode("activity:jiange", ContentModeKind.Team) with
        {
            TeamCounts = teams,
            TeamActivity = new(new Dictionary<string, IReadOnlyList<string>>(), new Dictionary<string, IReadOnlyList<string>>(),
                new Dictionary<string, IReadOnlyList<string>>(), 5, 1, 3, 1, new Dictionary<string, IReadOnlyList<string>>())
        }, new GameOptions { PlayerCount = 8, HumanSeat = 1, HumanTeamId = "team:blue" });
        ModePlayerLife[] objective =
        [
            new(1, Role.TeamA, true, "team:blue", null),
            new(3, Role.TeamB, false, "team:red", null),
            new(4, Role.TeamB, false, "team:red", null),
            new(2, Role.TeamB, true, "team:red", null)
        ];
        Require(jiange.Death.Evaluate(objective).WinningGroupId == "team:blue",
            "Jiange wins when both enemy spirits die even if an enemy machine survives.");
        Require(jiange.Death.Evaluate(objective.Select(player => player with { IsAlive = false }).ToArray()).WinningGroupId == "team:blue",
            "The existing spirit-objective-before-allied-elimination precedence is preserved.");

        var national = StandardModeRuleProfiles.Create(Mode("national:test", ContentModeKind.NationalWarLite), options);
        ModePlayerLife[] nationalLives =
        [new(0, Role.TeamA, true, "team:blue", "wei"), new(1, Role.TeamB, true, "team:red", "wei")];
        Require(national.Death.Evaluate(nationalLives) is { Kind: ModeDeathOutcomeKind.Faction, WinningGroupId: "wei" } &&
            !national.Death.NotifyAiOnDeath,
            "National victory uses the true faction without mistaking compatibility teams for player-visible relations.");

        var identity = StandardModeRuleProfiles.Create(Mode("identity:test", ContentModeKind.Identity), options);
        ModePlayerLife[] identityLives = [new(0, Role.Lord, false, null, null), new(1, Role.Renegade, true, null, null)];
        Require(identity.Death.Evaluate(identityLives).Winner == Winner.Renegade &&
            identity.Death.Evaluate(identityLives, zaowangRebelDeathWins: true).Winner == Winner.LordAndLoyalists &&
            identity.Death.EvaluatesIdentityVictory,
            "Identity preserves the lone Renegade result and its qualified Zaowang override.");
        var race = StandardModeRuleProfiles.Create(Mode("activity:dragon", ContentModeKind.Team) with
        { DragonBoatActivity = new(new Dictionary<string, IReadOnlyList<string>>()) }, options);
        Require(race.Death.Evaluate(identityLives).Kind == ModeDeathOutcomeKind.Preserve &&
            !race.Death.UsesIdentityRewards && !race.Death.EvaluatesIdentityVictory,
            "A score race does not replace the winner or run identity rewards on a participant death.");
        Require(StandardModeRuleProfiles.Create(Mode("survival:test", ContentModeKind.Survival), options)
            .Death.Evaluate(identityLives).Kind == ModeDeathOutcomeKind.SurvivalRank,
            "Survival records death rank and leaves final victory to its existing stable boundary.");
    }

    internal static void CardProjectionKeepsSourceIdentityAndModeLocalNames()
    {
        CardKind[] ordinary = [CardKind.Duel, CardKind.DrawTwo, CardKind.BarbarianAssault, CardKind.ArrowBarrage,
            CardKind.PeachGarden, CardKind.FiveGrains, CardKind.Dismantlement, CardKind.Snatch,
            CardKind.FireAttack, CardKind.IronChain, CardKind.BorrowedSword];
        var always = new SkillProgramCondition(SkillProgramConditionKind.Always, 0, []);
        var views = new List<SkillProgramViewAs>();
        foreach (var count in new[] { 1, 2 })
        foreach (var kind in ordinary) views.Add(View($"family-{count}-{kind}", kind, count));
        foreach (var kind in new[] { CardKind.Slash, CardKind.Dodge, CardKind.Peach, CardKind.Alcohol })
            views.Add(View("basic-" + kind, kind, forResponse: true));
        var fixedName = View("fixed-draw", CardKind.DrawTwo, 3);
        views.Add(fixedName);
        var allTricks = new SkillProgramTrigger("all-tricks", SkillProgramTriggerWindow.CardUseCommitted,
            null, null, null, [], 1, 13, [], [], null, ordinary, [], null, null, default, false,
            new SkillProgramTriggerCondition(SkillProgramTriggerConditionKind.Always, []), []);
        var original = new SkillProgram("fixture:mode-card-projection", 1, "fixture-source-hash", "fixture-runtime", 0,
            [], Array.AsReadOnly(views.ToArray()), [], [allTricks], [], []);
        var options = new GameOptions { PlayerCount = 8, HumanSeat = 0 };
        var plain = StandardModeRuleProfiles.Create(Mode("identity:classic-8", ContentModeKind.Identity), options).Cards;
        Require(ReferenceEquals(plain.ProjectProgram(original), original) && !plain.ProjectsSkillPrograms,
            "Ordinary identity does not derive a skill view or install activity-card rules.");

        var dragon = StandardModeRuleProfiles.Create(Mode("activity:dragon", ContentModeKind.Team) with
        { DragonBoatActivity = new(new Dictionary<string, IReadOnlyList<string>>()) }, options).Cards;
        var projected = dragon.ProjectProgram(original);
        Require(projected.Id == original.Id && projected.Revision == original.Revision &&
            projected.GameplayHash == original.GameplayHash && projected.RuntimeVersion == original.RuntimeVersion &&
            ReferenceEquals(projected.Activations, original.Activations) && ReferenceEquals(projected.CardPolicies, original.CardPolicies),
            "A derived view preserves content fingerprint, published identity, executable activations and existing policies.");
        Require(projected.ViewAs.Single(view => view.Id == "family-1-DrawTwo").OutputKind == CardKind.SameBoat &&
            projected.ViewAs.All(view => view.Id is not ("family-1-PeachGarden" or "family-2-ArrowBarrage")) &&
            ReferenceEquals(projected.ViewAs.Single(view => view.Id == fixedName.Id), fixedName),
            "Complete ordinary-trick families adopt replacement names and exclusions; an explicit named conversion stays intact.");
        Require(projected.ViewAs.Single(view => view.Id == "basic-Peach") is { OutputKind: CardKind.Zong, ForResponse: false } &&
            projected.ViewAs.Single(view => view.Id == "basic-Alcohol") is { OutputKind: CardKind.RealgarWine, ForResponse: false } &&
            projected.ViewAs.Single(view => view.Id == "basic-Dodge").ForResponse &&
            !projected.Triggers.Single().CardKinds.Contains(CardKind.ArrowBarrage) &&
            projected.Triggers.Single().CardKinds.Contains(CardKind.AgainstCurrent),
            "Basic replacement preserves response restrictions and static all-trick triggers see the same mode-local names.");
        Require(!dragon.IsDeclaredCardNameAllowed(CardKind.DrawTwo) && !dragon.IsDeclaredCardNameAllowed(CardKind.PeachGarden) &&
            dragon.IsDeclaredCardNameAllowed(CardKind.SameBoat) && plain.IsDeclaredCardNameAllowed(CardKind.DrawTwo),
            "Removed and replaced declaration names are excluded only by the selected match's card rules.");
        Require(dragon.IsCardAllowed(CardKind.DrawTwo) && !dragon.AllowsOrdinaryTrickOption(CardKind.DrawTwo) &&
            dragon.AllowsOrdinaryTrickOption(CardKind.DrawTwo, CardKind.DrawTwo) &&
            dragon.AllowsOrdinaryTrickOption(CardKind.SameBoat) && !plain.AllowsOrdinaryTrickOption(CardKind.SameBoat) &&
            plain.AllowsOrdinaryTrickOption(CardKind.SameBoat, CardKind.SameBoat) &&
            !plain.AllowsOrdinaryTrickOption(CardKind.SameBoat, CardKind.Duel),
            "A named conversion retains its explicit card while only the selected activity adds that card to ordinary name choices.");
        var duelCards = StandardModeRuleProfiles.Create(Mode(WushuangMode.ModeId, ContentModeKind.Team), options).Cards;
        var excluded = Enum.GetValues<CardKind>().First(kind => !WushuangCatalogue.IsCardAllowed(kind));
        Require(!duelCards.IsCardAllowed(excluded) && !duelCards.AllowsOrdinaryTrickOption(excluded, excluded),
            "The profile's card-use whitelist remains authoritative even for a specifically named conversion.");

        var guandu = StandardModeRuleProfiles.Create(Mode("activity:guandu", ContentModeKind.Team) with
        { GuanduActivity = new(new Dictionary<string, IReadOnlyList<string>>(), new Dictionary<string, string>(), [], []) }, options).Cards;
        var guanduProgram = guandu.ProjectProgram(original);
        Require(guanduProgram.ViewAs.Single(view => view.Id == "basic-Peach") is { OutputKind: CardKind.Peach, ForResponse: true } &&
            guanduProgram.ViewAs.Single(view => view.Id == "basic-Alcohol") is { OutputKind: CardKind.Intoxication, ForResponse: false },
            "Guandu replaces Alcohol alone and does not inherit Dragon Boat's Peach-to-Zong rule.");

        var wenhe = StandardModeRuleProfiles.Create(Mode("activity:wenhe", ContentModeKind.Survival) with
        { WenheActivity = new() }, options).Cards.ProjectProgram(original);
        var additions = wenhe.ViewAs.Where(view => view.Id.Contains(".activity-", StringComparison.Ordinal)).ToArray();
        Require(additions.Select(view => view.OutputKind).SequenceEqual(StandardModeRuleProfiles.WenheOrdinaryTrickKinds.Concat(StandardModeRuleProfiles.WenheOrdinaryTrickKinds)) &&
            additions.Take(4).All(view => view.Id.StartsWith("family-1-Duel.activity-", StringComparison.Ordinal)) &&
            additions.Skip(4).All(view => view.Id.StartsWith("family-2-Duel.activity-", StringComparison.Ordinal)),
            "Added ordinary names retain the published family-first order and original binding-derived suffixes.");
        var unaffected = new SkillProgram("fixture:fixed-name-only", 1, "fixture-fixed-hash", "fixture-runtime", 0,
            [], [fixedName], [], [], [], []);
        Require(ReferenceEquals(dragon.ProjectProgram(unaffected), unaffected) &&
            original.ViewAs.Any(view => view.OutputKind == CardKind.DrawTwo) &&
            original.ViewAs.All(view => view.OutputKind is not (CardKind.SameBoat or CardKind.Zong)) &&
            ReferenceEquals(plain.ProjectProgram(original), original),
            "An unchanged activity view returns the source reference, and projecting one match never edits another match's source.");

        SkillProgramViewAs View(string id, CardKind kind, int inputCount = 1, bool forResponse = false) =>
            new(id, [], [], kind, true, forResponse, always, inputCount);
    }

    private static ContentModeDefinition Mode(string id, ContentModeKind kind) =>
        new(id, id, 2, 8, new Dictionary<string, int>(), ModeKind: kind);
    private static int NoRandom(int _) => throw new InvalidOperationException("A fixed initiative rule consumed random state.");
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
