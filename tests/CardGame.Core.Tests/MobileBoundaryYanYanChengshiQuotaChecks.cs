using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileBoundaryYanYanChengshiQuotaChecks
{
    private const string Hero = "boundary:yan-yan-mobile";
    private const string Juzhan = "boundary:juzhan-mobile";
    private const string OpeningQuota = "fixture:chengshi-quota-opening";
    private const string Opponent = "fixture:chengshi-quota-opponent";
    private const string Deck = "fixture:chengshi-quota-deck";

    internal static void ChengshiAllowsExactlyTwoOrdinarySlashesNextActualTurnCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var target = new ShanheCombatantDefinition(Opponent, Hp: 8)
        {
            StartingHand = [], StartingEquipment = [new(CardKind.OffensiveHorse, Suit.Club, 4)]
        };
        var setup = new ShanheBattleSetup(83, 1, "fixture:chengshi-quota", Hero, 4, 0, 0, 0, 0, [OpeningQuota],
            Enumerable.Range(2, 5).Select(rank => new ShanheStartingCard(CardKind.Slash, Suit.Spade, rank)).ToArray(),
            [], [target], [], new string('d', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5
        }, registry);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot View(int seat) => game.CreateSnapshot(0).Players[seat];
        bool AtPlay() => Pending() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        int Debits(int turn) => Facts<CardUseDebitRecordedEvent>().Count(entry => entry.Debit.ActorSeat == 0 &&
            entry.Debit.TurnNumber == turn && entry.Debit.Query == SkillRuleQuery.SlashLimit);
        bool Used(int card, int turn) => game.CardMovements.Count(move => move.CardId == card && move.TurnNumber == turn &&
            move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1;
        LegalAction[] OrdinarySlashes() => game.GetHumanLegalActions().Where(action => action.Kind == LegalActionKind.Slash &&
            action.ConversionSource is null && action.TargetSeats.SequenceEqual([1])).ToArray();

        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
                {
                    game.State.TurnNumber, game.State.CurrentSeat, game.State.Phase, Prompt = Pending(),
                    Recent = game.Events.TakeLast(8).Select(entry => entry.Payload.GetType().Name)
                }));
        }

        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Chengshi quota command was rejected.");
        }

        static string? ChoiceAction(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
        bool CanInvoke() => Pending() is { PlayerSeat: 0, SkillPrompt.SkillId: Juzhan } prompt &&
            prompt.Choices.Any(choice => ChoiceAction(choice) == "activate");
        void Answer(Func<PromptChoice, bool> matches)
        {
            var prompt = Pending()!;
            var choice = prompt.Choices.Single(matches);
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        }

        void Reach(Func<bool> ready, bool skipJuzhan = false)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                if (skipJuzhan && CanInvoke())
                {
                    Answer(choice => ChoiceAction(choice) == "skip");
                    continue;
                }
                Need(Pending() is null && game.State.Winner == Winner.None, "The Chengshi fixture reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The expected Chengshi quota boundary was not reached.");
        }

        PlayCardCommand SlashCommand(LegalAction action) => new(0, action.CardId!.Value, action.TargetSeats,
            game.Revision, Pending()!.PromptId, action.PlayedCardKind);

        void Play(int card) => Submit(SlashCommand(OrdinarySlashes().Single(action => action.CardId == card)));

        Submit(new StartGameCommand());
        Reach(AtPlay);
        var openingTurn = game.State.TurnNumber;
        var cards = View(0).Hand.OrderBy(card => card.Rank).Select(card => card.Id).ToArray();
        var horse = View(1).Equipment.Single().Id;
        var temporary = Facts<TurnRuleModifierGrantedEvent>().Single().Modifier;
        Need(cards.Length == 5 && View(0).Hand.All(card => card.Kind == CardKind.Slash) && View(0).Equipment.Count == 0 &&
            temporary is { Query: SkillRuleQuery.SlashLimit, Operation: SkillRuleOperation.Add, Amount: 2, TurnSeat: 0 } &&
            temporary.TurnNumber == openingTurn && temporary.Source.SkillId == OpeningQuota,
            "A finite first-turn grant funds the three original Slashes without an equipped weapon.");

        foreach (var card in cards.Take(2))
        {
            Play(card);
            Reach(CanInvoke);
            Answer(choice => ChoiceAction(choice) == "skip");
            Reach(AtPlay);
        }
        Play(cards[2]);
        Reach(CanInvoke);
        var originalUse = game.ResolutionStack.OfType<CardUseFrame>().Single();
        Answer(choice => ChoiceAction(choice) == "activate");
        Reach(() => Pending()?.Choices.Any(choice => ChoiceAction(choice) == "select-target") == true);
        Answer(choice => ChoiceAction(choice) == "select-target" && choice.Targets.SequenceEqual([1]));
        Reach(() => Pending()?.Choices.Any(choice => ChoiceAction(choice) == "select-and-move-owned-card") == true);
        Answer(choice => ChoiceAction(choice) == "select-and-move-owned-card" && choice.Cards.SequenceEqual([horse]));
        Reach(AtPlay, skipJuzhan: true);

        var grant = Facts<BoundaryYanYanChengshiGrantedEvent>().Single();
        var laterCards = cards.Skip(2).ToArray();
        Need(grant.OwnerSeat == 0 && grant.UseId == originalUse.Id && grant.ActionId == originalUse.Action!.ActionId &&
            Facts<CardUseFinishedEvent>().Count(entry => entry.ResolutionId == originalUse.Id) == 1 &&
            Facts<BoundaryYanYanClaimPaidEvent>().Single() is { Count: 1 } &&
            cards.Take(3).All(card => Used(card, openingTurn)) && Debits(openingTurn) == 3 &&
            game.CardMovements.Count(move => move.CardId == horse && move.From == CardLocation.Equipment(1) &&
                move.To == CardLocation.Hand(0)) == 1 &&
            Facts<DirectedTurnCardPolicyGrantedEvent>().Single().Policy is { ActorSeat: 0, TargetSeat: 1 } policy &&
            policy.TurnNumber == openingTurn && policy.Effects.HasFlag(DirectedTurnCardPolicyEffect.ForbidTarget) &&
            View(0).Hand.Select(card => card.Id).ToHashSet().SetEquals(laterCards.Append(horse)) && OrdinarySlashes().Length == 0,
            "Juzhan on the third paid Slash takes the horse, returns the original Slash and issues the permanent count and the current turn's target ban.");

        Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));
        Reach(() => AtPlay() && game.State.TurnNumber > openingTurn);
        var nextTurn = game.State.TurnNumber;
        var reserved = OrdinarySlashes().Single(action => action.CardId == laterCards[2]);
        Need(Facts<TurnStartedEvent>().Where(entry => entry.ActorSeat == 0).Select(entry => entry.TurnNumber)
                .SequenceEqual([openingTurn, nextTurn]) &&
            Facts<TurnCardUseEffectsExpiredEvent>().Any(entry => entry.TurnSeat == 0 && entry.TurnNumber == openingTurn &&
                entry.GrantSequences.Contains(temporary.GrantSequence)) && Facts<TurnRuleModifierGrantedEvent>().Length == 1 &&
            laterCards.All(card => OrdinarySlashes().Any(action => action.CardId == card)) && Debits(nextTurn) == 0,
            "The first turn's finite grant has expired and all three remaining physical Slashes can target the original opponent at the fresh Play boundary.");

        var before = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(before.CreateSnapshot(0)) &&
            game.CardMovements.SequenceEqual(before.CardMovements) &&
            Facts<BoundaryYanYanChengshiGrantedEvent>().SequenceEqual([grant]),
            "Cold restore preserves the original permanent grant and the unspent later Play boundary.");

        foreach (var card in laterCards.Take(2))
        {
            Play(card);
            Reach(AtPlay, skipJuzhan: true);
        }
        Need(Debits(nextTurn) == 2 && laterCards.Take(2).All(card => Used(card, nextTurn)) &&
            OrdinarySlashes().Length == 0 && View(0).Hand.Any(card => card.Id == laterCards[2] && card.Kind == CardKind.Slash) &&
            View(0).Equipment.Count == 0 && View(1).IsAlive && game.GetAttackRange(0) >= game.GetCombatDistance(0, 1),
            "The later turn spends exactly two counted Slashes with the same opponent still alive and in range and the third printed Slash still in hand.");
        var beforeRejection = game.Revision;
        var rejected = game.Submit(SlashCommand(reserved));
        Need(!rejected.Accepted && rejected.Error?.Code == CommandErrorCode.IllegalAction && game.Revision == beforeRejection &&
            !Used(laterCards[2], nextTurn) && Debits(nextTurn) == 2 && Facts<BoundaryYanYanChengshiGrantedEvent>().SequenceEqual([grant]),
            "Submitting the previously legal third physical Slash against the same target fails at the exhausted finite quota without payment or a new grant.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-mobile-chengshi-quota", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{OpeningQuota}}","revision":1,"triggers":[{"id":"opening-three","window":"playPhaseStarting",
                    "subject":"owner","optional":false,"usageScope":"game","usageLimit":1,
                    "effects":[{"op":"grantTurnRuleModifier","target":"owner","ruleQuery":"slashLimit","ruleOperation":"add","amount":2}]}]}
                ]}
                """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [OpeningQuota] = new { name = "首回合出杀次数", description = "本局首次进入出牌阶段时，本回合用杀次数加二。" }
                }
            }));
            builder.AddSkill(new(OpeningQuota, catalog.Presentations[OpeningQuota].Name, catalog.Presentations[OpeningQuota].Description)
            {
                Program = catalog.Programs[OpeningQuota], ProgramPresentation = catalog.Presentations[OpeningQuota]
            });
            builder.AddGeneral(new(Opponent, "固定相邻对手", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 8), 12).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "乘势有限次数", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Opponent], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
