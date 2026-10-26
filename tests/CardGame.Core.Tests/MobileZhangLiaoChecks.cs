using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhangLiaoChecks
{
    private const string Skill = "mobile:tuxi", PeerHand = "fixture:mobile-zhang-liao-peer-hand";
    private const string Mode = "identity:classic-mobile-zhang-liao";

    public static void ZeroTargetsReplaceNormalDrawEvenWithoutOtherHandsCold()
    {
        foreach (var noOtherHands in new[] { false, true })
        {
            var s = Scene.Start(noOtherHands);
            s.Need(s.V(0).HandCount == 4 && Enumerable.Range(1, 3).All(i => s.V(i).HandCount == (noOtherHands ? 0 : 4)),
                "The real native opening has the intended owned entities and genuine empty/nonempty other hands before the replacement.");
            var hands = Enumerable.Range(0, 4).Select(i => s.V(i).Hand.Select(c => c.Id).ToArray()).ToArray();
            var moves = s.G.CardMovements.Count; var draws = s.E<CardsDrawnEvent>().Length;
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets") == true);
            var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
            var choices = s.P!.Choices;
            s.Need(root.WindowContext is { Window: SkillProgramTriggerWindow.DrawPhaseStarting, ParentFrameId: var parent } &&
                s.G.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(f => f.Id == parent && f.Window == SkillProgramTriggerWindow.DrawPhaseStarting && !f.NormalDrawReplaced) &&
                root.SelectedTargetSeats.Count == 0 && choices.Any(c => c.Targets.Count == 0) &&
                choices.All(c => c.Cards.Count == 0 && c.Parameters.GetValueOrDefault("minimum-targets") == "0" && c.Parameters.GetValueOrDefault("maximum-targets") == "2") &&
                (noOtherHands ? choices.Count == 1 : choices.Any(c => c.Targets.Count == 2)),
                "Activating Tuxi retains the exact native replacement frame and publishes the real zero-target selection; it stays legal when every other hand is empty.");
            if (noOtherHands) s.Cold();
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets" && c.Targets.Count == 0);
            s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            s.Need(Enumerable.Range(0, 4).All(i => s.V(i).Hand.Select(c => c.Id).SequenceEqual(hands[i])) &&
                s.G.CardMovements.Count == moves && s.E<CardsDrawnEvent>().Length == draws &&
                s.E<ProgramRandomHandCardsTakenEvent>().Where(e => e.SkillId == Skill).ToArray() is [{ OwnerSeat: 0, CardCount: 0, TargetSeats.Count: 0 }] &&
                s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed) == 1 &&
                !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Skill),
                "The committed empty set consumes its original replacement exactly once: no ordinary draw, no gain or material movement, unchanged real hands and a completed native phase return rather than cancellation.");
        }
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, bool noOtherHands)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected genuine ordinary mobile Zhang Liao command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing published original Tuxi choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 120 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, General(p.PlayerSeat), G.Revision, p.PromptId)); continue; }
                Need(P is null, "Unexpected ordinary Zhang Liao boundary while seeking the original draw replacement.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded original Zhang Liao draw replacement.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) && G.CardMovements.SequenceEqual(copy.CardMovements) &&
                G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
                "Cold preserves the actual empty target choice, original draw replacement parent and all native hand entities."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { NoOtherHands = noOtherHands, G.State.TurnNumber, G.State.CurrentSeat,
                Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId, Choices = p.Choices.Take(7).Select(c => new { c.Targets, c.Parameters }).ToArray(), Count = p.Choices.Count } : null,
                Hands = Enumerable.Range(0, 4).Select(i => sHand(i)).ToArray(),
                Facts = G.Events.Select(e => e.Payload).Where(e => e is ProgramBindingStartedEvent or ProgramBindingResolvedEvent or ProgramRandomHandCardsTakenEvent or CardsDrawnEvent)
                    .TakeLast(7).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray() }));
            object sHand(int i) => new { Seat = i, V(i).HandCount };
        }
        public static Scene Start(bool noOtherHands)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(noOtherHands));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry), registry, noOtherHands);
            s.Accept(new StartGameCommand()); s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-zhang-liao-general-" + seat;
    private sealed class Fixture(bool noOtherHands) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-zhang-liao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:zhang-liao");
            if (noOtherHands)
            {
                var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                    skills = new[] { new { id = PeerHand, revision = 1, modifiers = new[] { new { id = "actual-initial-hand", query = "initialHandSize", operation = "add", value = -4, priority = 0 } } } } });
                var labels = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [PeerHand] = new { name = "实际其他人空手", description = "原生初始4张减4张。" } } });
                var c = SkillProgramCatalog.Load(rules, labels);
                foreach (var (id, program) in c.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = c.Presentations[id] });
            }
            b.AddGeneral(new(General(0), "实际移动张辽", "supporter", Skill, "wei", 4));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(General(seat), "真实同桌", "supporter", noOtherHands ? PeerHand : "standard:none", "shu", 4));
            const string deck = "fixture:mobile-zhang-liao-deck";
            b.AddDeck(new(deck, "原生替代摸牌实体", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            b.AddMode(new(Mode, "普通移动张辽零目标", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 },
                deck, GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
