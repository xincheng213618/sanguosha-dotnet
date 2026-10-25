using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileCaoJieChecks
{
    private const string Owner = "fixture:mobile-cao-jie-owner", Mode = "identity:classic-mobile-cao-jie";
    public static void QualifiedStartingPopulationAndClassicDefaultPaidCold()
    {
        foreach (var mobile in new[] { true, false })
        {
            var s = Scene.Start(mobile); s.Play(0); s.End();
            s.Reach(() => s.P?.SkillPrompt?.SkillId == s.Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.At("public-draft-show"); var rootId = s.Root!.Id;
            s.Need(s.Root.PublicHandDraft!.RecipientSeats.SequenceEqual([1, 2]) && s.V(0).HandCount == 6,
                "The real Ending freezes only seats1/2 with four cards below HP5, and the original draft frame has already drawn its exact two cards.");
            s.Answer(c => Action(c) == "public-draft-show"); s.At("public-draft-show"); s.Answer(c => Action(c) == "public-draft-show"); s.At("public-draft-start");
            var starts = s.P!.Choices.SelectMany(c => c.Targets).Order().ToArray();
            s.Need(starts.SequenceEqual(mobile ? [1, 2] : [0, 1, 2, 3]) && s.Root is { PublicHandDraft.Stage: 2 } f && f.Id == rootId,
                mobile ? "Mobile Huimin permits only its two frozen benefit participants as the starting recipient, excluding living nonparticipants0/3."
                    : "Omitting the new policy preserves classic Huimin's existing default: every living character, including nonparticipants0/3, remains a legal starting point.");
            s.Cold(); var start = mobile ? 2 : 0; s.Answer(c => Action(c) == "public-draft-start" && c.Targets.SequenceEqual([start])); s.At("public-draft-gain");
            var first = mobile ? 2 : 1; var second = mobile ? 1 : 2;
            s.Need(s.P!.PlayerSeat == first && s.Root!.PublicHandDraft!.RecipientSeats.SequenceEqual([first, second]),
                "The accepted start rotates the original frozen participants in native table order.");
            s.Answer(c => Action(c) == "public-draft-gain"); s.At("public-draft-gain");
            s.Need(s.P!.PlayerSeat == second && s.V(first).HandCount == 5 && s.Root is { PublicHandDraft.Cursor: 1 } paid && paid.Id == rootId &&
                s.E<ProgramHandDraftCardObtainedEvent>().Where(e => e.FrameId == rootId).Select(e => e.RecipientSeat).SequenceEqual([first]),
                "One recipient has already obtained the real first shown card and no longer has low hand count; the same paid frame keeps the original second recipient.");
            s.Cold(); s.Answer(c => Action(c) == "public-draft-gain"); s.Play(1);
            s.Need(s.E<ProgramHandDraftCardObtainedEvent>().Where(e => e.FrameId == rootId).Select(e => e.RecipientSeat).SequenceEqual([first, second]) &&
                s.V(0).HandCount == 4 && s.V(1).HandCount == 5 && s.V(2).HandCount == 5 &&
                s.G.CardMovements.Count(m => m.Reason.Value == $"skill-program.{s.Skill}.population-draw" && m.To == CardLocation.Hand(0)) == 2 &&
                s.G.CardMovements.Count(m => m.Reason.Value == $"skill-program.{s.Skill}.public-draft" && m.From == CardLocation.Hand(0)) == 2,
                "Cold recovery never redraws or redistributes a paid entity: each frozen beneficiary receives exactly one original revealed hand card under either start policy.");
        }
    }
    private static string? Action(PromptChoice c) => c.Parameters.GetValueOrDefault("program-action");
    private static string Peer(int seat) => "fixture:mobile-cao-jie-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry, bool mobile)
    {
        public GameEngine G = game;
        public string Skill => mobile ? "mobile:huimin" : "classic:huimin";
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.SkillId == Skill && f.PublicHandDraft is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void At(string action) => Reach(() => P?.Choices.Any(c => Action(c) == action) == true);
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual Cao Jie draft command."); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (P is { } p)
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded actual Cao Jie draft boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))) && G.CardMovements.SequenceEqual(copy.CardMovements),
                "Cold replay retains the original paid draft, its frozen beneficiary roster, shown entities and current native recipient."); G = copy;
        }
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { P, Current = G.State.CurrentSeat, Turn = G.State.TurnNumber })); }
        public static Scene Start(bool mobile)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r), r, mobile);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture(bool mobile) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-cao-jie", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:cao-jie", "classic:cao-jie");
            b.AddGeneral(new(Owner, "真实惠民起点策略", "supporter", mobile ? "mobile:huimin" : "classic:huimin", "qun", 3));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "冻结受益者与非受益者", "supporter", "standard:none", "wei", seat < 3 ? 5 : 4));
            const string deck = "fixture:mobile-cao-jie-deck";
            b.AddDeck(new(deck, "固定真实闪实体", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            b.AddMode(new(Mode, "真实惠民已付款原帧", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
