using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryHuangYueyingMobileChecks
{
    private const string Hero = "boundary:huang-yueying-mobile", Skill = "boundary:jizhi-mobile",
        Owner = "fixture:mobile-huang-yueying-owner", Mode = "identity:classic-mobile-huang-yueying";
    private const string DrawReason = "skill-program." + Skill + ".Draw", DiscardReason = "skill-program." + Skill + ".MoveBoundCards";

    public static void ConvertedOrdinaryTrickPaysDrawnBasicForLimitColdAndDelayedTrickDoesNotTrigger()
    {
        var s = Scene.Start(CardKind.Dodge); s.Play();
        var action = s.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Dismantlement && a.ConversionSource?.SkillId == "classic:qixi");
        var material = action.CardId!.Value; var hand = s.V(0).HandCount; var limit = s.V(0).HandLimit;
        s.PlayAction(action); s.Reach(s.Offered);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "discard"));
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var native = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == root.WindowContext!.CardUse!.ParentCardUseFrameId);
        var drawn = root.CardSetBindings.Single(b => b.Name == "drawn").CardIds.Single();
        s.Need(native.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.Dismantlement, PhysicalCards.Count: 1 } actual &&
            actual.PhysicalCards.Single().CardId == material && actual.ConversionChain.Any(c => c.SkillId == "classic:qixi") &&
            root.TriggerId == "ordinary-trick-used" && s.V(0).Hand.Any(c => c.Id == drawn && c.Kind == CardKind.Dodge) &&
            s.V(0).HandCount == hand && s.V(0).HandLimit == limit &&
            s.G.CardMovements.Count(m => m.CardId == drawn && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == DrawReason) == 1,
            "A real one-material Qixi use qualifies mobile Jizhi and actually draws its own Basic entity before any discard or hand-limit entitlement.");
        var programId = root.Id; var useId = native.Id;
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "discard"); s.Play();
        s.Need(s.V(0).HandLimit == limit + 1 && s.V(0).HandCount == hand - 1 &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == drawn).Location == CardLocation.DiscardPile &&
            s.G.CardMovements.Count(m => m.CardId == drawn && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == DiscardReason) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == programId && e.SkillId == Skill && e.Activated && e.Completed) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId) == 1,
            "Cold returns to the same native converted trick: discard exactly its new Basic, add one actual-turn hand-limit point, and finish once without reusing a starting hand card or repaying Qixi.");

        var delayed = Scene.Start(CardKind.Indulgence); delayed.Play();
        var delayedAction = delayed.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Indulgence && a.PlayedCardKind is null);
        var delayedId = delayedAction.CardId!.Value;
        delayed.PlayAction(delayedAction); delayed.Play();
        delayed.Need(!delayed.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Skill) &&
            delayed.G.CreateCardZoneDiagnostics().Single(c => c.CardId == delayedId).Location == CardLocation.Judgment(delayedAction.TargetSeats.Single()),
            "A genuinely paid delayed trick still places its original entity but never opens mobile Jizhi, unlike the separate OL nonconverted-trick edition.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public bool Offered() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void PlayAction(LegalAction a) => Accept(new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
            { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Huang Yueying command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 140 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p)
                { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:mobile-huang-yueying-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.Nullification }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                if (P is { Kind: DecisionKind.SelectTargetCard }) { Answer(_ => true); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected mobile Huang Yueying boundary: " + Short());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The small native mobile Huang Yueying scene missed its boundary.");
        }
        public void Cold()
        { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the native converted use, actual drawn Basic receipt and original Jizhi choice."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        private string Short() => JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Hand = V(0).HandCount, Limit = V(0).HandLimit });
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + Short()); }
        public static Scene Start(CardKind use)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(use));
            var scene = new Scene(registry, GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(CardKind use) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-huang-yueying-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:huang-yueying", "classic:gan-ning");
            b.AddGeneral(new(Owner, "移动界黄月英实际场景", "supporter", Skill, "shu", 3, AdditionalSkillIds: ["boundary:qicai-current", "classic:qixi"]));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:mobile-huang-yueying-peer-" + i).ToArray();
            foreach (var peer in peers) b.AddGeneral(new(peer, "真实陪测角色", "supporter", "standard:none", "wei", 5));
            const string deck = "fixture:mobile-huang-yueying-deck";
            // Retain the already-verified four-seat seed/deal layout from the OL
            // qualification check; its index 28 is the owner's controlled use.
            b.AddDeck(new(deck, "固定基本牌及真实延时锦囊", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 56)
                .Select(i => new ContentDeckPhysicalCard(i == 28 && use == CardKind.Indulgence ? "standard:indulgence" : "standard:dodge", Suit.Spade, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动集智原生使用资格", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
