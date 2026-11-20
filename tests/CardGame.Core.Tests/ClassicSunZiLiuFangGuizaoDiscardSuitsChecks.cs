using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ClassicSunZiLiuFangGuizaoDiscardSuitsChecks
{
    private const string Guizao = "classic:guizao", LvKai = "ol:lv-kai", Bijing = "ol:bijing";
    private const string Payer = "fixture:guizao-payer", Peer = "fixture:guizao-peer", Deck = "fixture:guizao-deck";
    private const string Hold = "fixture:guizao-hold", Watch = "fixture:guizao-recovery-watch";
    private const string BijingPayment = "skill-program.ol:bijing.bijing-punish";

    internal static void GuizaoUsesActualDiscardSuitsAndRejectsEffectiveDuplicatesCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        Check(content, Suit.Club, expectsGuizao: true);
        Check(content, Suit.Heart, expectsGuizao: false);
    }

    private static void Check(ContentRegistry content, Suit armorSuit, bool expectsGuizao)
    {
        var game = Start(content, armorSuit);
        PendingDecision? Prompt() => Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        IEnumerable<T> Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>();
        bool ReachedEnd() => Facts<TurnEndedEvent>().Any(fact => fact.ActorSeat == 1);

        void Answer(Func<PromptChoice, bool> select)
        {
            var prompt = Prompt() ?? throw new InvalidOperationException("The expected Guizao fixture prompt is missing.");
            Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));
        }

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 120 && !ready(); step++)
            {
                if (Prompt() is { PlayerSeat: 0, SkillPrompt.SkillId: "ol:tunan" })
                {
                    Answer(choice => ActionIs(choice, "skip"));
                    continue;
                }
                Require(Prompt()?.PlayerSeat != 0, "An unexpected human prompt blocked the Guizao fixture.");
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            Require(ready(), $"The Guizao fixture did not reach the next boundary. Prompt {Prompt()?.Prompt}.");
        }

        Accept(game, new StartGameCommand());
        Reach(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Bijing && Prompt()!.Choices.Any(choice => ActionIs(choice, "activate")));
        Answer(choice => ActionIs(choice, "activate"));
        for (var index = 0; index < 2; index++)
        {
            var selected = Prompt()!.Choices.First(choice => ActionIs(choice, "select-owned-cards") && choice.Cards.Count == 1);
            Answer(choice => choice.Id == selected.Id);
        }
        if (Prompt()?.Choices.Any(choice => ActionIs(choice, "finish-owned-cards")) == true)
            Answer(choice => ActionIs(choice, "finish-owned-cards"));
        var marked = Facts<ProgramBijingMarkedEvent>().Single().CardIds;

        Reach(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        var dodge = Prompt()!.Choices.First(choice => choice.Cards.Count == 1 && marked.Contains(choice.Cards.Single()));
        Answer(choice => choice.Id == dodge.Id);
        Reach(() => Prompt() is { PlayerSeat: 1, RequiredCardCount: 2 } pay &&
            pay.Choices.Any(choice => ActionIs(choice, "bijing-punish-discard")));
        var owner = game.CreateSnapshot(1).Players.Single(player => player.Seat == 1);
        var armor = owner.Equipment.Single(card => card.Kind == CardKind.SilverLion);
        var hand = owner.Hand.Single(card => card is { Kind: CardKind.Dodge, Suit: Suit.Spade, Rank: 6 });
        Answer(choice => choice.Cards.SequenceEqual([armor.Id]));
        Answer(choice => choice.Cards.SequenceEqual([hand.Id]));
        Reach(() => Prompt() is { PlayerSeat: 1, SkillPrompt.SkillId: Watch });

        var paid = Facts<NativeLogicalDiscardInvocationRecordedEvent>().Single(fact => fact.Reason.Value == BijingPayment);
        var moves = game.CardMovements.Where(move => move.Reason.Value == BijingPayment).ToArray();
        var materials = Facts<NativeLogicalDiscardMaterialCompletedEvent>().Where(fact => fact.InvocationId == paid.InvocationId).ToArray();
        Require(paid is { InitiatorSeat: 0, ExecutorSeat: 1, SubjectSeat: 1, ActualCount: 2, HandCount: 1,
                    Phase: { Kind: ActualDiscardRecoveryPhaseKind.Discard, ActualTurnOwnerSeat: 1, ActorSeat: 1 } } &&
            moves.Select(move => move.CardId).SequenceEqual([armor.Id, hand.Id]) &&
            moves.Select(move => move.From).SequenceEqual([CardLocation.Equipment(1), CardLocation.Hand(1)]) &&
            moves.Select(move => move.PrintedSuit).SequenceEqual([armorSuit, Suit.Spade]) &&
            moves.All(move => move.To == CardLocation.DiscardPile) &&
            materials.Select(material => material.CompletionSequence).SequenceEqual(moves.Select(move => (long)move.Sequence)) &&
            Facts<SilverLionRemovedRecoveryEvent>().Single() is { PlayerSeat: 1, RecoveredAmount: 1 },
            "Bijing pays the original equipment and hand as actual self-discard in seat 1's discard phase before the Silver Lion recovery child returns.");

        var saved = GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint()));
        var cold = GameReplay.Restore(saved, content);
        Require(cold.CardMovements.SequenceEqual(game.CardMovements) &&
            SnapshotJson.Serialize(cold.CreateSnapshot(1)) == SnapshotJson.Serialize(game.CreateSnapshot(1)),
            "Cold public-command replay preserves the paid mixed-zone batch and recovery choice.");
        game = cold;
        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Guizao || ReachedEnd());
        if (!expectsGuizao)
        {
            Require(ReachedEnd() && Prompt()?.SkillPrompt?.SkillId != Guizao &&
                !Facts<ProgramBindingStartedEvent>().Any(fact => fact.SkillId == Guizao),
                "A heart Silver Lion and the Hongyan-modified spade Dodge have the same effective suit, so Guizao must stay unavailable.");
            return;
        }

        Require(Prompt() is { PlayerSeat: 1, SkillPrompt.SkillId: Guizao },
            "A club Silver Lion and the Hongyan-modified spade Dodge give the original phase two distinct effective suits and must offer Guizao.");
        Answer(choice => ActionIs(choice, "activate"));
        Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw") == true);
        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw");
        Reach(ReachedEnd);
        Require(game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
            move.Reason.Value == "skill-program.classic:guizao.Draw") == 1,
            "The accepted Guizao choice draws one real card and returns to the next phase.");
    }

    private static bool ActionIs(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;

    private static GameEngine Start(ContentRegistry content, Suit armorSuit)
    {
        var payer = new ShanheCombatantDefinition(Payer, Hp: 7, ExtraSkillIds: [Hold, Watch, "classic:hongyan"])
        {
            StartingHand = [new(CardKind.SilverLion, armorSuit, 1), new(CardKind.Slash, Suit.Spade, 7),
                new(CardKind.Dodge, Suit.Spade, 6), new(CardKind.Dodge, Suit.Club, 7)]
        };
        var setup = new ShanheBattleSetup(39, 1, "guizao-native-discard-suits", LvKai, 3, 0, 0, 0, 0, [Hold],
            [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3),
             new(CardKind.Dodge, Suit.Diamond, 4), new(CardKind.Dodge, Suit.Spade, 5)],
            [], [payer, new(Peer, Hp: 8), new(Peer, Hp: 8)], [], new string('b', 64));
        return GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16
        }, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:classic-guizao-discard-suits", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "classic:sun-zi-liu-fang", "classic:xiao-qiao",
                LvKai, "boundary:ling-tong-mobile");
            builder.AddCard(StandardContentRegistry.CreateWithClassicGenerals().GetCard("classic:silver-lion"));
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Hold}}","revision":1,"modifiers":[{"id":"hold","query":"handLimit","operation":"add","value":30,"priority":0}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"paid-recovery","window":"afterHpRecovered","subject":"owner","optional":false,"isSkillInvocation":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"healed","options":[{"id":"continue"}]}]}
                  ]}
                ]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Hold] = new { name = "保留手牌", description = "提高弃牌阶段手牌上限。" },
                    [Watch] = new
                    {
                        name = "回复结算", description = "在回复体力后继续结算。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" }
                    }
                }
            });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
            {
                var shown = catalog.Presentations[id];
                builder.AddSkill(new(id, shown.Name, shown.Description) { Program = program, ProgramPresentation = shown });
            }
            builder.AddGeneral(new(Payer, "瑰藻弃牌者", "", Guizao, "wei", 8));
            builder.AddGeneral(new(Peer, "同桌", "", Hold, "wei", 8));
            builder.AddDeck(new(Deck, "固定闪实体牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 30).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 9)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "经典瑰藻弃牌花色", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [LvKai, Payer, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
