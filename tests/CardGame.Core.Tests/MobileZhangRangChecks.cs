using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhangRangChecks
{
    private const string Mobile = "mobile:taoluan", Classic = "classic:taoluan", Owner = "fixture:mobile-zhang-rang-owner",
        Probe = "fixture:mobile-zhang-rang-health", Mode = "identity:classic-mobile-zhang-rang", Deck = "fixture:mobile-zhang-rang-deck";

    public static void DeclineLosesHpBeforeDisablingAndReturnsOriginalFrameCold()
    {
        foreach (var mobile in new[] { false, true })
        {
            var s = Scene.Start(mobile); s.Play(); var initialHp = s.V(0).Hp;
            s.Need(initialHp > 1, "The order comparison begins above the actual native dying threshold.");
            var action = s.ConvertDrawTwo(); s.DeclineGift();
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Probe);
            var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == s.Skill);
            var hp = s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single();
            s.Need(s.V(0).Hp == initialHp - 1 && hp.Change is { Kind: HpChangeKind.Loss, TargetSeat: 0 } &&
                hp.Change.HpBefore == initialHp && hp.Change.HpAfter == initialHp - 1 &&
                hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && s.DisabledFacts().Length == (mobile ? 0 : 1) &&
                s.Facts<ProgramActionCategoryGiftResolvedEvent>().Single() is { GiftCardId: null, ProviderSeat: 1 } gift && gift.FrameId == root.Id,
                "The actual declined completion owns its real paid HP child: mobile is still enabled inside that child, while classic retains its earlier disable timing.");
            var frameId = root.Id; s.Cold(); s.Continue(); s.Play();
            s.Need(s.V(0).Hp == initialHp - 1 && s.DisabledFacts().Length == 1 &&
                !s.G.GetLegalActions(0).Any(a => a.ConversionSource?.SkillId == s.Skill) &&
                s.Facts<ProgramActionCategoryGiftResolvedEvent>().Count(e => e.FrameId == frameId) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == action.CardId && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1,
                "Cold resumes the original completion frame, disables once after the paid HP child returns, and repeats neither the gift decision nor the physical card payment nor HP loss.");
        }
    }

    public static void DeclinedLastHpEntersNativeDyingBeforeDisableAndDiesCold()
    {
        var s = Scene.Start(mobile: true, dying: true); s.Play(); s.PrepareLastHp();
        var action = s.ConvertDrawTwo(); s.DeclineGift();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Probe);
        var dying = s.G.ResolutionStack.OfType<DyingFrame>().Single();
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Mobile);
        s.Need(s.V(0).Hp == 0 && dying.VictimSeat == 0 && dying.ParentFrameId == root.Id && s.DisabledFacts().Length == 0 &&
            s.Facts<ProgramActionCategoryGiftResolvedEvent>().Single().GiftCardId is null,
            "The real last-HP refusal enters native dying from the original completion before the later disable instruction runs.");
        s.Cold(); s.Continue();
        s.Reach(() => !s.V(0).IsAlive && s.G.State.Winner != Winner.None);
        s.Need(s.Facts<PlayerDiedEvent>().Count(e => e.VictimSeat == 0) == 1 && s.DisabledFacts().Length == 0 &&
            s.Facts<ProgramActionCategoryGiftResolvedEvent>().Length == 1 &&
            s.G.CardMovements.Count(m => m.CardId == action.CardId && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1,
            "Native rescue refusal after cold performs one actual death; the dead owner's remaining tail is cancelled without another HP loss, card payment or disable fact.");
    }

    private static string Peer(int seat) => "fixture:mobile-zhang-rang-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry, string skill)
    {
        public GameEngine G = game;
        public string Skill => skill;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public ProgramBooleanStateChangedEvent[] DisabledFacts() => Facts<ProgramBooleanStateChangedEvent>().Where(e =>
            e.OwnerSeat == 0 && e.SkillId == skill && e.StateId == "taoluan-disabled" && e.Value).ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void PrepareLastHp()
        {
            // Ordinary identity setup adds the Lord's bonus to both maximum and initial HP.
            Need(V(0).Hp == 2, "A one-base-HP Lord really starts at two HP before native fixture preparation.");
            Accept(new UseProgramSkillCommand(0, Probe, "prepare-last-hp", [], [], G.Revision, P!.PromptId));
            Play();
            Need(V(0).Hp == 1 && !G.ResolutionStack.OfType<DyingFrame>().Any() && DisabledFacts().Length == 0,
                "One actual fixture HP loss establishes the last HP without entering dying or using Taoluan.");
        }
        public LegalAction ConvertDrawTwo()
        {
            var action = G.GetLegalActions(0).First(a => a.ConversionSource?.SkillId == skill && a.PlayedCardKind == CardKind.DrawTwo);
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId,
                action.PlayedCardKind, action.TargetCardId) { ConversionSource = action.ConversionSource });
            return action;
        }
        public void DeclineGift()
        {
            Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
            Answer(c => c.Targets.SequenceEqual([1]));
            Reach(() => P is { PlayerSeat: 1 } && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "different-action-category-decline"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "different-action-category-decline");
        }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> select)
        { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(select).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180 && !done(); step++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select)
                { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.Nullification })
                { Answer(c => c.Parameters.GetValueOrDefault("response") == "pass"); continue; }
                if (P is { Kind: DecisionKind.RescueDying })
                { Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die"); continue; }
                Need(P is null, "Unexpected mobile Zhang Rang native boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The fixed Zhang Rang completion did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold retains the original paid completion, private gift decision, deferred disable and typed HP/dying child.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(P)); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Zhang Rang command."); }
        public static Scene Start(bool mobile, bool dying = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile, dying));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6 }, r), r, mobile ? Mobile : Classic);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture(bool mobile, bool dying) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-zhang-rang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, mobile ? "mobile:zhang-rang" : "classic:zhang-rang");
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Probe}}","revision":1,
                 "activations":[{"id":"prepare-last-hp","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"usesPerPhase":1,
                  "effects":[{"op":"loseHp","target":"owner","amount":1}]}],
                 "triggers":[{"id":"real-paid-child","window":"{{(dying ? "dyingEntering" : "afterHpLost")}}","subject":"owner","optional":false,"isSkillInvocation":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Probe] = new { name = "真实失HP子窗", description = "原生付款后的观察或濒死窗口。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续原生结算" },
                        activationLabels = new Dictionary<string, string> { ["prepare-last-hp"] = "原生准备最后一点体力" } } } }));
            b.AddSkill(new(Probe, programs.Presentations[Probe].Name, programs.Presentations[Probe].Description)
                { Program = programs.Programs[Probe], ProgramPresentation = programs.Presentations[Probe] });
            b.AddGeneral(new(Owner, "真实张让来源", "supporter", mobile ? Mobile : Classic, "qun", dying ? 1 : 2, [Probe]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实拒绝索牌者", "supporter", "standard:none", "wei", 20));
            b.AddDeck(new(Deck, "固定真实装备实体", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "移动滔乱失HP与失效顺序", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
