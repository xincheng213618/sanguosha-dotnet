using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundarySunLuBanScenario
{
    internal const string Zenhui = "boundary:zenhui-mobile", Jiaojin = "boundary:jiaojin-mobile", Driver = "fixture:boundary-sunluban-driver", Watch = "fixture:boundary-sunluban-watch";
    private const string Hero = "boundary:sun-lu-ban-mobile", Male = "fixture:boundary-sunluban-male", Female = "fixture:boundary-sunluban-female", Quiet = "fixture:boundary-sunluban-quiet", Deck = "fixture:boundary-sunluban-deck";

    internal static (ContentRegistry Registry, GameEngine Game) CreateScene(string pattern = "gain", bool withWatch = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        ShanheStartingCard[] hand = pattern switch
        {
            "borrowed" => [new(CardKind.BorrowedSword, Suit.Club, 12), new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.GudingBlade, Suit.Spade, 1)],
            "self-chain" => [new(CardKind.IronChain, Suit.Club, 12)],
            "black-draw" => [new(CardKind.DrawTwo, Suit.Spade, 12)],
            "defense" => [new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.SilverLion, Suit.Heart, 1)],
            _ => [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Slash, Suit.Spade, 8)]
        };
        var setup = new ShanheBattleSetup(47, 1, "boundary-sunluban-" + pattern, Hero, 3, 0, 0, 0, 0,
            withWatch ? [Quiet, Driver, Watch] : [Quiet, Driver], hand, [],
            [new(Male, Hp: 4) { StartingHand = pattern is "gain" or "black-draw" ? [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Peach, Suit.Diamond, 3)] : [] },
             new(Female, Hp: 4) { StartingHand = [] }], [new(Male, Hp: 4) { StartingHand = [] }], new string('f', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
        Accept(game, new StartGameCommand()); Play(game);
        if (pattern == "borrowed")
        {
            EquipPeer(game, 1, CardKind.Crossbow); EquipPeer(game, 2, CardKind.GudingBlade);
        }
        return (registry, game);
    }

    internal static PendingDecision? Decision(GameEngine game) => game.PendingDecision ?? game.CreateSnapshot(0).PendingDecision;
    internal static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); if (!result.Accepted) throw new InvalidOperationException(result.Error?.Message ?? "Rejected Sun Lu Ban scene command."); }
    internal static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Decision(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    internal static void Reach(GameEngine game, Func<PendingDecision?, bool> done)
    {
        for (var step = 0; step < 200; step++)
        {
            var prompt = Decision(game); if (done(prompt)) return;
            if (prompt is not null) throw new InvalidOperationException("Unexpected Sun Lu Ban decision: " + prompt.Kind + ", seat " + prompt.PlayerSeat + ", skill " + prompt.SkillPrompt?.SkillId);
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded Sun Lu Ban scene did not reach its next native decision.");
    }
    internal static void Play(GameEngine game) => Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    internal static void Use(GameEngine game, string activation, int[] targets) =>
        Accept(game, new UseProgramSkillCommand(0, Driver, activation, [], targets, game.Revision, Decision(game)!.PromptId));
    internal static int UseCard(GameEngine game, CardKind kind, int[] targets)
    {
        var card = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == kind);
        Accept(game, new PlayCardCommand(0, card.Id, targets, game.Revision, Decision(game)!.PromptId, kind)); return card.Id;
    }
    internal static int InvitationSeat(GameEngine game) => game.CreateSnapshot(0).Players.Single(player => player.Seat != 0 && player.HandCount > 0).Seat;
    internal static int GainPrimaryTarget(GameEngine game)
    {
        var view = game.CreateSnapshot(0);
        return view.Players.Single(player => player.Seat != 0 && player.TeamId == view.Players[0].TeamId).Seat;
    }
    internal static int MaleSourceSeat(GameEngine game) => game.CreateSnapshot(0).Players.First(player => player.GeneralId == Male).Seat;
    internal static int FemaleSourceSeat(GameEngine game) => game.CreateSnapshot(0).Players.Single(player => player.GeneralId == Female).Seat;
    internal static void BeginZenhui(GameEngine game, int? selected = null)
    {
        Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Zenhui && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
        Answer(game, choice => choice.Targets.SequenceEqual([selected ?? InvitationSeat(game)]));
    }
    internal static bool OwnBranch(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Zenhui && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "boundary-sun-lu-ban-choice");
    internal static bool WatchPending(PendingDecision? prompt) => prompt?.SkillPrompt?.SkillId == Watch;
    internal static void ContinueWatch(GameEngine game) => Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
    private static void EquipPeer(GameEngine game, int peer, CardKind kind)
    {
        var card = game.CreateSnapshot(0).Players[0].Hand.First(card => card.Kind == kind).Id;
        Use(game, "equip-peer", [peer]);
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        Answer(game, choice => choice.Cards.SequenceEqual([card])); Play(game);
    }

    internal static string FixtureRules => $$$$"""
        {"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[
          {"id":"{{{{Quiet}}}}","revision":1,"modifiers":[{"id":"hold","query":"handLimit","operation":"add","value":20,"priority":0},{"id":"fixture-range","query":"attackRange","operation":"set","value":4,"priority":0}],"triggers":[
            {"id":"quiet-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
            {"id":"native-observer","window":"nativeDrawStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
          {"id":"{{{{Driver}}}}","revision":1,"activations":[
            {"id":"equip-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
              {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetEquipment","targetRef":{"kind":"selectedTarget"},"resultBind":"gear","awaitMovementTriggers":true}]},
            {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]}]},
          {"id":"{{{{Watch}}}}","revision":1,"triggers":[
            {"id":"role-finalized","window":"cardUseBeforeTargetEffects","ownerRelation":"actor","cardKinds":["borrowedSword"],"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"role-return","options":[{"id":"continue"}]}]},
            {"id":"obtained","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.boundary:zenhui-mobile.SelectAndMoveOwnedCard"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"obtained-return","options":[{"id":"continue"}]}]},
            {"id":"native-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{{{CardMoveReasons.Draw.Value}}}}"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-return","options":[{"id":"continue"}]}]},
            {"id":"discarded-equipment","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementReasons":["skill-program.boundary:jiaojin-mobile.SelectAndMoveOwnedCard"],"movementOccurrence":"perOwnerBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid-return","options":[{"id":"continue"}]}]}]}
        ]}
        """;
    internal static string FixturePresentation => $$$$"""
        {"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{
          "{{{{Quiet}}}}":{"name":"固定手牌","description":"不增加普通摸牌，保留原手牌。"},
          "{{{{Driver}}}}":{"name":"原生场景驱动","description":"通过公开程序能力装备目标和造成两点真实伤害。"},
          "{{{{Watch}}}}":{"name":"原子窗返回","description":"原生获得与弃置子窗暂停。","optionLabels":{"continue":"返回原结算"}}
        }}
        """;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-sunluban", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            new StandardClassicGeneralPackage().Register(new PhysicalCardSupportBuilder(builder));
            var programs = SkillProgramCatalog.Load(FixtureRules, FixturePresentation);
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Male, "男性角色", "supporter", Quiet, "shu", 4));
            builder.AddGeneral(new(Female, "女性角色", "supporter", Quiet, "wei", 4, Gender: GeneralGender.Female));
            builder.AddDeck(new(Deck, "固定实体牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "孙鲁班原生结算场景", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Male, Female], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
    // Reuse the genuine package definitions, including their complete equipment
    // metadata, without adding unrelated generals, skills, decks or modes.
    private sealed class PhysicalCardSupportBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddCard(ContentCardDefinition definition)
        {
            if (definition.LegacyKind is CardKind.BorrowedSword or CardKind.GudingBlade or CardKind.SilverLion)
                inner.AddCard(definition);
        }
        public void AddSkill(ContentSkillDefinition definition) { }
        public void AddGeneral(ContentGeneralDefinition definition) { }
        public void AddDeck(ContentDeckRecipe definition) { }
        public void AddMode(ContentModeDefinition definition) { }
    }
}
