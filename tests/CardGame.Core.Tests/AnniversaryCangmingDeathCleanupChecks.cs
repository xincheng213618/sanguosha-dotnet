using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class AnniversaryCangmingDeathCleanupChecks
{
    private const string Sea = "anniversary:cangming";
    private const string Driver = "fixture:cangming-death-driver", Victim = "fixture:cangming-death-victim";
    private const string Survivor = "fixture:cangming-death-survivor", Deck = "fixture:cangming-death-deck";
    private const string Deposit = "skill-program.anniversary-sun-quan.ming";
    private const string Discard = "skill-program.anniversary-sun-quan.ming-death";

    internal static void XingshangKeepsEditionScopeForOriginalMingDeathDiscardCold()
    {
        Check("classic:cao-pi", "classic:xingshang", claimsMing: true);
        Check("mobile:cao-pi", "mobile:xingshang", claimsMing: false);
    }

    private static void Check(string hero, string xingshang, bool claimsMing)
    {
        var (game, content) = Start(hero);
        var play = Next(game);
        var deposit = game.CardMovements.Single(move =>
            move.Reason.Value == Deposit && move.From == CardLocation.Hand(1));
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            deposit.To is { Zone: CardZoneKind.PrivatePersistentPile, OwnerSeat: 1 } &&
            game.CardMovements.Last(move => move.CardId == deposit.CardId) == deposit,
            "The genuine opening Cangming deposits the victim's fixed hand card and keeps it there through the first Play phase.");
        Accept(game, new UseProgramSkillCommand(0, Driver, "lose-hp", [], [1], game.Revision, play.PromptId));

        var prompt = Next(game);
        var death = game.ResolutionStack.OfType<DeathFrame>().Single();
        var window = game.ResolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single();
        var ming = game.CardMovements.Single(move => move.CardId == deposit.CardId && move.Reason.Value == Discard);
        var equipment = game.CardMovements.Single(move =>
            move.From == CardLocation.Equipment(1) && move.Reason == CardMoveReasons.DeathEquipmentDiscard);
        Require(prompt is { PlayerSeat: 0 } && prompt.SkillPrompt?.SkillId == xingshang &&
            death.VictimSeat == 1 && window.DeathFrameId == death.Id && !window.BeforeDeathCleanup &&
            ming.From == deposit.To && ming.To == CardLocation.DiscardPile &&
            equipment.To == CardLocation.DiscardPile && equipment.Sequence < ming.Sequence &&
            death.NativeCleanupDiscardEntries is { } entries && entries.SequenceEqual([equipment, ming]) &&
            game.CardMovements.Last(move => move.CardId == ming.CardId) == ming,
            $"{xingshang} opens on the original death with the physical equipment and atomic Ming discards recorded on the same frame.");

        var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(cold.CardMovements.SequenceEqual(game.CardMovements) &&
            cold.ResolutionStack.OfType<DeathFrame>().Single().NativeCleanupDiscardEntries is { } restoredEntries &&
            restoredEntries.SequenceEqual([equipment, ming]) &&
            Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(cold.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold public-command replay preserves the exact death cleanup and each seat's view.");

        Activate(cold);
        Require(Next(cold) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard },
            "The real Xingshang trigger completes and returns to Play.");
        var gains = cold.CardMovements.Where(move =>
            move.Reason.Value == $"skill-program.{xingshang}.claim-died-cards.claim-death-cleanup").ToArray();
        var expected = claimsMing ? new[] { equipment.CardId, ming.CardId } : new[] { equipment.CardId };
        Require(gains.Select(move => move.CardId).SequenceEqual(expected) &&
            gains.All(move => move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0)) &&
            cold.CreateCardZoneDiagnostics().Single(card => card.CardId == ming.CardId).Location ==
                (claimsMing ? CardLocation.Hand(0) : CardLocation.DiscardPile),
            $"{xingshang} must claim precisely the original cleanup entries allowed by its edition.");
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
            {
                var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
                if (pass is null)
                    return prompt;
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, pass.Id, game.Revision));
            }
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Cangming death cleanup did not reach the next public decision.");
    }

    private static (GameEngine Game, ContentRegistry Content) Start(string hero)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(hero));
        var victim = new ShanheCombatantDefinition(Victim, Hp: 1)
        {
            StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)],
            StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 1)]
        };
        var setup = new ShanheBattleSetup(89, 1, "cangming-death-cleanup", hero, 3, 0, 0, 0, 0,
            [Sea, Driver], [new(CardKind.Dodge, Suit.Heart, 3)], [],
            [victim, new(Survivor) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture(string hero) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-cangming-death-cleanup", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            if (hero == "mobile:cao-pi")
                CharacterFixtureContent.Register(builder, hero, "classic:cao-pi", "boundary:cao-pi-mobile", "anniversary:shen-sun-quan");
            else
                CharacterFixtureContent.Register(builder, hero, "anniversary:shen-sun-quan");
            var rules = $$"""
                {"skills":[{"id":"{{Driver}}","revision":1,"activations":[{"id":"lose-hp","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"loseHp","target":"selectedTarget","amount":1}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "失去体力", description = "令另一名角色失去1点体力。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Victim, "溟牌持有者", "supporter", "standard:none", "qun", 4));
            builder.AddGeneral(new(Survivor, "仍在场的角色", "supporter", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定普通摸牌", 0, 4, []) { PhysicalCards =
                [new("standard:dodge", Suit.Heart, 5), new("standard:dodge", Suit.Diamond, 6),
                 new("standard:dodge", Suit.Heart, 7), new("standard:dodge", Suit.Diamond, 8)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "沧溟死亡清理", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [hero, Victim, Survivor], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
