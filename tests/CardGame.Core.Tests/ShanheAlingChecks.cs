using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheAlingChecks
{
    private const string Rebirth = StandardShanheExpandedCompanionPackage.NiepanSkillId, Driver = "fixture:aling-driver";
    public static void DyingNiepanRestoresBothStatesAndDrawsBeforeRecovery()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "aling-niepan", "fixture:aling-owner", 3, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Lightning, Suit.Spade, 1)],
            [new(CardKind.QinggangSword, Suit.Spade, 6)], [new("fixture:aling-peer") { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);
        Submit(game, new StartGameCommand()); Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
        var lightning = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Lightning);
        Submit(game, new PlayCardCommand(0, lightning.CardId!.Value, lightning.TargetSeats, game.Revision, game.PendingDecision!.PromptId));
        Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
        var original = game.CreateSnapshot(0).Players[0];
        var ids = original.Hand.Concat(original.Equipment).Concat(original.Judgment).Select(card => card.Id).ToArray();
        Need(original.HandCount == 1 && original.Equipment.Count == 1 && original.Judgment.Count == 1,
            "Aling starts this focused dying regression with actual cards in all three owned zones.");
        Submit(game, new UseProgramSkillCommand(0, Driver, "suffer", [], [], game.Revision, game.PendingDecision!.PromptId));
        Reach(game, pending => pending.Kind == DecisionKind.RescueDying &&
            pending.Choices.Any(choice => choice.Parameters.GetValueOrDefault("skill-id") == Rebirth));
        Need(game.CreateSnapshot(0).Players[0] is { Hp: 0, IsChained: true, IsFaceDown: true },
            "Real lethal damage reaches self-dying with both altered character states.");
        var prompt = game.PendingDecision!;
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId,
            prompt.Choices.First(choice => choice.Parameters.GetValueOrDefault("skill-id") == Rebirth).Id, game.Revision));
        Reach(game, pending => pending.Kind == DecisionKind.PlayCard);
        var result = game.CreateSnapshot(0).Players[0]; var events = game.Events.Select(envelope => envelope.Payload).ToArray();
        Need(result is { Hp: 3, HandCount: 3, IsChained: false, IsFaceDown: false, Equipment.Count: 0, Judgment.Count: 0 } &&
            ids.All(id => game.CardMovements.Count(move => move.CardId == id && move.To == CardLocation.DiscardPile &&
                move.Reason.Value == "skill-program." + Rebirth + ".DiscardOwnedZoneCards") == 1),
            "The accepted limited rescue discards each original HEJ card once, turns face up, unchains, draws three and returns to three health.");
        var draw = Array.FindIndex(events, value => value is CardsDrawnEvent { PlayerSeat: 0, Count: 3 });
        var recovery = Array.FindIndex(events, value => value is RecoveryAppliedEvent { TargetSeat: 0, RemainingHp: 3 });
        Need(draw >= 0 && recovery > draw && game.CardMovements.Count(move => move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program." + Rebirth + ".Draw") == 3,
            "The actual three-card native draw precedes recovery, matching the companion's published order.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Aling native command rejected."); }
    private static void Reach(GameEngine game, Func<PendingDecision, bool> stop)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.PendingDecision is { } prompt && stop(prompt)) return;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Aling did not reach the real dying rescue or resumed Play boundary.");
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:aling-niepan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var assembly = typeof(StandardShanheExpandedCompanionPackage).Assembly;
            using var rules = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shanhe-companion-niepan.rules.json")!);
            using var presentation = new StreamReader(assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.shanhe-companion-niepan.presentation.json")!);
            var catalog = SkillProgramCatalog.Load(rules.ReadToEnd(), presentation.ReadToEnd()); var shown = catalog.Presentations[Rebirth];
            builder.AddSkill(new(Rebirth, shown.Name, shown.Description) { Program = catalog.Programs[Rebirth], ProgramPresentation = shown, Tags = SkillTag.Limited });
            var driver = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                "activations":[{"id":"suffer","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                  "effects":[{"op":"setChainedState","target":"owner","chained":true},{"op":"setFaceState","target":"owner","faceDown":true},
                    {"op":"damage","target":"owner","amount":3}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "实际濒死驱动", description = "连环、翻面后受到真实伤害" } } }));
            builder.AddSkill(new(Driver, "实际濒死驱动", "真实状态和伤害") { Program = driver.Programs[Driver] });
            builder.AddGeneral(new("fixture:aling-owner", "阿绫涅槃", "", Rebirth, "qun", 3, [Driver]));
            builder.AddGeneral(new("fixture:aling-peer", "固定对手", "", null, "wei", 10));
            builder.AddDeck(new("fixture:aling-deck", "固定实体桃", 0, 0, [new("standard:peach", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "阿绫濒死复原", 2, 2, new Dictionary<string, int>(), "fixture:aling-deck",
                GeneralPoolIds: ["fixture:aling-owner", "fixture:aling-peer"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
