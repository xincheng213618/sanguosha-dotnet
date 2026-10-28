using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceWeizhuNativeChecks
{
    private const string Skill = StandardShanheSourceGuardTrioPackage.WeizhuSkillId;
    private const string Actor = "fixture:source-weizhu-owner", Ally = "fixture:source-weizhu-ally", Enemy = "fixture:source-weizhu-enemy";
    private const string Driver = "fixture:source-weizhu-driver", Watch = "fixture:source-weizhu-hand-child", Deck = "fixture:source-weizhu-deck";

    public static void OtherTeamDamageWaitsForActualHandPaymentAndNeverProtectsSelfOrEnemy()
    {
        var s = Scene.Start();
        var ally = s.G.CreateSnapshot(0).Players.Single(p => p.GeneralId == Ally).Seat;
        var enemy = s.G.CreateSnapshot(0).Players.Single(p => p.GeneralId == Enemy).Seat;
        var hp = s.V(ally).Hp;
        var material = s.V().Hand.First().Id;
        s.Damage(ally); s.Offer(); s.Answer(c => c.Cards.SequenceEqual([material]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var guard = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var child = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Watch);
        var window = s.G.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single(f => f.Id == guard.WindowContext!.ParentFrameId);
        Need(guard.InstructionIndex == 1 && guard.PendingMovementContinuation is not null &&
            window is { Prevented: false, Amount: 1 } && window.TargetSeat == ally && s.V(ally).Hp == hp &&
            child.WindowContext?.MovementBatch is { OriginSkillId: Skill, OriginOwnerSeat: 0 } batch &&
            batch.Movements.Any(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) &&
            s.E<ProgramDamagePreventedEvent>().All(e => e.SkillId != Skill) &&
            s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != guard.Id),
            "A teammate's real pending damage waits on exactly one paid Hand entity and its original movement child before the existing prevention node can run.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        Need(s.V(ally).Hp == hp && s.E<ProgramDamagePreventedEvent>().Count(e => e.SkillId == Skill &&
                e.FrameId == window.Id && e.OwnerSeat == 0 && e.TargetSeat == ally && e.Amount == 1) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == guard.Id && e.Completed) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == child.Id && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1 &&
            s.E<DamageAppliedEvent>().All(e => e.TargetSeat != ally),
            "The real paid child returns once to the same protection window and prevents this actual teammate damage without replacing the payment or creating a false damage fact.");

        s.Damage(ally); s.Offer(); s.Answer(c => c.Cards.Count == 1 && c.Cards[0] == s.V().Hand.First().Id); s.Play();
        Need(s.E<ProgramDamagePreventedEvent>().Count(e => e.SkillId == Skill && e.TargetSeat == ally) == 2 && s.V(ally).Hp == hp,
            "A second independent pending damage in the same turn can be protected with another actual Hand payment; source14034 has no turn quota.");
        var offers = s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill);
        var selfHp = s.V().Hp; s.SelfDamage(); s.Play();
        var enemyHp = s.V(enemy).Hp; s.Damage(enemy); s.Play();
        Need(s.V().Hp == selfHp - 1 && s.V(enemy).Hp == enemyHp - 1 &&
            s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill) == offers,
            "Own damage and enemy damage follow their real native damage paths without offering this teammate-only protection.");
        s.EmptyHand(); s.Play();
        Need(s.V().HandCount == 0 && s.V().Equipment.Any(c => c.Kind == CardKind.QinggangSword),
            "The actual command removes the owner's Hand while retaining a real equipment entity.");
        s.Damage(ally); s.Play();
        Need(s.V(ally).Hp == hp - 1 && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill) == offers &&
            s.V().Equipment.Any(c => c.Kind == CardKind.QinggangSword) && s.G.State.Winner == Winner.None,
            "No Hand payment means no protection even when equipment remains; no equipment cost, battle completion or recovery fixture substitutes for the source rule.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var r = G.Submit(command); Need(r.Accepted, (r.Error?.Message ?? "Rejected source14034 command.") + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Damage(int target) => Use("damage", [target]);
        public void SelfDamage() => Use("self", []);
        public void EmptyHand() => Use("empty", []);
        private void Use(string activation, int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets, G.Revision, P!.PromptId));
        public void Offer()
        {
            Until(() => P?.SkillPrompt?.SkillId == Skill);
            if (P!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
                Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Need(P?.Choices.Any(c => c.Cards.Count == 1) == true, "The accepted teammate protection requires one actual Hand entity." + Diagnostic());
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The short fresh protection encounter remains active." + Diagnostic());
                if (P is { } p)
                {
                    if (p.PlayerSeat != 0) Accept(new AdvanceOneStepCommand(G.Revision));
                    else if (p.SkillPrompt?.SkillId == Watch) Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                    else throw new InvalidOperationException("Unexpected source14034 native boundary." + Diagnostic());
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source14034 exceeded bounded native progression." + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P,
            Stack = G.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name }),
            Recent = G.Events.TakeLast(5).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(131, 1, "source-weizhu-native", Actor, 9, 0, 0, 0, 0, [],
                [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3), new(CardKind.Dodge, Suit.Diamond, 4)],
                [new(CardKind.QinggangSword, Suit.Spade, 6)], [new(Enemy) { StartingHand = [] }],
                [new(Ally) { StartingHand = [] }], new string('c', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            { PlayerGeneralId = StandardShanheSourceGuardTrioPackage.GuardHellId, StartingCards = [],
                EnemyRoster = [new("classic:guan-yu")], AllyRoster = [new("classic:guan-yu")] })
                .WithExtension(new Fixture(), m => m.Id == ShanheBattleSetup.BattleModeId ? m with
                { DeckId = Deck, GeneralPoolIds = m.GeneralPoolIds!.Concat([Actor, Ally, Enemy]).Distinct().ToArray() } : m);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-weizhu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                    {"id":"self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1}]},
                    {"id":"empty","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-hand-payment","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Driver] = new { name = "实际伤害与手牌清空入口", description = "只发出原生命令。" },
                        [Watch] = new { name = "卫主实际手牌付款孩子", description = "等待原付款帧返回。",
                            optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                    } }));
            foreach (var (id, program) in catalog.Programs)
                b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Actor, "卫主原技能持有者", "supporter", Skill, "qun", 9, [Driver, Watch]));
            b.AddGeneral(new(Ally, "公开队友", "supporter", null, "qun", 9));
            b.AddGeneral(new(Enemy, "公开敌人", "supporter", null, "wei", 9));
            b.AddDeck(new(Deck, "小型真实摸牌堆", 0, 2, [new("standard:dodge", 48)]));
        }
    }
}
