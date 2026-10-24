using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ModePresentationChecks
{
    public static void WordingVisibilityAndRequiredContractsBelongToProfile()
    {
        var context = new ModePresentationContext(8, 2, "你", "青队", "1 主公、2 忠臣、4 反贼、1 内奸", "4 青队、4 赤队", "4 魏、4 蜀");
        var identity = Feature(Mode("identity:test", ContentModeKind.Identity));
        Require(identity.StartLog(context) == "8人身份局开始，主公先行动。" && identity.RulesText(context) == context.RoleSummary,
            "The installed mode retains the existing identity opening wording without resolving native services.");
        var national = Feature(Mode("national:test", ContentModeKind.NationalWarLite));
        Require(national.StartLog(context) == "8人国战简化局开始，1号位先行动。" &&
            national.RulesText(context) == "4 魏、4 蜀；每名角色选择两名同势力武将，势力和武将默认暗置，明置后公开；消灭其他势力获胜" &&
            national.WinnerLog(new(Winner.NationalFactionA, "魏势力", null, null, "魏", null)) == "游戏结束：魏势力获胜。 ",
            "National-war presentation preserves faction wording and the established winning-log punctuation.");
        var duel = Feature(Mode(WushuangMode.ModeId, ContentModeKind.Team));
        Require(duel.StartLog(context) == "单骑无双开始，本局随机先手为 你（03 号位）。" &&
            duel.WinnerLog(new(Winner.Draw, "无人", null, null, null, null)) == "游戏结束：本局平局。",
            "The duel's mode-specific opening and draw messages stay above the native logging mechanism.");
        var survival = Feature(Mode("survival:test", ContentModeKind.Survival));
        Require(survival.StartLog(context) == "8人文和乱武开始，随机3号位先行动，独自存活者获胜。" &&
            survival.WinnerLog(new(Winner.TeamA, "青队", null, null, null, "你")) == "游戏结束：你获胜。 ",
            "The ranking mode names its surviving seat rather than a compatibility identity or team.");
        var boat = Feature(Mode("boat:winner", ContentModeKind.Team) with
            { DragonBoatActivity = new(new Dictionary<string, IReadOnlyList<string>>()) });
        ModeFactionDisplayName[] factionNames = [new("wei", "魏")];
        var boatWinner = new ModeWinnerPresentationContext(Winner.TeamA, "compatibility", "team:wei", "魏军", null, null, factionNames);
        factionNames[0] = new("wei", "changed");
        Require(boat.WinnerLog(boatWinner) == "游戏结束：魏队获胜。 " &&
            boatWinner.FactionNames is IList<ModeFactionDisplayName> { IsReadOnly: true },
            "Boat team interpretation belongs to the mode and consumes a frozen generic faction-name list.");

        var options = new GameOptions { PlayerCount = 8 };
        var hidden = StandardModeRuleProfiles.Create(Mode("national:test", ContentModeKind.NationalWarLite), options).Visibility;
        Require(hidden is { ConcealsRoleCompletely: true, UsesHiddenFaction: true, UsesSecondaryGeneral: true,
                AllowsGeneralReveal: true, ShowsPublicTeams: false } &&
            StandardModeRuleProfiles.Create(Mode("identity:test", ContentModeKind.Identity) with { DualGenerals = true }, options)
                .Visibility is { UsesSecondaryGeneral: true, UsesHiddenFaction: false, AllowsGeneralReveal: false } &&
            StandardModeRuleProfiles.Create(Mode("team:test", ContentModeKind.Team), options).Visibility.ShowsPublicTeams,
            "Hidden factions, public teams, dual-general display and reveal permission are independent immutable profile choices.");
        var required = StandardModeRuleProfiles.Create(Mode(WushuangMode.ModeId, ContentModeKind.Team), options).RequiredFeatureContracts;
        Require(required.SequenceEqual([typeof(IWushuangModeFeature)]) && required is IList<Type> { IsReadOnly: true } &&
            StandardModeRuleProfiles.Create(Mode("identity:test", ContentModeKind.Identity), options).RequiredFeatureContracts.Count == 0,
            "Only the selected duel profile requires its optional runtime; Core need not name the duel when enforcing dependencies.");
        var baseline = StandardModeRuleProfiles.Create(Mode("identity:test", ContentModeKind.Identity), options);
        try
        {
            _ = new ModeRuleSet(baseline.Opening, baseline.Death, baseline.Cards, false, false, false, false,
                requiredFeatureContracts: [typeof(GameEngine)]);
            throw new InvalidOperationException("A non-feature implementation was accepted as a required contract.");
        }
        catch (ArgumentException) { }
    }

    public static void RevealCommandRetainsPromptOrderAndLazyNativeHost()
    {
        var host = new RevealHost();
        var resolves = 0;
        var feature = (IMatchModeCommandHandler)Feature(Mode("national:test", ContentModeKind.NationalWarLite),
            () => { resolves++; return host; });
        Require(resolves == 0 && feature.CommandTypes.SequenceEqual([typeof(RevealGeneralCommand)]),
            "Constructing a pure setup/presentation feature does not resolve its reveal host.");
        var command = new RevealGeneralCommand(0, GeneralSelectionSlot.Primary, 0, new PromptId(5));
        Require(feature.Submit(command).Accepted && resolves == 1 && host.Trace.SequenceEqual(["validate", "slots", "accept", "reveal:Primary:False"]),
            "A response-phase reveal uses native prompt ownership, exact legal slots and one original command-session action.");
        host.Trace.Clear(); host.Validation = new(CommandErrorCode.InvalidPrompt, "stale");
        Require(feature.Submit(command with { Slot = (GeneralSelectionSlot)999 }).Error?.Code == CommandErrorCode.InvalidPrompt &&
            host.Trace.SequenceEqual(["validate", "reject"]),
            "Prompt validation retains precedence over malformed slots and never opens a second payment or action boundary.");
        host.Trace.Clear(); host.Validation = null; host.Kind = DecisionKind.SelectGeneral;
        Require(feature.Submit(command).Error?.Code == CommandErrorCode.IllegalAction && host.Trace.SequenceEqual(["validate", "reject"]),
            "A general-selection prompt cannot become a revealable response merely because the mode supports reveal.");
        host.Trace.Clear(); host.Kind = DecisionKind.PlayCard;
        var ordinary = (IMatchModeCommandHandler)Feature(Mode("identity:test", ContentModeKind.Identity), () => host);
        Require(ordinary.Submit(command).Error?.Message == "General reveal is only available in the national-war lite mode." &&
            host.Trace.SequenceEqual(["validate", "reject"]),
            "Forbidden-mode rejection remains compatible while its qualification lives in the installed mode implementation.");
    }

    public static void GeneralChoicesFamiliesAndFactionPrecedenceRemainModeOwned()
    {
        var primary = new ModeGeneralSelectionFacts(Role.Lord, true, "主将", 4);
        var candidate = new ModeGeneralChoiceFacts("fixture:general", "候选", "仁德", "shu", "蜀", 3, 2, "fixture:ticket");
        var identity = (IMatchModeSetupPresentation)Feature(Mode("identity:classic-presentation", ContentModeKind.Identity));
        var single = identity.GeneralChoice(primary, candidate, fullRoster: true);
        Require(single.Label == "选择 候选（仁德）" && single.Parameters["identity-max-hp"] == "4" &&
            single.Parameters["health-preview"] == "主公体力上限为基础 3 + 1 = 4。初始体力为 3/4。" &&
            single.Parameters["full-roster"] == "true" && single.ContentIds.SequenceEqual([candidate.Id]) &&
            single.Parameters is IDictionary<string, string> { IsReadOnly: true },
            "The installed setup supplies unchanged choice labels and metadata with frozen exposed collections.");
        var nationalFeature = Feature(Mode("national:presentation", ContentModeKind.NationalWarLite));
        var national = (IMatchModeSetupPresentation)nationalFeature;
        var dual = national.GeneralChoice(primary, candidate);
        Require(dual.Label == "选择 候选（仁德 · 蜀） · 基础体力 3 · 组合上限 3（4+3 平均向下取整）" &&
            dual.Parameters["combined-max-hp"] == "3" && national.SelectionPrompt(primary) ==
                "主将 主将（基础体力 4）· 请选择副将，卡片显示组合后的体力上限。" &&
            national.SetupCompletedText(new(["主将", "候选"])) ==
                "国战选将完成，双将与势力保持暗置；出牌阶段或杀响应时可按需明置一名武将。",
            "National hidden-faction wording and dual-health preview retain their exact original text and no identity Lord bonus.");
        var shenwu = (IMatchModeSetupPresentation)Feature(Mode("shenwu:presentation", ContentModeKind.Identity) with
        {
            ShenwuActivity = new([], [candidate.Id], [], [], [0], 1, [], 5, [], new Dictionary<string, string>())
        });
        var god = shenwu.GeneralChoice(primary, candidate);
        Require(god.Parameters["candidate-general-id"] == candidate.CandidateId && god.Parameters["shenwu-shared-god"] == "true" &&
            god.Parameters["health-preview"] == "本地五阶：初始体力 4/5。此神将来自三人共享候选；选择后其他队友不能再选。",
            "God-sharing selection keeps its candidate identity and tier-health preview above the native prompt publisher.");
        string[] names = ["主将", "候选"];
        var completed = new ModeSetupCompletionFacts(names); names[0] = "changed";
        Require(identity.SetupCompletedText(completed) == "选将完成并公开：主将、候选。" &&
            completed.PublicGeneralNames is IList<string> { IsReadOnly: true },
            "Setup completion owns its frozen public names before formatting.");
        var families = Setup(Mode("family:presentation", ContentModeKind.Identity) with
        {
            GeneralSelectionPoolIds = ["a", "b"],
            GeneralVariantGroups = new Dictionary<string, IReadOnlyList<string>> { ["person"] = new[] { "a", "b" } }
        });
        Require(families.RemoveGeneralIdsAfterSelection("a").SequenceEqual(["a", "b"]) &&
            families.RemoveGeneralIdsAfterSelection("absent").Count == 0 &&
            Setup(Mode("national:presentation", ContentModeKind.NationalWarLite)).RemoveGeneralIdsAfterSelection("a").Count == 0,
            "Only family drafting removes the selected person's registered variants from remaining choices.");
        var campaign = new ShanheBattleSetup(1, 1, "fixture", "hero", 4, 0, 0, 0, 0, [], [], [], [], [], "fixture")
            { DefaultGodFactionId = "qun" };
        var policy = (IMatchModeFactionPolicy)Feature(Mode("faction:presentation", ContentModeKind.Identity),
            options: new GameOptions { PlayerCount = 8, ShanheBattle = campaign });
        var facts = new ModeFactionFacts("team:wu", "wei", "jin", "shu", "wu", "god");
        Require(policy.EffectiveFaction(facts) == "jin" &&
            policy.EffectiveFaction(facts with { NativeOverride = null }) == "shu" &&
            policy.EffectiveFaction(facts with { NativeOverride = null, LibraryFaction = null }) == "wu" &&
            policy.EffectiveFaction(facts with { NativeOverride = null, LibraryFaction = null, ChosenFaction = null }) == "qun" &&
            policy.ScenarioGodFaction("wei") is null &&
            ((IMatchModeFactionPolicy)nationalFeature).EffectiveFaction(facts) == "wei" &&
            ((IMatchModeFactionPolicy)Feature(Mode("boat:presentation", ContentModeKind.Team) with
                { DragonBoatActivity = new(new Dictionary<string, IReadOnlyList<string>>()) })).EffectiveFaction(facts) == "wu" &&
            StandardModeRuleProfiles.Create(Mode("survival:presentation", ContentModeKind.Survival), new GameOptions()).Visibility.UsesSurvivalRanking,
            "Boat team and assigned national faction precede native, library, chosen and printed-god campaign fallbacks exactly as before.");
    }

    private static IMatchModePresentation Feature(ContentModeDefinition mode, Func<IModeNationalRevealHost>? reveal = null,
        GameOptions? options = null) =>
        (IMatchModePresentation)new StandardModeProfileFactory().Create(Context(mode, reveal, options));

    private static IMatchModeSetupFeature Setup(ContentModeDefinition mode) =>
        (IMatchModeSetupFeature)new StandardModeSetupFactory().Create(Context(mode));

    private static ModeFeatureCreationContext Context(ContentModeDefinition mode,
        Func<IModeNationalRevealHost>? reveal = null, GameOptions? options = null)
    {
        var hosts = new ModeHostRegistry().Register<IModeSetupRandom>(() => new NoRandom());
        if (reveal is not null) hosts.Register(reveal);
        return new ModeFeatureCreationContext(
            options ?? new GameOptions { PlayerCount = 8, AdvanceAfterHumanCommands = false }, mode, ContentRegistry.Build(), hosts);
    }
    private static ContentModeDefinition Mode(string id, ContentModeKind kind) => new(id, "测试", 8, 8,
        new Dictionary<string, int>(), ModeKind: kind,
        TeamCounts: kind == ContentModeKind.Team ? new Dictionary<string, int> { ["team:a"] = 4, ["team:b"] = 4 } : null);
    private sealed class NoRandom : IModeSetupRandom
    {
        public int Next(int maximum) => throw new InvalidOperationException("A presentation constructor cannot consume gameplay random.");
        public void Shuffle<T>(IList<T> values) => throw new InvalidOperationException("A presentation constructor cannot shuffle gameplay state.");
    }
    private sealed class RevealHost : IModeNationalRevealHost
    {
        internal List<string> Trace { get; } = [];
        internal DecisionKind Kind { get; set; } = DecisionKind.RespondDodge;
        internal CommandError? Validation { get; set; }
        public DecisionKind? PendingKind => Kind;
        public CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId, CommandErrorCode inactiveCode)
        { Trace.Add("validate"); return Validation; }
        public IReadOnlyList<GeneralSelectionSlot> LegalRevealSlots(int actorSeat) { Trace.Add("slots"); return [GeneralSelectionSlot.Primary]; }
        public CommandResult Accept(Action action) { Trace.Add("accept"); action(); return new(true, null, 0, null!); }
        public CommandResult Reject(CommandErrorCode code, string message) { Trace.Add("reject"); return new(false, new(code, message), 0, null!); }
        public void RevealHumanGeneral(GeneralSelectionSlot slot, bool advanceToHumanBoundary) => Trace.Add($"reveal:{slot}:{advanceToHumanBoundary}");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
