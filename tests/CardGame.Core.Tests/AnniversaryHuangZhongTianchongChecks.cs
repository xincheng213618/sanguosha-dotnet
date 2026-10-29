using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryHuangZhongTianchongChecks
{
    private const string Hero = "anniversary:shen-huang-zhong", Skill = "anniversary-god:lieqiong";
    private const string Driver = "fixture:huang-zhong-two-hits", Peer = "fixture:huang-zhong-peer", Deck = "fixture:huang-zhong-deck";

    public static void ArmoredNativeSlashSecondBodyTianchongReturnsThroughFengshang()
    {
        var setup = new ShanheBattleSetup(71, 1, "huang-zhong-paid-dying", Hero, 4, 0, 0, 0, 0, [Driver],
            [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Spade, 8),
                new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Club, 3)],
            [new(CardKind.ShanheXingtianAxe)],
            [new(StandardShanheSourceMouDongZhuoPackage.GeneralId, MaxHpBonus: 6, Hp: 26)
                { InitialArmor = 9, StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(StandardShanheCompanionPackage.ManzhuId) { StartingHand = [] }], new string('a', 64))
            { DefaultGodFactionId = "qun" };
        var required = setup with { ExtraSkillIds = [], EnemyRoster = [setup.EnemyRoster[0], new("classic:guan-yu")] };
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(required).WithExtension(new Fixture(),
            mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck,
                GeneralPoolIds = mode.GeneralPoolIds!.Append(Peer).Distinct(StringComparer.Ordinal).ToArray() } : mode);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false, AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 3
        }, registry);
        Accept(game, new StartGameCommand());
        Play(game);
        Slash(game);
        Body(game, "Qihai");
        Play(game);
        Need(game.CreateSnapshot(0).Players[2] is { Hp: 26, Armor: 5 } && Events<AnniversaryHzBodyIssuedEvent>(game).Count() == 1,
            "The first actual four-damage Slash consumes four armor and completes its Qihai choice once.");
        Slash(game);
        Reach(game, () => IsBody(game));
        var prompt = P(game)!;
        Need(prompt.Choices.Count == 5 && game.CreateSnapshot(0).Players[2] is { Hp: 26, Armor: 1 } &&
            Events<DamageAppliedEvent>(game).Count(e => e.SourceSeat == 0 && e.TargetSeat == 2 && e.Amount == 4 &&
                e.ArmorPayment is { ArmorAbsorbed: 4, HpLossAmount: 0 }) == 2 &&
            Events<CardTargetRestrictionGrantedEvent>(game).Count(e => e.Restriction.Source.SkillId == StandardShanheXingtianPackage.SkillId) == 2,
            "The second genuine armored hit offers all five bodies after one original paid S5 axe seal.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "Tianchong");
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
        var paid = root.AnniversaryHuangZhong!;
        Need(paid is { Stage: AnniversaryHzStage.TianchongChildren, HpPaid: true, Amount: 26, DyingId: > 0 } &&
            Events<ProgramSkillHpLostEvent>(game).Single(loss => loss.FrameId == root.Id) is { Amount: 26, RemainingHp: 0 } &&
            game.ResolutionStack.Last() is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingEntering } entry &&
            entry.ResumeDyingFrameId == paid.DyingId && entry.Candidates.Any(c => c.SkillId == StandardShanheSourceMouDongZhuoPackage.FengshangSkillId),
            "The published Tianchong answer commits the exact 26-HP invoice and real Fengshang dying-entry window beneath the original damage candidate.");
        Play(game);
        Need(!game.ResolutionStack.Any(frame => frame.Id == root.Id) &&
            Events<DyingResolvedEvent>(game).Count(e => e.ResolutionId == paid.DyingId && e.VictimSeat == 2) == 1 &&
            Events<AnniversaryHzBodyIssuedEvent>(game).Count(e => e.Body == AnniversaryHzBody.Tianchong) == 1 &&
            Events<ProgramSkillHpLostEvent>(game).Count(loss => loss.FrameId == root.Id && loss.Amount == 26) == 1 &&
            Events<ShanheSourceMouAllocationEvent>(game).Count() == 1 && !Events<ShanheSourceMouWineIssuedEvent>(game).Any() &&
            !game.CreateSnapshot(0).Players[2].IsAlive && game.CreateSnapshot(0).Players[0].MaxHp == 5 &&
            Events<CardUseFinishedEvent>(game).Count(e => e.CardKind == CardKind.Slash) == 2,
            "Normal AI completes Fengshang's physical allocation; the paid axe still forbids its rescue Alcohol, then Tianchong grants maximum HP once and the original native Slash returns to Play without another cost or stale choice.");
    }

    private static bool IsBody(GameEngine game) => P(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Skill } p &&
        p.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "anniversary-huang-zhong");
    private static void Body(GameEngine game, string body) { Reach(game, () => IsBody(game)); Answer(game, c => c.Parameters.GetValueOrDefault("option") == body); }
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static IEnumerable<T> Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>();
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void Accept(GameEngine game, GameCommand command) { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected Tianchong command."); }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> select)
    { var prompt = P(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(select).Id, game.Revision)); }
    private static void Slash(GameEngine game)
    {
        var actions = game.GetHumanLegalActions();
        var action = actions.FirstOrDefault(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([2])) ??
            throw new InvalidOperationException("Missing Tianchong fixture native Slash: " + JsonSerializer.Serialize(new
            {
                game.Revision, Prompt = P(game),
                Hand = game.CreateSnapshot(0).Players[0].Hand, Equipment = game.CreateSnapshot(0).Players[0].Equipment,
                Skills = game.CreateSnapshot(0).Players[0].Skills,
                Target = game.CreateSnapshot(0).Players[2], Actions = actions.Take(20),
                Debits = Events<CardUseDebitRecordedEvent>(game), Spent = Events<AnniversaryHzZhanjueSpentEvent>(game)
            }));
        Accept(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
    }
    private static void Play(GameEngine game) => Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0);
    private static void Reach(GameEngine game, Func<bool> stop)
    {
        for (var step = 0; step < 220; step++)
        {
            if (stop()) return;
            if (P(game) is { PlayerSeat: 0 } prompt)
            {
                if (prompt.Kind == DecisionKind.PlayCard) { Accept(game, new AdvanceOneStepCommand(game.Revision)); continue; }
                var axe = prompt.SkillPrompt?.SkillId == StandardShanheXingtianPackage.SkillId;
                var ownDodges = game.CreateSnapshot(0).Players[0].Hand.Where(c => c.Kind == CardKind.Dodge).Select(c => c.Id).ToHashSet();
                var selected = prompt.Choices.FirstOrDefault(choice => axe && choice.Parameters.GetValueOrDefault("program-action") == "activate") ??
                    prompt.Choices.FirstOrDefault(choice => axe && choice.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards") ??
                    prompt.Choices.FirstOrDefault(choice => axe && choice.Cards is [var id] && ownDodges.Contains(id) && choice.Parameters.GetValueOrDefault("owned-selection-role") != "unpick") ??
                    prompt.Choices.FirstOrDefault(choice => axe && choice.Targets.SequenceEqual([2])) ??
                    prompt.Choices.FirstOrDefault(choice => prompt.SkillPrompt?.SkillId == Skill && choice.Parameters.GetValueOrDefault("program-action") == "activate") ??
                    prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") ??
                    prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") is "let-die" or "pass") ??
                    throw new InvalidOperationException("Unexpected human Tianchong boundary: " + JsonSerializer.Serialize(prompt));
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, selected.Id, game.Revision));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Tianchong did not reach its native boundary: " + JsonSerializer.Serialize(new
        { Prompt = P(game), Frames = game.ResolutionStack.Select(frame => frame.GetType().Name) }));
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-huang-zhong-tianchong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "modifiers":[{"id":"two-slashes","query":"slashLimit","operation":"add","value":1,"priority":0},
                    {"id":"reach-population-distance-boss","query":"outgoingDistance","operation":"add","value":-4,"priority":0}],
                  "damageModifiers":[{"id":"four-point-native-slash","cardKinds":["slash"],"amount":3,"condition":"always","sourceScope":"ownerUsed"}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "两段原生四伤", description = "护甲后的真实裂穹选择" } } }));
            builder.AddSkill(new(Driver, "两段原生四伤", "护甲后的真实裂穹选择") { Program = rules.Programs[Driver] });
            builder.AddGeneral(new(Peer, "保留战局敌人", "", null, "wei", 4));
            builder.AddDeck(new(Deck, "无随机起手的实体库存", 0, 0, [new("standard:dodge", 60)]));
        }
    }
}
