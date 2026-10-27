using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheTerrainChecks
{
    private const string Driver = "fixture:shanhe-terrain-driver", General = "fixture:shanhe-terrain-owner";
    private const string Npc = "fixture:shanhe-terrain-other", Deck = "fixture:shanhe-terrain-deck";
    private const string Second = "fixture:shanhe-terrain-second";

    public static void ForestTenthUseKeepsResponseAndResetsForNextActualPlayPhaseCold()
    {
        var scene = new Scene(StandardShanheTerrainPackage.ForestId, extraPlay: true);
        scene.Play();
        var phase = scene.V(0).ActualPlayPhaseCardUseState!;
        var weapons = scene.V(0).Hand.Where(card => card.Kind == CardKind.QinggangSword).OrderBy(card => card.Id).ToArray();
        for (var index = 0; index < 9; index++) { scene.UseCard(weapons[index].Id); scene.Play(); }
        Need(scene.V(0).ActualPlayPhaseCardUseState is { UseCount: 9 }, "The inserted Play phase records nine actual equipment uses exactly once each.");
        var duel = scene.V(0).Hand.Single(card => card.Kind == CardKind.Duel);
        scene.UseCard(duel.Id, [2]);
        scene.Reach(() => scene.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash });
        Need(scene.V(0).ActualPlayPhaseCardUseState is { UseCount: 10 } &&
            scene.P!.Choices.Any(choice => choice.Cards.Count == 1),
            "The accepted tenth card keeps resolving; a Slash played in response to its Duel remains available at the cap.");
        scene.Cold();
        scene.Answer(choice => choice.Cards.Count == 1);
        scene.Play();
        Need(scene.V(0).ActualPlayPhaseCardUseState is { UseCount: 10 } &&
            scene.G.Events.Any(item => item.Payload is CardActionAcceptedEvent
                { Action: { ActorSeat: 0, Type: CardActionType.Response, EffectiveKind: CardKind.Slash } }) &&
            !scene.G.GetHumanLegalActions().Any(action => action.CardId == weapons[9].Id),
            "A merely played Slash does not consume another use, and the eleventh physical use is absent from legal actions.");
        var before = SnapshotJson.Serialize(scene.G.CreateSnapshot(0));
        Need(!scene.G.Submit(new PlayCardCommand(0, weapons[9].Id, [], scene.G.Revision, scene.P!.PromptId)).Accepted &&
            SnapshotJson.Serialize(scene.G.CreateSnapshot(0)) == before,
            "Submitting an eleventh use directly is rejected without changing the prepared public result.");
        scene.Cold();
        scene.Accept(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId));
        scene.Play();
        Need(scene.V(0).ActualPlayPhaseCardUseState is { UseCount: 0 } next &&
            next.TurnNumber == phase.TurnNumber && next.PhaseInstanceId != phase.PhaseInstanceId,
            "The later ordinary Play phase of the same actual turn has a distinct, empty use ledger.");
        scene.UseCard(weapons[9].Id); scene.Play();
        Need(scene.V(0).ActualPlayPhaseCardUseState is { UseCount: 1 } && scene.V(0).Equipment.Any(card => card.Id == weapons[9].Id),
            "The former eleventh card can be genuinely used once the next Play phase starts.");
        scene.Cold();
    }

    public static void TerrainNumericPoliciesRejectMalformedStaticShapes()
    {
        foreach (var kind in new[] { "minimumHandLimit", "maximumOwnPlayCardUses" })
        foreach (var suffix in new[] { "\"value\":0", "\"value\":1001", "\"value\":8,\"cardKinds\":[\"slash\"]",
                     "\"value\":8,\"condition\":{\"kind\":\"handCountAtLeast\",\"value\":1}" })
        {
            var rejected = false;
            try { SkillProgramCatalog.Load($$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:terrain-policy","revision":1,"cardPolicies":[{"id":"bounded","kind":"{{kind}}",{{suffix}}}]}]}""",
                $$"""{"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"fixture:terrain-policy":{"name":"地形策略","description":"静态形状"} } }"""); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "A numeric floor or use-cap must have a bounded unconditional number and no card filters.");
        }
    }

    private sealed class Scene
    {
        private readonly ContentRegistry _registry;
        public GameEngine G { get; private set; }
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public T[] Facts<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public Scene(string terrain, bool extraPlay = false, bool preservation = false)
        {
            _registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShanheTerrainPackage(), new Fixture());
            var opening = extraPlay
                ? Enumerable.Range(0, 10).Select(_ => new ShanheStartingCard(CardKind.QinggangSword, Suit.Spade, 6))
                    .Concat([new(CardKind.Duel, Suit.Heart, 1), new(CardKind.Slash, Suit.Spade, 7)]).ToArray()
                : Enumerable.Range(0, 3).Select(_ => new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7)).ToArray();
            var setup = new ShanheBattleSetup(71, 1, "terrain", General, 4, 0, 0, 0, 0,
                extraPlay ? ["boundary:dangxian"] : preservation ? [terrain] : [], opening, [],
                [new(Npc, Hp: extraPlay || preservation ? 1 : null) { StartingHand = extraPlay ? [new(CardKind.Slash, Suit.Club, 8)] : [] },
                 new(preservation ? "boundary:cai-wen-ji" : Npc) { StartingHand = [] }], [new(Npc) { StartingHand = [] }], new string('c', 64))
                { TerrainSkillId = terrain, BossTransition = preservation ? new(0, Second, 6, 6) : null };
            G = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 5, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, _registry);
            Accept(new StartGameCommand());
        }
        public void UseCard(int card, IReadOnlyList<int>? targets = null) => Accept(new PlayCardCommand(0, card, targets ?? [], G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = P!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Terrain native command rejected."); }
        public void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 160; step++)
            {
                if (condition()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard && G.ResolutionStack.OfType<CardUseFrame>().Any())
                    { Accept(new AdvanceOneStepCommand(G.Revision)); continue; }
                    if (prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "skip"))
                    { Answer(choice => choice.Parameters.GetValueOrDefault("option") == "skip"); continue; }
                    throw new InvalidOperationException("Unexpected terrain human prompt: " + JsonSerializer.Serialize(prompt));
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Terrain check did not reach its native boundary.");
        }
        public void Cold()
        {
            var before = SnapshotJson.Serialize(G.CreateSnapshot(0));
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), _registry);
            Need(before == SnapshotJson.Serialize(G.CreateSnapshot(0)), "Cold replay preserves the native terrain and phase quota boundary.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-terrain", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var program = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "modifiers":[{"id":"wide-limit","query":"handLimit","operation":"add","value":10,"priority":0}],
                  "activations":[{"id":"lower-limit","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                    "targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"grantTurnRuleModifier","target":"owner","ruleQuery":"handLimit","ruleOperation":"add","amount":-12}]},
                    {"id":"finish","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                     "targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}]}
                """, $$"""{"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Driver}}":{"name":"手牌上限驱动","description":"真实手牌上限修正"} } }""");
            builder.AddSkill(new(Driver, "手牌上限驱动", "真实手牌上限修正")
            { Program = program.Programs[Driver], ProgramPresentation = program.Presentations[Driver] });
            builder.AddGeneral(new(General, "地形玩家", "", Driver, "qun", 4));
            builder.AddGeneral(new(Npc, "地形其他角色", "", "standard:none", "qun", 4));
            builder.AddGeneral(new(Second, "地形第二形态", "", "standard:none", "qun", 6));
            builder.AddDeck(new(Deck, "固定闪", 4, 2, [new("standard:dodge", 80)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "地形实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [General, Npc, Second, "boundary:cai-wen-ji"], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
