using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGouXiangArmorChecks
{
    private const string Owner = "fixture:gouxiang-owner", Enemy = "fixture:gouxiang-enemy",
        Deck = "fixture:gouxiang-deck", Seal = "fixture:gouxiang-turn-seal";
    private const string Skill = StandardShanheGouXiangArmorPackage.SkillId;
    private const string TakeReason = "skill-program.shanhe:gouxiang-armor-14088.random-hand";

    public static void ActualMatchingDodgeStealsOncePerResponseAndHonoursArmorEffectiveness()
    {
        foreach (var scenario in new[] { "suit", "rank", "same-colour", "double", "bypass", "sealed", "replaced" })
        {
            var scene = Scene.Start(scenario);
            var armor = scene.View(0).Equipment.Single(card => card.Kind == CardKind.GouXiang);
            Need(scene.HasArmorSkill && armor.Suit == Suit.Club && armor.Rank == 2 &&
                scene.Game.CardMovements.Count(move => move.CardId == armor.Id && move.From == CardLocation.OutsideGame &&
                    move.To == CardLocation.Equipment(0)) == 1,
                "The real source20883 Club2 armor enters its native slot and grants exactly its equipment-bound rule.");
            var dodges = scene.View(0).Hand.Where(card => card.Kind == CardKind.Dodge).Select(card => card.Id).ToArray();
            var originalEnemyHand = scene.View(1).Hand.Select(card => card.Id).ToArray();
            var slashCard = scene.View(1).Hand.Single(card => card.Kind == CardKind.Slash).Id;
            if (scenario == "replaced")
            {
                var replacement = scene.View(0).Hand.Single(card => card.Kind == CardKind.BaguaFormation).Id;
                var equip = scene.Game.GetHumanLegalActions().Single(action => action.CardId == replacement && action.Kind == LegalActionKind.Equip);
                scene.Submit(new PlayCardCommand(0, replacement, equip.TargetSeats, scene.Game.Revision,
                    scene.Prompt!.PromptId, equip.PlayedCardKind));
                scene.Play();
                Need(!scene.HasArmorSkill && scene.View(0).Equipment.Any(card => card.Id == replacement) &&
                    scene.Game.CardMovements.Count(move => move.CardId == armor.Id && move.From == CardLocation.Equipment(0)) == 1,
                    "Using a real replacement armor removes the Gouxiang entity and its granted source before the incoming Slash.");
            }
            scene.Submit(new EndPlayPhaseCommand(0, scene.Game.Revision, scene.Prompt!.PromptId));
            scene.Reach(() => scene.Prompt is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
            var slash = scene.Events<CardUseDeclaredEvent>().Single(fact => fact.SourceSeat == 1 && fact.CardKind == CardKind.Slash);
            Need(slash.CardId == slashCard && scene.Game.CardMovements.Count(move => move.CardId == slashCard &&
                move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) == 1,
                "The actual enemy uses its paid physical Slash; the armor does not synthesize an incoming attack or response.");
            var expected = scenario is "suit" or "rank" ? 1 : scenario == "double" ? 2 : 0;
            for (var response = 0; response < dodges.Length; response++)
            {
                scene.Answer(choice => choice.Cards.SequenceEqual([dodges[response]]));
                if (response + 1 < dodges.Length)
                {
                    scene.Reach(() => scene.Prompt is { Kind: DecisionKind.RespondDodge, PlayerSeat: 0 });
                    Need(scene.Takes.Length == 1 && scene.Events<CardResponseCompletedEvent>().Count(fact =>
                        fact.EffectiveKind == CardKind.Dodge && fact.ParentFrameId == slash.ResolutionId) == 1 &&
                        !scene.Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == slash.ResolutionId),
                        "The first real Dodge of Wushuang already steals once while the original Slash still requires its second response.");
                }
            }
            scene.Reach(() => scene.Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == slash.ResolutionId) &&
                scene.Game.ResolutionStack.Count == 0);
            var transfers = scene.Game.CardMovements.Where(move => move.Reason.Value == TakeReason).ToArray();
            var responses = scene.Events<CardActionAcceptedEvent>().Select(fact => fact.Action)
                .Where(action => action.Type == CardActionType.Response && action.EffectiveKind == CardKind.Dodge).ToArray();
            Need(scene.Takes.Length == expected && scene.Takes.All(fact => fact.OwnerSeat == 0 && fact.CardCount == 1 &&
                    fact.TargetSeats.SequenceEqual([1])) && transfers.Length == expected &&
                transfers.Select(move => move.CardId).Distinct().Count() == expected &&
                transfers.All(move => originalEnemyHand.Contains(move.CardId) && move.CardId != slashCard &&
                    move.From == CardLocation.Hand(1) && move.To == CardLocation.Hand(0) &&
                    scene.View(0).Hand.Any(card => card.Id == move.CardId)) &&
                scene.View(1).HandCount == 2 - expected && scene.View(0).Hp == 10 &&
                scene.Events<DamageAppliedEvent>().Length == 0,
                "Each qualifying actual Dodge takes one real random remaining enemy Hand entity; colour alone, native armor bypass, a real turn seal and equipment loss take none.");
            Need(responses.Length == dodges.Length && responses.All(action => action.ActorSeat == 0 && action.ProviderSeat == 0 &&
                    action.OpponentSeat == 1 && action.PhysicalCards.Count == 1 && dodges.Contains(action.PhysicalCards[0].CardId)) &&
                responses.Select(action => action.ActionId).Distinct().Count() == dodges.Length &&
                scene.Events<CardResponseCompletedEvent>().Count(fact => fact.ParentFrameId == slash.ResolutionId &&
                    fact.ActorSeat == 0 && fact.ProviderSeat == 0 && fact.EffectiveKind == CardKind.Dodge) == dodges.Length &&
                dodges.All(id => scene.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Respond) == 1) &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == slash.ResolutionId) == 1 &&
                scene.Game.State.Winner == Winner.None,
                "Every genuine response and material is committed once and returns to the same Slash; double Dodge neither coalesces nor repeats the armor trigger.");
            if (scenario == "sealed")
                Need(scene.Events<CardTargetRestrictionGrantedEvent>().Any(fact =>
                    fact.Restriction.TargetSeat == 0 && fact.Restriction.Restriction == SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn),
                    "The negative case uses a real current-turn armor restriction, rather than deleting or faking its armor grant.");
        }
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players.Single(player => player.Seat == seat);
        public bool HasArmorSkill => View(0).Skills?.Any(skill => skill.ContentId == Skill) == true;
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public ProgramRandomHandCardsTakenEvent[] Takes => Events<ProgramRandomHandCardsTakenEvent>().Where(fact => fact.SkillId == Skill).ToArray();
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected real Gouxiang command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt!;
            var choice = prompt.Choices.Single(predicate);
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, Game.Revision));
        }
        public void Play() => Reach(() => Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0);
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 320; step++)
            {
                if (ready()) return;
                Need(Game.State.Winner == Winner.None, "The Gouxiang fixture must remain a bounded live attack.");
                if (Prompt is { } prompt && !(prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0))
                    throw new InvalidOperationException("Unexpected Gouxiang decision: " + JsonSerializer.Serialize(new
                    { prompt.Kind, prompt.PlayerSeat, Skill = prompt.SkillPrompt?.SkillId, Game.State.Phase, Game.State.CurrentSeat }));
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Gouxiang did not reach its bounded native boundary.");
        }
        public static Scene Start(string scenario)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheGouXiangArmorPackage(), new Fixture(scenario));
            var slashSuit = scenario is "rank" or "same-colour" ? Suit.Spade : Suit.Club;
            var cards = new List<ShanheStartingCard>
            { new(CardKind.Dodge, scenario == "rank" ? Suit.Heart : Suit.Club, scenario == "rank" ? 7 : 2) };
            if (scenario == "double") cards.Add(new(CardKind.Dodge, Suit.Heart, 7));
            if (scenario == "replaced") cards.Add(new(CardKind.BaguaFormation, Suit.Spade, 2));
            var setup = new ShanheBattleSetup(71, 1, "gouxiang-native", Owner, 10, 0, 0, 0, 0, [], cards,
                [new(CardKind.GouXiang, Suit.Club, 2)],
                [new(Enemy) { StartingHand = [new(CardKind.Slash, slashSuit, 7), new(CardKind.Peach, Suit.Heart, 3),
                    new(CardKind.Peach, Suit.Diamond, 4)], StartingEquipment = scenario == "bypass" ? [new(CardKind.QinggangSword, Suit.Spade, 6)] : [] }],
                [], new string('b', 64));
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, registry));
            scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:gouxiang-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            if (scenario == "double") CharacterFixtureContent.Register(builder, "classic:lu-bu");
            var rules = SkillProgramCatalog.Load(JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Seal, revision = 1, triggers = new[] { new
                {
                    id = "actual-turn-armor-seal", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false,
                    usageScope = "game", usageLimit = 1, effects = new object[]
                    {
                        new { op = "selectTarget", target = "owner", targetKind = "otherLiving" },
                        new { op = "grantTurnCardTargetRestriction", target = "selectedTarget", targetRestriction = "armorIneffectiveForTurn" }
                    }
                } } } }
            }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Seal] = new { name = "实际禁甲", description = "当前真实回合令防具无效。" } } }));
            builder.AddSkill(new(Seal, "实际禁甲", "当前真实回合令防具无效。")
            { Program = rules.Programs[Seal], ProgramPresentation = rules.Presentations[Seal] });
            builder.AddGeneral(new(Owner, "钩镶持有者", "supporter", "standard:none", "shu", 10));
            builder.AddGeneral(new(Enemy, "真实杀来源", "supporter", "standard:none", "qun", 6,
                scenario == "double" ? ["classic:wushuang"] : scenario == "sealed" ? [Seal] : []));
            builder.AddDeck(new(Deck, "无额外自然牌", 0, 0, [new("standard:dodge", 32)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "钩镶真实响应", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }

    private static void Need(bool yes, string reason)
    { if (!yes) throw new InvalidOperationException(reason); }
}
