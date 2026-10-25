using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhongYaoChecks
{
    private const string Driver = "fixture:mobile-zhong-yao-driver", Watch = "fixture:mobile-zhong-yao-watch";

    public static void RemadeHuomoOwnConversionQuotaAndPaidDodgeCold()
    {
        var (game, registry) = Create(false);
        var physical = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash &&
            action.ConversionSource is null && action.TargetSeat == 1);
        Play(game, physical, 1); Settle(game);
        var converted = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash &&
            action.ConversionSource?.SkillId == "mobile:huomo" && action.TargetSeat == 1);
        var cost = converted.CardId!.Value;
        Play(game, converted, 1);
        var restored = Restore(game, registry);
        Settle(game); Settle(restored); Equal(game, restored);
        Require(game.CardMovements.Count(move => move.CardId == cost && move.From == CardLocation.Hand(0) && move.To == CardLocation.DrawPile) == 1 &&
            game.CardMovements.All(move => move.CardId != cost || move.To != CardLocation.DiscardPile),
            "A physical Slash must leave the remade Huomo Slash quota unused; its later conversion pays one real top-deck cost.");
        Require(game.GetHumanLegalActions().All(action => action.ConversionSource?.SkillId != "mobile:huomo" ||
            action.PlayedCardKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash)),
            "One actual Huomo Slash consumes its shared ordinary/fire/thunder name quota.");

        Request(game);
        var dodge = Prompt(game)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("conversion-skill-id") == "mobile:huomo");
        var dodgeCost = dodge.Cards.Single();
        Answer(game, choice => choice.Id == dodge.Id);
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Watch);
        Require(game.CardMovements.Count(move => move.CardId == dodgeCost && move.From == CardLocation.Hand(0) && move.To == CardLocation.DrawPile) == 1,
            "The native Huomo Dodge must pay the real top card before opening its response observer.");
        var dodgePaymentSequence = game.CardMovements.Single(move => move.CardId == dodgeCost &&
            move.From == CardLocation.Hand(0) && move.To == CardLocation.DrawPile).Sequence;
        restored = Restore(game, registry);
        foreach (var branch in new[] { game, restored })
        {
            Answer(branch, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Settle(branch);
            Require(branch.CardMovements.Count(move => move.CardId == dodgeCost && move.From == CardLocation.Hand(0) && move.To == CardLocation.DrawPile) == 1 &&
                branch.CardMovements.Count(move => move.CardId == dodgeCost && move.Sequence > dodgePaymentSequence &&
                    move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason.Value == $"skill-program.{Watch}.Draw") == 1,
                $"Paid response recovery must preserve the original Slash frame and let its observer draw the top cost once. cost={dodgeCost}; payment={dodgePaymentSequence}; pending={Prompt(branch)?.Kind}/{Prompt(branch)?.SkillPrompt?.SkillId}; location={JsonSerializer.Serialize(branch.CreateCardZoneDiagnostics().Single(card => card.CardId == dodgeCost).Location)}; movements={JsonSerializer.Serialize(branch.CardMovements.Where(move => move.CardId == dodgeCost))}");
            Request(branch);
            Require(Prompt(branch)!.Choices.Any(choice => choice.Cards.Count == 1 && !choice.Parameters.ContainsKey("conversion-skill-id")),
                "The second real Slash must retain a native physical Dodge choice so its response window remains inspectable.");
            Require(Prompt(branch)!.Choices.All(choice => choice.Parameters.GetValueOrDefault("conversion-skill-id") != "mobile:huomo"),
                "The returned physical cost cannot restore an already paid Huomo Dodge quota in the same actual turn.");
            Answer(branch, choice => choice.Cards.Count == 0); Settle(branch);
            Require(branch.Events.Select(item => item.Payload).OfType<SkillUsageConsumedEvent>().Count(item =>
                item.SkillId == "mobile:huomo" && item.Scope == SkillUsageScope.Turn && item.UsageId == "view-as-actual-turn:huomo-dodge") == 1,
                "The resumed native Dodge consumes exactly one actual-turn allowance.");
        }
        Equal(game, restored);
    }

    public static void RemadeZuodingChecksOwnerOrOriginalTargetDamage()
    {
        var (game, registry) = Create(true);
        Use(game, "damage", 3); Settle(game);
        SlashForZuoding(game);
        Require(Prompt(game)?.PlayerSeat == 1 && Prompt(game)?.SkillPrompt?.SkillId == "mobile:zuoding",
            "Damage to an unrelated player must not disable the remade Zuoding owner.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Require(Prompt(game)!.Choices.Any(choice => choice.Targets.SequenceEqual(new[] { 2 })),
            "An undamaged owner may choose the actual designated Slash target even after unrelated damage.");
        var restored = Restore(game, registry);
        foreach (var branch in new[] { game, restored }) { Answer(branch, choice => choice.Targets.SequenceEqual(new[] { 2 })); Settle(branch); }
        Equal(game, restored);

        Use(game, "damage", 1); Settle(game);
        // Both owner 1 and original target 2 have now actually suffered damage.
        Require(game.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Any(item => item.TargetSeat == 1) &&
            game.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Any(item => item.TargetSeat == 2),
            "The rejected qualification must follow real damage to both participants.");
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Slash && item.TargetSeat == 2);
        Play(game, action, 2); Settle(game, forbiddenZuodingOwner: 1);

        var fresh = Create(true).Game;
        Use(fresh, "damage", 1); Settle(fresh);
        SlashForZuoding(fresh);
        Answer(fresh, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Require(Prompt(fresh)!.Choices.Any(choice => choice.Targets.SequenceEqual(new[] { 2 })),
            "A damaged Zuoding owner must still be allowed to help an undamaged original target.");
        Answer(fresh, choice => choice.Targets.SequenceEqual(new[] { 2 })); Settle(fresh);
    }

    private static void SlashForZuoding(GameEngine game)
    {
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Slash && item.TargetSeat == 2);
        Play(game, action, 2);
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == "mobile:zuoding" && prompt.PlayerSeat == 1);
    }
    private static void Request(GameEngine game)
    {
        Use(game, "request", 1);
        Reach(game, prompt => prompt.PlayerSeat == 1 && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "request-slash"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "request-slash" && choice.Cards.Count == 1);
        Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Kind == DecisionKind.RespondDodge);
    }
    private static void Use(GameEngine game, string activation, int target) => Accept(game,
        new UseProgramSkillCommand(0, Driver, activation, [], [target], game.Revision, Prompt(game)!.PromptId));
    private static void Play(GameEngine game, LegalAction action, int target) => Accept(game,
        new PlayCardCommand(0, action.CardId!.Value, [target], game.Revision, Prompt(game)!.PromptId, action.PlayedCardKind)
        { ConversionSource = action.ConversionSource });
    private static PendingDecision? Prompt(GameEngine game) => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat, true).PendingDecision).FirstOrDefault(prompt => prompt is not null);
    private static void Reach(GameEngine game, Func<PendingDecision, bool> predicate)
    {
        for (var step = 0; step < 160; step++)
        {
            if (Prompt(game) is { } prompt && predicate(prompt)) return;
            if (Prompt(game)?.SkillPrompt?.SkillId == "mobile:zuoding") Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException($"Mobile Zhong Yao fixture did not reach its native boundary: {Prompt(game)?.Kind}/{Prompt(game)?.SkillPrompt?.SkillId}.");
    }
    private static void Settle(GameEngine game, int? forbiddenZuodingOwner = null)
    {
        for (var step = 0; step < 180 && game.ResolutionStack.Count > 0; step++)
        {
            Require(Prompt(game) is not { SkillPrompt.SkillId: "mobile:zuoding" } current || current.PlayerSeat != forbiddenZuodingOwner,
                "A damaged Zuoding owner must receive no invocation when the sole original target was also damaged.");
            if (Prompt(game) is { } prompt && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"))
                Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
            else if (Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge }) Answer(game, choice => choice.Cards.Count == 0);
            else if (Prompt(game) is { Kind: DecisionKind.Nullification }) Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "pass");
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        Require(game.ResolutionStack.Count == 0, "The original card/program parent must finish without a paused child.");
        if (Prompt(game) is null) Reach(game, prompt => prompt.PlayerSeat == 0 && prompt.Kind == DecisionKind.PlayCard);
    }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = Prompt(game)!; Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Require(result.Accepted, result.Error?.Message ?? "Mobile Zhong Yao command rejected."); }
    private static GameEngine Restore(GameEngine game, ContentRegistry registry) => GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
    private static void Equal(GameEngine a, GameEngine b) => Require(SnapshotJson.Serialize(a.CreateSnapshot(0, true)) == SnapshotJson.Serialize(b.CreateSnapshot(0, true)) &&
        a.CardMovements.SequenceEqual(b.CardMovements) && a.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType())).SequenceEqual(b.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType()))),
        "Cold restoration must preserve the original pending choice, actual name quotas, events and physical movements.");
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private static (GameEngine Game, ContentRegistry Registry) Create(bool zuoding)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true), new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new Fixture(zuoding));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, HumanSeat = 0, HumanRole = Role.Lord, PlayerCount = 4,
            ModeId = "identity:classic-mobile-zhong-yao", UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 20 }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral && prompt.PlayerSeat == 0);
        Accept(game, new SelectGeneralCommand(0, "fixture:mobile-zhong-yao-owner", game.Revision, Prompt(game)!.PromptId));
        Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == 0);
        return (game, registry);
    }
    private sealed class Fixture(bool zuoding) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhong-yao", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[
                {"id":"fixture-range","query":"outgoingDistance","operation":"add","value":-10,"priority":0},
                {"id":"fixture-slash-limit","query":"slashLimit","operation":"add","value":20,"priority":0}],
               "activations":[
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"requested"}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-dodge","window":"cardResponseAccepted","ownerRelation":"actor","cardKinds":["dodge"],"optional":true,"effects":[{"op":"draw","target":"owner","amount":1}]}]}
            ]}
            """, JsonSerializer.Serialize(new
            {
                schemaVersion = 3,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "实际规则驱动", description = "实际伤害和原生请求" },
                    [Watch] = new { name = "付款观察", description = "真实闪支付后摸一张" }
                }
            }));
            foreach (var skill in new[] { Driver, Watch }) builder.AddSkill(new(skill, catalog.Presentations[skill].Name, catalog.Presentations[skill].Description)
            { Program = catalog.Programs[skill], ProgramPresentation = catalog.Presentations[skill] });
            builder.AddGeneral(new("fixture:mobile-zhong-yao-owner", "钟繇驱动", "supporter", zuoding ? "standard:none" : "mobile:huomo", "wei", BaseHp: 20,
                AdditionalSkillIds: zuoding ? [Driver] : [Driver, Watch]));
            foreach (var seat in Enumerable.Range(1, 3)) builder.AddGeneral(new($"fixture:mobile-zhong-yao-{seat}", "实际目标", "supporter", zuoding ? "mobile:zuoding" : "standard:none", "wei", BaseHp: 20));
            builder.AddDeck(new("fixture:mobile-zhong-yao-deck", "实体基本及黑色非基本牌", 20, 0, [])
            { PhysicalCards = Enumerable.Range(0, 240).Select(index => new ContentDeckPhysicalCard(zuoding ? "standard:slash" :
                index % 3 == 0 ? "standard:crossbow" : index % 3 == 1 ? "standard:dodge" : "standard:slash", Suit.Spade, index % 13 + 1)).ToArray() });
            builder.AddMode(new("identity:classic-mobile-zhong-yao", "移动钟繇检查", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, "fixture:mobile-zhong-yao-deck",
                GeneralCandidateCount: 4, GeneralPoolIds: ["fixture:mobile-zhong-yao-owner", "fixture:mobile-zhong-yao-1", "fixture:mobile-zhong-yao-2", "fixture:mobile-zhong-yao-3"]));
        }
    }
}
