using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

internal static class MouHuCheErChecks
{
    private const string Hero = "mou:hu-che-er", Kongwu = "mou:kongwu";
    private const string Driver = "fixture:kongwu-driver", Quiet = "fixture:kongwu-quiet";
    private const string Peer = "fixture:kongwu-peer", Unrelated = "fixture:kongwu-unrelated";
    private const string EquipmentSkill = "fixture:kongwu-equipment", Deck = "fixture:kongwu-deck";

    public static void AcceptedDebtSuppressesLaterEquipmentUntilItsExactTurnEndCold()
    {
        foreach (var (die, useHorse) in new[] { (false, false), (true, false), (true, true) })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(die, useHorse));
            var setup = new ShanheBattleSetup(17, 1, "kongwu-shared-state", Hero, 8, 4, 0, 0, 0, [Driver, Quiet],
                Enumerable.Range(1, 4).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Club, rank)).ToArray(), [],
                [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
                [new(Peer, Hp: 2) { StartingHand = [new(CardKind.Slash, Suit.Heart, 7)] }], new string('b', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry);
            PendingDecision? P() => game.CreateSnapshot(0).PendingDecision;
            T[] E<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
            CharacterState Player(int seat) => ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine.GetField("_players",
                BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!)[seat];
            bool Effective(int seat, string skill) => game.GetRuntimeModule<CoreGameEngineNationalSkillsRuntime>().HasRuntimeSkill(Player(seat), skill);
            bool DirectedDistance(int toHolder) => game.GetCombatDistance(2, 1) == toHolder && game.GetCombatDistance(1, 2) == 1;
            bool HorseEquipped(int id) => game.CreateSnapshot(0).Players[1].Equipment.SingleOrDefault()?.Id == id;
            void Submit(GameCommand command)
            { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Kongwu command rejected."); }
            void Answer(Func<PromptChoice, bool> predicate)
            {
                var prompt = P()!;
                Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
            }
            void Reach(Func<bool> done)
            {
                for (var step = 0; step < 200; step++)
                {
                    if (done()) return;
                    var prompt = P();
                    if (prompt is { Kind: DecisionKind.RescueDying })
                        Answer(choice => choice.Cards.Count == 0);
                    else if (prompt is null) Submit(new AdvanceOneStepCommand(game.Revision));
                    else throw new InvalidOperationException("Kongwu missed its native boundary: die=" + die + "; prompt=" + JsonSerializer.Serialize(prompt));
                }
                throw new InvalidOperationException("Kongwu did not reach its bounded target turn: die=" + die);
            }
            void Cold()
            {
                var before = SnapshotJson.Serialize(game.CreateSnapshot(0));
                game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
                Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == before, "The original Kongwu debt and pending native child survive cold replay.");
            }

            Submit(new StartGameCommand());
            Reach(() => P() is { Kind: DecisionKind.PlayCard });
            Submit(new UseProgramSkillCommand(0, Kongwu, "show-hand", [], [], game.Revision, P()!.PromptId));
            Reach(() => P()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "target") == true);
            Answer(choice => choice.Targets.SequenceEqual([1]));
            Reach(() => P()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "hand") == true);
            Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "hand");
            Reach(() => P() is { Kind: DecisionKind.PlayCard });
            var paid = E<MouHuCheErCompletedEvent>().Single();
            Need(paid.Target == 1 && E<MouHuCheErDiscardedEvent>().Length == 1 &&
                E<MouHuCheErCountedEvent>().Single().Count == 0,
                "The genuine Kongwu reveals and discards the target's only basic card, completes its native refill, and freezes its paid actual Play identity.");
            Submit(new EndPlayPhaseCommand(0, game.Revision, P()!.PromptId));
            Reach(() => E<MouHuCheErDebtEvent>().Length == 1 && P()?.SkillPrompt?.SkillId == Driver);
            var lifetime = Player(1).SkillGrants.Restrictions.Single().Lifetime;
            var origin = lifetime.Origin!;
            Need(origin.FrameId == paid.FrameId && origin.Source is { OwnerSeat: 0, SkillId: Kongwu, BindingId: "show-hand" } &&
                origin.Source.SkillInstanceId == paid.Source.SkillInstanceId &&
                lifetime.Until.SequenceEqual([new(SkillStateBoundaryEvent.TurnEnded, 1, SkillStateBoundaryTiming.Before,
                    paid.Phase.TurnNumber, SkipIssued: true)]) &&
                Player(1).SkillGrants.Grants.All(grant => grant.SkillId != EquipmentSkill),
                "The exact completed phase signs a source-specific restriction through the target's next actual turn end.");
            Cold();
            if (useHorse)
                Need(game.CreateSnapshot(0).Players[1].Equipment.Count == 0 && DirectedDistance(1),
                    "The adjacent seats have unit distance in both directions before the defensive horse enters.");
            Answer(choice => choice.Targets.SequenceEqual([1]));
            Reach(() => P()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue") == true);
            Need(!Effective(0, Kongwu) && !Effective(1, Unrelated),
                "The caster lost Kongwu and the independent skill restriction is active.");
            EquipmentChangedEvent? horse = null;
            if (useHorse)
            {
                horse = E<EquipmentChangedEvent>().SingleOrDefault(change => change is
                    { PlayerSeat: 1, CardKind: CardKind.DefensiveHorse, Slot: EquipmentSlot.DefensiveHorse })
                    ?? throw new InvalidOperationException("The only defensive horse did not enter seat 1's equipment: " +
                        JsonSerializer.Serialize(game.CardMovements.Where(move => move.CardKind == CardKind.DefensiveHorse)));
                Need(HorseEquipped(horse.CardId) && Player(1).SkillGrants.Grants.All(grant =>
                    !grant.SourceId.StartsWith("equipment:", StringComparison.Ordinal)) &&
                    game.CardMovements.Any(move => move.CardId == horse.CardId &&
                    move.From == CardLocation.Processing && move.To == CardLocation.Equipment(1) &&
                    move.Reason == CardMoveReasons.EquipmentEnter) && DirectedDistance(1),
                    "The printed defensive horse has no skill grant and contributes no incoming distance under the accepted debt.");
            }
            else
            {
                var equipment = Player(1).SkillGrants.Grants.Single(grant => grant.SkillId == EquipmentSkill);
                Need(equipment.SourceId.StartsWith("equipment:", StringComparison.Ordinal) &&
                    game.CreateSnapshot(0).Players[1].Equipment.Count == 1 &&
                    game.GetRuntimeModule<CoreGameEngineNationalSkillsRuntime>().HasOwnedRuntimeSkill(Player(1), EquipmentSkill) &&
                    !Effective(1, EquipmentSkill),
                    "Real native equipment entry creates its own owned grant while the accepted restriction still makes it ineffective.");
            }
            Cold();
            if (horse is not null)
                Need(HorseEquipped(horse.CardId) && DirectedDistance(1),
                    "Cold restore preserves both the mounted horse and its suppressed incoming distance.");
            Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(() => E<MouHuCheErDebtTurnEvent>().Any(turn => turn.FrameId == paid.FrameId));
            var due = E<MouHuCheErDebtTurnEvent>().Single(turn => turn.FrameId == paid.FrameId);
            Need((useHorse || !Effective(1, EquipmentSkill)) && Player(0).IsAlive == !die && due.Target == 1 && due.Turn > origin.IssuedTurn &&
                E<SkillStateExpiredEvent>().All(expired => expired.Origin != origin),
                "Loss of the issuing skill, and optionally its caster's real death, cannot revoke the debt accepted before the target's actual next turn.");
            if (horse is not null)
                Need(HorseEquipped(horse.CardId) && DirectedDistance(1),
                    "The same defensive horse remains ineffective when its holder's next actual turn begins.");
            Reach(() => E<TurnEndedEvent>().Any(ended => ended.ActorSeat == due.Target && ended.TurnNumber == due.Turn));
            var started = game.Events.Single(entry => entry.Payload is TurnStartedEvent turn && turn.ActorSeat == due.Target && turn.TurnNumber == due.Turn);
            var ended = game.Events.Single(entry => entry.Payload is TurnEndedEvent turn && turn.ActorSeat == due.Target && turn.TurnNumber == due.Turn);
            var expiry = game.Events.Single(entry => entry.Payload is SkillStateExpiredEvent expired && expired.Origin == origin);
            var draw = E<ActualDiscardRecoveryPhaseStartedEvent>().Single(entry => entry.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase &&
                phase.ActorSeat == due.Target && phase.TurnNumber == due.Turn);
            Need(game.CardMovements.Count(move => move.TurnNumber == due.Turn && move.Sequence > draw.MovementSequenceBefore &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(due.Target) && move.Reason == CardMoveReasons.Draw) == 1,
                "The accepted debt still reduces this exact normal Draw from two physical cards to one.");
            Need((useHorse || Effective(1, EquipmentSkill)) && !Effective(1, Unrelated) &&
                Player(1).SkillGrants.Restrictions.Single().Lifetime.Origin?.Source.SkillId == Driver &&
                expiry.Payload is SkillStateExpiredEvent { Seat: 1 } && started.Sequence < expiry.Sequence && expiry.Sequence < ended.Sequence,
                "The original target turn end revokes only that debt origin, restoring later equipment and retaining an independent restriction.");
            if (horse is not null)
                Need(HorseEquipped(horse.CardId) && DirectedDistance(2),
                    "At the holder's actual turn end the same horse restores only the incoming distance bonus.");
            Cold();
            Need(E<MouHuCheErDebtEvent>().Length == 1 && E<SkillStateExpiredEvent>().Count(expired => expired.Origin == origin) == 1 &&
                (useHorse || Effective(1, EquipmentSkill)),
                "Cold replay keeps the one original payment and one original expiry without reissuing a debt or suppressing an unrelated source.");
            if (horse is not null)
                Need(HorseEquipped(horse.CardId) && DirectedDistance(2),
                    "Cold replay preserves the mounted horse and the restored incoming distance bonus.");
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture(bool die, bool useHorse) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-kongwu-state", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var death = die ? """,{"op":"loseHp","target":"owner","amount":20}""" : "";
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{{Driver}}}","revision":1,"triggers":[{"id":"after-accepted-debt","window":"turnEnding","subject":"owner","optional":false,"effects":[
                {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Kongwu}}}"]}},
                {"op":"useRandomDeckEquipment","target":"owner","resultBind":"late-equipment"},
                {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Unrelated}}}"]},"until":[{"event":"roundAdvanced","timing":"after"}]},
                {"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}{{{death}}}]}]}
            ]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Quiet] = new { name = "Quiet", description = "Fixed native draw" },
                    [Driver] = new { name = "Debt observation", description = "Native equipment after source loss", optionLabels = new { @continue = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddSkill(new(Unrelated, "Independent", "Independent source"));
            builder.AddGeneral(new(Peer, "Peer", "", Unrelated, "wei", 8));
            if (!useHorse)
            {
                builder.AddSkill(new(EquipmentSkill, "Equipment source", "Native physical equipment grant"));
                builder.AddEquipmentContent(new("fixture:kongwu-equipment-binding", [EquipmentCatalog.Get(CardKind.Crossbow)],
                    [new(CardKind.Crossbow, EquipmentSkill, "equipment:fixture-kongwu")]));
            }
            // The derived Shanhe battle seed keeps card 31 below Kongwu's opening refill.
            var physicalCards = useHorse
                ? Enumerable.Range(1, 60).Select(id => id == 31
                    ? new ContentDeckPhysicalCard("standard:defensive_horse", Suit.Spade, 5)
                    : new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray()
                : Enumerable.Range(0, 60)
                    .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 7)).ToArray();
            builder.AddDeck(new(Deck, "Fixed native equipment", 0, 2, []) { PhysicalCards = physicalCards });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Kongwu shared state", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
