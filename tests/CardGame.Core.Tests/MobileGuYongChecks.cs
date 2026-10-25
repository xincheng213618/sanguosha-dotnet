using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileGuYongChecks
{
    private const string Hero = "mobile:gu-yong", Skill = "mobile:bingyi", Owner = "fixture:mobile-gu-yong-owner",
        Mode = "identity:classic-mobile-gu-yong-fixture", Deck = "fixture:mobile-gu-yong-deck";

    public static void RevealPrecedesRecipientChoiceAndMixedHandCompletesCold()
    {
        var s = Scene.Start(mixed: false); s.Play(0);
        var hand = s.V(0).Hand.Select(c => c.Id).ToArray(); var before = s.Counts();
        s.End(0); s.Offer();
        s.Need(!s.Facts<ProgramCardsRevealedEvent>().Any(e => e.SkillId == Skill),
            "The optional invocation has not shown the hand or asked for beneficiaries yet.");
        s.Activate(); s.Targets();
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var reveal = s.Facts<ProgramCardsRevealedEvent>().Single(e => e.FrameId == root.Id && e.Bind == "shown-hand");
        s.Need(reveal.Cards.Select(c => c.Id).SequenceEqual(hand) && root.SelectedTargetSeats.Count == 0 &&
            hand.All(id => s.Zone(id) == CardLocation.Hand(0)) && s.Counts().SequenceEqual(before) &&
            s.P!.Choices.Any(c => c.Targets.Count == 0) && s.P.Choices.All(c => c.Targets.Count <= hand.Length),
            "All actual hand entities are already publicly shown before the first same-color beneficiary choice, remain in hand, and permit zero through X recipients.");
        s.Cold(); s.Answer(c => c.Targets.Count == 0); s.Play(1);
        s.Need(s.Counts().SequenceEqual(before) && s.Draws() == 0 &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.FrameId == root.Id && e.Activated && e.Completed) == 1,
            "An explicitly committed empty beneficiary set completes once after the reveal and draws for nobody.");
        for (var seat = 1; seat < 4; seat++) { s.End(seat); s.Play(seat == 3 ? 0 : seat + 1); }
        before = s.Counts(); s.End(0); s.Offer(); s.Activate(); s.Targets();
        s.Need(s.Facts<ProgramCardsRevealedEvent>().Count(e => e.SkillId == Skill) == 2,
            "The owner's next actual ending shows the whole hand before its fresh beneficiary choice.");
        s.Answer(c => c.Targets.SequenceEqual([1, 2])); s.Play(1);
        s.Need(s.V(0).Hand.Count == before[0] && s.V(1).Hand.Count == before[1] + 1 &&
            s.V(2).Hand.Count == before[2] + 1 && s.V(3).Hand.Count == before[3] && s.Draws() == 2,
            "Exactly the two actually selected recipients draw one real entity each; no automatic owner or unselected recipient is added.");

        s = Scene.Start(mixed: true); s.Play(0); hand = s.V(0).Hand.Select(c => c.Id).ToArray(); before = s.Counts();
        s.Need(s.V(0).Hand.Any(c => c.Suit == Suit.Heart) && s.V(0).Hand.Any(c => c.Suit == Suit.Spade),
            "The fixed mixed-hand scene contains real red and black hand entities.");
        s.End(0); s.Offer(); s.Activate(); s.Play(1);
        reveal = s.Facts<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Skill);
        s.Need(reveal.Cards.Select(c => c.Id).SequenceEqual(hand) && hand.All(id => s.Zone(id) == CardLocation.Hand(0)) &&
            s.Counts().SequenceEqual(before) && s.Draws() == 0 &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.FrameId == reveal.FrameId && e.Activated && e.Completed) == 1,
            "A genuinely mixed whole hand is still shown, then the original binding completes successfully with no beneficiary prompt, no payment and no draw.");
        s.Cold();
    }

    private static string Peer(int seat) => "fixture:mobile-gu-yong-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int[] Counts() => Enumerable.Range(0, 4).Select(i => V(i).Hand.Count).ToArray();
        public int Draws() => G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:bingyi.Draw");
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Targets() => Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets") == true);
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 150 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select)
                { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                Need(P is null, "Unexpected native Bingyi boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The small fixed Bingyi scene missed its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var original = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(original == Dump(), "Cold keeps the original whole-hand reveal and uncommitted beneficiary choice, or the completed mixed-hand binding, exactly once.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, Hand = V(0).Hand, G.State.TurnNumber })); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Bingyi command."); }
        public static Scene Start(bool mixed)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mixed));
            var game = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
                HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture(bool mixed) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-gu-yong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:gu-yong");
            b.AddGeneral(new(Owner, "移动秉壹真实持有者", "supporter", Skill, "wu", 8));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "实际同桌", "supporter", "standard:none", "wu", 8));
            b.AddDeck(new(Deck, "固定真实手牌", mixed ? 6 : 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(i => new ContentDeckPhysicalCard("standard:dodge", mixed && i % 2 == 0 ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "秉壹先展示后选择", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
