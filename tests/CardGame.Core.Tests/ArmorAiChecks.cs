using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ArmorAiChecks
{
    private const string Driver = "fixture:armor-ai-driver", Owner = "fixture:armor-ai-owner";
    private const string Ally = "fixture:armor-ai-ally", FirstEnemy = "fixture:armor-ai-first-enemy";
    private const string SecondEnemy = "fixture:armor-ai-second-enemy", Deck = "fixture:armor-ai-deck";
    private const string Jueqing = "classic:jueqing", Xiaoming = "shanhe:xiaoming", Gangzhi = "ol:gangzhi";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(83, 1, "fixture-armor-ai-production", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [],
            [new("standard:cao-cao")], [], new string('b', 64))));

    private static void PolicyProjectionDoesNotReadUnpublishedRecipientSkills()
    {
        var scene = Scene.Start("visibility");
        scene.Use("armor-other", [2]);
        CheckProjection(); scene.Cold(); CheckProjection();
        scene.Use("hit-two", [2]);
        Need(scene.V(2) is { Hp: 4, Armor: 3 } && scene.Events<DamageAppliedEvent>().Length == 0 &&
            scene.Events<DamageReplacedWithHpLossEvent>() is [{ TargetSeat: 2, Amount: 2 }],
            "Limiting an AI projection must not disable the recipient's real acquired replacement policy or spend Armor in the actual command.");
        scene.Cold();

        void CheckProjection()
        {
            var before = scene.Boundary();
            var actors = ((IEnumerable<CharacterState>)typeof(GameEngine)
                .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(scene.G)!).ToArray();
            Need(new[] { Jueqing, Gangzhi }.All(id => actors[2].SkillGrants.Grants.Any(grant =>
                    grant.IsEnabled && grant.SourceId == "campaign:shanhe" && grant.SkillId == id)),
                "Both policies must be genuine enabled encounter grants, rather than imaginary skill flags.");
            var complete = scene.G.CreateSnapshot(0);
            // Exercise the projection boundary with reduced visibility; this is
            // not an assertion that the visible team fixture is an identity-concealment mode.
            var limited = complete with { Players = Array.AsReadOnly(complete.Players.Select(player =>
                player.Seat == 2 ? player with { Skills = [], SecondarySkills = [] } : player).ToArray()) };
            var method = typeof(GameEngine).GetMethod("CreateAiSkillContext", BindingFlags.Instance | BindingFlags.NonPublic)!;
            var visible = (PlayerSkillContext)method.Invoke(scene.G, [actors[2], 0, complete])!;
            var unavailable = (PlayerSkillContext)method.Invoke(scene.G, [actors[2], 0, limited])!;
            var own = (PlayerSkillContext)method.Invoke(scene.G, [actors[2], 2, limited])!;
            Need(visible.DamageBecomesHpLoss && visible.OtherParticipantDamageBecomesHpLoss &&
                !unavailable.DamageBecomesHpLoss && !unavailable.OtherParticipantDamageBecomesHpLoss &&
                own.DamageBecomesHpLoss && own.OtherParticipantDamageBecomesHpLoss,
                "Complete foreign projection and actual owner know both policies; a foreign projection omitting both skill columns cannot reveal either policy.");
            var owner = scene.V();
            var effects = scene.Registry.GetSkill(Driver).Program!.Activations.Single(a => a.Id == "hit-two").Effects;
            var context = new PlayerSkillContext(0, owner.Hp, owner.MaxHp, owner.HandCount, TurnPhase.Play, IsOwnTurn: true);
            var known = ProgramCompositionAi.Estimate(effects, context, publicContext: new(1, SelectedTarget: visible)).Hint;
            var unknown = ProgramCompositionAi.Estimate(effects, context, publicContext: new(1, SelectedTarget: unavailable)).Hint;
            Need(known is { TargetDamage: 0, TargetDirectHpLoss: 2 } &&
                known.PredictTarget(6, 8, 3) is { HpAfter: 4, ArmorAfter: 3 } &&
                unknown is { TargetDamage: 2, TargetDirectHpLoss: 0 } &&
                unknown.PredictTarget(6, 8, 3) is { HpAfter: 6, ArmorAfter: 1 } && scene.Boundary() == before,
                "Real registered program estimates must consume only the supplied public policy context, leave the actual game unchanged and remain stable after cold restoration.");
        }
    }

    private static ProgramAiEstimate Estimate(Scene scene, string activationId)
    {
        var owner = scene.V();
        var effects = scene.Registry.GetSkill(Driver).Program!.Activations.Single(a => a.Id == activationId).Effects;
        return ProgramCompositionAi.Estimate(effects,
            new PlayerSkillContext(0, owner.Hp, owner.MaxHp, owner.HandCount, TurnPhase.Play, IsOwnTurn: true)
            { Armor = owner.Armor });
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public ContentRegistry Registry { get; } = registry;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public LegalAction Action(string id) => G.GetHumanLegalActions().Single(a =>
            a.ProgramSkillId == Driver && a.ProgramActivationId == id);
        public void Use(string id, int[]? targets = null)
        {
            Accept(new UseProgramSkillCommand(0, Driver, id, [], targets ?? [], G.Revision, G.PendingDecision!.PromptId));
            Play();
        }
        public void UseUntilDying(string id, int target)
        {
            Accept(new UseProgramSkillCommand(0, Driver, id, [], [target], G.Revision, G.PendingDecision!.PromptId));
            for (var step = 0; step < 120; step++)
            {
                if (V(target).Hp <= 0 && G.ResolutionStack.OfType<DyingFrame>().Any(frame => frame.VictimSeat == target)) return;
                Need(V(target).IsAlive, "The direct-loss witness must pause before the native dying response resolves.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native dying child.");
        }
        public void PlayCard(CardKind kind, int target)
        {
            var action = G.GetHumanLegalActions().Single(a => a.CardId is { } id &&
                V().Hand.Any(c => c.Id == id && c.Kind == kind) && a.ConversionSource is null &&
                a.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision,
                G.PendingDecision!.PromptId, action.PlayedCardKind, action.TargetCardId));
            Play();
        }
        private void Accept(GameCommand command)
        {
            var result = G.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Armor AI fixture rejected a genuine command.");
        }
        private void Play()
        {
            for (var step = 0; step < 180; step++)
            {
                if (G.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
                Need(G.State.Winner == Winner.None, "The small Armor AI scenario must not finish a match.");
                Need(G.PendingDecision?.PlayerSeat != 0,
                    "Unexpected Armor AI fixture prompt: " + JsonSerializer.Serialize(G.PendingDecision));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded Armor AI fixture did not return to Play.");
        }
        public string Boundary() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
            Facts = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())),
            G.CardMovements, Zones = G.CreateCardZoneDiagnostics(),
            Checkpoint = GameCheckpointJson.Serialize(G.CreateCheckpoint())
        });
        public void Cold()
        {
            var before = Boundary();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), Registry);
            Need(Boundary() == before, "Cold replay must preserve the actual AI command, Armor payments and every public/private view exactly.");
        }
        public static Scene Start(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var extras = scene is "jueqing" or "negative" ? new[] { Jueqing } : Array.Empty<string>();
            ShanheStartingCard[] cards = scene is "jueqing" or "maximum"
                ? [new(CardKind.Slash, Suit.Spade, 6)] : [];
            var setup = new ShanheBattleSetup(83, 1, "fixture-armor-ai-" + scene, Owner,
                scene == "sequences" ? 2 : 8, 0, 0, 0, 0, extras, cards, [],
                [new(FirstEnemy, Hp: scene is "targets" or "negative" ? 1 : 6,
                    ExtraSkillIds: scene == "visibility" ? [Jueqing, Gangzhi] : []), new(SecondEnemy, Hp: scene == "targets" ? 2 : 6)],
                [new(Ally)], new string('e', 64))
            { TacticSkillIds = scene == "maximum" ? [Xiaoming] : [] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var result = new Scene(game, registry);
            result.Accept(new StartGameCommand()); result.Play(); return result;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-armor-ai", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var id in new[] { Jueqing, Xiaoming, Gangzhi }) builder.AddSkill(Production.Value.GetSkill(id));
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                  {"id":"armor-self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantArmor","target":"owner","amount":3}]},
                  {"id":"armor-other","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"grantArmor","target":"selectedTarget","amount":3}]},
                  {"id":"hit-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":2}]},
                  {"id":"lose-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":2}]},
                  {"id":"self-double","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":2},{"op":"damage","target":"owner","amount":2}]},
                  {"id":"grant-first","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantArmor","target":"owner","amount":3},{"op":"damage","target":"owner","amount":2},{"op":"damage","target":"owner","amount":2}]},
                  {"id":"damage-first","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":2},{"op":"grantArmor","target":"owner","amount":3},{"op":"damage","target":"owner","amount":2}]},
                  {"id":"self-loss","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"owner","amount":2}]},
                  {"id":"self-recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recover","target":"owner","amount":3}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                { [Driver] = new { name = "真实护甲AI操作", description = "通过原生伤害、扣血和赋甲指令检查公开AI估计" } }
            });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "护甲AI使用者", "", Driver, "shu", 8));
            foreach (var id in new[] { Ally, FirstEnemy, SecondEnemy }) builder.AddGeneral(new(id, "真实候选", "", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "零自然摸牌真实牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, i % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "护甲AI原生战斗", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, FirstEnemy, SecondEnemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
