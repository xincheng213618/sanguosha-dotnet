using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheKunlunFieldEquipmentChecks
{
    private const string Skill = StandardShanheKunlunMirrorPackage.SkillId, Driver = "fixture:kunlun-field-driver";
    private const string Owner = "fixture:kunlun-field-owner", Peer = "fixture:kunlun-field-peer", Deck = "fixture:kunlun-field-deck";

    internal static void OriginalMirrorLearnsAndPreventsNativeDuelCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheKunlunMirrorPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "kunlun-native", Owner, 8, 0, 0, 0, 0, [],
            [new(CardKind.KunlunMirror, Suit.Spade, 12), new(CardKind.Duel, Suit.Diamond, 1),
                new(CardKind.Duel, Suit.Diamond, 2), new(CardKind.Snatch, Suit.Club, 3), new(CardKind.Duel, Suit.Diamond, 3)],
            [], [new(Peer) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot View(int seat) => game.CreateSnapshot(0).Players[seat];
        bool HasGrant(int seat) => View(seat).Skills?.Any(skill => skill.ContentId == Skill) == true;
        IEnumerable<T> Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>();
        void Need(bool success, string message)
        {
            if (!success)
                throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
                {
                    game.State.TurnNumber, game.State.CurrentSeat, game.State.Phase, Prompt = Prompt(),
                    Recent = game.Events.TakeLast(8).Select(entry => entry.Payload.GetType().Name)
                }));
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Native Kunlun Mirror command rejected.");
        }
        void Play()
        {
            for (var step = 0; step < 120; step++)
            {
                var prompt = Prompt();
                if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0) return;
                Need(prompt is null or { Kind: DecisionKind.PlayCard }, "An unexpected human prompt interrupted the native trick.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(false, "The fixed Kunlun Mirror scene did not return to its Play boundary.");
        }
        int UseTrick(CardKind kind, int rank, int? targetCardId = null)
        {
            var card = View(0).Hand.Single(item => item.Kind == kind && item.Rank == rank);
            var action = game.GetHumanLegalActions().First(candidate => candidate.CardId == card.Id &&
                candidate.TargetSeats.SequenceEqual([1]) && candidate.TargetCardId == targetCardId);
            Submit(new PlayCardCommand(0, card.Id, action.TargetSeats, game.Revision, Prompt()!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            Play();
            return card.Id;
        }
        long UseId(int cardId) => Facts<CardUseDeclaredEvent>().Single(fact => fact.CardId == cardId).ResolutionId;
        bool PaidAndFinished(int cardId) => game.CardMovements.Count(move => move.CardId == cardId &&
            move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
            Facts<CardUseFinishedEvent>().Count(fact => fact.CardId == cardId && fact.ResolutionId == UseId(cardId)) == 1;

        Submit(new StartGameCommand());
        Play();
        var mirror = View(0).Hand.Single(card => card.Kind == CardKind.KunlunMirror);
        Need(mirror.Suit == Suit.Spade && mirror.Rank == 12 && !HasGrant(0) && !HasGrant(1),
            "The original Kunlun Mirror in hand grants neither character its equipment skill.");
        Submit(new UseProgramSkillCommand(0, Driver, "place", [mirror.Id], [1], game.Revision, Prompt()!.PromptId));
        Play();
        Need(HasGrant(1) && View(1).Equipment.Single().Id == mirror.Id &&
            Facts<EquipmentChangedEvent>().Single(fact => fact.CardId == mirror.Id) is
                { PlayerSeat: 1, Slot: EquipmentSlot.Treasure, CardKind: CardKind.KunlunMirror } &&
            game.CardMovements.Count(move => move.CardId == mirror.Id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Equipment(1) && move.Reason == CardMoveReasons.EquipmentEnter) == 1,
            "The same physical mirror enters the recipient's Treasure slot and grants its real equipment skill.");

        var hp = View(1).Hp;
        var first = UseTrick(CardKind.Duel, 1);
        var learned = Facts<KunlunMirrorTrickLearnedEvent>().Single().Receipt;
        var instance = "equipment:shanhe-kunlun-mirror:" + mirror.Id;
        Need(learned is { Learning: true, Amount: 1, Source: { OwnerSeat: 1, SkillId: Skill },
                Origin: { EffectiveKind: CardKind.Duel, CardUserSeat: 0, SourceSeat: 0, TargetSeat: 1, CardActionId: > 0 } } &&
            learned.Source.SkillInstanceId == instance && learned.Origin.NativeCardId == first && learned.Origin.AttackResolutionId == UseId(first) &&
            Facts<DamageRequestedEvent>().Count(fact => fact.ResolutionId == learned.DamageFrameId && fact.SourceCard == CardKind.Duel) == 1 &&
            Facts<DamageAppliedEvent>().Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } && View(1).Hp == hp - 1 &&
            !Facts<KunlunMirrorPreventionIssuedEvent>().Any() && PaidAndFinished(first),
            "The first actual Duel damage applies and teaches its exact trick name to the original physical mirror instance.");

        var before = SnapshotJson.Serialize(game.CreateSnapshot(0));
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == before && game.ResolutionStack.Count == 0 &&
            Facts<KunlunMirrorTrickLearnedEvent>().Single().Receipt == learned && View(1).Equipment.Single().Id == mirror.Id,
            "Cold restore preserves the original learn receipt and mirror at the clean Play boundary.");

        var second = UseTrick(CardKind.Duel, 2);
        var prevention = Facts<KunlunMirrorPreventionIssuedEvent>().Single().Receipt;
        Need(prevention is { Learning: false, Amount: 1, Source: { OwnerSeat: 1, SkillId: Skill },
                Origin: { EffectiveKind: CardKind.Duel, TargetSeat: 1, CardActionId: > 0 } } &&
            prevention.Source.SkillInstanceId == instance && prevention.Origin.CardName == learned.Origin.CardName &&
            prevention.Origin.NativeCardId == second && prevention.Origin.AttackResolutionId == UseId(second) &&
            Facts<ProgramDamagePreventedEvent>().Count(fact => fact.FrameId == prevention.WindowFrameId && fact.SkillId == Skill &&
                fact.TargetSeat == 1 && fact.Amount == 1) == 1 && Facts<DamageAppliedEvent>().Count() == 1 &&
            View(1).Hp == hp - 1 && Facts<KunlunMirrorTrickLearnedEvent>().Single().Receipt == learned && PaidAndFinished(second),
            "After cold restore the same equipped grant prevents the second actual Duel without learning the same name again.");

        UseTrick(CardKind.Snatch, 3, mirror.Id);
        Need(View(0).Hand.Any(card => card.Id == mirror.Id) && View(1).Equipment.Count == 0 && !HasGrant(1),
            "The actual Snatch removes the original mirror and its exact equipment grant.");
        var third = UseTrick(CardKind.Duel, 3);
        Need(View(1).Hp == hp - 2 && PaidAndFinished(third) && Facts<DamageAppliedEvent>().Count() == 2 &&
            Facts<KunlunMirrorTrickLearnedEvent>().Single().Receipt == learned && Facts<KunlunMirrorPreventionIssuedEvent>().Count() == 1,
            "The same trick applies actual damage after the original treasure leaves, while its stored name remains single.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-kunlun-field-equipment", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
                  {"id":"place","minCards":1,"maxCards":1,"sourceZones":["hand"],"cardCategories":["equipment"],
                   "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"mirror"},
                    {"op":"placeSelectedEquipment","target":"selectedTarget","sourceBind":"mirror"}]}]}]}
                """, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "原生宝物检查", description = "实际装备昆仑镜。" } }
            }));
            builder.AddSkill(new(Driver, programs.Presentations[Driver].Name, programs.Presentations[Driver].Description)
                { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "实际出牌者", "supporter", Driver, "shu", 8));
            builder.AddGeneral(new(Peer, "实际受伤者", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "固定黑桃闪", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 5), 30).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "昆仑镜原生决斗", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
