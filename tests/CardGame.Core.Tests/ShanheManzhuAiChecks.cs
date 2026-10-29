using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheManzhuAiChecks
{
    private const string Player = "fixture:manzhu-player", Enemy = "fixture:manzhu-enemy", Other = "fixture:manzhu-other";
    private const string Twin = "fixture:manzhu-twin", Manzhu = StandardShanheCompanionPackage.ManzhuId;

    public static void EnemyDodgeAiPaysHpChainsZhaxiangAndKeepsFirstResponseCursor()
    {
        foreach (var twin in new[] { false, true })
        {
            var game = Start(twin: twin);
            Play(game);
            var initialHand = game.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).ToArray();
            var targets = twin ? new[] { 2, 3 } : [2];
            Use(game, game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual(targets)));
            Reach(game, () => P(game) is { PlayerSeat: 1 } p &&
                p.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "wushuang-zengou"));
            var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Last();
            var receipt = frame.WushuangZengou!;
            var attack = game.ResolutionStack.OfType<CardUseFrame>().Single(use => use.Id == receipt.AttackFrameId);
            Need(frame.SkillId == "ol:zengou" && receipt.DodgerSeat == 2 && receipt.DodgeCursor == 0 &&
                attack.CardAttack!.RequiredDodgeResponses == (twin ? 2 : 1) && attack.TargetSeats.Count == targets.Length &&
                P(game)!.Choices.Any(choice => choice.Cards.Count == 1) &&
                P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("cost") == "hp"),
                "Normal AI activates the actual OL skill at the first Dodge, including multi-target double-Dodge attacks, with both real cost alternatives available.");
            var dodge = receipt.Cards.Single();
            Play(game);
            var invoice = Events<WushuangZengouPaidEvent>(game).Single();
            var hand = game.CreateSnapshot(1).Players[1].Hand;
            Need(invoice.CostCardId is null && invoice.DodgerSeat == 2 && invoice.DodgeCursor == 0 &&
                game.CreateSnapshot(0).Players[1].Hp == 2 && initialHand.All(id => hand.Any(card => card.Id == id)) &&
                hand.Count == initialHand.Length + 4 && hand.Any(card => card.Id == dodge) &&
                Events<ProgramSkillHpLostEvent>(game).Count(loss => loss.SkillId == "ol:zengou" && loss.TargetSeat == 1 && loss.Amount == 1) == 1 &&
                game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
                    move.Reason.Value == "skill-program.boundary:zhaxiang.Draw") == 3 &&
                Events<WushuangZengouInvalidatedEvent>(game).Single().Payment == invoice &&
                Events<WushuangZengouGainedCardEvent>(game).Single().CardId == dodge &&
                game.CreateCardZoneDiagnostics().Single(card => card.CardId == dodge).Location == CardLocation.Hand(1) &&
                game.CreateSnapshot(0).Players[2].Hp == 7 &&
                !Events<RequiredResponseProgressEvent>(game).Any(progress => progress.ResponderSeat == 2 && progress.ResponseCount > 0),
                "AI prefers genuine HP payment with Zhaxiang: lose once, draw three, invalidate the exact first Dodge, obtain it once and allow the hit without consuming a successful response.");
        }
    }

    public static void AiPreservesLastHpRangeAndConvertedDuelBoundaries()
    {
        var low = Start(hp: 1);
        Play(low);
        var cost = low.CreateSnapshot(1).Players[1].Hand.Single().Id;
        Use(low, low.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([2])));
        Play(low);
        Need(Events<WushuangZengouPaidEvent>(low).Single().CostCardId == cost && low.CreateSnapshot(0).Players[1].Hp == 1 &&
            !Events<ProgramSkillHpLostEvent>(low).Any(loss => loss.SkillId == "ol:zengou") && low.CreateSnapshot(0).Players[2].Hp == 7,
            "At one HP the same AI pays an available non-basic card rather than assuming a rescue or missing the enemy Dodge.");

        var far = Start(far: true);
        Play(far);
        Use(far, far.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([3])));
        Play(far);
        Need(!Events<WushuangZengouPaidEvent>(far).Any() && far.CreateSnapshot(0).Players[3].Hp == 8 && far.CreateSnapshot(0).Players[1].Hp == 3,
            "An actual Dodge beyond Manzhu's own attack range remains effective and charges no cost.");

        var arrows = Start(arrows: true);
        Play(arrows);
        Use(arrows, arrows.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.ArrowBarrage));
        Reach(arrows, () => P(arrows) is { PlayerSeat: 1 } p &&
            p.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "wushuang-zengou"));
        var arrowReceipt = arrows.ResolutionStack.OfType<ProgramSkillFrame>().Last().WushuangZengou!;
        Need(arrowReceipt.DodgerSeat == 2 && arrowReceipt.DodgeCursor == 0 &&
            arrows.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == arrowReceipt.AttackFrameId).CardKind == CardKind.ArrowBarrage,
            "A real Arrow Barrage Dodge retains the exact native group-attack response parent.");
        Play(arrows);
        Need(Events<WushuangZengouPaidEvent>(arrows).Single().CostCardId is null &&
            Events<WushuangZengouInvalidatedEvent>(arrows).Single().Payment.DodgerSeat == 2 &&
            Events<WushuangZengouGainedCardEvent>(arrows).Single().CardId == arrowReceipt.Cards.Single() &&
            arrows.CreateSnapshot(0).Players[1].Hp == 2 && arrows.CreateSnapshot(0).Players[2].Hp == 7,
            "The same normal AI HP-to-Zhaxiang combo nullifies a non-Slash Dodge and returns to the original group attack.");

        var duel = Start(magic: true);
        Play(duel);
        Use(duel, duel.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([2])));
        Play(duel);
        Need(Events<NativeSlashTargetDuelStartedEvent>(duel).Single().TargetSeat == 2 &&
            !Events<WushuangZengouPaidEvent>(duel).Any() && !Events<CardUseDebitRefundedEvent>(duel).Any() &&
            !Events<CardActionAcceptedEvent>(duel).Any(e => e.Action.Type == CardActionType.Response && e.Action.EffectiveKind == CardKind.Dodge) &&
            duel.CreateSnapshot(0).Players[1].Hp == 3 && duel.CreateSnapshot(1).Players[1].HandCount == 1,
            "The real same-color black Slash uses its native Duel slice, not a Dodge window; current AI also skips the zero-net-value black Longyin offer without paying a card.");
    }

    private static GameEngine Start(int hp = 3, bool twin = false, bool far = false, bool magic = false, bool arrows = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
            new StandardShanheCompanionPackage(), new Fixture(twin));
        var setup = new ShanheBattleSetup(71, 1, "manzhu-ai", magic ? "ol:mo-zhang-fei" : Player, magic ? 5 : 8,
            0, 0, 0, 0, [], [new(arrows ? CardKind.ArrowBarrage : CardKind.Slash, Suit.Spade, 7)], [new(CardKind.QilinBow)],
            [new(Enemy) { StartingHand = far ? [] : [new(CardKind.Dodge), new(CardKind.Dodge)] },
                new(Other) { StartingHand = far ? [new(CardKind.Dodge)] : [] }],
            [new(Manzhu, Hp: hp) { StartingHand = arrows ? [new(CardKind.DrawTwo), new(CardKind.Dodge)] : [new(CardKind.DrawTwo)] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, AdvanceAfterHumanCommands = false,
            UseInteractiveDiscard = false, AiPolicyVersion = AiDecisionPolicy.CurrentVersion, MaxTurns = 3
        }, registry);
        Accept(game, new StartGameCommand());
        return game;
    }

    private static IEnumerable<T> Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 4)
        .Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected Manzhu AI command."); }
    private static void Use(GameEngine game, LegalAction action) => Accept(game, new PlayCardCommand(0, action.CardId!.Value,
        action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind)
        { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
    private static void Play(GameEngine game) => Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Reach(GameEngine game, Func<bool> stop)
    {
        for (var step = 0; step < 180; step++)
        {
            if (stop()) return;
            if (P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard })
                throw new InvalidOperationException("Manzhu child returned to Play before the expected boundary: " + JsonSerializer.Serialize(new
                { Frames = game.ResolutionStack.Select(frame => frame.GetType().Name), Events = game.Events.TakeLast(12)
                    .Select(e => new { Type = e.Payload.GetType().Name, Data = JsonSerializer.Serialize(e.Payload, e.Payload.GetType()) }) }));
            if (P(game) is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
            {
                var choice = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip") ??
                    prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") == "pass") ?? prompt.Choices[0];
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Missing Manzhu AI boundary: " + JsonSerializer.Serialize(new
        { PendingDecision = P(game), Events = game.Events.TakeLast(12).Select(e => e.Payload) }));
    }

    private sealed class Fixture(bool twin) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-manzhu-ai", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Twin}}","revision":1,
                  "modifiers":[{"id":"extra-target","query":"cardTargetCount","operation":"add","value":1,"priority":0,"cardKinds":["slash"]}],
                  "cardPolicies":[{"id":"two-dodges","kind":"minimumResponseCount","cardKinds":["slash"],"requiredCardKinds":["dodge"],"value":2}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Twin] = new { name = "双闪双目标", description = "真实响应游标夹具" } } }));
            builder.AddSkill(new(Twin, "双闪双目标", "真实响应游标夹具") { Program = rules.Programs[Twin] });
            builder.AddGeneral(new(Player, "固定主将", "", twin ? Twin : null, "wei", 8));
            builder.AddGeneral(new(Enemy, "固定敌人", "", null, "wei", 8));
            builder.AddGeneral(new(Other, "其他敌人", "", null, "wei", 8));
            builder.AddDeck(new("fixture:manzhu-deck", "固定实体闪", 0, 0, [new("standard:dodge", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "曼珠 AI 响应", 4, 4, new Dictionary<string, int>(), "fixture:manzhu-deck",
                GeneralPoolIds: [Player, Manzhu, Enemy, Other, "ol:mo-zhang-fei"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
