using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ModeEntryAssemblyChecks
{
    public static void EveryRegisteredModeComposesWithoutLiveHostAccess()
    {
        var allActivities = ComposedSkillContentRegistry.CreateShowcase(includeActivities: true,
            includeBossActivities: true, includeHulaoActivities: true, includeShenwuActivities: true,
            includeNianActivities: true, includeHellActivities: true, includeWenheActivities: true,
            includeDragonBoatActivities: true, includeGuanduActivities: true);
        var singleDuel = StandardWushuangContentRegistry.Create();
        var covered = new List<ContentModeDefinition>();
        foreach (var registry in new[] { allActivities, singleDuel })
        foreach (var mode in registry.Modes.Values.OrderBy(definition => definition.Id, StringComparer.Ordinal))
        {
            Inspect(registry, mode);
            covered.Add(mode);
        }

        Require(covered.Any(mode => mode.ModeKind == ContentModeKind.Identity) &&
            covered.Any(mode => mode.ModeKind == ContentModeKind.Team && !HasActivity(mode)) &&
            covered.Any(mode => mode.ModeKind == ContentModeKind.NationalWarLite) &&
            covered.Any(mode => mode.TeamActivity is not null) && covered.Any(mode => mode.HulaoActivity is not null) &&
            covered.Any(mode => mode.BossActivity is not null && mode.HulaoActivity is null && mode.HellActivity is null && mode.NianActivity is null) &&
            covered.Any(mode => mode.ShenwuActivity is not null) && covered.Any(mode => mode.NianActivity is not null) &&
            covered.Any(mode => mode.HellActivity is not null) && covered.Any(mode => mode.WenheActivity is not null) &&
            covered.Any(mode => mode.DragonBoatActivity is not null) && covered.Any(mode => mode.GuanduActivity is not null) &&
            covered.Any(mode => mode.Id == WushuangMode.ModeId),
            "The real registries must exercise every installed ordinary and activity mode family, including the independent single-duel registry.");

        // A survival profile still owns ranking and completion when it does not
        // opt into Wenhe's optional round rules, aspirations or tier bonuses.
        var survival = allActivities.Modes.Values.First(mode => mode.ModeKind == ContentModeKind.Survival);
        Inspect(allActivities, survival with { WenheActivity = null });
    }

    private static void Inspect(ContentRegistry registry, ContentModeDefinition mode)
    {
        var options = new GameOptions
        {
            Seed = 17, ModeId = mode.Id, PlayerCount = mode.MinPlayers, HumanSeat = 0,
            HumanRole = mode.ModeKind == ContentModeKind.Identity ? Role.Lord : null,
            UseInteractiveSetup = true
        };
        var context = new ModeFeatureCreationContext(options, mode, registry, ColdHosts());
        var features = ModeFeatureComposition.Create(context);
        var runtime = new MatchModeRuntime(features.ToArray());
        runtime.ValidateNativePorts();

        var setup = runtime.Find<IMatchModeSetupFeature>();
        var profile = runtime.Find<IMatchModeRuleProvider>();
        Require(setup is not null && profile is not null && setup is not IMatchModeRuleProvider &&
            profile is not IMatchModeSetupFeature && profile is IMatchModeAllegiancePolicy && profile.Rules is not null,
            $"Mode '{mode.Id}' must compose independent setup and immutable rule-profile slots without reading a running host.");
        foreach (var contract in profile!.Rules.RequiredFeatureContracts)
            Require(features.Count(contract.IsInstanceOfType) == 1,
                $"Mode '{mode.Id}' requires exactly one installed '{contract.Name}' feature.");

        Expect<IWushuangModeFeature>(features, mode.Id == WushuangMode.ModeId, mode.Id);
        Expect<IHellEncounterFeature>(features, mode.HellActivity is not null, mode.Id);
        Expect<INianChallengeFeature>(features, mode.NianActivity is not null, mode.Id);
        Expect<IWenheBattleRulesFeature>(features, mode.ModeKind == ContentModeKind.Survival, mode.Id);
        Expect<IMatchModeDeathRanking>(features, mode.ModeKind == ContentModeKind.Survival, mode.Id);
        Expect<IDragonBoatBattleRulesFeature>(features, mode.DragonBoatActivity is not null, mode.Id);
        Expect<IGuanduBattleRulesFeature>(features, mode.GuanduActivity is not null, mode.Id);
        Expect<IModeFlowInterruptionHandler>(features, mode.HulaoActivity is not null, mode.Id);
        Expect<IModeDeckRecycleObserver>(features, mode.HulaoActivity is not null, mode.Id);

        var commands = features.OfType<IMatchModeCommandHandler>().SelectMany(handler => handler.CommandTypes).ToArray();
        Require(commands.Contains(typeof(TickNianClockCommand)) == (mode.NianActivity is not null),
            $"Mode '{mode.Id}' can install the Nian clock command only with its Nian configuration.");
        foreach (var command in new[] { typeof(TickWushuangClockCommand), typeof(BuyWushuangOfferCommand),
            typeof(RerollWushuangShopCommand), typeof(UpgradeWushuangShopCommand), typeof(FinishWushuangShopCommand),
            typeof(ToggleWushuangShopLockCommand) })
            Require(commands.Contains(command) == (mode.Id == WushuangMode.ModeId),
                $"Mode '{mode.Id}' must not borrow the optional single-duel command '{command.Name}'.");

        if (!HasActivity(mode) && mode.Id != WushuangMode.ModeId && mode.ModeKind != ContentModeKind.Survival)
            Require(runtime.RuleObservers.Count == 0 && runtime.RuntimeHooks.Count == 0 && runtime.PromptAnswers.Count == 0 &&
                !features.OfType<IMatchModeNativeFrameProvider>().SelectMany(provider => provider.FrameRoutes).Any() &&
                !features.OfType<IMatchModeDamageRuleProvider>().SelectMany(provider => provider.DamageRules).Any(),
                $"Ordinary mode '{mode.Id}' must not register activity observers, schedulers, native routes, answers or damage callbacks.");
    }

    private static bool HasActivity(ContentModeDefinition mode) => mode.TeamActivity is not null || mode.BossActivity is not null ||
        mode.HulaoActivity is not null || mode.ShenwuActivity is not null || mode.NianActivity is not null ||
        mode.HellActivity is not null || mode.WenheActivity is not null || mode.DragonBoatActivity is not null || mode.GuanduActivity is not null;

    private static void Expect<T>(IEnumerable<object> features, bool expected, string modeId) where T : class =>
        Require(features.OfType<T>().Count() == (expected ? 1 : 0),
            $"Mode '{modeId}' must {(expected ? "install exactly one" : "omit")} optional '{typeof(T).Name}' contract.");

    private static ModeHostRegistry ColdHosts() => new ModeHostRegistry()
        .Register<IWushuangModeHost>(Proxy<IWushuangModeHost>)
        .Register<IModeSetupRandom>(Proxy<IModeSetupRandom>)
        .Register<IModeEncounterHost>(Proxy<IModeEncounterHost>)
        .Register<IHellEncounterHost>(Proxy<IHellEncounterHost>)
        .Register<INianChallengeHost>(Proxy<INianChallengeHost>)
        .Register<IStandardBattleHost>(Proxy<IStandardBattleHost>)
        .Register<IModeBattleRulesHost>(Proxy<IModeBattleRulesHost>)
        .Register<IWenheRuleFlowHost>(Proxy<IWenheRuleFlowHost>)
        .Register<IGuanduEventFlowHost>(Proxy<IGuanduEventFlowHost>)
        .Register<IModeNationalRevealHost>(Proxy<IModeNationalRevealHost>)
        .Register<IModeSetupEquipmentHost>(Proxy<IModeSetupEquipmentHost>);

    private static T Proxy<T>() where T : class => (T)DispatchProxy.Create(typeof(T), typeof(ColdModeHostConstructionProbe));
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}

/// <summary>Construction may resolve a dedicated host, but every attempted live operation fails immediately.</summary>
public class ColdModeHostConstructionProbe : DispatchProxy
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args) =>
        throw new InvalidOperationException($"Mode construction accessed live host member '{targetMethod?.DeclaringType?.Name}.{targetMethod?.Name}'.");
}
