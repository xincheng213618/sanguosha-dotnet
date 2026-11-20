using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlTengFangLanLuochongChecks
{
    private const string TengFangLan = "ol:teng-fang-lan", Luochong = "ol:luochong";
    private const string Hurt = "fixture:luochong-dying-damage", Victim = "fixture:luochong-victim";
    private const string Mode = "identity:classic-ol-luochong-reservation", Deck = "fixture:ol-luochong-reservation-deck";
    private static readonly string[] Generals = [TengFangLan, Victim, "fixture:luochong-recipient", "fixture:luochong-peer"];

    internal static void LuochongReservesItsPaidOptionAndTargetDuringNestedDyingCold()
    {
        var (game, content) = Start();
        var ownerHp = game.CreateSnapshot(0).Players[0].Hp;
        var recipientHand = game.CreateSnapshot(2).Players[2].HandCount;
        Require(game.CreateSnapshot(1).Players[1].Hp == 1, "The first Luochong may lose the chosen participant's last HP.");

        ActivateLuochong(game);
        var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Luochong);
        Answer(game, choice => IsPair(choice, "loseHp", 1));
        Reach(game, IsLuochongOffer);
        var dying = game.ResolutionStack.OfType<DyingFrame>().Single();
        var paused = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == original.Id);
        Require(dying is { VictimSeat: 1 } && dying.ParentFrameId == original.Id &&
            paused.LuochongPending is { OptionId: "loseHp", TargetSeat: 1 } && game.CreateSnapshot(1).Players[1].Hp == 0 &&
            Facts<ProgramSkillHpLostEvent>(game).Single(fact => fact.FrameId == original.Id) is { TargetSeat: 1, Amount: 1, RemainingHp: 0 } &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 0, Amount: 1 } && game.CreateSnapshot(0).Players[0].Hp == ownerHp - 1,
            "The original accepted loss has put its participant into native Dying and the real first damage inside it has offered Luochong again.");

        ActivateLuochong(game);
        var second = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Luochong && frame.Id != original.Id);
        var restored = Cold(game, content);
        var pairs = P(restored)!.Choices.Where(choice => choice.Parameters.GetValueOrDefault("program-action") == "luochong-pick").ToArray();
        Require(pairs.Any(choice => IsPair(choice, "drawTwo", 2)), "The nested Luochong can spend another option on a different participant.");
        Answer(restored, choice => IsPair(choice, "drawTwo", 2));
        Reach(restored, prompt => prompt.Kind == DecisionKind.RescueDying);
        var rescueSeat = P(restored)!.PlayerSeat;
        var peach = P(restored)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("response") == "peach");
        Answer(restored, choice => choice.Id == peach.Id);
        Settle(restored);

        var resolved = Facts<ProgramLuochongResolvedEvent>(restored).ToArray();
        Require(resolved.Length == 2 && resolved[0] is { OptionId: "drawTwo", TargetSeat: 2, DrawCount: 2 } && resolved[0].FrameId == second.Id &&
            resolved[1] is { OptionId: "loseHp", TargetSeat: 1, LostHp: 1 } && resolved[1].FrameId == original.Id && resolved[0].RoundNumber == resolved[1].RoundNumber &&
            Facts<ProgramSkillHpLostEvent>(restored).Count(fact => fact.FrameId == original.Id) == 1 &&
            Facts<DyingResponseEvent>(restored).Single(fact => fact.UsedPeach) is { } response && response.ResponderSeat == rescueSeat &&
            response.ResolutionId == dying.Id && response.PeachCardId == peach.Cards.Single() &&
            Facts<DyingResolvedEvent>(restored).Single(fact => fact.ResolutionId == dying.Id).Survived && restored.CreateSnapshot(1).Players[1].Hp == 1 &&
            restored.CreateSnapshot(2).Players[2].HandCount == recipientHand + 2 - (rescueSeat == 2 ? 1 : 0),
            "The nested draw and native Peach return through their own frames and the original HP loss is paid once.");
        Require(pairs.All(choice => choice.Parameters.GetValueOrDefault("option") != "loseHp" && !choice.Targets.Contains(1)),
            $"The in-flight original option and participant must both be reserved. Offered pairs: {string.Join(", ", pairs.Select(choice => $"{choice.Parameters["option"]}:{choice.Targets.Single()}"))}.");
    }

    private static bool IsLuochongOffer(PendingDecision prompt) => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Luochong } &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    private static bool IsPair(PromptChoice choice, string option, int seat) =>
        choice.Parameters.GetValueOrDefault("option") == option && choice.Targets.SequenceEqual([seat]);
    private static void ActivateLuochong(GameEngine game)
    {
        Reach(game, IsLuochongOffer);
        Activate(game);
        Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "luochong-pick"));
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the exact original paid option, native Dying, and nested Luochong menu.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral || IsLuochongOffer(prompt));
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral || IsLuochongOffer(prompt));
        }
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-luochong-reservation", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Hurt}}","revision":1,"triggers":[
                {"id":"first-dying-damage","window":"dyingEntering","subject":"any","optional":false,"usageScope":"game","usageLimit":1,
                 "effects":[{"op":"damage","target":"owner","amount":1}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Hurt] = new { name = "濒死子伤", description = "公开测试濒死中的原生伤害。" } } });
            var program = SkillProgramCatalog.Load(rules, presentation).Programs[Hurt];
            builder.AddSkill(new(Hurt, "濒死子伤", "公开测试濒死中的原生伤害。") { Program = program });
            CharacterFixtureContent.Register(new DyingDamageBuilder(builder), TengFangLan);
            builder.AddGeneral(new(Victim, "失血目标", "supporter", "standard:none", "wei", 4) { InitialHp = 1 });
            foreach (var general in Generals.Skip(2))
                builder.AddGeneral(new(general, "同桌", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定救援实体桃", 2, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3), 24).ToArray()
            });
            builder.AddMode(new(Mode, "落宠本轮原选择", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class DyingDamageBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => inner.AddGeneral(general.Id == TengFangLan ?
            general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Hurt] } : general);
        public void AddSkill(ContentSkillDefinition skill) => inner.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => inner.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => inner.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => inner.AddMode(mode);
    }
}
