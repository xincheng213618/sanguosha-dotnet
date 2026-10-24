using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryFaZhengMobileChecks
{
    private const string Hero = "boundary:fa-zheng-mobile", MobileXuan = "boundary:xuanhuo-mobile", MobileEnyuan = "boundary:enyuan-mobile",
        Owner = "fixture:boundary-fz-mobile-owner", Quiet = "fixture:boundary-fz-mobile-quiet", Rank = "fixture:boundary-fz-mobile-rank", Watch = "fixture:boundary-fz-mobile-watch",
        Mode = "identity:classic-boundary-fz-mobile", Deck = "fixture:boundary-fz-mobile-deck";

    public static void ActualAttackRangeNativeSlashOrAtomicTakeEnyuanCold()
    {
        // The same real high-rank Slash privilege used to enlarge the common
        // helper; preserve that behavior for unrelated callers while narrowing
        // this edition's explicit attack-range instruction.
        var legacy = Scene.Start(false); legacy.CommandActor(); legacy.Reach(() => legacy.TargetMenu);
        Need(legacy.G.GetCombatDistance(1, 3) == 2 && legacy.G.GetAttackRange(1) == 1 && legacy.P!.Choices.Any(choice => choice.Targets.SequenceEqual([3])),
            "The actual rank-range policy still gives the legacy request its prospective distant target; the shared helper's old permissions remain intact.");
        foreach (var useSlash in new[] { true, false })
        {
            var scene = Scene.Start(true); var originalHand = scene.V(0).HandCount; var sourceHand = scene.V(1).HandCount;
            scene.CommandActor(); scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Watch);
            var parent = scene.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == MobileXuan);
            Need(scene.V(1).HandCount == sourceHand + 2 && scene.V(0).HandCount == originalHand && parent.SelectedTargetSeats.SequenceEqual([1]),
                "The replacement gives the other role two real native cards before targeting, and never pays the owner's normal draw in parallel.");
            scene.Cold(); scene.Continue(); scene.Reach(() => scene.TargetMenu);
            Need(scene.G.GetCombatDistance(1, 3) == 2 && scene.G.GetAttackRange(1) == 1 && !scene.P!.Choices.Any(choice => choice.Targets.SequenceEqual([3])) &&
                scene.P.Choices.Any(choice => choice.Targets.SequenceEqual([0])) && scene.P.Choices.Any(choice => choice.Targets.SequenceEqual([2])),
                "Mobile Xuanhuo offers actual attack-range roles only; rank-based Slash distance cannot add a remote role, and Fa Zheng remains a legal other person relative to the commanded actor.");
            scene.Cold(); scene.Answer(choice => choice.Targets.SequenceEqual([useSlash ? 2 : 0])); scene.Reach(() => scene.SlashPayment);
            if (useSlash)
            {
                var id = scene.P!.Choices.First(choice => choice.Parameters.GetValueOrDefault("request-option") == "use").Cards.Single();
                scene.Cold(); scene.Answer(choice => choice.Parameters.GetValueOrDefault("request-option") == "use" && choice.Cards.SequenceEqual([id])); scene.Reach(() => scene.Playing);
                Need(scene.G.Events.Select(entry => entry.Payload).OfType<DamageAppliedEvent>().Count(entry => entry.SourceSeat == 1 && entry.TargetSeat == 2 && entry.Amount == 1) == 1 &&
                    scene.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) == 1 &&
                    scene.V(0).HandCount == originalHand && !scene.G.CardMovements.Any(move => move.Reason.Value == "skill-program.boundary:xuanhuo-mobile.take-selected-target-cards"),
                    "The actor's original physical Slash really resolves once after cold payment recovery and bypasses the refusal take branch.");
            }
            else
            {
                scene.Answer(choice => choice.Parameters.GetValueOrDefault("request-option") == "decline"); scene.Reach(() => scene.Taking);
                scene.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "take-other-card"); scene.Reach(() => scene.Taking);
                scene.Cold(); scene.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "take-other-card"); scene.Reach(() => scene.Optional(MobileEnyuan));
                var taken = scene.G.CardMovements.Where(move => move.Reason.Value == "skill-program.boundary:xuanhuo-mobile.take-selected-target-cards" && move.To == CardLocation.Hand(0)).ToArray();
                Need(taken.Length == 2 && taken.All(move => move.From == CardLocation.Hand(1)) && scene.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(window => taken.All(move => window.Batch.Movements.Contains(move))) &&
                    scene.V(0).HandCount == originalHand + 2 && scene.V(1).HandCount == sourceHand,
                    "Refusal obtains two original HE entities in one real source-owner batch, enabling Enyuan only after the complete actual gain.");
                scene.Cold(); scene.Activate(); scene.Reach(() => scene.Playing);
                Need(scene.V(0).HandCount == originalHand + 2 && scene.V(1).HandCount == sourceHand + 1 &&
                    scene.G.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) && move.Reason.Value == "skill-program.boundary:enyuan-mobile.Draw") == 1 &&
                    taken.All(move => scene.G.CardMovements.Count(current => current.CardId == move.CardId && current.Reason == move.Reason) == 1),
                    "The actual atomic two-card gain issues one Enyuan native reward, while cold returns never repeat Xuanhuo's original Draw2 or obtained entities.");
            }
        }
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry, bool mobile)
    {
        internal GameEngine G = game;
        private string Xuan => mobile ? MobileXuan : "classic:xuanhuo";
        internal PendingDecision? P => G.PendingDecision ?? Enumerable.Range(0, 4).Select(viewer => G.CreateSnapshot(viewer).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        internal PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        internal bool TargetMenu => P?.SkillPrompt?.SkillId == Xuan && P.PlayerSeat == 0 && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("request-option") == "target");
        internal bool SlashPayment => P?.SkillPrompt?.SkillId == Xuan && P.PlayerSeat == 1 && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("request-option") == "decline");
        internal bool Taking => P?.SkillPrompt?.SkillId == Xuan && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "take-other-card");
        internal bool Playing => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
        internal bool Optional(string skill) => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        internal void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected Fa Zheng native command."); }
        internal void Answer(Func<PromptChoice, bool> predicate) { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        internal void Activate() => Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        internal void Continue() => Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        internal void CommandActor()
        {
            Activate(); Reach(() => P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
            Answer(choice => choice.Targets.SequenceEqual([1]));
        }
        internal void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.RespondDodge }) Answer(choice => choice.Parameters.GetValueOrDefault("response") == "take-damage");
                else if (!mobile && P?.SkillPrompt?.SkillId == Watch) Continue();
                else if (P is not null) throw new InvalidOperationException("Unexpected Fa Zheng scene decision " + P.Kind + " seat " + P.PlayerSeat + " skill " + P.SkillPrompt?.SkillId);
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded Fa Zheng scene did not reach its native boundary.");
        }
        internal void Cold()
        {
            var before = Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(G.CreateSnapshot(viewer))).ToArray();
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before.SequenceEqual(Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(restored.CreateSnapshot(viewer)))), "The original draw, exact attack-range candidates, native physical material and refusal slot cursor survive JSON cold restore."); G = restored;
        }
        internal static Scene Start(bool mobile)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, registry);
            var scene = new Scene(game, registry, mobile); scene.Accept(new StartGameCommand());
            for (var step = 0; step < 100; step++)
            {
                if (scene.Optional(scene.Xuan)) return scene;
                if (scene.P is { Kind: DecisionKind.SelectGeneral } prompt) scene.Accept(new SelectGeneralCommand(prompt.PlayerSeat, prompt.PlayerSeat == 0 ? Owner : prompt.ValidContentIds.First(id => id != Owner), game.Revision, prompt.PromptId));
                else if (scene.P is not null) throw new InvalidOperationException("Unexpected Fa Zheng setup prompt " + scene.P.Kind);
                else scene.Accept(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The exact public setup did not reach Xuanhuo's draw replacement.");
        }
    }

    private sealed class Fixture(bool mobile) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-fz-mobile", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "classic:fa-zheng");
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] {
                new { id = Quiet, revision = 1, modifiers = new[] { new { id = "keep-original-hand", query = "handLimit", operation = "add", value = 20, priority = 0 } } },
                new { id = Rank, revision = 1, cardPolicies = new[] { new { id = "real-rank-distance", kind = "slashRangeFromEffectiveRank", cardKinds = new[] { "slash", "fireSlash", "thunderSlash" } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "original-commanded-draw", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                    movementReasons = new[] { "skill-program.boundary:xuanhuo-mobile.Draw", "skill-program.classic:xuanhuo.Draw" }, usageScope = "game", usageLimit = 1, optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "real-draw", options = new[] { new { id = "continue" } } } } } } }
            } });
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "保留原牌", description = "真实身份手牌" }, [Rank] = new { name = "真实点数杀距离", description = "点数决定特定杀距离，仍保留实际攻击范围一" },
                [Watch] = new { name = "原摸牌子窗", description = "眩惑真实Draw2后暂停一次", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs) builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description) { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "完整移动版法正来源", "supporter", mobile ? MobileEnyuan : "classic:enyuan", "shu", 3, [mobile ? MobileXuan : "classic:xuanhuo", Quiet]));
            var pool = new List<string> { Owner };
            for (var index = 1; index < 4; index++) { var id = "fixture:boundary-fz-mobile-peer-" + index; pool.Add(id); builder.AddGeneral(new(id, "真实受令角色", "supporter", Quiet, "wei", 4, [Rank, Watch])); }
            builder.AddDeck(new(Deck, "固定真实十三点杀", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 128).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 13)).ToArray() });
            builder.AddMode(new(Mode, "四人真实身份输入", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: pool));
        }
    }
}
