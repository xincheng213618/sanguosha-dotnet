using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceArmoryCommanderChecks
{
    private const string Jueyan = StandardShanheSourceArmoryCommanderPackage.JueyanSkillId;
    private const string Jizhi = StandardShanheSourceArmoryCommanderPackage.JizhiSkillId;
    private const string Tuxing = StandardShanheSourceArmoryCommanderPackage.TuxingSkillId;
    private const string Driver = "fixture:source-armory-slots", Watch = "fixture:source-armory-health-child", Deck = "fixture:source-armory-deck";

    public static void ActualSlotPaymentsKeepTuxingRecipientAndGrantExactTreasureJizhi()
    {
        var treasure = Scene.Start(yujue: true);
        treasure.Skill(Jueyan, "treasure"); treasure.Play();
        Need(treasure.V is { Hp: 4, MaxHp: 5 } && treasure.HealthChildren == 1 &&
            treasure.Maxima.Select(fact => fact.Delta).SequenceEqual([1]) &&
            treasure.Events<ProgramTurnSkillsGrantedEvent>().Any(fact => fact.OwnerSeat == 0 && fact.SkillId == Jueyan &&
                fact.BindingId == "treasure" && fact.GrantedSkillIds.SequenceEqual([Jizhi])) &&
            treasure.Events<ProgramTurnSkillsGrantedEvent>().All(fact => !fact.GrantedSkillIds.Contains("classic:jizhi")),
            "The actual treasure payment increases the source owner's maximum and HP once, returns its original health child, and grants exact source870 instead of the weaker classic Jizhi. " + treasure.Diagnostic());
        var trick = treasure.Use(CardKind.DrawTwo);
        treasure.Until(() => treasure.P?.SkillPrompt?.SkillId == Jizhi);
        var original = treasure.Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == trick);
        treasure.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        treasure.Until(() => treasure.P?.SkillPrompt?.SkillId == Jizhi &&
            treasure.P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "discard"));
        var drawn = treasure.G.CardMovements.Single(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
            move.Reason.Value == "skill-program." + Jizhi + ".Draw").CardId;
        Need(treasure.V.Hand.Single(card => card.Id == drawn).Kind == CardKind.Dodge &&
            treasure.Events<CardUseFinishedEvent>().All(fact => fact.ResolutionId != original.ResolutionId),
            "The precise temporary Jizhi draws a real Basic entity before the original unconverted trick finishes.");
        treasure.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "discard"); treasure.Play();
        Need(treasure.G.CardMovements.Count(move => move.CardId == drawn && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.DiscardPile) == 1 && treasure.V.HandLimit == treasure.V.Hp + 1 &&
            treasure.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == original.ResolutionId) == 1,
            "Discarding that exact Jizhi Basic grants the current-turn hand-limit point and returns to complete the same paid trick once.");

        treasure.Skill("ol:yujue", "yujue-launch");
        treasure.Until(() => treasure.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "yujue-slot") == true);
        treasure.Answer(choice => choice.Parameters.GetValueOrDefault("slot") == nameof(EquipmentSlot.Armor));
        treasure.Until(() => treasure.P?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "yujue-target") == true);
        treasure.Answer(choice => choice.Targets.SequenceEqual([1])); treasure.Play();
        Need(treasure.V is { Hp: 5, MaxHp: 6 } && treasure.Maxima.Select(fact => fact.Delta).SequenceEqual([1, 1]) &&
            treasure.Events<ProgramYujueSlotPaidEvent>().Length == 1 && treasure.Events<ProgramYujueInvokedEvent>().Length == 1,
            "The old genuine Yujue route still pays and receives one card, with exactly one Tuxing growth after its former private duplicate call is removed.");
        var ownMaximum = treasure.V.MaxHp;
        treasure.Skill(Driver, "seal-other", [1]); treasure.Play();
        Need(treasure.V.MaxHp == ownMaximum && treasure.Maxima.Length == 2 &&
            treasure.G.CreateSnapshot(0).Players[1] is { Hp: 8, MaxHp: 8 },
            "A Tuxing actor abolishing another recipient's slot cannot apply its own growth to either participant.");
        treasure.Accept(new EndPlayPhaseCommand(0, treasure.G.Revision, treasure.P!.PromptId));
        treasure.Until(() => treasure.G.State.TurnNumber == 3 && treasure.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            treasure.G.ResolutionStack.Count == 0);
        Need(treasure.Events<ProgramTurnSkillGrantExpiredEvent>().Count(fact => fact.RecipientSeat == 0 && fact.SkillId == Jizhi &&
                fact.TurnExpiry == new SkillGrantTurnExpiry(1, 0)) == 1,
            "The actual turn end expires the precise temporary source870 instance once. " + treasure.Diagnostic());
        var laterTrick = treasure.Use(CardKind.DrawTwo); treasure.Play();
        Need(treasure.Events<ProgramBindingStartedEvent>().Count(fact => fact.SkillId == Jizhi) == 1 &&
            treasure.Events<CardUseFinishedEvent>().Count(fact => fact.CardId == laterTrick) == 1,
            "The next actual turn's ordinary trick finishes without retaining the expired treasure Jizhi offer.");

        var mounts = Scene.Start();
        mounts.Skill(Jueyan, "mounts"); mounts.Play();
        Need(mounts.V is { Hp: 5, MaxHp: 6 } && mounts.HealthChildren == 1 &&
            mounts.Maxima.Select(fact => fact.Delta).SequenceEqual([1, 1]) &&
            mounts.Events<RecoveryAppliedEvent>().Where(fact => fact.TargetSeat == 0).Sum(fact => fact.Amount) == 2 &&
            mounts.Events<ProgramGameDamageBonusArmedEvent>().All(fact => fact.SkillId != Tuxing),
            "The real two-mount source823 payment counts both abolished slots, with two native growth/recovery payments and no premature all-slot bonus.");
        mounts.Skill(Driver, "seal-area"); mounts.Play();
        Need(mounts.V is { Hp: 5, MaxHp: 5 } && mounts.Maxima.Select(fact => fact.Delta).SequenceEqual([1, 1, 1, 1, 1, -4]) &&
            mounts.Events<RecoveryAppliedEvent>().Where(fact => fact.TargetSeat == 0).Sum(fact => fact.Amount) == 5 &&
            mounts.Events<ProgramGameDamageBonusArmedEvent>().Count(fact => fact.OwnerSeat == 0 && fact.SkillId == Tuxing && fact.Amount == 1) == 1,
            "A real whole-area abolition counts only the three remaining slots, then pays the one four-maximum reduction and arms one permanent Tuxing bonus.");
        mounts.Skill(Driver, "seal-area"); mounts.Play();
        mounts.Skill(Driver, "seal-zero"); mounts.Play();
        Need(mounts.V is { Hp: 5, MaxHp: 5 } && mounts.Maxima.Length == 6 &&
            mounts.Events<ProgramGameDamageBonusArmedEvent>().Count(fact => fact.SkillId == Tuxing) == 1,
            "Repeating an already abolished area or zero-capacity slot does not invent another growth payment or all-slot reward.");
        var slash = mounts.Use(CardKind.Slash); mounts.Play();
        Need(mounts.Events<DamageAppliedEvent>().First(fact => fact.SourceSeat == 0 && fact.TargetSeat == 1).Amount == 2 &&
            mounts.Events<CardUseFinishedEvent>().Count(fact => fact.CardId == slash) == 1 && mounts.G.State.Winner == Winner.None,
            "The newly armed bonus affects the next actual native Slash and its original card finishes without completing the battle.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public int HealthChildren { get; private set; }
        public MaximumHpChangedEvent[] Maxima => Events<MaximumHpChangedEvent>().Where(fact => fact.PlayerSeat == 0 && fact.SkillId == Tuxing).ToArray();
        public T[] Events<T>() => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native source2486 command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, G.Revision)); }
        public void Skill(string skill, string activation, int[]? targets = null) =>
            Accept(new UseProgramSkillCommand(0, skill, activation, [], targets ?? [], G.Revision, P!.PromptId));
        public int Use(CardKind kind)
        {
            var card = V.Hand.First(item => item.Kind == kind);
            var action = G.GetHumanLegalActions().First(item => item.CardId == card.Id && item.ConversionSource is null &&
                (kind != CardKind.Slash || item.TargetSeats.SequenceEqual([1])));
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            return card.Id;
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                if (P?.SkillPrompt?.SkillId == Watch)
                {
                    var child = G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
                    var root = G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Jueyan || frame.SkillId == Driver);
                    Need(root.OwnerSeat == 0 && Maxima.Length > 0 && Events<ProgramBindingResolvedEvent>().All(fact => fact.FrameId != root.Id),
                        "The real Tuxing maximum-HP observer is a child of the still-paid original abolition program.");
                    HealthChildren++; Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
                    Need(Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == child.Id && fact.Completed) <= 1,
                        "The exact health child never returns twice.");
                    continue;
                }
                Need(G.State.Winner == Winner.None && P is null, "Unexpected source2486 prompt: " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing native source2486 boundary: " + Diagnostic());
        }
        public string Diagnostic() => JsonSerializer.Serialize(new
        { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Pending = P, V.Hp, V.MaxHp, HealthChildren, Maxima,
            Slots = V.EquipmentSlotCapacities,
            Recoveries = Events<RecoveryAppliedEvent>(), TurnGrants = Events<ProgramTurnSkillsGrantedEvent>(),
            ExpiredGrants = Events<ProgramTurnSkillGrantExpiredEvent>(),
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, Type = frame.GetType().Name }) });
        public static Scene Start(bool yujue = false)
        {
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-armory", StandardShanheSourceArmoryCommanderPackage.NightmareId,
                3, 0, 0, 0, 0, yujue ? [Driver, Watch, "ol:yujue"] : [Driver, Watch],
                [new(CardKind.DrawTwo, Suit.Heart, 7), new(CardKind.DrawTwo, Suit.Diamond, 9), new(CardKind.Slash, Suit.Spade, 8)], [],
                [new("classic:guan-yu", MaxHpBonus: 4, Hp: 8) { StartingHand = yujue ? [new(CardKind.Dodge, Suit.Club, 3)] : [] }],
                [], new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-armory", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"seal-other","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"alterEquipmentSlots","target":"selectedTarget","amount":0,"equipmentSlots":["weapon"]}]},
                    {"id":"seal-area","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"abolishOwnerAreas","target":"owner","zones":["equipment"]},{"op":"draw","target":"owner","amount":1}]},
                    {"id":"seal-zero","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                     "effects":[{"op":"alterEquipmentSlots","target":"owner","amount":0,"equipmentSlots":["weapon"]}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"real-maximum-child","window":"afterHealthChanged","subject":"owner","optional":false,
                     "usageScope":"game","usageLimit":1,"hpChangeOccurrence":"perEvent","condition":{"kind":"healthChangeKindIs","hpChangeKind":"maximumHp"},
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Driver] = new { name = "实际废栏入口", description = "调用真实通用装备栏与装备区废除。" },
                        [Watch] = new { name = "实际图兴体力上限孩子", description = "完成后返回原废栏付款。",
                            optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                    } }));
            foreach (var id in catalog.Programs.Keys)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
            builder.AddDeck(new(Deck, "零随机起手和摸牌的真实闪牌堆", 0, 0, [new("standard:dodge", 32)]));
        }
    }
}
