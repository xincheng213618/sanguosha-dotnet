using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceZhangYanHighStageChecks
{
    private const string Driver = "fixture:source-zhangyan-high-driver", Deck = "fixture:source-zhangyan-high-deck";
    private const int BossSeat = 1;

    public static void HigherSource2653KeepsCumulativeSkillsAndRealSanshouAcrossItsNativeStage()
    {
        // Warm the existing medium factory cache before requesting recipes
        // that additionally require a physical Sanshou definition and program.
        var medium = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(6);
        _ = StandardShanheCampaignCatalog.CreateBattleRegistry(Setup(medium, Encounter(medium), 6, false));

        foreach (var difficulty in new[] { 11, 16, 21 })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var encounter = Encounter(source);
            var firstId = difficulty >= 16 ? StandardShanheSourceZhangYanPackage.ZhangniujiaoNightmareId :
                StandardShanheSourceZhangYanPackage.ZhangniujiaoHardId;
            Need(encounter.SourceCityId == 5068 && encounter.Enemies is [{ GeneralId: var id }] && id == firstId &&
                encounter.BossTransition is { GeneralId: StandardShanheSourceZhangYanPackage.ZhangYanHardId, Hp: 4, MaxHp: 4 } &&
                (encounter.Enemies[0].StartingCards ?? []).Any(IsSanshou) &&
                encounter.BossTransition.StartingEquipment is [var stageArmor] && IsSanshou(stageArmor),
                "The actual higher source city retains the matching parent tier and a complete ZhangYan stage, with HeartQ Sanshou in the first hand and second-form equipment recipes.");

            var setup = Setup(source, encounter, difficulty, true);
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] });
            var firstSkills = GeneralSkills(registry.Generals[firstId]);
            Need(firstSkills.SequenceEqual((difficulty >= 16 ? new[] { StandardShanheS5XuncanCaoshiPackage.LeijieId,
                    "classic:tiandu", "boundary:guicai" } : [StandardShanheS5XuncanCaoshiPackage.LeijieId, "classic:tiandu"]).Order()) &&
                GeneralSkills(registry.Generals[StandardShanheSourceZhangYanPackage.ZhangYanHardId]).SequenceEqual(new[]
                    { StandardShanheSourceZhangYanPackage.YanyuSkillId, StandardShanheSourceZhangYanPackage.BiriSkillId,
                      StandardShanheSourceZhangYanPackage.DongdangSkillId }.Order()) &&
                registry.Cards.Values.Any(card => card.LegacyKind == CardKind.ShanheSanshou) &&
                registry.Skills.ContainsKey(StandardShanheSanshouArmorPackage.SkillId),
                "The production factory, including its warmed dependency cache, contains exact cumulative printed skills and the real Sanshou card/program rather than medium-only or ordinary ZhangYan content.");

            var scene = new Scene(setup, registry.WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode));
            scene.Submit(new StartGameCommand());
            scene.Reach(() => scene.Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && scene.Game.ResolutionStack.Count == 0);
            var firstArmor = scene.Boss.Hand.Single(card => card.Kind == CardKind.ShanheSanshou);
            Need(scene.Boss is { GeneralId: var actualFirst, Hp: 3, MaxHp: 3, HandCount: 5 } && actualFirst == firstId &&
                scene.Boss.Equipment.Count == 0 && firstArmor is { Suit: Suit.Heart, Rank: 12 },
                "The first form has its actual cumulative body and holds the extra physical Sanshou beside four ordinary initial cards, without silently pre-equipping it.");

            var before = scene.Game.CardMovements.Select(move => move.Sequence).DefaultIfEmpty(0).Max();
            var oldTurn = scene.Game.State.TurnNumber;
            var round = scene.Events<RoundStartedEvent>().Last().RoundNumber;
            scene.Submit(new UseProgramSkillCommand(0, Driver, "stage", [], [BossSeat], scene.Game.Revision, scene.Prompt!.PromptId));
            scene.Reach(() => scene.Events<ShanheBossStageStartedEvent>().Length == 1 &&
                scene.Game.State.CurrentSeat == BossSeat && scene.Game.State.Phase == TurnPhase.Play && scene.Game.ResolutionStack.Count == 0);
            var equipped = scene.Boss.Equipment.Single();
            Need(scene.Boss is { GeneralId: StandardShanheSourceZhangYanPackage.ZhangYanHardId, Hp: 4, MaxHp: 4, IsAlive: true } &&
                equipped is { Kind: CardKind.ShanheSanshou, Suit: Suit.Heart, Rank: 12 } && equipped.Id != firstArmor.Id &&
                scene.Boss.Hand.Any(card => card.Id == firstArmor.Id) &&
                scene.Game.CardMovements.Count(move => move.Sequence > before && move.CardId == equipped.Id &&
                    move.From == CardLocation.OutsideGame && move.To == CardLocation.Equipment(BossSeat)) == 1 &&
                new[] { StandardShanheSourceZhangYanPackage.YanyuSkillId, StandardShanheSourceZhangYanPackage.BiriSkillId,
                        StandardShanheSourceZhangYanPackage.DongdangSkillId, StandardShanheSanshouArmorPackage.SkillId }
                    .All(skillId => scene.Boss.Skills!.Any(skill => skill.ContentId == skillId)) &&
                !scene.Boss.Skills!.Any(skill => skill.ContentId is "classic:tiandu" or "boundary:guicai" ||
                    skill.ContentId == StandardShanheS5XuncanCaoshiPackage.LeijieId),
                "The native transition installs one new real Sanshou armor entity, retains the prior hand entity, binds its skill and replaces all first-form printed skills with the full four-HP source phase.");
            Need(scene.Events<ShanheBossTurnInterruptedEvent>() is [{ PlayerSeat: 0, BossSeat: BossSeat }] &&
                scene.Events<ShanheSourceDongdangActivatedEvent>().Length == 1 &&
                scene.Events<TurnStartedEvent>().Last().ActorSeat == BossSeat && scene.Game.State.TurnNumber > oldTurn &&
                scene.Events<RoundStartedEvent>().Last().RoundNumber == round,
                "The actual higher-stage Dongdang records its single native activation and starts an actionable boss turn in the same round after retiring the original actor.");
            var entry = scene.Events<ShanheSourceDongdangActivatedEvent>().Single();
            Need(entry.BossSeat == BossSeat && entry.InterruptedSeat == 0 && entry.TurnNumber == oldTurn &&
                entry.PreviousGeneralId == firstId && entry.GeneralId == StandardShanheSourceZhangYanPackage.ZhangYanHardId &&
                entry.Source.SkillId == StandardShanheSourceZhangYanPackage.DongdangSkillId &&
                entry.Source.BindingId == ShanheSourceDongdangRules.BindingId && entry.Source.OwnerSeat == BossSeat &&
                !string.IsNullOrWhiteSpace(entry.Source.SkillInstanceId),
                "The recorded entry activation retains the effective boss-owned Dongdang instance, original actor/turn and exact source form replacement.");

            scene.Reach(() => scene.BossSlashes().Length >= 1 && scene.BossSlashes().All(use =>
                scene.Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.ResolutionId)) && scene.Game.ResolutionStack.Count == 0);
            var slash = scene.BossSlashes()[0];
            Need(scene.Game.CardMovements.Count(move => move.CardId == slash.CardId && move.From == CardLocation.Hand(BossSeat) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == slash.ResolutionId) == 1 &&
                scene.Events<ShanheBossStageStartedEvent>().Length == 1 && scene.Game.State.Winner == Winner.None,
                "The complete higher source panel proceeds to an actual paid physical Slash and completes it once without repeating its stage or settling the battle.");
        }
    }

    private static ShanheEncounterDefinition Encounter(ShanheCampaignDefinition source) => source.Nodes
        .Single(node => node.Id == "source-city-5068").Encounters!.Single(encounter => encounter.SourceBattleId == 2653);
    private static bool IsSanshou(ShanheStartingCard card) => card is { Kind: CardKind.ShanheSanshou, Suit: Suit.Heart, Rank: 12 };
    private static string[] GeneralSkills(ContentGeneralDefinition general) => new[] { general.SkillId }.OfType<string>()
        .Concat(general.AdditionalSkillIds ?? []).Order().ToArray();
    private static ShanheBattleSetup Setup(ShanheCampaignDefinition source, ShanheEncounterDefinition encounter, int difficulty, bool driver) =>
        new(71, 1, "fixture:source-zhangyan-high-" + difficulty, "classic:guan-yu", 34, 30, 0, 0, 0,
            driver ? [Driver] : [], [new(CardKind.Dodge, Suit.Club, 2)], [],
            encounter.Enemies.Select(member => member with
            { StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7), 4).ToArray() }).ToArray(), [],
            ShanheCampaignContent.GetContentHash(source))
        { SourceDifficultyId = difficulty, BossTransition = encounter.BossTransition };

    private sealed class Scene(ShanheBattleSetup setup, ContentRegistry registry)
    {
        public GameEngine Game { get; } = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot Boss => Game.CreateSnapshot(BossSeat).Players[BossSeat];
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public CardUseDeclaredEvent[] BossSlashes() => Events<CardUseDeclaredEvent>()
            .Where(fact => fact.SourceSeat == BossSeat && fact.CardKind == CardKind.Slash).ToArray();
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 300; step++)
            {
                if (done()) return;
                Need(Game.State.Winner == Winner.None, "The bounded higher-stage fixture ended before its required native boundary.");
                if (Prompt is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0)
                    { Submit(new AdvanceOneStepCommand(Game.Revision)); continue; }
                    var pass = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0 &&
                        choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(pass is not null, $"Unexpected higher ZhangYan boundary: {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, pass!.Id, Game.Revision));
                    continue;
                }
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException($"Higher ZhangYan flow exceeded its bound: {Prompt?.Kind}/{Game.State.CurrentSeat}/{Game.State.Phase}.");
        }
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected higher-stage native command."); }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-zhangyan-high", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"stage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "阶段攻击", description = "真实致死伤害触发来源高档阶段。" } } }));
            builder.AddSkill(new(Driver, "阶段攻击", "真实致死伤害触发来源高档阶段。")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddDeck(new(Deck, "固定实体杀", 0, 2, [new("standard:slash", 100)]));
        }
    }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
