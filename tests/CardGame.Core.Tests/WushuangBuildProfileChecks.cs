using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class WushuangBuildProfileChecks
{
    private const string Driver = "fixture:wushuang-build-driver";
    private const string Once = "fixture:wushuang-build-once";
    private const string Neutral = "fixture:wushuang-build-neutral";
    private const string Suppressor = "fixture:wushuang-build-suppressor";

    public static void VisibleRealGeneralsAndNativeSkillChangesUpdateTheBuildCold()
    {
        var registry = ContentRegistry.Build(new Fixture(StandardWushuangContentRegistry.Create()));
        var game = GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(17) with
            { UseInteractiveSetup = true, AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false }, registry);
        void Accept(GameCommand command)
        {
            var result = game.Submit(command);
            Require(result.Accepted, result.Error?.Message ?? "Build-profile fixture command rejected.");
        }
        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 120; i++)
            {
                if (ready()) return;
                if (game.CreateSnapshot(0).Wushuang is { CanShop: true } shop)
                {
                    if (shop.Phase == WushuangShopPhase.OpeningTactic)
                        Accept(new BuyWushuangOfferCommand(0, shop.Offers[0].Id, null, game.Revision));
                    else Accept(new FinishWushuangShopCommand(0, game.Revision));
                }
                else if (Pending() is { } p)
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") == "pass");
                    Require(choice is not null, "Unexpected human boundary in the bounded build fixture.");
                    Accept(new AnswerPromptCommand(0, p.PromptId, choice!.Id, game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The bounded native build fixture missed its boundary.");
        }
        bool AtPlay() => Pending() is { Kind: DecisionKind.PlayCard };
        WushuangBuildProfile Own() => game.CreateSnapshot(0).Players[0].WushuangBuild!;
        void Cold()
        {
            var before = Enumerable.Range(0, 2).Select(s => SnapshotJson.Serialize(game.CreateSnapshot(s))).ToArray();
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Require(before.SequenceEqual(Enumerable.Range(0, 2).Select(s => SnapshotJson.Serialize(game.CreateSnapshot(s)))),
                "Cold restoration preserves both public build profiles and their exact current private views.");
        }
        void Change(string activation)
        {
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], [], game.Revision, Pending()!.PromptId));
            Reach(AtPlay); Cold();
        }

        Accept(new StartGameCommand());
        Reach(() => Pending() is { Kind: DecisionKind.SelectGeneral });
        Accept(new SelectGeneralCommand(0, "classic:guo-jia", game.Revision, Pending()!.PromptId));
        Reach(() => game.CreateSnapshot(0).Wushuang is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
        Require(Own() is { InjuryDrawPerDamagePoint: 2, InjuryRecoverPerDamagePoint: 0, ProactiveHpCost: 0 },
            "The real classic Guo Jia program can retain its two injury-drawn cards; optional distribution is not forced enemy income.");
        var enemy = game.CreateSnapshot(0).Players[1];
        Require(enemy.GeneralId == "boundary:xu-sheng" && enemy.Hand.Count == 0 &&
            enemy.WushuangBuild is { SlashBurstBonus: 1, SlashSealsTargetCards: true, InjuryDrawPerDamagePoint: 0 },
            "The real boundary Xu Sheng exposes its typed Slash damage and target-card hold through public skills, without revealing its hand.");
        Cold();
        Reach(AtPlay);
        Change("add");
        Require(Own() is { InjuryDrawPerDamagePoint: 2, InjuryRecoverPerDamagePoint: 0, SlashBurstBonus: 1,
                SlashSealsTargetCards: true, ProactiveHpCost: 1 },
            "Native grants update the same general's build; a once-per-game injury reward and dying rescue are not perpetual injury returns.");
        var printedYijiCheckpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        Change("drop");
        Require(Own() == new WushuangBuildProfile(0, 0, 0, false, 0),
            "Losing the actual printed and acquired skill sources immediately removes their build traits.");
        Change("restore-yiji");
        Require(Own().InjuryDrawPerDamagePoint == 2, "A real later Yiji acquisition restores its own injury benefit.");
        // Acquired skills disappear from the visible list when suppressed.
        // Restore the native prefix retaining the printed Yiji source to test
        // the stronger case: a visible held skill that is currently inactive.
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(printedYijiCheckpoint), registry);
        Change("suppress");
        Require(game.CreateSnapshot(0).Players[0].Skills!.Any(s => s.ContentId == "classic:yiji") &&
            Own() == new WushuangBuildProfile(0, 0, 0, false, 0),
            "A visible but currently HP-suppressed Yiji contributes no active injury income, including after cold restoration.");
    }

    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(ContentRegistry source) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wushuang-build", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.CopyModeFeatureFactoriesFrom(source);
            foreach (var card in source.Cards.Values) b.AddCard(card);
            foreach (var skill in source.Skills.Values) b.AddSkill(skill);
            foreach (var general in source.Generals.Values)
                b.AddGeneral(general.Id == "classic:guo-jia" ? general with
                    { BaseHp = 30, AdditionalSkillIds = (general.AdditionalSkillIds ?? []).Append(Driver).ToArray() } :
                    general.Id == "boundary:xu-sheng" ? general with { BaseHp = 30 } : general);
            foreach (var deck in source.Decks.Values) b.AddDeck(deck);
            const string deckId = "fixture:wushuang-build-deck";
            b.AddDeck(new(deckId, "Fixed native build fixture", 4, 0, [])
                { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            foreach (var mode in source.Modes.Values) b.AddMode(mode.Id == WushuangMode.ModeId ? mode with
                { GeneralPoolIds = ["classic:guo-jia", "boundary:xu-sheng"], GeneralCandidateCount = 2, DeckId = deckId } : mode);
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
             {"id":"{{{Driver}}}","revision":1,"activations":[
              {"id":"add","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["boundary:pojun","classic:kujin","classic:niepan","{{{Once}}}"]}]},
              {"id":"drop","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["classic:yiji","boundary:pojun","classic:kujin","classic:niepan","{{{Once}}}"],"sourceBind":"{{{Neutral}}}"}]},
              {"id":"restore-yiji","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["classic:yiji"]}]},
              {"id":"suppress","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["{{{Suppressor}}}"]}]}]},
             {"id":"{{{Once}}}","revision":1,"triggers":[{"id":"one-injury","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamagePoint","usageScope":"game","usageLimit":1,"optional":true,"effects":[{"op":"draw","target":"owner","amount":9}]}]}
            ]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
                { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new[] { Driver, Once }.ToDictionary(id => id, id => new { name = id, description = "Native dynamic build fixture" }) }));
            foreach (var (id, program) in catalog.Programs)
                b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddSkill(new(Neutral, "No build trait", "Native replacement receipt target."));
            b.AddSkill(new(Suppressor, "Current HP suppression", "Actual active-binding qualification.") { SuppressionRule = new(30) });
        }
    }
}
