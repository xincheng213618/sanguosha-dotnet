using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ClassicSunLiangKuizhuDiscardBudgetChecks
{
    private const string SunLiang = "classic:sun-liang", Kuizhu = "classic:kuizhu", LvKai = "ol:lv-kai", Bijing = "ol:bijing";
    private const string Hold = "fixture:kuizhu-hold", Watch = "fixture:kuizhu-recovery-watch";
    private const string Peer = "fixture:kuizhu-peer", Deck = "fixture:kuizhu-deck";
    private const string BijingPayment = "skill-program.ol:bijing.bijing-punish";

    internal static void KuizhuCountsActualBijingDiscardsInItsOwnPhaseCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        PendingDecision? Prompt() => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        T[] Facts<T>() => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}. Prompt {Prompt()?.Prompt}.");
        }

        void Accept(GameCommand command)
        {
            var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Require(result.Accepted, result.Error?.Message ?? "The published command was rejected.");
        }

        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.FirstOrDefault(predicate);
            Require(choice is not null, "The expected choice is available.");
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice!.Id, game.Revision));
        }

        void Reach(Func<PendingDecision, bool> matches)
        {
            for (var step = 0; step < 120; step++)
            {
                if (Prompt() is { } prompt)
                {
                    if (matches(prompt)) return;
                    if (prompt is { PlayerSeat: 0, SkillPrompt.SkillId: "ol:tunan" })
                    {
                        Answer(c => ActionIs(c, "skip"));
                        continue;
                    }
                    Require(prompt.PlayerSeat != 0, "An unexpected human prompt blocked the discard-phase scenario.");
                }
                Accept(new AdvanceOneStepCommand(game.Revision));
            }
            Require(false, "The expected prompt was not reached.");
        }

        Accept(new StartGameCommand());
        Reach(p => p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Accept(new EndPlayPhaseCommand(0, game.Revision, Prompt()!.PromptId));
        Reach(p => p.SkillPrompt?.SkillId == Bijing && p.Choices.Any(c => ActionIs(c, "activate")));
        Answer(c => ActionIs(c, "activate"));
        for (var i = 0; i < 2; i++)
            Answer(c => ActionIs(c, "select-owned-cards") && c.Cards.Count == 1);
        if (Prompt()?.Choices.Any(c => ActionIs(c, "finish-owned-cards")) == true)
            Answer(c => ActionIs(c, "finish-owned-cards"));
        var marked = Facts<ProgramBijingMarkedEvent>().Single().CardIds;

        Reach(p => p is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        Answer(c => c.Cards.Count == 1 && marked.Contains(c.Cards.Single()));
        Reach(p => p.Choices.Any(c => ActionIs(c, "bijing-punish-discard")));
        Require(Prompt() is { PlayerSeat: 1, RequiredCardCount: 2, MinimumCardCount: 2 }, "Sun Liang must select the actual two-card Bijing payment.");
        var armor = game.CreateSnapshot(1).Players.Single(p => p.Seat == 1).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id;
        Answer(c => c.Cards.SequenceEqual([armor]));
        Answer(c => c.Cards.Count == 1);
        Reach(p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Watch });

        var paid = Facts<NativeLogicalDiscardInvocationRecordedEvent>().Single(e => e.Reason.Value == BijingPayment);
        var materials = game.CardMovements.Where(m => m.Reason.Value == BijingPayment).ToArray();
        Require(paid is { InitiatorSeat: 0, ExecutorSeat: 1, SubjectSeat: 1, ActualCount: 2, HandCount: 1,
            Phase: { Kind: ActualDiscardRecoveryPhaseKind.Discard, ActualTurnOwnerSeat: 1, ActorSeat: 1 } } &&
            materials is [{ CardId: var equipmentId, From.Zone: CardZoneKind.Equipment }, { From.Zone: CardZoneKind.Hand }] && equipmentId == armor &&
            materials.All(m => m.From.OwnerSeat == 1 && m.To == CardLocation.DiscardPile && m.Sequence > paid.SequenceBefore && m.Sequence <= paid.SequenceAfter),
            "The original two physical discards are paid by Sun Liang in his own actual discard phase.");
        Require(Facts<SilverLionRemovedRecoveryEvent>() is [{ PlayerSeat: 1, RecoveredAmount: 1 }], "The real equipment loss opens an already-paid recovery child.");

        var before = State(game);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(State(game) == before, "The paid discard window restores from accepted commands with the same state.");
        Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Kuizhu });
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Kuizhu);
        Require(frame.DiscardBudgetDraft?.Budget == paid.ActualCount,
            $"Kuizhu must count both native discards. Expected {paid.ActualCount}, got {frame.DiscardBudgetDraft?.Budget}.");
        Answer(c => c.Parameters.GetValueOrDefault("mode") == "draw");
        Answer(c => c.Targets.SequenceEqual([0]));
        Answer(c => c.Targets.SequenceEqual([1]));
        Answer(c => c.Parameters.GetValueOrDefault("mode") == "finish");
        var committed = Facts<ProgramDiscardBudgetCommittedEvent>().Single(e => e.FrameId == frame.Id);
        Require(committed is { OwnerSeat: 1, Budget: 2, Mode: "draw" } && committed.Participants.Select(p => p.Seat).SequenceEqual([0, 1]),
            "Both choices legally commit against the original discard budget.");
        var replay = GameReplay.Replay(game.CreateCheckpoint().Options, game.AcceptedCommands, content);
        Require(State(replay) == State(game), "The accepted draw choices replay with the same visible state and card movements.");
    }

    private static bool ActionIs(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;

    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))).ToArray(),
        Frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())).ToArray(),
        game.CardMovements,
        Events = game.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray()
    });

    private static GameEngine Start(ContentRegistry content)
    {
        var setup = new ShanheBattleSetup(39, 1, "additional-native-self-discard", LvKai, 3, 0, 0, 0, 0, [Hold],
            [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3), new(CardKind.Dodge, Suit.Diamond, 4), new(CardKind.Dodge, Suit.Spade, 5)], [],
            [new(SunLiang, MaxHpBonus: 5, Hp: 7, ExtraSkillIds: [Hold, Watch])
            {
                StartingHand = [new(CardKind.SilverLion, Suit.Club, 1), new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Dodge, Suit.Heart, 6), new(CardKind.Dodge, Suit.Club, 7)]
            }, new(Peer, Hp: 8), new(Peer, Hp: 8)], [], new string('b', 64));
        return GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16
        }, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:classic-kuizhu-discard-budget", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            // The fixture can read exact native phase receipts before Kuizhu itself requests them.
            CharacterFixtureContent.Register(builder, SunLiang, LvKai, "boundary:ling-tong-mobile");
            builder.AddCard(StandardContentRegistry.CreateWithClassicGenerals().GetCard("classic:silver-lion"));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Hold}}","revision":1,"modifiers":[{"id":"hold","query":"handLimit","operation":"add","value":30,"priority":0}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"paid-recovery","window":"afterHpRecovered","subject":"owner","optional":false,"isSkillInvocation":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"healed","options":[{"id":"continue"}]}]}
                  ]}
                ]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Hold] = new { name = "保留手牌", description = "提高弃牌阶段手牌上限。" },
                    [Watch] = new { name = "回复结算", description = "在回复体力后继续结算。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                }
            }));
            foreach (var (id, program) in catalog.Programs)
            {
                var presentation = catalog.Presentations[id];
                builder.AddSkill(new(id, presentation.Name, presentation.Description) { Program = program, ProgramPresentation = presentation });
            }
            builder.AddGeneral(new(Peer, "同桌", "", Hold, "wei", 8));
            builder.AddDeck(new(Deck, "固定闪实体牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 90).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 9)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "经典溃诛弃牌预算", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [LvKai, SunLiang, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
