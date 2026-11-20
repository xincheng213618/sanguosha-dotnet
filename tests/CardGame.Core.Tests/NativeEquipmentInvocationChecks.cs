using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class NativeEquipmentInvocationChecks
{
    private const string Hero = "anniversary:shen-hua-tuo", Wolong = "classic:wolong-zhuge-liang";
    private const string Jingyu = "anniversary-god:jingyu", Bazhen = "classic:bazhen";
    private const string Watch = "fixture:native-bagua-watch", Peer = "fixture:native-bagua-peer";
    private const string Deck = "fixture:native-bagua-deck", JingyuDraw = "skill.shen-hua-tuo.jingyu.draw";

    internal static void BaguaKeepsPhysicalAndVirtualEquipmentIdentityCold()
    {
        foreach (var virtualBagua in new[] { false, true })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var dodge = new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 2);
            var setup = new ShanheBattleSetup(31, 1, "native-bagua-invocation", Hero, 3, 0, 0, 0, 0,
                virtualBagua ? [Watch, Bazhen] : [Watch],
                virtualBagua ? [dodge] : [dodge, new(CardKind.BaguaFormation, Suit.Spade, 2)], [],
                [new(Peer) { StartingHand = [new(CardKind.Slash, Suit.Club, 7)] }], [], new string('b', 64))
            { DefaultGodFactionId = "wu" };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry);

            PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
            IEnumerable<T> Facts<T>() where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>();
            void Need(bool success, string message)
            {
                if (!success)
                    throw new InvalidOperationException($"{message} Virtual={virtualBagua}, seat={game.State.CurrentSeat}, " +
                        $"phase={game.State.Phase}, prompt={JsonSerializer.Serialize(Prompt())}, " +
                        $"recent={string.Join(',', game.Events.TakeLast(10).Select(item => item.Payload.GetType().Name))}.");
            }
            void Submit(GameCommand command)
            {
                var result = game.Submit(command);
                Need(result.Accepted, result.Error?.Message ?? "Bagua command rejected.");
            }
            void Reach(Func<bool> done)
            {
                for (var i = 0; i < 80; i++)
                {
                    if (done()) return;
                    Need(Prompt() is null, "An unexpected human prompt interrupted the native equipment sequence.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
                Need(false, "The expected native equipment boundary was not reached.");
            }
            void Answer(Func<PromptChoice, bool> match)
            {
                var prompt = Prompt()!;
                var choice = prompt.Choices.SingleOrDefault(match);
                Need(choice is not null, "The expected native choice was not issued.");
                Submit(new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, game.Revision));
            }

            Submit(new StartGameCommand());
            Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
            var equipmentId = 0;
            if (!virtualBagua)
            {
                equipmentId = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.BaguaFormation).Id;
                Submit(new PlayCardCommand(0, equipmentId, [], game.Revision, Prompt()!.PromptId));
                Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
                Need(game.CardMovements.Count(move => move.CardId == equipmentId &&
                    move.From == CardLocation.Processing && move.To == CardLocation.Equipment(0) &&
                    move.Reason == CardMoveReasons.EquipmentEnter) == 1,
                    "The physical Bagua must enter equipment through its native use.");
            }

            var defender = game.CreateSnapshot(0).Players[0];
            var dodgeId = defender.Hand.Single(card => card.Kind == CardKind.Dodge && card.Suit == Suit.Heart).Id;
            Need(virtualBagua
                ? defender.Equipment.Count == 0 && defender.Skills?.Any(skill => skill.ContentId == Bazhen) == true
                : defender.Equipment.Single().Id == equipmentId,
                "The defender must have the expected real equipment or the real unarmored Bazhen policy.");
            Submit(new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
            Reach(() => Prompt() is { Kind: DecisionKind.RespondDodge, IncomingCard: CardKind.Slash });
            var attack = Facts<CardUseDeclaredEvent>().Single(use => use.SourceSeat == 1 && use.CardKind == CardKind.Slash);
            Answer(choice => choice.Parameters.GetValueOrDefault("response") == "bagua");
            Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch);

            var issued = Facts<NativeEquipmentInvocationIssuedEvent>().Single();
            var invocation = Facts<SkillInvocationStartedEvent>().Single(start => start.ReturnKind == SkillInvocationReturnKind.NativeEquipment);
            var original = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId);
            var receipt = original.NativeEquipmentInvocation;
            Need(receipt is { Kind: NativeEquipmentInvocationKind.Bagua, ActorSeat: 0, TargetSeat: 0 } &&
                receipt.EquipmentCardId == equipmentId && receipt.PhysicalCardIds.Count == 0 &&
                receipt.ParentFrameId == original.Id && receipt.AttackId == original.Id &&
                issued.ParentFrameId == original.Id && issued.AttackId == original.Id &&
                issued.OccurrenceKey == receipt.OccurrenceKey && invocation.OccurrenceKey == issued.OccurrenceKey &&
                invocation.ParentFrameId == original.Id && invocation.SkillInstanceId == "" &&
                Facts<ShenHuaTuoJingyuReservedEvent>().Count(reaction => reaction.InvocationFrameId == invocation.FrameId &&
                    reaction.OwnerSeat == 0 && reaction.SkillName == issued.SkillKey) == 1 &&
                game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                    move.Reason.Value == JingyuDraw) == 1 && !Facts<JudgmentRequestedEvent>().Any(),
                "The original Slash must own the exact Bagua receipt while a real Jingyu draw suspends the invocation before judgment.");

            var pending = SnapshotJson.Serialize(game.CreateSnapshot(0));
            var originalReceipt = JsonSerializer.Serialize(receipt);
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == pending &&
                JsonSerializer.Serialize(game.ResolutionStack.Single(frame => frame.Id == original.Id).NativeEquipmentInvocation) == originalReceipt &&
                Facts<NativeEquipmentInvocationIssuedEvent>().Single() == issued &&
                Facts<SkillInvocationStartedEvent>().Single(start => start.FrameId == invocation.FrameId) == invocation,
                "Cold restore must preserve the same accepted parent, equipment id and invocation.");

            Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(() => Prompt() is { Kind: DecisionKind.RespondDodge });
            Need(Facts<NativeEquipmentInvocationReturnedEvent>().Single() is { Canceled: false } returned &&
                returned.InvocationId == invocation.FrameId && returned.ParentFrameId == original.Id &&
                returned.OccurrenceKey == issued.OccurrenceKey &&
                Facts<JudgmentResolvedEvent>().Single() is { Succeeded: false } judgment &&
                judgment.ParentResolutionId == original.Id && judgment.Reason == JudgmentReasons.BaguaDefense &&
                Prompt()!.Choices.All(choice => choice.Parameters.GetValueOrDefault("response") != "bagua"),
                "The original invocation must return once and its black judgment must leave the same Slash awaiting a real Dodge.");
            Answer(choice => choice.Parameters.GetValueOrDefault("response") == "dodge" && choice.Cards.SequenceEqual([dodgeId]));
            Need(game.CreateSnapshot(0).Players[0].Hp == defender.Hp &&
                game.CardMovements.Any(move => move.CardId == dodgeId && move.To == CardLocation.DiscardPile) &&
                Facts<NativeEquipmentInvocationIssuedEvent>().Count() == 1 &&
                Facts<NativeEquipmentInvocationReturnedEvent>().Count() == 1 &&
                Facts<JudgmentRequestedEvent>().Count() == 1,
                "The real hand Dodge settles the original Slash without reissuing Bagua.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-bagua-invocation", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, Wolong);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,
                    "triggers":[{"id":"pause-after-jingyu","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                        "movementOccurrence":"perBatch","movementReasons":["{{JingyuDraw}}"],"optional":false,"isSkillInvocation":false,
                        "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                    "name":"观察静域","description":"静域实际摸牌后暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "Slash source", "", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "Fixed black judgments", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7), 30).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Native Bagua invocation", 2, 2,
                new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
