using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ProgramComposedConsumerChecks
{
    private const string Hero = "mou:chen-gong-mobile", Mingce = "mou-mobile:mingce";
    private const string Ally = "fixture:composed-ally", EnemyA = "fixture:composed-enemy-a", EnemyB = "fixture:composed-enemy-b";
    private const string HpChild = "fixture:composed-hp-child", GiftChild = "fixture:composed-gift-child", Quiet = "fixture:composed-quiet";
    private const string DrawReason = "skill-program.mou-mobile:mingce.Draw", GiftReason = "skill-program.mou-mobile:mingce.give";

    public static void PaidMingceWaitsForHpChildrenAndResumesOrdinaryEffects()
    {
        foreach (var scenario in new[] { "normal", "source-loss", "owner-death" })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scenario));
            var game = Start(registry, scenario);
            Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            Accept(game, new UseProgramSkillCommand(0, Mingce, "gift-and-plan", [], [1], game.Revision, P(game)!.PromptId));
            Reach(game, () => P(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "mou-chen-gong-gift") == true);
            var given = P(game)!.Choices.First().Cards.Single();
            Answer(game, P(game)!.Choices.First());
            if (scenario == "source-loss")
            {
                Reach(game, () => P(game)?.SkillPrompt?.SkillId == GiftChild);
                var owner = ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine.GetField("_players",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(game)!)[0];
                foreach (var grant in owner.SkillGrants.Grants.Where(g => g.SkillId == Mingce).ToArray())
                    owner.SkillGrants.SetEnabled(grant.GrantId, false);
                Answer(game, P(game)!.Choices.Single());
            }
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == HpChild);
            var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Mingce);
            var child = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single(w => w.Change.ParentFrameId == root.Id);
            Need(child.Change is { Kind: HpChangeKind.Loss, TargetSeat: 1, Amount: 1, HpBefore: 4, HpAfter: 3 } &&
                E<ProgramOptionChosenEvent>(game).Single(e => e.FrameId == root.Id).OptionId == "lose-hp" &&
                E<MouChenGongGiftPaidEvent>(game).Count(e => e.FrameId == root.Id && e.CardId == given) == 1 &&
                !Draws(game).Any() && Ce(game) == 0,
                "The real Give is paid once, then the recipient's native HP child pauses before the owner's ordinary Draw2 and Ce.");
            if (scenario == "source-loss")
                Need(!game.CreateSnapshot(0).Players[0].Skills!.Any(s => s.Id == Mingce),
                    "Source loss during the real gift movement child precedes the recipient's paid ordinary choice.");
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(game.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == root.Id) &&
                game.CardMovements.Count(m => m.CardId == given && m.Reason.Value == GiftReason) == 1,
                "Cold restoration retains the original paid frame and original gifted entity.");
            var resumed = game;
            Reach(game, () => E<ProgramSkillResolvedEvent>(resumed).Any(e => e.FrameId == root.Id) || resumed.State.Winner != Winner.None);
            Need(game.CardMovements.Count(m => m.CardId == given && m.From == CardLocation.Hand(0) && m.To == CardLocation.Hand(1) && m.Reason.Value == GiftReason) == 1 &&
                E<MouChenGongGiftPaidEvent>(game).Count(e => e.FrameId == root.Id) == 1 &&
                E<ProgramSkillHpLostEvent>(game).Count(e => e.FrameId == root.Id && e.TargetSeat == 1 && e.Amount == 1) == 1,
                "Returning from the saved HP child never repays the Give or recipient HP loss.");
            if (scenario == "owner-death")
            {
                Need(!game.State.Players[0].IsAlive && !Draws(game).Any() && Ce(game) == 0,
                    "Death inside the recipient's HP child prevents the later owner Draw2 and Ce.");
                continue;
            }
            var events = game.Events.Select(e => e.Payload).ToArray();
            var hpReturn = Array.FindIndex(events, e => e is ProgramBindingResolvedEvent r && r.SkillId == HpChild && r.Completed);
            var drawReturn = Array.FindIndex(events, e => e is ProgramDrawCompletedEvent d && d.FrameId == root.Id && d.RequestedCount == 2);
            var marker = Array.FindIndex(events, e => e is PlayerMarkerChangedEvent m && m.ResolutionId == root.Id && m.Marker == PlayerMarkerKind.MouChenGongCe);
            Need(hpReturn >= 0 && hpReturn < drawReturn && drawReturn < marker && Draws(game).Count() == 2 && Ce(game) == 1 &&
                E<ProgramSkillResolvedEvent>(game).Count(e => e.FrameId == root.Id && e.Completed) == 1,
                "The complete HP child returns before one native Draw2, then one Ce, including after original source loss.");
        }
    }

    private static IEnumerable<CardMovementRecord> Draws(GameEngine game) => game.CardMovements.Where(m => m.Reason.Value == DrawReason && m.To == CardLocation.Hand(0));
    private static int Ce(GameEngine game) => game.CreateSnapshot(0).Players[0].Markers?.SingleOrDefault(m => m.Kind == PlayerMarkerKind.MouChenGongCe)?.Count ?? 0;
    private static IEnumerable<T> E<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static void Answer(GameEngine game, PromptChoice choice)
    {
        var prompt = P(game)!;
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }
    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 160; step++)
        {
            if (reached()) return;
            if (P(game) is { PlayerSeat: 0 } prompt)
            {
                var pass = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") == "let-die");
                Need(pass is not null, "Unexpected human boundary: " + JsonSerializer.Serialize(prompt));
                Answer(game, pass!);
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded real Mingce composition did not reach its expected boundary.");
    }
    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
        Need(result.Accepted, result.Error?.Message ?? "The real Mingce fixture command was rejected.");
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private static GameEngine Start(ContentRegistry registry, string scenario)
    {
        var setup = new ShanheBattleSetup(7, 1, "composed-mingce", Hero, 3, 0, 0, 0, 0,
            scenario == "source-loss" ? [GiftChild] : [],
            [new(CardKind.Dodge, Suit.Club, 7), new(CardKind.Dodge, Suit.Heart, 8)], [],
            [new(EnemyA, Hp: 8) { StartingHand = [] }, new(EnemyB, Hp: 8) { StartingHand = [] }],
            [new(Ally, Hp: 4) { StartingHand = [] }], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
        Accept(game, new StartGameCommand());
        return game;
    }

    private sealed class Fixture(string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-composed-mingce", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var death = scenario == "owner-death" ? ",{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"otherPublicTeamAllies\"},{\"op\":\"loseHp\",\"target\":\"selectedTarget\",\"amount\":3}" : "";
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{HpChild}}","revision":1,"triggers":[{"id":"recipient-loss","window":"afterHpLost","subject":"owner","optional":false,
                    "usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"hp-return","options":[{"id":"continue"}]}{{death}}]}]},
                  {"id":"{{GiftChild}}","revision":1,"triggers":[{"id":"paid-source-loss","window":"cardsMoved","subject":"owner","optional":false,
                    "sourceZones":["hand"],"movementReasons":["{{GiftReason}}"],"movementOccurrence":"perBatch",
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"gift-return","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """;
            var optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" };
            var labels = new Dictionary<string, object> { [HpChild] = new { name = "HP child", description = "Pause the native recipient loss", optionLabels },
                [GiftChild] = new { name = "Paid source loss", description = "Pause after the real Give", optionLabels }, [Quiet] = new { name = "Quiet", description = "Skip unrelated turns" } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Ally, "Mingce recipient", "", Quiet, "qun", 4, [HpChild]));
            foreach (var id in new[] { EnemyA, EnemyB }) builder.AddGeneral(new(id, "Other", "", Quiet, "qun", 8));
            const string deck = "fixture:composed-mingce-deck";
            builder.AddDeck(new(deck, "Fixed Dodge deck", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Mingce composition", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Ally, EnemyA, EnemyB], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
