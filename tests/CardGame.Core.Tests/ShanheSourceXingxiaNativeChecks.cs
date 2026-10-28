using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceXingxiaNativeChecks
{
    private const string Skill = StandardShanheSourceBeigongDuoPackage.XingxiaSkillId;
    private const string Actor = "fixture:source-xingxia-owner", Ally = "fixture:source-xingxia-ally";
    private const string HandPayer = "fixture:source-xingxia-hand", EquipmentPayer = "fixture:source-xingxia-equipment";
    private const string NoRed = "fixture:source-xingxia-no-red", Quiet = "fixture:source-xingxia-quiet";
    private const string Watch = "fixture:source-xingxia-paid-child", Deck = "fixture:source-xingxia-deck";
    private const string PaymentReason = "skill-program.source-xingxia.discard";

    public static void OwnPlayStartWaitsForRealRedHandAndEquipmentPaymentsAndTwoRoundSpacing()
    {
        var s = Scene.Start();
        var friend = s.Seat(Ally); var handPayer = s.Seat(HandPayer);
        var equipmentPayer = s.Seat(EquipmentPayer); var noRed = s.Seat(NoRed);
        Need(s.G.CreateSnapshot(0).Players.Count(p => p.IsHuman) == 1 && s.V(0).IsHuman &&
            s.V(friend).TeamId == s.V(0).TeamId && s.V(handPayer).TeamId != s.V(0).TeamId,
            "The fresh encounter uses production public teams and only seat0 is human.");
        var hand = s.G.CreateSnapshot(handPayer).Players[handPayer].Hand.Single().Id;
        var equipment = s.V(equipmentPayer).Equipment.Single().Id;
        s.ChooseFriend(friend);
        s.PaidChild(handPayer, hand, CardLocation.Hand(handPayer));
        var first = s.E<SourceXingxiaIssuedEvent>().Single();
        Need(first.OwnerSeat == 0 && first.FriendSeat == friend && first.Round == 1 &&
            s.V(friend).Hp == 10 && s.V(handPayer).Hp == 12 && s.V(equipmentPayer).Hp == 12 && s.V(noRed).Hp == 12,
            "The first own Play issues compulsory friend Fire2 before enemies; the paid first enemy avoids damage while later enemies still await their original turn in the same skill.");
        s.ContinuePaidChild();
        s.PaidChild(equipmentPayer, equipment, CardLocation.Equipment(equipmentPayer), first.ProgramFrameId);
        Need(s.E<SourceXingxiaPaymentEvent>().Select(e => e.CardId).SequenceEqual([hand, equipment]) &&
            s.V(noRed).Hp == 12,
            "The next AI enemy pays its actual red equipment, preserving the original source frame and leaving the later no-red enemy unresolved.");
        s.ContinuePaidChild(); s.Play();
        Need(s.V(friend).Hp == 10 && s.V(handPayer).Hp == 12 && s.V(equipmentPayer).Hp == 12 && s.V(noRed).Hp == 11 &&
            s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).Select(e => (e.TargetSeat, e.Amount, e.Nature))
                .SequenceEqual([(friend, 2, DamageNature.Fire), (noRed, 1, DamageNature.Fire)]) &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == first.ProgramFrameId && e.Completed) == 1 &&
            new[] { hand, equipment }.All(id => s.G.CardMovements.Count(m => m.CardId == id &&
                m.To == CardLocation.DiscardPile && m.Reason.Value == PaymentReason) == 1),
            "Both exact red payments return once, then the enemy with no red material takes Fire1; the source frame completes only after the actual native sequence.");

        s.EndPlay(); s.Play();
        Need(s.Round == 2 && s.E<SourceXingxiaIssuedEvent>().Length == 1 && s.V(friend).Hp == 10 && s.V(noRed).Hp == 11,
            "The next actual round reaches the owner's Play without another source1117 issue or damage.");
        s.EndPlay(); s.ChooseFriend(friend); s.Play();
        Need(s.Round == 3 && s.E<SourceXingxiaIssuedEvent>().Select(e => e.Round).SequenceEqual([1, 3]) &&
            s.E<SourceXingxiaIssuedEvent>().Select(e => e.ProgramFrameId).Distinct().Count() == 2 &&
            s.V(friend).Hp == 8 && s.V(handPayer).Hp == 11 && s.V(equipmentPayer).Hp == 11 && s.V(noRed).Hp == 10 &&
            s.E<SourceXingxiaPaymentEvent>().Length == 2 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed) == 2 &&
            s.G.State.Winner == Winner.None,
            "Two actual rounds after its first issue the locked skill triggers again; spent original payments are not replayed and all three now-redless enemies take one real fire damage each.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public int Round => E<RoundStartedEvent>().Last().RoundNumber;
        public int Seat(string id) => G.CreateSnapshot(0).Players.Single(p => p.GeneralId == id).Seat;
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, (result.Error?.Message ?? "Source1117 native command rejected.") + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choices = p.Choices.Where(predicate).ToArray();
            Need(p.PlayerSeat == 0 && choices.Length == 1, "The actual human participant has exactly one intended native choice. " + Diagnostic());
            Submit(new AnswerPromptCommand(0, p.PromptId, choices[0].Id, G.Revision));
        }
        public void ChooseFriend(int friend)
        {
            Until(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Targets.SequenceEqual([friend])));
            Need(P!.PlayerSeat == 0 && P.Choices.All(c => c.Targets.SequenceEqual([friend])) &&
                P.Choices.All(c => c.Parameters.GetValueOrDefault("program-action") != "skip"),
                "Locked source1117 requires its actual other public-team friend and has no optional skip. " + Diagnostic());
            Answer(c => c.Targets.SequenceEqual([friend]));
        }
        public void PaidChild(int payer, int card, CardLocation from, long? sourceFrame = null)
        {
            Until(() => P?.SkillPrompt?.SkillId == Watch &&
                P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            var parent = G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SourceXingxia is not null);
            var child = G.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.SkillId == Watch);
            Need((sourceFrame is null || parent.Id == sourceFrame) && parent.OwnerSeat == 0 &&
                parent.SourceXingxia is { Stage: SourceXingxiaStage.PaymentChildren, PaymentCardId: var paid } receipt && paid == card &&
                receipt.Enemies[receipt.Index] == payer && parent.PendingMovementContinuation is { SubjectSeat: var subject } && subject == payer &&
                child.OwnerSeat == payer && child.WindowContext?.MovementBatch is { OriginSkillId: Skill, OriginOwnerSeat: 0 } batch &&
                batch.Movements.Any(m => m.CardId == card && m.From == from && m.To == CardLocation.DiscardPile && m.Reason.Value == PaymentReason) &&
                G.CardMovements.Count(m => m.CardId == card && m.From == from && m.To == CardLocation.DiscardPile && m.Reason.Value == PaymentReason) == 1 &&
                E<SourceXingxiaPaymentEvent>().Count(e => e.ProgramFrameId == parent.Id && e.PayerSeat == payer && e.CardId == card) == 1 &&
                E<ProgramBindingResolvedEvent>().All(e => e.FrameId != parent.Id),
                "A real red Hand/equipment payment pauses its own movement child under the same paid source1117 frame before moving to the next enemy. " + Diagnostic());
        }
        public void ContinuePaidChild() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void EndPlay() => Submit(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 700; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded source1117 fixture stays in a live encounter. " + Diagnostic());
                if (P is { PlayerSeat: 0 }) throw new InvalidOperationException("Unexpected source1117 human boundary. " + Diagnostic());
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source1117 exceeded bounded native progression. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P,
            Frames = G.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name }),
            Recent = G.Events.TakeLast(5).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });

        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(147, 1, "source-xingxia-native", Actor, 12, 0, 0, 0, 0, [], [], [],
                [new(HandPayer) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] },
                 new(EquipmentPayer) { StartingHand = [], StartingEquipment = [new(CardKind.QinggangSword, Suit.Diamond, 6)] },
                 new(NoRed) { StartingHand = [] }],
                [new(Ally) { StartingHand = [] }], new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            {
                PlayerGeneralId = StandardShanheSourceBeigongDuoPackage.BeigongHardId,
                EnemyRoster = setup.EnemyRoster.Select(member => member with { GeneralId = "classic:guan-yu" }).ToArray(),
                AllyRoster = setup.AllyRoster.Select(member => member with { GeneralId = "classic:guan-yu" }).ToArray()
            }).WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
            { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Actor, Ally, HandPayer, EquipmentPayer, NoRed]).Distinct().ToArray() } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 5, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 16
            }, registry));
            scene.Submit(new StartGameCommand());
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-xingxia", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"actual-idle-turn","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"original-red-hand-payment-child","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{PaymentReason}}"],"optional":false,"isSkillInvocation":false,
                    "effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},
                               {"op":"chooseOption","target":"selectedTarget","resultBind":"paid","options":[{"id":"continue"}]}]},
                    {"id":"original-red-equipment-payment-child","window":"cardsMoved","subject":"owner",
                    "sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["{{PaymentReason}}"],"optional":false,"isSkillInvocation":false,
                    "effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},
                               {"op":"chooseOption","target":"selectedTarget","resultBind":"paid","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Quiet] = new { name = "保持短局的真实阶段", description = "摸牌后跳过本人出牌阶段。" },
                        [Watch] = new { name = "行夏真实付款孩子", description = "等待实际付款孩子后继续原帧。",
                            optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                    } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Actor, "行夏原技能持有者", "supporter", Skill, "qun", 12));
            builder.AddGeneral(new(Ally, "真实公开友方", "supporter", Quiet, "qun", 12));
            builder.AddGeneral(new(HandPayer, "红手牌付款敌军", "supporter", Quiet, "wei", 12, [Watch]));
            builder.AddGeneral(new(EquipmentPayer, "红装备付款敌军", "supporter", Quiet, "wei", 12, [Watch]));
            builder.AddGeneral(new(NoRed, "无红牌敌军", "supporter", Quiet, "wei", 12));
            builder.AddDeck(new(Deck, "固定黑色摸牌", 0, 2, [])
            { PhysicalCards = Enumerable.Range(0, 96).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
        }
    }
}
