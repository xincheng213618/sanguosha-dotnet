using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileSunZiLiuFangChecks
{
    private const string Skill = "mobile:jiyu", Owner = "fixture:mobile-sun-zi-liu-fang-owner",
        Driver = "fixture:mobile-jiyu-extra-play", Hold = "fixture:mobile-jiyu-discard-child", Flip = "fixture:mobile-jiyu-turnover-child",
        Mode = "identity:classic-mobile-jiyu-fixture", Deck = "fixture:mobile-jiyu-deck";

    public static void ExtraQuotaUsesWholeTurnTargetsAndOriginalPaidDiscardCold()
    {
        var s = Scene.Start(spadeOnly: false); s.Play();
        var suit = s.V(1).Hand.First(c => c.Suit != Suit.Spade && s.V(0).Hand.Any(h => h.Suit != c.Suit)).Suit;
        var cost = s.V(1).Hand.First(c => c.Suit == suit).Id;
        s.Use(1); s.ChooseDiscard(1, cost); s.Hold();
        var start = s.Facts<MobileJiyuInvokedEvent>().Single();
        s.Need(!start.ExtraUse && start.Target == 1 && s.Zone(cost) == CardLocation.DiscardPile &&
            s.Facts<MobileJiyuCompletedEvent>().Length == 0 &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(1) && m.To == CardLocation.DiscardPile) == 1 &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == start.FrameId).CardSetBindings
                .Any(b => b.Name == "discard" && b.CardIds.SequenceEqual([cost]) && b.SourceLocations.SequenceEqual([CardLocation.Hand(1)])),
            "The base use commits its actual Play and target before a real target-owned hand discard; quota is not awarded while its paid native child is pending.");
        s.Cold(); s.Release(); s.Play();
        s.Need(s.Facts<MobileJiyuCompletedEvent>() is [{ EarnedExtraUse: true }] && s.Action() is { } action &&
            !action.SelectableTargetSeats!.Contains(1) && action.SelectableTargetSeats.Contains(2) &&
            s.G.GetHumanLegalActions().Where(a => a.CardId is not null && a.Kind != LegalActionKind.Recast)
                .All(a => s.V(0).Hand.FirstOrDefault(c => c.Id == a.CardId)?.Suit != suit),
            "A usable other-suit real hand card earns one extra use, the original suit is natively prohibited, and extra use cannot revisit the actual-turn target.");
        var second = s.V(2).Hand.FirstOrDefault(c => c.Suit == suit) ?? s.V(2).Hand.First(c => c.Suit != Suit.Spade);
        s.Use(2); s.ChooseDiscard(2, second.Id); s.Hold(); s.Release(); s.Play();
        s.Need(s.Facts<MobileJiyuInvokedEvent>() is [_, { Target: 2, ExtraUse: true }] &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DiscardPile) == 1,
            "The earned extra use selects a different real target and cold recovery never repeats the original payment.");
        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == start.Phase.TurnNumber && s.Action()!.SelectableTargetSeats!.Contains(1),
            "Normal Play following the inserted Play has its own base use and may select a previous actual-turn target.");
        var third = s.V(1).Hand.First(c => c.Suit != Suit.Spade);
        s.Use(1); s.ChooseDiscard(1, third.Id); s.Hold(); s.Release(); s.Play();
        var last = s.Facts<MobileJiyuInvokedEvent>().Last();
        s.Need(!last.ExtraUse && last.Phase != start.Phase && last.Phase.TurnNumber == start.Phase.TurnNumber &&
            (s.Action() is null || !s.Action()!.SelectableTargetSeats!.Contains(1) && !s.Action()!.SelectableTargetSeats!.Contains(2)),
            "Only base use resets with actual Play; any newly earned extra use still excludes targets chosen anywhere in this actual turn.");
        s.Cold();
    }

    public static void BaseUseNeedsNoUsableHandAndSpadeTurnsOverBeforeHpLoss()
    {
        var s = Scene.Start(spadeOnly: true); s.Play();
        s.Need(s.V(0).Hand.All(c => c.Kind == CardKind.Dodge) && s.Action() is not null,
            "The current mobile base use is available even when every actual hand card is an unusable ordinary Dodge.");
        var hp = s.V(1).Hp; var cost = s.V(1).Hand[0].Id;
        s.Use(1); s.ChooseDiscard(1, cost); s.Hold(); s.Release();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Flip);
        s.Need(s.V(0).IsFaceDown && s.V(1).Hp == hp && s.Facts<MobileJiyuCompletedEvent>().Length == 0,
            "The real spade payment first turns the owner over and waits for that native child before losing the target's HP or awarding quota.");
        s.Cold(); s.Release(); s.Play();
        s.Need(s.V(1).Hp == hp - 1 && s.Facts<MobileJiyuCompletedEvent>() is [{ EarnedExtraUse: false }] && s.Action() is null,
            "After the original turnover child returns, the target really loses one HP; no actually usable hand means no extra phase use.");
    }
    private static string Peer(int seat) => "fixture:mobile-jiyu-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public LegalAction? Action() => G.GetHumanLegalActions().SingleOrDefault(a => a.ProgramSkillId == Skill);
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Use(int target) => Accept(new UseProgramSkillCommand(0, Skill, "hand-discard-restriction", [], [target], G.Revision, P!.PromptId));
        public void ChooseDiscard(int seat, int card)
        {
            Reach(() => P is { } p && p.PlayerSeat == seat && p.SkillPrompt?.SkillId == Skill && p.Choices.Any(c => c.Cards.Contains(card)));
            Answer(c => c.Cards.SequenceEqual([card]));
        }
        public void Hold() => Reach(() => P?.SkillPrompt?.SkillId == MobileSunZiLiuFangChecks.Hold);
        public void Release() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p)
                { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer(p.PlayerSeat), G.Revision, p.PromptId)); continue; }
                Need(P is null, "Unexpected native Jiyu boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The fixed Jiyu scene missed its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var original = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(original == Dump(), "Cold retains the original paid native discard/turnover child, actual Play allowance and whole-turn target history exactly once.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, Hand = V(0).Hand })); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Jiyu command."); }
        public static Scene Start(bool spadeOnly)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(spadeOnly));
            var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture(bool spadeOnly) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-jiyu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:sun-zi-liu-fang", "classic:sun-zi-liu-fang");
            var c = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
            {"id":"{{Driver}}","revision":1,"triggers":[{"id":"extra-first-play","window":"turnStartBeforeNormalFlow","subject":"owner","usageScope":"game","usageLimit":1,"optional":false,
              "effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]},
            {"id":"{{Hold}}","revision":1,"triggers":[{"id":"paid-discard","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perOwnerBatch",
              "movementReasons":["skill-program.mobile:jiyu.MoveBoundCards"],"optional":false,"isSkillInvocation":false,
              "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
            {"id":"{{Flip}}","revision":1,"triggers":[{"id":"real-turnover","window":"characterTurnedOver","subject":"owner","optional":false,"isSkillInvocation":false,
              "effects":[{"op":"chooseOption","target":"owner","resultBind":"turned","options":[{"id":"continue"}]}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "实际出牌阶段", description = "原生插入阶段。" },
                    [Hold] = new { name = "实际弃牌子窗", description = "原生已付款子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                    [Flip] = new { name = "实际翻面子窗", description = "原生已翻面子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description)
                { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动讥谀完整持有者", "supporter", Skill, "wei", 20, [Driver, Hold, Flip]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实弃手牌角色", "supporter", "standard:none", "wei", 20, [Hold]));
            Suit[] suits = [Suit.Heart, Suit.Club, Suit.Diamond, Suit.Spade];
            b.AddDeck(new(Deck, "固定真实花色与可用牌", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 120)
                .Select(i => new ContentDeckPhysicalCard(spadeOnly ? "standard:dodge" : "standard:crossbow", spadeOnly ? Suit.Spade : suits[i % 4], 7)).ToArray() });
            b.AddMode(new(Mode, "移动讥谀阶段与实际回合", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
