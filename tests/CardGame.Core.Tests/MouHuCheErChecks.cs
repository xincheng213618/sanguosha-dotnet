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
        foreach (var (die, equipmentKind) in new[]
        {
            (false, CardKind.Crossbow), (true, CardKind.Crossbow),
            (true, CardKind.DefensiveHorse), (true, CardKind.QinggangSword), (true, CardKind.JadeSeal),
            (false, CardKind.SilverLion)
        })
        {
            var checksGrant = equipmentKind == CardKind.Crossbow;
            var checksDraw = equipmentKind == CardKind.JadeSeal;
            var checksLion = equipmentKind == CardKind.SilverLion;
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(die, equipmentKind));
            var setup = new ShanheBattleSetup(17, 1, "kongwu-shared-state", Hero, 8, 4, 0, 0, 0, [Driver, Quiet],
                Enumerable.Range(1, 4).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Club, rank)).ToArray(), [],
                [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
                [new(Peer, Hp: 2) { StartingHand = [new(CardKind.Slash, Suit.Heart, 7)] }], new string('b', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = checksDraw ? 8 : 4
            }, registry);
            PendingDecision? P() => game.CreateSnapshot(0).PendingDecision;
            T[] E<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
            CharacterState Player(int seat) => ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine.GetField("_players",
                BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!)[seat];
            bool Effective(int seat, string skill) => game.GetRuntimeModule<CoreGameEngineNationalSkillsRuntime>().HasRuntimeSkill(Player(seat), skill);
            void CheckPrintedEffect(int? cardId, int expected, string boundary)
            {
                if (checksGrant) return;
                var equipped = game.CreateSnapshot(0).Players[1].Equipment;
                var entityMatches = cardId is { } id
                    ? equipped.SingleOrDefault() is { IsBone: false } card && card.Id == id && card.Kind == equipmentKind
                    : equipped.Count == 0;
                Need(entityMatches, $"{equipmentKind} must have its expected physical state at {boundary}.");
                if (checksDraw || checksLion) return;
                var valueMatches = equipmentKind == CardKind.DefensiveHorse
                    ? game.GetCombatDistance(2, 1) == expected && game.GetCombatDistance(1, 2) == 1
                    : game.GetAttackRange(1) == expected;
                Need(valueMatches, $"{equipmentKind} must yield {expected} at {boundary}.");
            }
            int NormalDrawCount(int seat, int turn)
            {
                ActualDiscardRecoveryPhaseStartedEvent StartOf(ActualDiscardRecoveryPhaseKind kind) =>
                    E<ActualDiscardRecoveryPhaseStartedEvent>().Single(entry => entry.Phase.Kind == kind &&
                        entry.Phase.ActualTurnOwnerSeat == seat && entry.Phase.ActorSeat == seat && entry.Phase.TurnNumber == turn);
                var before = StartOf(ActualDiscardRecoveryPhaseKind.Draw).MovementSequenceBefore;
                var after = StartOf(ActualDiscardRecoveryPhaseKind.Play).MovementSequenceBefore;
                return game.CardMovements.Count(move => move.Sequence > before && move.Sequence <= after &&
                    move.TurnNumber == turn && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(seat) && move.Reason == CardMoveReasons.Draw);
            }
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
            CheckPrintedEffect(null, 1, "the pre-equipment decision");
            var holderHpBeforeEquipment = game.CreateSnapshot(0).Players[1].Hp;
            Answer(choice => choice.Targets.SequenceEqual([1]));
            if (checksLion)
            {
                Reach(() => P()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "discard-lion") == true);
                var holder = game.CreateSnapshot(0).Players[1];
                Need(game.GetRuntimeModule<CoreNativeDamageHandlesRuntime>().UsesFormalSilverLion &&
                    holder.Hp > 0 && holder.Hp < holder.MaxHp && holder.Hp == holderHpBeforeEquipment - 1 &&
                    holder.Equipment.SingleOrDefault() is { Id: 31, Kind: CardKind.SilverLion, IsBone: false } &&
                    game.CardMovements.Any(move => move.CardId == 31 && move.From == CardLocation.Processing &&
                        move.To == CardLocation.Equipment(1) && move.Reason == CardMoveReasons.EquipmentEnter) &&
                    E<ProgramSkillHpLostEvent>().Any(loss => loss.TargetSeat == 1 && loss.Amount == 1) &&
                    Player(1).SkillGrants.ArePrintedEquipmentEffectsSuppressed,
                    "Under the accepted debt, the actual Lion installs and its holder is genuinely wounded before the discard command.");
                Cold();
                Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "discard-lion");
            }
            Reach(() => P()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue") == true);
            Need(!Effective(0, Kongwu) && !Effective(1, Unrelated),
                "The caster lost Kongwu and the independent skill restriction is active.");
            int? printedEquipmentId = null;
            if (!checksGrant)
            {
                var slot = EquipmentCatalog.Get(equipmentKind).Slot;
                var equipment = E<EquipmentChangedEvent>().SingleOrDefault(change => change.PlayerSeat == 1 &&
                    change.CardId == 31 && change.CardKind == equipmentKind && change.Slot == slot)
                    ?? throw new InvalidOperationException($"The only {equipmentKind} did not enter seat 1's equipment: " +
                        JsonSerializer.Serialize(game.CardMovements.Where(move => move.CardKind == equipmentKind)));
                if (!checksLion)
                    printedEquipmentId = equipment.CardId;
                Need(Player(1).SkillGrants.Grants.All(grant => !grant.SourceId.StartsWith("equipment:", StringComparison.Ordinal)) &&
                    game.CardMovements.Any(move => move.CardId == equipment.CardId &&
                    move.From == CardLocation.Processing && move.To == CardLocation.Equipment(1) &&
                    move.Reason == CardMoveReasons.EquipmentEnter),
                    $"The printed {equipmentKind} enters equipment through native resolution without a skill grant.");
                CheckPrintedEffect(printedEquipmentId, 1, "the late equipment sequence under the debt");
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
            CheckPrintedEffect(printedEquipmentId, 1, "cold restore of the accepted debt");
            if (checksLion)
            {
                var departure = game.CardMovements.SingleOrDefault(move => move.CardId == 31 && move.CardKind == CardKind.SilverLion &&
                    move.From == CardLocation.Equipment(1) && move.To == CardLocation.DiscardPile);
                Need(departure is not null && game.CreateCardZoneDiagnostics().Single(card => card.CardId == 31).Location == CardLocation.DiscardPile &&
                    game.CreateSnapshot(0).Players[1].Hp == holderHpBeforeEquipment - 1 &&
                    Player(1).SkillGrants.ArePrintedEquipmentEffectsSuppressed &&
                    E<SilverLionRemovedRecoveryEvent>().All(recovery => recovery.PlayerSeat != 1) &&
                    E<RecoveryAppliedEvent>().All(recovery => recovery.TargetSeat != 1) &&
                    game.ResolutionStack.All(frame => frame is not RecoveryReplacementFrame
                        { Attempt: { TargetSeat: 1, Completion.Producer: RecoveryAttemptProducer.SilverLion } } &&
                        frame.PendingRecoveryAttempts?.Any(attempt => attempt.TargetSeat == 1 &&
                            attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion) != true),
                    "Cold recovery preserves the original Lion discard and the holder's wound without a paid or queued Lion recovery.");
            }
            Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(() => E<MouHuCheErDebtTurnEvent>().Any(turn => turn.FrameId == paid.FrameId));
            var due = E<MouHuCheErDebtTurnEvent>().Single(turn => turn.FrameId == paid.FrameId);
            Need((!checksGrant || !Effective(1, EquipmentSkill)) && Player(0).IsAlive == !die && due.Target == 1 && due.Turn > origin.IssuedTurn &&
                E<SkillStateExpiredEvent>().All(expired => expired.Origin != origin),
                "Loss of the issuing skill, and optionally its caster's real death, cannot revoke the debt accepted before the target's actual next turn.");
            CheckPrintedEffect(printedEquipmentId, 1, "the holder's next actual turn start");
            bool IsHolderDraw(ActualDiscardRecoveryPhaseStartedEvent entry) =>
                entry.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase &&
                phase.ActorSeat == due.Target && phase.TurnNumber == due.Turn;
            Reach(() => E<ActualDiscardRecoveryPhaseStartedEvent>().Any(IsHolderDraw));
            CheckPrintedEffect(printedEquipmentId, 1, "the holder's actual draw phase");
            Reach(() => E<TurnEndedEvent>().Any(ended => ended.ActorSeat == due.Target && ended.TurnNumber == due.Turn));
            var started = game.Events.Single(entry => entry.Payload is TurnStartedEvent turn && turn.ActorSeat == due.Target && turn.TurnNumber == due.Turn);
            var ended = game.Events.Single(entry => entry.Payload is TurnEndedEvent turn && turn.ActorSeat == due.Target && turn.TurnNumber == due.Turn);
            var expiry = game.Events.Single(entry => entry.Payload is SkillStateExpiredEvent expired && expired.Origin == origin);
            Need(NormalDrawCount(due.Target, due.Turn) == 1,
                "The accepted debt still reduces this exact normal Draw from two physical cards to one.");
            Need((!checksGrant || Effective(1, EquipmentSkill)) && !Effective(1, Unrelated) &&
                Player(1).SkillGrants.Restrictions.Single().Lifetime.Origin?.Source.SkillId == Driver &&
                expiry.Payload is SkillStateExpiredEvent { Seat: 1 } && started.Sequence < expiry.Sequence && expiry.Sequence < ended.Sequence,
                "The original target turn end revokes only that debt origin, restoring later equipment and retaining an independent restriction.");
            CheckPrintedEffect(printedEquipmentId, 2, "the holder's actual turn end");
            Cold();
            Need(E<MouHuCheErDebtEvent>().Length == 1 && E<SkillStateExpiredEvent>().Count(expired => expired.Origin == origin) == 1 &&
                (!checksGrant || Effective(1, EquipmentSkill)),
                "Cold replay keeps the one original payment and one original expiry without reissuing a debt or suppressing an unrelated source.");
            CheckPrintedEffect(printedEquipmentId, 2, "cold restore after expiry");

            if (checksDraw)
            {
                Reach(() => E<ActualDiscardRecoveryPhaseStartedEvent>().Any(entry => entry.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase &&
                    phase.ActorSeat == due.Target && phase.ActualTurnOwnerSeat == due.Target && phase.TurnNumber > due.Turn));
                var nextTurn = E<TurnStartedEvent>().First(turn => turn.ActorSeat == due.Target && turn.TurnNumber > due.Turn).TurnNumber;
                Need(NormalDrawCount(due.Target, nextTurn) == 3,
                    "The same Jade Seal adds a third physical card to the holder's next normal Draw after the debt expires.");
                Cold();
                Need(NormalDrawCount(due.Target, due.Turn) == 1 && NormalDrawCount(due.Target, nextTurn) == 3 &&
                    game.CreateSnapshot(0).Players[due.Target].Equipment.SingleOrDefault() is { Id: 31, Kind: CardKind.JadeSeal, IsBone: false } &&
                    E<EquipmentChangedEvent>().Count(change => change.CardId == 31 && change.PlayerSeat == due.Target && change.Slot == EquipmentSlot.Treasure) == 1 &&
                    E<MouHuCheErDebtEvent>().Length == 1 && E<MouHuCheErDebtTurnEvent>().SequenceEqual([due]) &&
                    E<SkillStateExpiredEvent>().Count(expired => expired.Origin == origin) == 1,
                    "The same Jade Seal stays equipped through cold replay, and the target draws one under the debt then three on the next actual turn.");
            }
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture(bool die, CardKind equipmentKind) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-kongwu-state", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var death = die ? """,{"op":"loseHp","target":"owner","amount":20}""" : "";
            var lionDeparture = equipmentKind == CardKind.SilverLion ? """
                ,{"op":"loseHp","target":"selectedTarget","amount":1}
                ,{"op":"chooseOption","target":"owner","resultBind":"lion-ready","options":[{"id":"discard-lion"}]}
                ,{"op":"selectOwnedCards","target":"selectedTarget","zones":["equipment"],"amount":1,"resultBind":"lion"}
                ,{"op":"moveBoundCards","target":"owner","sourceBind":"lion","destination":"discardPile","awaitMovementTriggers":true}
                """ : "";
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
              {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,
                "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
              {"id":"{{{Driver}}}","revision":1,"triggers":[{"id":"after-accepted-debt","window":"turnEnding","subject":"owner","optional":false,"effects":[
                {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Kongwu}}}"]}},
                {"op":"useRandomDeckEquipment","target":"owner","resultBind":"late-equipment"},
                {"op":"skillState","target":"selectedTarget","action":"suppress","match":{"skillIds":["{{{Unrelated}}}"]},"until":[{"event":"roundAdvanced","timing":"after"}]}{{{lionDeparture}}},
                {"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}{{{death}}}]}]}
            ]}
            """;
            var optionLabels = new Dictionary<string, string> { ["continue"] = "继续" };
            if (equipmentKind == CardKind.SilverLion)
                optionLabels.Add("discard-lion", "弃置已装备的白银狮子");
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Quiet] = new { name = "Quiet", description = "Fixed native draw" },
                    [Driver] = new { name = "Debt observation", description = "Native equipment after source loss", optionLabels } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddSkill(new(Unrelated, "Independent", "Independent source"));
            builder.AddGeneral(new(Peer, "Peer", "", Unrelated, "wei", 8));
            if (equipmentKind == CardKind.Crossbow)
            {
                builder.AddSkill(new(EquipmentSkill, "Equipment source", "Native physical equipment grant"));
                builder.AddEquipmentContent(new("fixture:kongwu-equipment-binding", [EquipmentCatalog.Get(CardKind.Crossbow)],
                    [new(CardKind.Crossbow, EquipmentSkill, "equipment:fixture-kongwu")]));
            }
            if (equipmentKind == CardKind.SilverLion)
                builder.AddCard(StandardContentRegistry.CreateWithClassicGenerals().GetCard("classic:silver-lion"));
            ContentDeckPhysicalCard equipment = equipmentKind switch
            {
                CardKind.Crossbow => new("standard:crossbow", Suit.Club, 7),
                CardKind.DefensiveHorse => new("standard:defensive_horse", Suit.Spade, 5),
                CardKind.QinggangSword => new("standard:qinggang_sword", Suit.Spade, 6),
                CardKind.JadeSeal => new("standard:jade_seal", Suit.Club, 1),
                CardKind.SilverLion => new("classic:silver-lion", Suit.Club, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(equipmentKind))
            };
            // The derived Shanhe battle seed keeps card 31 below Kongwu's opening refill.
            var physicalCards = equipmentKind == CardKind.Crossbow
                ? Enumerable.Repeat(equipment, 60).ToArray()
                : Enumerable.Range(1, 60).Select(id => id == 31 ? equipment : new("standard:dodge", Suit.Club, 7)).ToArray();
            builder.AddDeck(new(Deck, "Fixed native equipment", 0, 2, []) { PhysicalCards = physicalCards });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Kongwu shared state", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
