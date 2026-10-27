using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceYanyuChecks
{
    private const string Skill = StandardShanheSourceZhangYanPackage.YanyuSkillId;
    private const string Deck = "fixture:source-yanyu-deck", Watch = "fixture:source-yanyu-gain";

    public static void LockedRedAndBlackUseExactJudgmentsAndStopAtThree()
    {
        foreach (var red in new[] { false, true })
        {
            var scene = Scene.Start(red ? Suit.Heart : Suit.Spade, hp: 8, ally: false, watch: red);
            scene.Reach(() => scene.Game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                scene.Game.ResolutionStack.Count == 0);
            var results = scene.Events<JudgmentResolvedEvent>().Where(fact => fact.Reason == "shanhe.source-yanyu-3292").ToArray();
            var settled = scene.Events<ShanheSourceYanyuCardSettledEvent>();
            Need(results.Length == (red ? 3 : 1) && settled.Length == results.Length &&
                scene.Events<ShanheSourceYanyuDamageRequestedEvent>().Length == (red ? 3 : 0) &&
                scene.Game.CreateSnapshot(0).Players[0].Hp == (red ? 5 : 8),
                "Locked Yanyu judges the actual other preparation actor, stops at the first black result, and caps an all-red sequence at three actual fire attempts.");
            var damages = scene.Events<DamageAppliedEvent>();
            Need(damages.Length == (red ? 3 : 0) && damages.All(fact => fact.SourceSeat == scene.Owner &&
                fact.TargetSeat == 0 && fact.Amount == 1 && fact.Nature == DamageNature.Fire),
                "Every red judgment really deals source-owned native fire damage; black creates no damage.");
            foreach (var result in results)
            {
                var receipt = settled.Single(fact => fact.JudgmentFrameId == result.ResolutionId);
                var card = result.CardId!.Value;
                var moves = scene.Game.CardMovements.Where(move => move.CardId == card).ToArray();
                Need(receipt.CardId == card && receipt.Red == red && receipt.MovementSequence is { } sequence &&
                    moves.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Judgment(0)) == 1 &&
                    moves.Count(move => move.From == CardLocation.Judgment(0) && move.To == CardLocation.Processing) == 1 &&
                    moves.Count(move => move.Sequence == sequence && move.From == CardLocation.Processing &&
                        move.To == (red ? CardLocation.Hand(scene.Owner) : CardLocation.DiscardPile)) == 1 &&
                    moves.Count(move => move.From == CardLocation.Processing) == 1,
                    "Each final judged entity moves from its real judgment through its original program binding and reaches its single correct destination exactly once.");
            }
            Need(scene.Events<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == Skill) == 1 &&
                scene.Game.ResolutionStack.Count == 0 && scene.Game.State.Winner == Winner.None,
                "The whole bounded trigger returns to the native human Play boundary once.");
            if (red)
                Need(scene.Game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(scene.Owner) &&
                    move.Reason.Value == "skill-program." + Watch + ".Draw") == 1 &&
                    scene.Game.CreateSnapshot(scene.Owner).Players[scene.Owner].HandCount == 4,
                    "The genuine obtained-card child draws once and returns to the original bounded sequence before the following judgment.");
        }
    }

    private sealed class Scene(GameEngine game, int owner)
    {
        public GameEngine Game { get; } = game;
        public int Owner { get; } = owner;
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public static Scene Start(Suit suit, int hp, bool ally, bool watch)
        {
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-yanyu", "classic:guan-yu", hp,
                hp > 4 ? hp - 4 : 0, 0, 0, 0, [], [], [],
                [new(StandardShanheSourceZhangYanPackage.ZhangYanBaseId)
                    { StartingHand = [], ExtraSkillIds = watch ? [Watch] : [] }],
                ally ? [new("classic:ma-chao") { StartingHand = [] }] : [], "fixture:source-yanyu");
            return Boot(setup, suit, ally ? 2 : 1);
        }
        public static Scene StartLethal(bool otherEnemy)
        {
            var enemies = new List<ShanheCombatantDefinition>
                { new("classic:guan-yu", Hp: 1) { StartingHand = [] } };
            if (otherEnemy) enemies.Add(new("classic:ma-chao", MaxHpBonus: 16, Hp: 20) { StartingHand = [] });
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-yanyu-lethal", StandardShanheSourceZhangYanPackage.ZhangYanBaseId,
                4, 0, 0, 0, 0, [], [], [], enemies, [], "fixture:source-yanyu");
            var scene = Boot(setup, Suit.Diamond, 0);
            scene.Reach(() => scene.Game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                scene.Game.ResolutionStack.Count == 0);
            return scene;
        }
        private static Scene Boot(ShanheBattleSetup setup, Suit suit, int owner)
        {
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
                { EnemyRoster = setup.EnemyRoster.Select(enemy => enemy with { ExtraSkillIds = [] }).ToArray() })
                .WithExtension(new Fixture(suit), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 1 + setup.EnemyRoster.Count + setup.AllyRoster.Count, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry);
            var scene = new Scene(game, owner); scene.Submit(new StartGameCommand()); return scene;
        }
        public void EndOwnPlay() => Submit(new EndPlayPhaseCommand(0, Game.Revision, Game.CreateSnapshot(0).PendingDecision!.PromptId));
        public void Reach(Func<bool> stop)
        {
            for (var step = 0; step < 240; step++)
            {
                if (stop()) return;
                if (Game.State.Winner != Winner.None)
                    throw new InvalidOperationException("Yanyu's native terminal command did not fulfill its assertion boundary: " + Diagnostic());
                var prompt = Game.CreateSnapshot(0).PendingDecision;
                if (prompt is { PlayerSeat: 0 } && prompt.Kind != DecisionKind.PlayCard)
                {
                    if (prompt.SkillPrompt?.SkillId == Skill && prompt.Choices is [var selected] && selected.Targets.Count == 1)
                    { Submit(new AnswerPromptCommand(0, prompt.PromptId, selected.Id, Game.Revision)); continue; }
                    var choice = prompt.Choices.FirstOrDefault(option => option.Cards.Count == 0 &&
                        option.Parameters.GetValueOrDefault("response") is "pass" or "let-die" or "take-damage");
                    Need(choice is not null, "Unexpected native Yanyu human boundary: " + JsonSerializer.Serialize(prompt));
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, Game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Source Yanyu exceeded its bounded native progression: " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        {
            Game.State.Winner, Game.State.CurrentSeat, Game.State.Phase, Game.State.TurnNumber,
            Owner, Players = Game.CreateSnapshot(Owner).Players.Select(player => new { player.Seat, player.GeneralId, player.IsAlive, player.Hp, player.HandCount }),
            Pending = Game.CreateSnapshot(0).PendingDecision,
            Settled = Events<ShanheSourceYanyuCardSettledEvent>(), Requested = Events<ShanheSourceYanyuDamageRequestedEvent>(),
            Stack = Game.ResolutionStack.Select(frame => new { Type = frame.GetType().Name, frame.Id,
                Program = frame is ProgramSkillFrame program ? new { program.SkillId, program.InstructionIndex,
                    program.ShanheSourceYanyu, program.AttackAttempt, program.PendingMovementContinuation } : null }),
            LastEvents = Game.Events.TakeLast(12).Select(envelope => new { Type = envelope.Payload.GetType().Name, Value = JsonSerializer.Serialize(envelope.Payload, envelope.Payload.GetType()) }),
            LastMoves = Game.CardMovements.TakeLast(8)
        });
        private void Submit(GameCommand command)
        { var result = Game.Submit(command); if (!result.Accepted) throw new InvalidOperationException((result.Error?.Message ?? "Rejected native source Yanyu command.") + " " + Diagnostic()); }
    }

    private sealed class Fixture(Suit suit) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-yanyu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"native-gain-child","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","usageScope":"turn","usageLimit":1,"optional":false,
                    "effects":[{"op":"draw","target":"owner","amount":1}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实获牌子结算", description = "本回合首次获得牌后摸一张牌。" } } }));
            builder.AddSkill(new(Watch, "真实获牌子结算", "本回合首次获得牌后摸一张牌。")
                { Program = programs.Programs[Watch], ProgramPresentation = programs.Presentations[Watch] });
            builder.AddDeck(new(Deck, "固定判定实体", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", suit, 7)).ToArray() });
        }
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
