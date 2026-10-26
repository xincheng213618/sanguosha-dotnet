using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheZuiquanChecks
{
    private const string Owner = "fixture:zuiquan-owner", Enemy = "fixture:zuiquan-enemy", Deck = "fixture:zuiquan-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(
        new StandardContentPackage(), new StandardShanheZuiquanPackage(), new Fixture()));

    public static void CompleteSlashFamilyBypassesDodgesWithoutWineAndKeepsNativeAlcohol()
    {
        // Giving the defender the tactic must not silence a different actor's Slash.
        var baseline = Scene.Start(CardKind.Slash, ownerHasTactic: false);
        var baselineDodge = baseline.View(1).Hand.Single().Id;
        baseline.UseCard(CardKind.Slash, [1]);
        Need(baseline.View(1).Hp == 12 && baseline.Events<DamageAppliedEvent>().Length == 0 &&
            baseline.Events<CardRespondedEvent>().Count(e => e.CardId == baselineDodge && e.ResponderSeat == 1) == 1,
            "The control's real defender owns Zuiquan but still pays its actual Dodge against another actor's ordinary Slash.");

        foreach (var kind in new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash })
        {
            var scene = Scene.Start(kind, ownerHasTactic: true);
            var dodge = scene.View(1).Hand.Single().Id;
            var slash = scene.UseCard(kind, [1]);
            AssertUncanceledHit(scene, slash, dodge, kind, 1);
            Need(scene.Events<AlcoholAppliedEvent>().Length == 0 && !scene.View().HasAlcoholEffect,
                "Each complete-family physical Slash is protected while sober; no Wine use or extra Wine prerequisite is manufactured.");
        }

        var wineScene = Scene.Start(CardKind.Slash, ownerHasTactic: true);
        var retainedDodge = wineScene.View(1).Hand.Single().Id;
        var wine = wineScene.UseCard(CardKind.Alcohol, []);
        Need(wineScene.View().HasAlcoholEffect &&
            wineScene.Events<AlcoholAppliedEvent>() is [{ SourceSeat: 0, DamageBonus: 1 }] &&
            wineScene.Events<NullificationRespondedEvent>().Length == 0 &&
            wineScene.Events<CardUseFinishedEvent>().Count(e => e.CardId == wine && e.CardKind == CardKind.Alcohol) == 1,
            "A genuine paid Wine retains its ordinary non-counterspell native effect and finishes its original use once.");
        var boostedSlash = wineScene.UseCard(CardKind.Slash, [1]);
        AssertUncanceledHit(wineScene, boostedSlash, retainedDodge, CardKind.Slash, 2);
        Need(!wineScene.View().HasAlcoholEffect && wineScene.Events<AlcoholAppliedEvent>().Length == 1,
            "The following protected native Slash consumes the existing Wine bonus once instead of creating a new alcohol effect.");
    }

    private static void AssertUncanceledHit(Scene scene, int slashId, int dodgeId, CardKind kind, int amount)
    {
        var declared = scene.Events<CardUseDeclaredEvent>().Single(e => e.CardId == slashId);
        Need(scene.View(1).Hp == 12 - amount && scene.View(1).Hand.Single().Id == dodgeId &&
            scene.Events<CardRespondedEvent>().All(e => e.CardId != dodgeId) &&
            scene.Events<ResponseRequestedEvent>().All(e => e.TargetSeat != 1 || e.RequiredCardKind != CardKind.Dodge) &&
            scene.Events<DamageAppliedEvent>() is [{ SourceSeat: 0, TargetSeat: 1, Amount: var paid, SourceCardUseResolutionId: var use }] &&
            paid == amount && use == declared.ResolutionId && declared.CardKind == kind &&
            scene.Events<ProgramBindingResolvedEvent>().Count(e => e.SkillId == StandardShanheZuiquanPackage.SkillId && e.Completed) == 1 &&
            scene.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == declared.ResolutionId) == 1 &&
            scene.Game.CardMovements.Count(e => e.CardId == slashId && e.From == CardLocation.Hand(0) && e.To == CardLocation.Processing) == 1,
            "Zuiquan prohibits the actual Dodge response for its own full-family Slash, retains the target's real entity, applies the native damage and completes one once-paid original use.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        private PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot View(int seat = 0) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Zuiquan command."); }
        private void Play()
        {
            for (var step = 0; step < 120; step++)
            {
                if (Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                if (Prompt is { PlayerSeat: 0 } prompt)
                {
                    var preserveDamage = prompt.Choices.SingleOrDefault(choice =>
                        choice.Parameters.GetValueOrDefault("program-action") == "native-cold-damage" &&
                        choice.Parameters.GetValueOrDefault("action") == "cold-damage");
                    Need(preserveDamage is not null, "An uncanceled Slash reached an unexpected human decision.");
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, preserveDamage!.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded Zuiquan scene did not return to Play.");
        }
        public int UseCard(CardKind kind, int[] targets)
        {
            var card = View().Hand.Single(card => card.Kind == kind);
            var action = Game.GetHumanLegalActions().First(candidate => candidate.CardId == card.Id &&
                candidate.ConversionSource is null && candidate.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, Game.Revision, Prompt!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            Play();
            return card.Id;
        }
        public static Scene Start(CardKind kind, bool ownerHasTactic)
        {
            var setup = new ShanheBattleSetup(113, 1, "fixture:zuiquan", Owner, 12, 0, 0, 0, 0,
                [], [new(kind, Suit.Heart, 8), new(CardKind.Alcohol, Suit.Spade, 9)], [],
                [new(Enemy, ExtraSkillIds: [StandardShanheZuiquanPackage.SkillId])
                    { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)] }], [], new string('f', 64))
                { TacticSkillIds = ownerHasTactic ? [StandardShanheZuiquanPackage.SkillId] : [] };
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-zuiquan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Owner, "醉拳使用者", "supporter", "standard:none", "qun", 12));
            builder.AddGeneral(new(Enemy, "固定持闪守军", "supporter", "standard:none", "wei", 12));
            builder.AddCard(new("fixture:zuiquan-ice-slash", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            builder.AddDeck(new(Deck, "零普通摸牌实体闪", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "醉拳响应实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
