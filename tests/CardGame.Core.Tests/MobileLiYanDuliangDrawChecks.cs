using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileLiYanDuliangDrawChecks
{
    private const string Duliang = "mobile:duliang";
    private const string Owner = "fixture:mobile-li-yan-owner";
    private const string Driver = "fixture:mobile-li-yan-driver";
    private const string Clock = "fixture:mobile-li-yan-clock";
    private const string Pause = "fixture:mobile-li-yan-draw";
    private const string Mode = "identity:mobile-li-yan";

    public static void InsertedDrawConsumesStackBeforeNaturalDrawCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var scene = new Scene(GameEngine.CreateStandard(new()
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 10
        }, registry));
        scene.Submit(new StartGameCommand());

        scene.Play(0);
        scene.Grant();
        scene.Play(0);
        scene.EndPlay();
        scene.Play(0);
        scene.Grant();
        scene.Play(0);
        scene.Activate(Driver, "lose-duliang", []);
        scene.Play(0);

        var issued = scene.Events<MobileLiYanNextDrawBonusIssuedEvent>();
        var ownerPlay = scene.Events<ActualDiscardRecoveryPhaseStartedEvent>()
            .Where(entry => entry.Phase.ActorSeat == 0 && entry.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play)
            .Select(entry => entry.Phase).ToArray();
        var transfers = scene.Game.CardMovements
            .Where(move => move.From == CardLocation.Hand(1) && move.To == CardLocation.Hand(0)).ToArray();
        Need(ownerPlay is [var insertedPlay, var normalPlay] && insertedPlay.Token != normalPlay.Token &&
            insertedPlay.TurnNumber == normalPlay.TurnNumber &&
            issued.Length == 2 && issued.All(grant => grant.TargetSeat == 1 && grant.Amount == 1) &&
            issued.Select(grant => grant.ProgramFrameId).Distinct().Count() == 2 &&
            transfers.Length == 2 && transfers.Select(move => move.CardId).Distinct().Count() == 2 &&
            scene.Events<MobileLiYanNextDrawBonusAssignedEvent>().Length == 0 &&
            scene.Game.CreateSnapshot(0).Players[0].Skills!.All(skill => skill.Id != Duliang),
            "Two real Play phases transfer different hand cards and issue two grants before Li Yan loses Duliang.");

        var beforeInserted = scene.Game.CardMovements.Count;
        scene.EndPlay();
        scene.ReachDrawStart();
        var first = scene.RecipientDraws().Single();
        var assignments = scene.Events<MobileLiYanNextDrawBonusAssignedEvent>();
        Need(scene.Game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(frame =>
                frame.OwnerSeat == 1 && frame.Window == SkillProgramTriggerWindow.DrawPhaseStarting) &&
            assignments.Length == 2 && assignments.All(grant => grant.Phase == first) &&
            assignments.Select(grant => (grant.ProgramFrameId, grant.InstructionIndex))
                .SequenceEqual(issued.Select(grant => (grant.ProgramFrameId, grant.InstructionIndex))) &&
            scene.DrawnCardsSince(beforeInserted).Length == 0,
            "The first actual Draw owns both original grants while its starting window is open, before any native draw.");

        var original = scene.Game;
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(original.CreateSnapshot(seat)) ==
                SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(original.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack) &&
            original.CardMovements.SequenceEqual(restored.CardMovements) &&
            assignments.SequenceEqual(restored.Events.Select(entry => entry.Payload).OfType<MobileLiYanNextDrawBonusAssignedEvent>()),
            "Cold replay restores the first actual Draw and both original assignments.");
        scene.Game = restored;

        scene.AnswerOption("continue");
        scene.ReachDrawStart();
        var phases = scene.RecipientDraws();
        var insertedCards = scene.DrawnCardsSince(beforeInserted);
        Need(phases.Length == 2 && phases[0] == first && phases[1].Token != first.Token &&
            phases[1].TurnNumber == first.TurnNumber && insertedCards.Length == 4,
            "The inserted Draw moves four physical deck cards before the separate natural Draw starts in the same turn.");

        var beforeNormal = scene.Game.CardMovements.Count;
        scene.AnswerOption("continue");
        scene.Play(1);
        var normalCards = scene.DrawnCardsSince(beforeNormal);
        Need(normalCards.Length == 2 && insertedCards.Concat(normalCards).Select(move => move.CardId).Distinct().Count() == 6 &&
            scene.Events<CardsDrawnEvent>().Where(draw => draw.PlayerSeat == 1).Select(draw => draw.Count).TakeLast(2).SequenceEqual([4, 2]) &&
            scene.Events<MobileLiYanNextDrawBonusAssignedEvent>().SequenceEqual(assignments),
            "The natural Draw moves only two physical deck cards, with no reassignment of the two issued grants.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; set; } = game;
        private PendingDecision? Pending => Enumerable.Range(0, 4)
            .Select(seat => Game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);

        public T[] Events<T>() => Game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

        public ActualDiscardRecoveryPhaseKey[] RecipientDraws() => Events<ActualDiscardRecoveryPhaseStartedEvent>()
            .Where(entry => entry.Phase.ActorSeat == 1 && entry.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw)
            .Select(entry => entry.Phase).ToArray();

        public CardMovementRecord[] DrawnCardsSince(int index) => Game.CardMovements.Skip(index)
            .Where(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
                move.Reason == CardMoveReasons.Draw).ToArray();

        public void Grant()
        {
            Activate(Duliang, "take-hand-and-benefit", [1]);
            Reach(prompt => prompt?.Choices.Any(choice =>
                choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card" &&
                choice.Parameters.GetValueOrDefault("skip") != "true");
            Reach(prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "next-draw") == true);
            AnswerOption("next-draw");
        }

        public void Activate(string skill, string binding, int[] targets) =>
            Submit(new UseProgramSkillCommand(0, skill, binding, [], targets, Game.Revision, Pending!.PromptId));

        public void Play(int seat) => Reach(prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == seat);

        public void ReachDrawStart() => Reach(prompt => prompt?.SkillPrompt?.SkillId == Pause);

        public void AnswerOption(string option) => Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == option);

        private void Answer(Func<PromptChoice, bool> choose)
        {
            var prompt = Pending!;
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(choose).Id, Game.Revision));
        }

        public void EndPlay()
        {
            var prompt = Pending!;
            Submit(new EndPlayPhaseCommand(prompt.PlayerSeat, Game.Revision, prompt.PromptId));
        }

        public void Submit(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? $"Duliang fixture command was rejected: {command}.");
        }

        private void Reach(Func<PendingDecision?, bool> ready)
        {
            for (var step = 0; step < 260; step++)
            {
                var prompt = Pending;
                if (ready(prompt))
                    return;
                switch (prompt)
                {
                    case { Kind: DecisionKind.SelectGeneral }:
                        Submit(new SelectGeneralCommand(prompt.PlayerSeat, prompt.PlayerSeat == 0 ? Owner : Peer(prompt.PlayerSeat),
                            Game.Revision, prompt.PromptId));
                        break;
                    case { Kind: DecisionKind.PlayCard }:
                        EndPlay();
                        break;
                    case { Kind: DecisionKind.DiscardCards }:
                        Submit(new DiscardCardsCommand(prompt.PlayerSeat, prompt.ValidCardIds.Take(prompt.RequiredCardCount).ToArray(),
                            prompt.PromptId, Game.Revision));
                        break;
                    case null:
                        Submit(new AdvanceOneStepCommand(Game.Revision));
                        break;
                    default:
                        var choice = prompt.Choices.FirstOrDefault(candidate => candidate.Parameters.GetValueOrDefault("program-action") == "skip") ??
                            prompt.Choices.FirstOrDefault(candidate => candidate.Cards.Count == 0 && candidate.Targets.Count == 0) ?? prompt.Choices.First();
                        Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, Game.Revision));
                        break;
                }
            }
            throw new InvalidOperationException($"Missing Duliang fixture boundary in turn {Game.State.TurnNumber}: {Pending?.Prompt}");
        }
    }

    private static string Peer(int seat) => "fixture:mobile-li-yan-peer-" + seat;

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-li-yan", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:li-yan", "classic:li-yan");
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"retain-materials","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "triggers":[{"id":"one-extra-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}],
                   "activations":[{"id":"lose-duliang","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{Duliang}}"],"sourceBind":"standard:none"}]}]},
                  {"id":"{{Clock}}","revision":1,"triggers":[{"id":"one-extra-draw","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"draw","phaseContinuation":"beforeNormalPreparation"}]}]},
                  {"id":"{{Pause}}","revision":1,"triggers":[{"id":"frozen-next-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"next-draw-pause","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var labels = new Dictionary<string, object>
            {
                [Driver] = new { name = "两次真实出牌阶段", description = "原生阶段插入及真实失技" },
                [Clock] = new { name = "额外摸牌阶段", description = "插入真实摸牌阶段" },
                [Pause] = new { name = "摸牌开始", description = "原生摸牌阶段暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = labels
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program });
            builder.AddGeneral(new(Owner, "移动李严实际阶段", "supporter", Duliang, "shu", 8, ["classic:fulin", Driver]));
            builder.AddGeneral(new(Peer(1), "未来真实摸牌受益者", "supporter", Clock, "wei", 8, [Pause]));
            for (var seat = 2; seat < 4; seat++)
                builder.AddGeneral(new(Peer(seat), "固定同桌", "supporter", "standard:none", "wei", 8));

            const string deck = "fixture:mobile-li-yan-deck";
            builder.AddDeck(new(deck, "同质原生实体闪", 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray()
            });
            builder.AddMode(new(Mode, "移动李严下一真实摸牌阶段", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
