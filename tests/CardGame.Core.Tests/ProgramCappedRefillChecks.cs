using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ProgramCappedRefillChecks
{
    private const string Skill = "fixture:capped-refill", Owner = "fixture:capped-refill-owner",
        Enemy = "fixture:capped-refill-enemy", Deck = "fixture:capped-refill-deck";
    private const string Reason = "skill-program.hand-count.draw";
    private const string CappedOwner = """{"op":"drawToHandCount","target":"owner","amount":5,"capAtTargetMaxHp":true}""";
    private const string FixedOwner = """{"op":"drawToHandCount","target":"owner","amount":5}""";
    private const string CappedTarget = """{"op":"drawToHandCount","target":"selectedTarget","amount":5,"capAtTargetMaxHp":true}""";
    private const string DynamicTarget = """{"op":"drawToHandCount","target":"selectedTarget","numberExpression":"livingPlayerCount"}""";

    public static void LoaderAndPublicAiHonorFixedCappedRefillGoals()
    {
        var capped = Load(CappedOwner);
        var plain = Load(FixedOwner);
        var target = Load(CappedTarget, selectedTarget: true);
        Need(Effect(capped).CapAtTargetMaxHp && !Effect(plain).CapAtTargetMaxHp &&
            capped.Programs[Skill].GameplayHash != plain.Programs[Skill].GameplayHash &&
            Effect(target) is { Target: SkillProgramEffectTarget.SelectedTarget, Amount: 5, CapAtTargetMaxHp: true },
            "The typed immutable effect retains the cap and existing content hashing distinguishes it from an uncapped fixed refill.");
        _ = Load(CappedOwner.Replace("\"amount\":5", "\"amount\":0", StringComparison.Ordinal));
        _ = Load(DynamicTarget.Replace("\"numberExpression\"", "\"capAtTargetMaxHp\":false,\"numberExpression\"", StringComparison.Ordinal), selectedTarget: true);
        foreach (var invalid in new[]
        {
            CappedOwner.Replace("true", "\"true\"", StringComparison.Ordinal),
            CappedOwner.Replace("\"amount\":5,", "", StringComparison.Ordinal),
            CappedOwner.Replace("\"amount\":5", "\"amount\":17", StringComparison.Ordinal),
            CappedOwner.Replace("\"owner\"", "\"selectedTargets\"", StringComparison.Ordinal),
            CappedOwner.Replace("\"amount\":5", "\"numberExpression\":\"livingPlayerCount\"", StringComparison.Ordinal),
            CappedOwner.Replace("drawToHandCount", "draw", StringComparison.Ordinal)
        }) Reject(invalid);

        var owner = new PlayerSkillContext(0, 3, 3, 1, TurnPhase.Play, IsOwnTurn: true);
        var recipient = new PlayerSkillContext(1, 4, 4, 1, TurnPhase.Play);
        Need(ProgramCompositionAi.Estimate([Effect(capped)], owner).Hint.OwnerDraw == 2 &&
            ProgramCompositionAi.Estimate([Effect(plain)], owner).Hint.OwnerDraw == 4 &&
            ProgramCompositionAi.Estimate([Effect(target)], owner, publicContext: new(2, SelectedTarget: recipient)).Hint.TargetDraw == 3 &&
            ProgramCompositionAi.Estimate([Effect(target)], owner, publicContext: new(2, SelectedTarget: recipient with { MaxHp = 8 })).Hint.TargetDraw == 4 &&
            ProgramCompositionAi.Estimate([Effect(capped)], owner with { HandCount = 6 }).Hint.OwnerDraw == 0 &&
            ProgramCompositionAi.Estimate([Effect(capped)], owner with { HpUndefined = true }).Hint.OwnerDraw == 0 &&
            ProgramCompositionAi.Estimate([Effect(target)], owner, publicContext: new(2, SelectedTarget: recipient with { HpUndefined = true })).Hint.TargetDraw == 0 &&
            ProgramCompositionAi.Estimate([Effect(Load(DynamicTarget, selectedTarget: true))], owner,
                publicContext: new(2, SelectedTarget: recipient, LivingPlayerCount: 2)).Hint.TargetDraw == 1,
            "Public AI estimates use the same min(fixed goal, recipient maximum HP), never price undefined HP, and retain ordinary fixed and living-player-count refill values.");
    }

    public static void NativeRefillCapsCurrentMaxHpAndPreservesExcessHands()
    {
        var s = Scene.Start();
        Need(s.View(0) is { MaxHp: 3, HandCount: 1 } && s.View(1) is { MaxHp: 4, HandCount: 1 },
            "The native scene starts with two different below-five public maximum HP values and real starting cards.");
        var beforeEvents = s.G.Events.Count;
        s.Use("capped-owner");
        var first = s.G.CardMovements.Where(move => move.Reason.Value == Reason).ToArray();
        Need(s.View(0).HandCount == 3 && first.Length == 2 &&
            first.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            s.G.Events.Skip(beforeEvents).Select(item => item.Payload).OfType<CardsDrawnEvent>().ToArray() is [{ PlayerSeat: 0, Count: 2 }],
            "A capped owner refill draws the exact two real entities in one native DrawN gain to MaxHp three, rather than drawing five and discarding.");
        s.Use("fixed-owner");
        Need(s.View(0).HandCount == 5 && s.G.CardMovements.Count(move => move.Reason.Value == Reason) == 4,
            "The existing uncapped fixed amount still reaches five even though maximum HP is only three.");
        var ownerCards = s.View(0).Hand.Select(card => card.Id).ToArray();
        var moves = s.G.CardMovements.Count;
        s.Use("capped-owner");
        Need(s.G.CardMovements.Count == moves && s.View(0).Hand.Select(card => card.Id).SequenceEqual(ownerCards),
            "A hand already above the capped goal is neither drawn into nor discarded from.");

        s.Use("dynamic-target", 1);
        Need(s.View(1).HandCount == 2 && s.G.CardMovements.Count(move => move.Reason.Value == Reason && move.To == CardLocation.Hand(1)) == 1,
            "The existing selected-target livingPlayerCount refill still draws to the two actual living players, independently from maximum HP.");
        s.Use("capped-target", 1);
        Need(s.View(1).HandCount == 4 && s.G.CardMovements.Count(move => move.Reason.Value == Reason && move.To == CardLocation.Hand(1)) == 3,
            "A selected recipient uses its own current maximum HP four and obtains only the two-card shortfall.");
        moves = s.G.CardMovements.Count;
        s.Use("capped-target", 1);
        var facts = s.G.Events.Select(item => item.Payload).ToArray();
        var started = facts.OfType<ProgramSkillStartedEvent>().Where(activation => activation.SkillId == Skill).ToArray();
        var completed = facts.OfType<ProgramSkillResolvedEvent>().Where(activation => activation.SkillId == Skill).ToArray();
        var grants = facts.OfType<ArmorGrantedEvent>().ToArray();
        var diagnostic = JsonSerializer.Serialize(new
        {
            movementBefore = moves, movementAfter = s.G.CardMovements.Count, handCount = s.View(1).HandCount,
            armor = s.View(0).Armor,
            resolvedBindings = facts.OfType<ProgramBindingResolvedEvent>().Where(binding => binding.SkillId == Skill).ToArray(),
            resolvedActivations = facts.OfType<ProgramSkillResolvedEvent>().Where(activation => activation.SkillId == Skill).ToArray(),
            armorGrants = facts.OfType<ArmorGrantedEvent>().ToArray(),
            frames = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Where(frame => frame.SkillId == Skill)
                .Select(frame => new { frame.Id, frame.ActivationId, frame.TriggerId, frame.InstructionIndex }).ToArray()
        });
        Need(s.G.CardMovements.Count == moves && s.View(1).HandCount == 4 && s.View(0).Armor == 6 &&
            started.Length == 6 && started.Select(activation => activation.FrameId).Distinct().Count() == 6 &&
            completed.Length == 6 && completed.All(activation => activation is { OwnerSeat: 0, Completed: true }) &&
            completed.Select(activation => activation.FrameId).ToHashSet().SetEquals(started.Select(activation => activation.FrameId)) &&
            grants.Length == 6 && grants.All(grant => grant is { PlayerSeat: 0, Amount: 1 }) &&
            grants.Select(grant => grant.ProgramFrameId).Distinct().Count() == 6 &&
            grants.Select(grant => grant.ProgramFrameId).ToHashSet().SetEquals(started.Select(activation => activation.FrameId)) &&
            !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Skill),
            "A filled recipient has no second gain or discard, while every native program returns once and executes its following instruction once. Facts: " + diagnostic);
    }

    private static SkillProgramCatalog Load(string effect, bool selectedTarget = false) => SkillProgramCatalog.Load(
        $$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,
          "activations":[{"id":"refill","minCards":0,"maxCards":0,"minTargets":{{(selectedTarget ? 1 : 0)}},
            "maxTargets":{{(selectedTarget ? 1 : 0)}},"targetKind":"anyLiving","usesPerTurn":null,"effects":[{{effect}}]}]}]}
        """, Presentation);
    private static SkillProgramEffect Effect(SkillProgramCatalog catalog) => catalog.Programs[Skill].Activations.Single().Effects.Single();
    private static string Presentation => JsonSerializer.Serialize(new
    {
        schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
        skills = new Dictionary<string, object> { [Skill] = new { name = "共享封顶补牌", description = "按接收者公开体力上限封顶。" } }
    });
    private static void Reject(string effect)
    {
        try { _ = Load(effect); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("The loader accepted an invalid capped-refill shape: " + effect);
    }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        private PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot View(int seat) => G.CreateSnapshot(seat).Players[seat];
        public void Use(string activation, int? target = null)
        {
            Accept(new UseProgramSkillCommand(0, Skill, activation, [], target is { } seat ? [seat] : [], G.Revision, P!.PromptId));
            Play();
        }
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native capped refill command."); }
        private void Play()
        {
            for (var step = 0; step < 100; step++)
            {
                if (P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(P?.PlayerSeat != 0 && G.State.Status != EngineStatus.Completed, "Unexpected native capped refill prompt.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native capped refill play boundary.");
        }
        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(83, 1, "fixture:capped-refill", Owner, 3, 0, 0, 0, 0,
                [], [new(CardKind.Dodge, Suit.Club, 2)], [],
                [new(Enemy) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 3)] }], [], new string('c', 64));
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, ContentRegistry.Build(new StandardContentPackage(), new Fixture())));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-capped-refill", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skill}}","revision":1,
                  "activations":[
                    {"id":"capped-owner","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{{CappedOwner}},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"fixed-owner","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{{FixedOwner}},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"capped-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{{CappedTarget}},{"op":"grantArmor","target":"owner","amount":1}]},
                    {"id":"dynamic-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{{DynamicTarget}},{"op":"grantArmor","target":"owner","amount":1}]}]}]}
                """, Presentation);
            builder.AddSkill(new(Skill, "共享封顶补牌", "按实际目标执行原生补牌。")
                { Program = catalog.Programs[Skill], ProgramPresentation = catalog.Presentations[Skill] });
            builder.AddGeneral(new(Owner, "低体力上限持有者", "supporter", Skill, "shu", 3));
            builder.AddGeneral(new(Enemy, "目标", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "真实补牌实体", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(index => new ContentDeckPhysicalCard("standard:dodge", (Suit)(index % 4), index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "共享封顶补牌实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
