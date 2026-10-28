using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceLiuSandaoChecks
{
    private const string Huchen = StandardShanheSourceLiuSandaoPackage.HuchenSkillId;
    private const string Zhanshen = StandardShanheSourceLiuSandaoPackage.ZhanshenSkillId;
    private const string Driver = "fixture:source-liusandao-driver", Watch = "fixture:source-liusandao-weapon-child";
    private const string Quiet = "fixture:source-liusandao-quiet", Deck = "fixture:source-liusandao-deck";
    private const string Ally = "fixture:source-liusandao-ally", Victim = "fixture:source-liusandao-victim";
    private static readonly string[] Survivors = ["fixture:source-liusandao-peer-a", "fixture:source-liusandao-peer-b", "fixture:source-liusandao-peer-c"];

    public static void ActualEnemyKillsFeedNormalDrawAndDeadAllyAwakeningReturnsIceSlash()
    {
        var s = Scene.Start(awakening: true);
        var weapon = s.V.Equipment.Single(card => card.Kind == CardKind.QinggangSword).Id;
        Need(s.V is { Hp: 5, MaxHp: 5 } && s.Events<ProgramBindingStartedEvent>().All(fact => fact.SkillId != Zhanshen),
            "A healthy source2479 with a living teammate does not awaken at its first actual Preparation.");
        s.Skill("kill", [s.Seat(Victim)]); s.Play();
        s.Skill("kill", [s.Seat(Ally)]); s.Play();
        s.Skill("wound"); s.Play();
        Need(s.Events<PlayerDiedEvent>().Count(fact => fact.KillerSeat == 0) == 2 && s.V is { Hp: 4, MaxHp: 5 },
            "The real commands create one enemy kill, one friendly kill and a real wound while surviving enemies keep the battle ongoing.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var original = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Zhanshen);
        var child = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
        Need(original.InstructionIndex == 2 && s.V is { Hp: 4, MaxHp: 4 } &&
            s.Events<MaximumHpChangedEvent>().Count(fact => fact.PlayerSeat == 0 && fact.SkillId == Zhanshen && fact.Delta == -1) == 1 &&
            s.G.CardMovements.Count(move => move.CardId == weapon && move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile &&
                move.Reason.Value == "skill-program." + Zhanshen + ".awakening-weapon-discard") == 1 &&
            s.Events<SkillsAcquiredEvent>().All(fact => fact.SourceSkillId != Zhanshen) &&
            s.Events<ProgramBindingResolvedEvent>().All(fact => fact.FrameId != original.Id),
            "The actual later Preparation pays one maximum HP and discards the exact equipped weapon before the original awakening grants anything; its real movement child retains that paid frame.");
        s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        Need(s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == original.Id && fact.Completed) == 1 &&
            s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == child.Id && fact.Completed) == 1 &&
            s.Events<SkillsAcquiredEvent>().Any(fact => fact.SourceSkillId == Zhanshen && fact.PlayerSeat == 0 &&
                fact.SkillIds.Order().SequenceEqual(new[] { "classic:mashu", "hulao:shenji" }.Order())) &&
            s.Events<ProgramNormalDrawAdjustedEvent>().Count(fact => fact.SkillId == Huchen && fact.OwnerSeat == 0 && fact.Adjustment == 1) == 1 &&
            s.Events<NativeDrawInvocationRecordedEvent>().Any(fact => fact.OwnerSeat == 0 && fact.TurnNumber == s.G.State.TurnNumber &&
                fact.Reason == CardMoveReasons.Draw && fact.Requested == 3 && fact.ActualCount == 3),
            "The exact awakening child returns once, grants source70/353 permanently, and Huchen adds only the actual enemy kill to the genuine two-card normal draw.");
        var targets = Survivors.Select(s.Seat).Order().ToArray();
        var ice = s.V.Hand.Where(card => card.Kind == CardKind.ColdSlash).Select(card => card.Id).ToArray();
        Need(ice.Length == 2, "Both fixed ice Slashes survive the bounded fixture's real discard phase.");
        foreach (var id in ice)
        {
            s.Use(id, targets); s.Play();
            Need(s.Events<CardUseFinishedEvent>().Count(fact => fact.CardId == id) == 1,
                "The actual ice Slash uses the original entity and all three extra-target recipients finish on the same native card.");
        }
        var ordinary = s.V.Hand.Single(card => card.Kind == CardKind.Slash).Id;
        Need(!s.G.GetHumanLegalActions().Any(action => action.CardId == ordinary && action.ConversionSource is null) &&
            ice.All(id => s.Events<CardUseDeclaredEvent>().Count(fact => fact.CardId == id) == 1) && s.G.State.Winner == Winner.None,
            "With its weapon really gone, source353 still permits exactly two ordinary ice Slashes and two extra targets, without completing the battle or replaying a paid use.");
    }

    public static void NonWushengConversionsKeepNuzhanMaterialsAndNaturalSlashUntouched()
    {
        var s = Scene.Start(awakening: false);
        var target = s.Seat(Survivors[^1]);
        var slash = s.V.Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var naturalHp = s.G.CreateSnapshot(0).Players[target].Hp;
        s.Use(slash, [target]); s.Play();
        Need(s.G.CreateSnapshot(0).Players[target].Hp == naturalHp - 1 && s.Events<NuzhanAppliedEvent>().Length == 0,
            "A genuine unconverted Slash uses its ordinary damage and cannot activate Nuzhan merely because the actor owns it.");
        var equipment = s.V.Hand.Single(card => card.Kind == CardKind.QinggangSword).Id;
        s.Convert(equipment, "equipment-slash", target); s.Play();
        var issued = s.Events<CardUseDeclaredEvent>().Single(fact => fact.CardId == equipment);
        Need(s.G.CreateSnapshot(0).Players[target].Hp == naturalHp - 3 &&
            s.Events<NuzhanAppliedEvent>().Single() is { DamageBonus: 1, IgnoredSlashLimit: false } &&
            s.Events<CardActionAcceptedEvent>().Any(fact => fact.Action.Type == CardActionType.Use && fact.Action.EffectiveKind == CardKind.Slash &&
                fact.Action.ConversionChain.Any(source => source.SkillId == Driver && source.BindingId == "equipment-slash") &&
                fact.Action.PhysicalCards.Select(cost => cost.CardId).SequenceEqual([equipment])),
            "A real non-Wusheng one-equipment conversion preserves its exact paid material and receives one Nuzhan damage bonus.");
        var trick = s.V.Hand.Single(card => card.Kind == CardKind.Duel).Id;
        Need(s.G.GetHumanLegalActions().Any(action => action.CardId == trick && action.ConversionSource?.SkillId == Driver &&
                action.ConversionSource.BindingId == "trick-slash"),
            "After natural and equipment Slashes exhaust the actual two-use weapon quota, the non-Wusheng Trick conversion remains a real legal unlimited Slash.");
        s.Convert(trick, "trick-slash", target); s.Play();
        Need(s.Events<NuzhanAppliedEvent>().Count(fact => fact.IgnoredSlashLimit && fact.PhysicalCardId == trick && fact.DamageBonus == 0) == 1 &&
            s.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == issued.ResolutionId) == 1 &&
            new[] { equipment, trick }.All(id => s.G.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing) == 1 && s.Events<CardUseFinishedEvent>().Count(fact => fact.CardId == id) == 1) &&
            s.G.CreateSnapshot(0).Players[target].Hp == naturalHp - 4 && s.G.State.Winner == Winner.None,
            "The genuine converted Trick Slash stays unlimited, every actual material and original use finishes once, and its base damage is not mistaken for the Equipment bonus.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public int Seat(string general) => G.CreateSnapshot(0).Players.Single(player => player.GeneralId == general).Seat;
        public T[] Events<T>() => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, (result.Error?.Message ?? "Rejected actual source2479 command.") + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, G.Revision)); }
        public void Skill(string activation, int[]? targets = null) =>
            Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId));
        public void Use(int id, int[] targets)
        {
            var action = G.GetHumanLegalActions().First(item => item.CardId == id && item.ConversionSource is null && item.TargetSeats.Order().SequenceEqual(targets.Order()));
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void Convert(int id, string binding, int target)
        {
            var action = G.GetHumanLegalActions().First(item => item.CardId == id && item.ConversionSource is { SkillId: Driver } source &&
                source.BindingId == binding && item.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource });
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 320; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None && P is null, "Unexpected source2479 boundary: " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing original source2479 boundary: " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Pending = P, Players = G.CreateSnapshot(0).Players.Select(player =>
            new { player.Seat, player.GeneralId, player.Hp, player.MaxHp, player.HandCount }),
            Deaths = Events<PlayerDiedEvent>(), Draws = Events<ProgramNormalDrawAdjustedEvent>(),
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, Type = frame.GetType().Name }) });
        public static Scene Start(bool awakening)
        {
            var source = awakening ? StandardShanheSourceLiuSandaoPackage.NightmareId : StandardShanheSourceLiuSandaoPackage.MediumId;
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-liusandao", source, 5, 0, 0, 0, 0, [Driver, Watch],
                [new(CardKind.ColdSlash, Suit.Spade, 3), new(CardKind.ColdSlash, Suit.Club, 4), new(CardKind.Slash, Suit.Spade, 5),
                 new(CardKind.Duel, Suit.Diamond, 6), new(CardKind.QinggangSword, Suit.Club, 7)], [new(CardKind.QinggangSword, Suit.Spade, 6)],
                [new(Victim, Hp: 1) { StartingHand = [] }, .. Survivors.Select(id => new ShanheCombatantDefinition(id) { StartingHand = [] })],
                [new(Ally, Hp: 1) { StartingHand = [] }], new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [],
                EnemyRoster = Enumerable.Range(0, 4).Select(_ => new ShanheCombatantDefinition("classic:guan-yu")).ToArray(),
                AllyRoster = [new("classic:guan-yu")] }).WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat(new[] { Ally, Victim }.Concat(Survivors)).Distinct(StringComparer.Ordinal).ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            { Seed = setup.BattleSeed, PlayerCount = 6, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-liusandao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"retain-native-materials","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "viewAs":[
                    {"id":"trick-slash","inputKinds":["duel"],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false},
                    {"id":"equipment-slash","inputKinds":["qinggangSword"],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":false}],
                   "activations":[
                    {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"wound","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-awakening-discard-child","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],
                    "movementOccurrence":"perBatch","movementReasons":["skill-program.{{Zhanshen}}.awakening-weapon-discard"],"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"bounded-peer-turn","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Driver] = new { name = "原生命令与非武圣转换", description = "真实击杀、失去体力及单牌转换材料。" },
                        [Watch] = new { name = "战神实际武器弃置孩子", description = "返回原已付觉醒。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                        [Quiet] = new { name = "安静的真实对手回合", description = "保留正常摸牌，跳过出牌。" }
                    } }));
            foreach (var id in catalog.Programs.Keys)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
            foreach (var id in new[] { Ally, Victim }.Concat(Survivors)) builder.AddGeneral(new(id, id, "supporter", Quiet, "wei", 8));
            var ice = CardCatalog.Get(CardKind.ColdSlash);
            builder.AddCard(new("fixture:source-liusandao-cold-slash", ice.DisplayName, ice.CategoryName, ice.Description, LegacyKind: CardKind.ColdSlash));
            builder.AddDeck(new(Deck, "零随机初手、真实正常摸二的黑闪牌堆", 0, 2, [])
                { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
        }
    }
}
