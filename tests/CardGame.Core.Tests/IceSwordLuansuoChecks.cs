using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class IceSwordChecks
{
    public static void ProtectedHandAfterOneEquipmentDiscardFinishesNativePreventionCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new LuansuoFixture());
        var setup = new ShanheBattleSetup(19, 1, "ice-sword-protected-hand", "fixture:ice-luan-owner", 8, 0, 0, 0, 0, [],
            [new(CardKind.Crossbow, Suit.Spade, 7)], [new(CardKind.IceSword, Suit.Club, 2)],
            [new("fixture:ice-luan-peer", Hp: 8) { StartingHand = [new(CardKind.Peach, Suit.Heart, 3)] },
             new("fixture:ice-luan-peer", Hp: 8) { StartingHand = [] }],
            [new("fixture:ice-luan-peer", Hp: 8) { StartingHand = [] }], new string('c', 64));
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        void Accept(GameCommand command) { var result = game.Submit(command); Require(result.Accepted, result.Error?.Message ?? "Rejected protected-hand fixture command."); }
        void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 90; i++) { if (ready()) return; Require(Prompt() is null, "Unexpected protected-hand native boundary."); Accept(new AdvanceOneStepCommand(game.Revision)); }
            throw new InvalidOperationException("The bounded protected-hand fixture missed its native boundary.");
        }
        void Answer(Func<PromptChoice, bool> select) { var p = Prompt()!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(select).Id, game.Revision)); }
        void Cold()
        {
            var before = SnapshotJson.Serialize(game.CreateSnapshot(0));
            game = GameReplay.Restore(RoundTrip(game.CreateCheckpoint()), registry);
            Require(before == SnapshotJson.Serialize(game.CreateSnapshot(0)), "Cold retains the real prevention, exact legal prompt and protected opaque hand.");
        }
        Accept(new StartGameCommand()); Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        var crossbow = game.CreateSnapshot(0).Players[0].Hand.Single(c => c.Kind == CardKind.Crossbow).Id;
        Accept(new UseProgramSkillCommand(0, "fixture:ice-luan-driver", "place", [crossbow], [2], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        Accept(new UseProgramSkillCommand(0, "fixture:ice-luan-driver", "arm", [], [], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        var hp = game.CreateSnapshot(0).Players[2].Hp;
        Accept(new UseProgramSkillCommand(0, "fixture:ice-luan-driver", "slash", [], [2], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.IceSword });
        Answer(c => c.Parameters.GetValueOrDefault("action") == "ice-sword-activate");
        Require(Prompt()!.Choices is [var only] && only.Cards.SequenceEqual([crossbow]) &&
            only.Parameters.GetValueOrDefault("target-zone") == "equipment",
            "The real Luansuo hand prohibition leaves only the public equipment as a legal first discard; no protected opaque slot is published.");
        Cold(); Answer(c => c.Cards.SequenceEqual([crossbow]));
        Cold(); Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        var resolved = game.Events.Select(e => e.Payload).OfType<IceSwordResolvedEvent>().Single();
        Require(resolved.Used && resolved.DiscardedCardIds.SequenceEqual([crossbow]) &&
            game.CardMovements.Count(m => m.CardId == crossbow && m.Reason == CardMoveReasons.IceSwordDiscard) == 1 &&
            !game.CardMovements.Any(m => m.From == CardLocation.Hand(2) && m.To == CardLocation.DiscardPile) &&
            game.CreateSnapshot(0).Players[2] is { Hp: var after, HandCount: 1 } && after == hp && game.ResolutionStack.Count == 0,
            "After one real discard and cold restoration, only protected hand remains: native prevention closes once without republishing an impossible second payment or dealing damage.");
    }

    private sealed class LuansuoFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ice-luansuo", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "anniversary:shen-pang-tong");
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"fixture:ice-luan-driver","revision":1,"activations":[
             {"id":"place","minCards":1,"maxCards":1,"sourceZones":["hand"],"cardCategories":["equipment"],"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"cost"},{"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"cost"}]},
             {"id":"arm","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["anniversary:luansuo"]}]},
             {"id":"slash","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"normalSlashTarget","useCardActionWindows":true}]}]}]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { ["fixture:ice-luan-driver"] = new { name = "Native Ice Sword inputs", description = "Real placement, skill grant and Slash." } } }));
            b.AddSkill(new("fixture:ice-luan-driver", "Native Ice Sword inputs", "Real inputs") { Program = catalog.Programs["fixture:ice-luan-driver"], ProgramPresentation = catalog.Presentations["fixture:ice-luan-driver"] });
            b.AddGeneral(new("fixture:ice-luan-owner", "Native source", "", "fixture:ice-luan-driver", "wei", 8));
            b.AddGeneral(new("fixture:ice-luan-peer", "Native target", "", "standard:none", "shu", 8));
            var ice = CardCatalog.Get(CardKind.IceSword); b.AddCard(new("fixture:ice-luan-sword", ice.DisplayName, ice.CategoryName, ice.Description, CardKind.IceSword));
            b.AddDeck(new("fixture:ice-luan-deck", "Fixed quiet deck", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Native protected hand", 4, 4, new Dictionary<string, int>(), "fixture:ice-luan-deck",
                GeneralPoolIds: ["fixture:ice-luan-owner", "fixture:ice-luan-peer"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
