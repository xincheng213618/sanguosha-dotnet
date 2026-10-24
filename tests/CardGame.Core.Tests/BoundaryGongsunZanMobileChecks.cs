using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryGongsunZanMobileChecks
{
    private const string Hero = "boundary:gongsun-zan-mobile", Qiaomeng = "boundary:qiaomeng-mobile",
        Quiet = "fixture:mobile-gsz-quiet", Driver = "fixture:mobile-gsz-driver", Watch = "fixture:mobile-gsz-watch", Deck = "fixture:mobile-gsz-deck";

    public static void LiveSignedHpDistanceIncludesFullHealthAndDying()
    {
        var scene = Scene.Start();
        Need(scene.V(0).Hp == 4 && scene.G.GetSeatDistance(0, 2) == 2 && scene.G.GetCombatDistance(0, 2) == 1 && scene.G.GetCombatDistance(2, 0) == 1,
            "At full four HP, the live outgoing contribution is minus three and the incoming lost-HP-minus-one contribution is minus one; only the final distance has a minimum of one.");
        scene.Use("hurt"); scene.Play(); scene.Use("hurt"); scene.Play();
        Need(scene.V(0).Hp == 2 && scene.G.GetCombatDistance(0, 2) == 1 && scene.G.GetCombatDistance(2, 0) == 3,
            "Actual HP loss immediately changes both directed formulas without changing the underlying seat distance.");
        scene.Use("hurt"); scene.Play();
        Need(scene.G.GetCombatDistance(0, 2) == 2 && scene.G.GetCombatDistance(2, 0) == 4, "At one HP, outgoing X is zero while incoming Y is two.");
        scene.Use("recover"); scene.Play();
        Need(scene.V(0).Hp == 2 && scene.G.GetCombatDistance(0, 2) == 1 && scene.G.GetCombatDistance(2, 0) == 3, "Native recovery changes the live distance contributions immediately.");
        scene.Use("hurt"); scene.Play(); scene.Use("hurt"); scene.Reach(() => scene.P is { Kind: DecisionKind.RescueDying });
        Need(scene.P is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 } && scene.P.ValidCardIds.Contains(scene.V(0).Hand.Single(card => card.Kind == CardKind.Peach).Id) &&
            scene.V(0) is { IsAlive: true, Hp: 0 } && scene.G.GetCombatDistance(0, 2) == 3 && scene.G.GetCombatDistance(2, 0) == 5,
            "The living dying role retains signed X=-1; a premature zero clamp would incorrectly leave its outgoing distance at two.");
    }

    public static void BlackActualSlashDiscardsOnlyEquipmentAndClaimsOriginalMountCold()
    {
        var scene = Scene.Start();
        var mount = scene.V(0).Hand.Single(card => card.Kind == CardKind.OffensiveHorse).Id;
        scene.Use("equip-target", [3]); scene.Reach(() => scene.P?.Choices.Any(choice => choice.Cards.Contains(mount)) == true);
        scene.Answer(choice => choice.Cards.Contains(mount)); scene.Play();
        Need(scene.V(3).Equipment.Single().Id == mount && scene.V(3).HandCount > 0, "The victim really owns a public equipped mount and private hand entities before Slash damage.");
        scene.Use("cardless", [3]); scene.Play();
        Need(scene.V(3).Hp == 5 && !scene.G.CardMovements.Any(move => move.Reason.Value == "skill-program.damage-target-mount.discard"), "A genuine native cardless colorless Slash causes damage without qualifying as black.");
        scene.Slash(Suit.Heart); scene.Play();
        Need(scene.V(3).Hp == 4 && scene.V(3).Equipment.Any(card => card.Id == mount), "A real red physical Slash also leaves the equipment untouched and exposes no mobile Qiaomeng opportunity.");
        var black = scene.V(0).Hand.Single(card => card.Kind == CardKind.Slash && card.Suit == Suit.Spade).Id;
        scene.Slash(Suit.Spade); scene.Reach(() => scene.Optional || scene.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(scene.V(3).Hp == 3 && scene.G.Events.Select(entry => entry.Payload).OfType<DamageAppliedEvent>().Count(damage => damage.SourceSeat == 0 && damage.TargetSeat == 3 && damage.Amount == 1) == 3 && scene.Optional,
            "The original native black Slash must really apply the third point of damage and expose Qiaomeng before returning to Play. " + JsonSerializer.Serialize(new {
                VictimHp = scene.V(3).Hp, Equipment = scene.V(3).Equipment.Select(card => card.Id), Pending = scene.P?.Kind,
                Damage = scene.G.Events.Select(entry => entry.Payload).OfType<DamageAppliedEvent>().Select(damage => new { damage.SourceSeat, damage.TargetSeat, damage.Amount }).ToArray(),
                Actions = scene.G.Events.Select(entry => entry.Payload).OfType<CardActionAcceptedEvent>().Where(entry => entry.Action.PhysicalCards.Any(cost => cost.CardId == black)).Select(entry => new { entry.Action.EffectiveKind, entry.Action.EffectiveSuit, entry.Action.EffectiveIsRed, entry.Action.PhysicalCards }).ToArray() }));
        var action = scene.G.ResolutionStack.OfType<CardUseFrame>().Single(use => use.Action?.PhysicalCards.Any(cost => cost.CardId == black) == true).Action!;
        Need(action is { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, EffectiveSuit: Suit.Spade, EffectiveIsRed: false } && action.PhysicalCards.Single().EffectiveIsRed == false,
            "The original real Use and its payment retain the accepted black appearance through damage; later zone state is not used to reconstruct the color.");
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"); scene.Reach(() => scene.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "damage-target-mount") == true);
        Need(scene.P!.Choices.Count == 1 && scene.P.Choices.All(choice => choice.Parameters.GetValueOrDefault("source-zone") == "Equipment") && scene.P.Choices.Single().Cards.SequenceEqual([mount]),
            "Only the victim's actual Equipment is offered; its private Hand never becomes an opaque payable slot in this edition.");
        scene.Answer(choice => choice.Cards.Contains(mount)); scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Watch);
        var mother = scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Qiaomeng);
        Need(mother.DamageTargetMount is { Receipt: { From.Zone: CardZoneKind.Equipment, To.Zone: CardZoneKind.DiscardPile }, ClaimIssued: false } &&
            scene.G.CardMovements.Count(move => move.CardId == mount && move.Reason.Value == "skill-program.damage-target-mount.discard") == 1 && !scene.V(0).Hand.Any(card => card.Id == mount),
            "The exact equipped mount has been paid to the real discard pile before its discard child, and has not yet been obtained in parallel.");
        scene.Cold(); scene.Continue(); scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Watch);
        Need(scene.V(0).Hand.Any(card => card.Id == mount) && scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == mother.Id).DamageTargetMount is { ClaimIssued: true, ClaimMovementSequence: not null },
            "Returning from the original discard child obtains the same entity through the original program's native gain child.");
        scene.Cold(); scene.Continue(); scene.Play();
        Need(scene.V(3).Hp == 3 && !scene.V(3).Equipment.Any(card => card.Id == mount) && scene.V(0).Hand.Any(card => card.Id == mount) &&
            scene.G.CardMovements.Count(move => move.CardId == mount && move.Reason.Value == "skill-program.damage-target-mount.discard") == 1 &&
            scene.G.CardMovements.Count(move => move.CardId == mount && move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program.damage-target-mount.claim") == 1,
            "Both cold returns retain the original native Slash damage, one paid discard and one claim, with no repeated payment or virtual mount.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        internal GameEngine G = game;
        internal PendingDecision? P => G.PendingDecision ?? G.CreateSnapshot(0).PendingDecision;
        internal PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        internal bool Optional => P?.SkillPrompt?.SkillId == Qiaomeng && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        internal void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Gongsun Zan scene command."); }
        internal void Answer(Func<PromptChoice, bool> predicate) { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        internal void Continue() => Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        internal void Use(string activation, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId));
        internal void Slash(Suit suit)
        {
            var card = V(0).Hand.Single(card => card.Kind == CardKind.Slash && card.Suit == suit);
            var action = G.GetHumanLegalActions().First(action => action.CardId == card.Id && action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([3]));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind) { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        internal void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        internal void Reach(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.RespondDodge }) Answer(choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
                else if (P is not null) throw new InvalidOperationException("Unexpected mobile Gongsun Zan decision " + P.Kind + " seat " + P.PlayerSeat + " skill " + P.SkillPrompt?.SkillId);
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded mobile Gongsun Zan scene did not reach its native boundary.");
        }
        internal void Cold()
        {
            var before = Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(G.CreateSnapshot(viewer))).ToArray();
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before.SequenceEqual(Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(restored.CreateSnapshot(viewer)))), "The exact paid equipment, original damage/use parent, discard/gain child cursor and player projections survive JSON cold restore."); G = restored;
        }
        internal static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(59, 1, "mobile-gsz", Hero, 4, 0, 0, 0, 0, [Quiet, Driver, Watch],
                [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Spade, 8), new(CardKind.OffensiveHorse, Suit.Club, 4), new(CardKind.Peach, Suit.Heart, 3)], [],
                [new("fixture:mobile-gsz-enemy", Hp: 6) { StartingHand = [new(CardKind.Slash, Suit.Heart, 9)] }, new("fixture:mobile-gsz-enemy", Hp: 6) { StartingHand = [new(CardKind.Slash, Suit.Heart, 9)] }],
                [new("fixture:mobile-gsz-ally", Hp: 6) { StartingHand = [] }], new string('c', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-gsz", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            static object Act(string id, object[] effects, int targets = 0) => new { id, minCards = 0, maxCards = 0, minTargets = targets, maxTargets = targets, targetKind = "otherLiving", usesPerTurn = (int?)null, effects };
            static object[] Pause() => [new { op = "chooseOption", target = "owner", resultBind = "observed", options = new[] { new { id = "continue" } } }];
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Quiet, revision = 1, modifiers = new object[] { new { id = "native-extra-slash", query = "slashLimit", operation = "add", value = 5, priority = 0 }, new { id = "keep-hand", query = "handLimit", operation = "add", value = 20, priority = 0 } },
                    triggers = new[] { new { id = "no-normal-draw", isSkillInvocation = false, window = "drawPhaseStarting", subject = "owner", optional = false, effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Driver, revision = 1, activations = new[] {
                    Act("hurt", [new { op = "loseHp", target = "owner", amount = 1 }]), Act("recover", [new { op = "recover", target = "owner", amount = 1 }]),
                    Act("cardless", [new { op = "useVirtualCard", target = "selectedTarget", outputKind = "slash", targetRestriction = "normalSlashTarget", useCardActionWindows = true }], 1),
                    Act("equip-target", [new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" }, zones = new[] { "hand" }, cardCategories = new[] { "equipment" }, count = 1,
                        destination = "selectedTargetEquipment", targetRef = new { kind = "selectedTarget" }, resultBind = "gear", awaitMovementTriggers = true }], 1) } },
                new { id = Watch, revision = 1, triggers = new object[] {
                    new { id = "paid-original-discard", window = "discardPileReceived", subject = "owner", movementReasons = new[] { "skill-program.damage-target-mount.discard" }, optional = false, usageScope = "game", usageLimit = 1, effects = Pause() },
                    new { id = "paid-original-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.damage-target-mount.claim" }, optional = false, usageScope = "game", usageLimit = 1, effects = Pause() } } }
            } });
            var presentations = new Dictionary<string, object> { [Quiet] = new { name = "保留真实手牌", description = "原生场景" }, [Driver] = new { name = "真实指令", description = "原生场景" },
                [Watch] = new { name = "原弃牌取得子窗", description = "原生场景", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = presentations }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            foreach (var peer in new[] { "enemy", "ally" }) builder.AddGeneral(new("fixture:mobile-gsz-" + peer, "真实装备目标", "supporter", Quiet, "wei", 6));
            builder.AddDeck(new(Deck, "原生场景真实牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界公孙瓒原生场景", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, "fixture:mobile-gsz-enemy", "fixture:mobile-gsz-ally"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
