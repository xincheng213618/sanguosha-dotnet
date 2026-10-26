using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLongdanRuleChecks
{
    private const string Skill = "boundary:longdan", Driver = "fixture:shanhe-longdan-driver";
    private const string Owner = "fixture:shanhe-longdan-owner", Enemy = "fixture:shanhe-longdan-enemy";
    private const string Deck = "fixture:shanhe-longdan-deck";

    public static void AcquiredAlcoholPeachHealsDuringPlayAndStillRescuesDying()
    {
        var s = Scene.Start();
        Need(s.View().GeneralId == Owner && s.View().Skills!.Any(skill => skill.ContentId == Skill) &&
            s.View().Skills!.All(skill => skill.ContentId != "boundary:yajiao"),
            "The unrelated general receives only the acquired Longdan skill through the real Shanhe extra-skill setup.");
        var materials = s.View().Hand.Where(card => card.Kind == CardKind.Alcohol).Select(card => card.Id).ToArray();
        Need(materials.Length == 2 && !s.Game.GetHumanLegalActions().Any(action =>
                action.CardId == materials[0] && action.Kind == LegalActionKind.Peach),
            "A full-health acquired owner cannot use Alcohol as an unnecessary Peach.");

        s.Use("wound-one");
        Need(s.View().Hp == 3, "The owner becomes wounded through a real native lose-HP instruction.");
        var heal = s.Game.GetHumanLegalActions().SingleOrDefault(action => action.CardId == materials[0] &&
            action.Kind == LegalActionKind.Peach && action.ConversionSource is
                { SkillId: Skill, BindingId: "alcohol-to-peach", OwnerSeat: 0 });
        Need(heal is not null, "The acquired four-way Longdan must offer Alcohol as Peach during the owner's real Play phase.");
        s.Accept(new PlayCardCommand(0, materials[0], heal!.TargetSeats, s.Game.Revision, s.Prompt!.PromptId,
            heal.PlayedCardKind, heal.TargetCardId)
        { ConversionSource = heal.ConversionSource, AdditionalConversionSources = heal.AdditionalConversionSources });
        s.Play();
        Need(s.View().Hp == 4 && s.Events<RecoveryAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 0, Amount: 1, RemainingHp: 4 }] &&
            s.Events<CardUseDeclaredEvent>().Single(use => use.CardId == materials[0]) is { CardKind: CardKind.Peach } &&
            s.Game.AcceptedCommands.OfType<PlayCardCommand>().Single(command => command.CardId == materials[0])
                .ConversionSource == heal.ConversionSource &&
            s.PaidOnceAndExpired(materials[0]),
            "A real converted Peach heals once, keeps the exact acquired conversion identity and pays the original Alcohol entity through the normal starting-resource cleanup.",
            new { Owner = s.View(), Recovery = s.Events<RecoveryAppliedEvent>(), Uses = s.Events<CardUseDeclaredEvent>(),
                Actions = s.Events<CardActionAcceptedEvent>(), Movements = s.Game.CardMovements.Where(move => move.CardId == materials[0]),
                Destroyed = s.Events<ShanheStartingResourceDestroyedEvent>() });

        s.Accept(new UseProgramSkillCommand(0, Driver, "dying-four", [], [], s.Game.Revision, s.Prompt!.PromptId));
        s.Reach(() => s.Prompt is { PlayerSeat: 0, Kind: DecisionKind.RescueDying });
        Need(s.View().Hp == 0, "The second native lose-HP instruction reaches the actual dying response window.");
        var response = s.Prompt!.Choices.SingleOrDefault(choice => choice.Cards.SequenceEqual([materials[1]]) &&
            choice.Parameters.GetValueOrDefault("conversion-skill-id") == Skill &&
            choice.Parameters.GetValueOrDefault("conversion-binding-id") == "alcohol-to-peach");
        Need(response is not null, "Enabling own-Play Alcohol-to-Peach preserves its independently legal native dying rescue.");
        s.Accept(new AnswerPromptCommand(0, s.Prompt.PromptId, response!.Id, s.Game.Revision));
        s.Play();
        Need(s.View().Hp == 1 && s.Events<DyingResponseEvent>() is
                [{ ResponderSeat: 0, UsedPeach: true, PeachCardId: var rescueId, UsedAlcohol: false,
                    UsedPeachPhysicalCardKind: CardKind.Alcohol }] && rescueId == materials[1] &&
            s.Events<DyingResolvedEvent>() is [{ VictimSeat: 0, Survived: true }] &&
            s.Events<RecoveryAppliedEvent>().Select(item => item.Amount).SequenceEqual([1, 1]) &&
            s.PaidOnceAndExpired(materials[1]) && !s.Events<AlcoholAppliedEvent>().Any() &&
            s.Game.ResolutionStack.All(frame => frame is not DyingFrame),
            "The same native response conversion rescues once as Peach, grants no wine bonus, and expires only its own physical starting Alcohol after the dying child returns.");
    }

    private static void Need(bool value, string message, object? details = null)
    { if (!value) throw new InvalidOperationException(details is null ? message : message + " " + JsonSerializer.Serialize(details)); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View() => Game.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected acquired Longdan command."); }
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Use(string activation)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], [], Game.Revision, Prompt!.PromptId)); Play(); }
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 100; step++)
            {
                if (condition()) return;
                Need(Prompt is not { PlayerSeat: 0 }, "Unexpected native Longdan prompt: " + JsonSerializer.Serialize(Prompt));
                Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Missing bounded acquired Longdan boundary.");
        }
        public bool PaidOnceAndExpired(int cardId) =>
            Game.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
            Game.CardMovements.Count(move => move.CardId == cardId && move.To == CardLocation.OutsideGame) == 1 &&
            Game.CardMovements.All(move => move.CardId != cardId || move.To != CardLocation.DiscardPile) &&
            Events<ShanheStartingResourceDestroyedEvent>().Count(item => item.CardId == cardId) == 1;
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(3033, 1, "fixture:longdan-own-play", Owner, 4, 0, 0, 0, 0, [Skill],
                [new(CardKind.Alcohol, Suit.Spade, 9), new(CardKind.Alcohol, Suit.Diamond, 9)], [],
                [new(Enemy)], [], new string('a', 64)) { StartingResourcesExpireOnUse = true };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-longdan-rule", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "boundary:zhao-yun");
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"wound-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"dying-four","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"loseHp","target":"owner","amount":4}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实失去体力", description = "原生回复与濒死流程驱动" } } }));
            builder.AddSkill(new(Driver, Driver, Driver) { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "额外技能龙胆持有者", "supporter", Driver, "shu", 4));
            builder.AddGeneral(new(Enemy, "无技能固定敌军", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "零自然摸牌固定牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河额外龙胆自疗与救援", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
