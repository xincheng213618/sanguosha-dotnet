using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Modes.Standard;

internal static class ModeAllegianceChecks
{
    private const string ModeId = "identity:standard-5";
    private const BindingFlags Native = BindingFlags.NonPublic | BindingFlags.Instance;

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

    public static void ExactProfileControlsNativeFactionAndTargetQueriesWithoutChangingMatchState()
    {
        var baselineRegistry = ContentRegistry.Build(new StandardContentPackage());
        var changedRegistry = ContentRegistry.Build(new StandardContentPackage(), new OverridePackage());
        var options = new GameOptions { Seed = 17, PlayerCount = 5, HumanSeat = 0, HumanRole = Role.Lord,
            ModeId = ModeId, UseInteractiveSetup = false, AdvanceAfterHumanCommands = false };
        var baseline = GameEngine.CreateStandard(options, baselineRegistry);
        var changed = GameEngine.CreateStandard(options, changedRegistry);
        var before = GameCheckpointJson.Serialize(changed.CreateCheckpoint());

        Require(Call<IReadOnlyList<string>>(baseline, "AvailableEffectiveFactionIds").Count == 5 &&
            Call<IReadOnlyList<string>>(changed, "AvailableEffectiveFactionIds").SequenceEqual(["qun"]) &&
            Call<int>(baseline, "GetPublicLivingFactionCount") > 0 && Call<int>(changed, "GetPublicLivingFactionCount") == 0,
            "The actual engine asks its exact profile for native faction choices and public-faction visibility.");
        Require(Targets(baseline).Count == 0 && Targets(changed).SequenceEqual([1, 2, 3, 4]) &&
            GameCheckpointJson.Serialize(changed.CreateCheckpoint()) == before && changed.ResolutionStack.Count == 0,
            "Native target selection follows the installed relation policy in an identity match, without creating commands, frames or payments.");

        static IReadOnlyList<int> Targets(GameEngine game) => Call<IReadOnlyList<int>>(game,
            "GetProgramTargetSeats", 0, SkillProgramTargetKind.PublicTeamEnemies, null, null, null);
    }

    private static IMatchModeAllegiancePolicy Policy(string id, ContentModeKind kind)
    {
        var mode = new ContentModeDefinition(id, id, 5, 5, new Dictionary<string, int>(), ModeKind: kind);
        var hosts = new ModeHostRegistry().Register<IModeSetupRandom>(() => new NoRandom());
        return (IMatchModeAllegiancePolicy)new StandardModeProfileFactory().Create(new ModeFeatureCreationContext(
            new GameOptions { PlayerCount = 5 }, mode, ContentRegistry.Build(), hosts));
    }

    private static T Call<T>(GameEngine engine, string method, params object?[] arguments) =>
        (T)(typeof(GameEngine).GetMethod(method, Native) ?? throw new InvalidOperationException(method)).Invoke(engine, arguments)!;

    private sealed class OverridePackage : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("test:allegiance-profile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddModeFeatureFactory(new OverrideFactory());
    }
    private sealed class OverrideFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "test:allegiance-profile";
        public string ModeId => ModeAllegianceChecks.ModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext context) =>
            new OverrideProfile(new StandardModeProfileFactory().Create(context));
    }

    // Keep every other standard profile service. Only the mode's interpretation
    // of allegiance changes, independently of its native identity setup owner.
    private sealed class OverrideProfile(IGameModeFeature original) : IMatchModeRuleProvider,
        IMatchModeConfigurationValidator, IMatchModeEquipmentProfile, IMatchModePresentation,
        IMatchModeSetupPresentation, IMatchModeFactionPolicy, IMatchModeCommandHandler, IMatchModeAllegiancePolicy
    {
        private T Service<T>() => (T)(object)original;
        public ModeRuleSet Rules => Service<IMatchModeRuleProvider>().Rules;
        public void ValidateOptions() => Service<IMatchModeConfigurationValidator>().ValidateOptions();
        public ModeEquipmentRecipe EquipmentRecipe => Service<IMatchModeEquipmentProfile>().EquipmentRecipe;
        public string StartLog(ModePresentationContext facts) => Service<IMatchModePresentation>().StartLog(facts);
        public string RulesText(ModePresentationContext facts) => Service<IMatchModePresentation>().RulesText(facts);
        public string WinnerLog(ModeWinnerPresentationContext facts) => Service<IMatchModePresentation>().WinnerLog(facts);
        public PromptChoice GeneralChoice(ModeGeneralSelectionFacts player, ModeGeneralChoiceFacts general, bool fullRoster = false) =>
            Service<IMatchModeSetupPresentation>().GeneralChoice(player, general, fullRoster);
        public string SelectionPrompt(ModeGeneralSelectionFacts facts) => Service<IMatchModeSetupPresentation>().SelectionPrompt(facts);
        public string SetupCompletedText(ModeSetupCompletionFacts facts) => Service<IMatchModeSetupPresentation>().SetupCompletedText(facts);
        public string? EffectiveFaction(ModeFactionFacts facts) => Service<IMatchModeFactionPolicy>().EffectiveFaction(facts);
        public string? ScenarioGodFaction(string? printed) => Service<IMatchModeFactionPolicy>().ScenarioGodFaction(printed);
        public IReadOnlyList<Type> CommandTypes => Service<IMatchModeCommandHandler>().CommandTypes;
        public CommandResult Submit(GameCommand command) => Service<IMatchModeCommandHandler>().Submit(command);
        public IReadOnlyList<string> NativeFactionChoices { get; } = Array.AsReadOnly(new[] { "qun" });
        public bool AllowsAvatarFactionChanges => Service<IMatchModeAllegiancePolicy>().AllowsAvatarFactionChanges;
        public bool AllowsLordSkillProjection => Service<IMatchModeAllegiancePolicy>().AllowsLordSkillProjection;
        public bool AllowsIdentityConcealment => Service<IMatchModeAllegiancePolicy>().AllowsIdentityConcealment;
        public bool AllowsPublicTeamSkillSynergy => Service<IMatchModeAllegiancePolicy>().AllowsPublicTeamSkillSynergy;
        public bool IsPublicFactionVisible(ModeFactionVisibilityFacts facts) => false;
        public Role? PublicIdentityDeathRole(ModeIdentityDeathFacts facts) => Service<IMatchModeAllegiancePolicy>().PublicIdentityDeathRole(facts);
        public bool SamePublicTeam(ModeAllegianceSeatFacts first, ModeAllegianceSeatFacts second) =>
            Service<IMatchModeAllegiancePolicy>().SamePublicTeam(first, second);
        public bool PublicTeamEnemy(ModeAllegianceSeatFacts owner, ModeAllegianceSeatFacts target) =>
            target.IsAlive && target.Seat != owner.Seat;
    }
    private sealed class NoRandom : IModeSetupRandom
    {
        public int Next(int exclusiveMax) => throw new InvalidOperationException("An allegiance policy cannot consume gameplay random during construction.");
        public void Shuffle<T>(IList<T> values) => throw new InvalidOperationException("An allegiance policy cannot shuffle during construction.");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
