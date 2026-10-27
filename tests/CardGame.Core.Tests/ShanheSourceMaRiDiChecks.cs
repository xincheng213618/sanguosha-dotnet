using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceMaRiDiChecks
{
    private const string Skill = StandardShanheSourceMaRiDiPackage.WeizhongSkillId;
    private const string Bingjie = StandardShanheSourceMaRiDiPackage.BingjieSkillId;
    private const string Watch = "fixture:source-maridi-gain-child", Deck = "fixture:source-maridi-deck";
    private const string DrawReason = "skill-program." + Skill + ".Draw";

    public static void ActualMaximumHpChangeFreezesLeastHandDrawAndReturnsThroughBingjie()
    {
        foreach (var peerInitialHand in new[] { 1, 0 })
        {
            const int initialHand = 1;
            var expected = peerInitialHand == 1 ? 2 : 1;
            var s = Scene.Start(peerInitialHand);
            Need(s.V is { Hp: 3, MaxHp: 3 } && s.V.HandCount == initialHand &&
                s.G.CreateSnapshot(0).Players[1].HandCount == peerInitialHand && s.Draws == 0,
                "Before the actual first Play payment, the source520 body has its real 3/3 health and the intended public hand comparison.");
            s.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
            var producer = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
            var payment = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Bingjie);
            var draw = s.Events<NativeDrawInvocationRecordedEvent>().Single(fact => fact.ParentFrameId == producer.Id);
            Need(s.V is { Hp: 2, MaxHp: 2 } && s.V.HandCount == initialHand + expected && s.Draws == expected &&
                payment.MaMiDi is { Stage: MaMiDiStage.MaximumChildren, MaximumBefore: 3, MaximumAfter: 2, HpBefore: 3, HpAfter: 2 } &&
                s.Events<MaMiDiMaximumIssuedEvent>().Single() is { OwnerSeat: 0, MaximumBefore: 3, MaximumAfter: 2 } &&
                draw.OwnerSeat == 0 && draw.Requested == expected && draw.ActualCount == expected && draw.Reason.Value == DrawReason &&
                s.Events<ProgramBindingResolvedEvent>().All(fact => fact.FrameId != producer.Id && fact.FrameId != payment.Id) &&
                s.Events<BingjieGrantedEvent>().Length == 0,
                "The genuine Bingjie maximum-HP cost suspends its original parent while the one Weizhong Draw pays the frozen least-hand count and waits for its real hand-gain child.");
            var drawnCards = s.G.CardMovements.Where(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
                move.Reason.Value == DrawReason).Select(move => move.CardId).ToHashSet();
            Need(drawnCards.Count == expected,
                "The source239 draw receives exactly the requested distinct physical entities; a tied minimum remains one two-card draw after its hand stops being minimum.");

            // DrawCards issues one frozen request, but DrawOne moves each physical
            // card in its own batch. The perBatch observer must return for each.
            var children = new HashSet<long>();
            var batches = new HashSet<long>();
            var observedCards = new HashSet<int>();
            while (s.P?.SkillPrompt?.SkillId == Watch)
            {
                var child = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch);
                var batch = child.WindowContext?.MovementBatch;
                Need(children.Count < expected && children.Add(child.Id) && batch is not null && batches.Add(batch.Id) &&
                    batch.Movements.Count > 0 && batch.Movements.All(move => move.From == CardLocation.DrawPile &&
                        move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason && drawnCards.Contains(move.CardId) && observedCards.Add(move.CardId)) &&
                    s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == producer.Id && frame.SkillId == Skill) &&
                    s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == payment.Id && frame.MaMiDi is
                        { Stage: MaMiDiStage.MaximumChildren, MaximumBefore: 3, MaximumAfter: 2 }) &&
                    s.V is { Hp: 2, MaxHp: 2 } && s.Draws == expected &&
                    s.Events<NativeDrawInvocationRecordedEvent>().Count(fact => fact.ParentFrameId == producer.Id) == 1 &&
                    s.Events<MaMiDiMaximumIssuedEvent>().Length == 1 && s.Events<BingjieGrantedEvent>().Length == 0,
                    "Every legitimate per-card gain batch suspends the same original draw and maximum-HP payment, observes distinct paid entities, and leaves the frozen request and cost untouched.");
                s.Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
                s.Until(() => s.P?.SkillPrompt?.SkillId == Watch ||
                    s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && s.G.ResolutionStack.Count == 0);
            }
            Need(s.Draws == expected && s.Events<NativeDrawInvocationRecordedEvent>().Count(fact => fact.ParentFrameId == producer.Id) == 1 &&
                s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == producer.Id && fact.Completed) == 1 &&
                s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == payment.Id && fact.Completed) == 1 &&
                children.Count == batches.Count && observedCards.SetEquals(drawnCards) &&
                s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == Watch && fact.Completed) == children.Count &&
                children.All(id => s.Events<ProgramBindingResolvedEvent>().Count(fact => fact.FrameId == id && fact.Completed) == 1) &&
                s.Events<BingjieGrantedEvent>().Length == 1 && s.Events<MaMiDiMaximumIssuedEvent>().Length == 1 &&
                s.G.State is { TurnNumber: 1, CurrentSeat: 0, Phase: TurnPhase.Play },
                "All exact gain children cover the paid physical cards once and return through the one original draw, Weizhong binding and Bingjie frame, completing the first natural Play entry without recalculating, drawing or paying again.");
        }
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public int Draws => G.CardMovements.Count(move => move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason);
        public T[] Events<T>() => G.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native source2452 command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P!;
            Need(prompt.PlayerSeat == 0, "Only the actual human answers this maximum-HP or draw-child prompt.");
            Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, G.Revision));
        }
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 100; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None && P is null, "Unexpected native source2452 decision: " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing actual maximum-HP or draw-child return: " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new
        { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Pending = P, Draws,
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, Type = frame.GetType().Name }) });

        public static Scene Start(int peerInitialHand)
        {
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-maridi-2452", StandardShanheSourceMaRiDiPackage.NightmareId,
                3, 0, 0, 0, 0, [Watch], [new(CardKind.Dodge, Suit.Heart, 2)], [],
                [new("classic:guan-yu") { StartingHand = peerInitialHand == 0 ? [] : [new(CardKind.Dodge, Suit.Club, 3)] }], [], new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry));
            scene.Accept(new StartGameCommand());
            scene.Until(() => scene.P?.SkillPrompt?.SkillId == Bingjie &&
                scene.P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-maridi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"actual-weizhong-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementOccurrence":"perBatch","movementReasons":["{{DrawReason}}"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"return","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实威重摸牌孩子", description = "完成后返回原摸牌与秉节付款。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddDeck(new(Deck, "零随机起手、零自然摸牌的真实闪牌堆", 0, 0, [new("standard:dodge", 24)]));
        }
    }
}
