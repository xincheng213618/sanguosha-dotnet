using System.Reflection;
using System.Runtime.ExceptionServices;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class NativeCardEffectEpisodeRecoveryChecks
{
    private const string Bingcai = "mou-ol:bingcai", Driver = "fixture:legacy-alcohol-driver",
        Rescue = "fixture:legacy-alcohol-rescue", Hp = "fixture:legacy-alcohol-hp";
    private const BindingFlags Hidden = BindingFlags.Static | BindingFlags.NonPublic;

    public static void RealRecastRepeatsBoundSelfRecoveryWithoutAnotherUseOrRescue()
    {
        var s = Scene.Start();
        var turn = s.G.State.TurnNumber;
        s.Need(s.G.State.Players[0].Hp == 1 && !s.E<CardUseDeclaredEvent>().Any(e => e.SourceSeat == 0),
            "The verified small fixture reaches its first actual turn with one HP and no preceding Basic use.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "hurt-self", [], [], s.G.Revision, s.P!.PromptId));
        s.Option(Rescue, "flip-choice");
        var dyingId = s.G.ResolutionStack.OfType<DyingFrame>().Single().Id;
        var producerId = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Rescue).Id;
        var driverId = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver).Id;
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "flip");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Bingcai &&
            s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Bingcai && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "material"));
        var recast = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "material");
        var material = recast.Cards.Single();
        s.Cold();
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "material" && c.Cards.SequenceEqual([material]));
        s.Option(Hp, "hp-return");

        var paid = s.E<MouBingcaiRecastPaidEvent>().Single();
        var issued = s.E<NativeCardEffectEpisodeIssuedEvent>().Single(e => e.Episode.Issue.Source.SkillId == Bingcai);
        var useId = issued.UseFrameId;
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(u => u.Id == useId);
        s.Need(paid is { OwnerSeat: 0, MatchesDamage: true, IsTrick: false, PrintedKind: CardKind.Crossbow } &&
            paid.CardId == material && paid.From == CardLocation.Hand(0) &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == paid.From &&
                m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.RecastDiscard && m.Sequence == paid.MovementSequence) == 1 &&
            s.E<NativeRecastInvocationRecordedEvent>().Count(e => e.InvocationId == paid.RecastInvocationId &&
                e.ParentFrameId == paid.FrameId && e.PrimaryCount == 1 && e.ActualRecastCount == 1 && e.ActualDrawCount == 1) == 1 &&
            s.E<MouBingcaiDrawReturnedEvent>().Count(e => e.FrameId == paid.FrameId && e.ActualDrawCount == 1) == 1 &&
            s.E<MouBingcaiExtraEffectIssuedEvent>().Count(e => e.FrameId == paid.FrameId && e.UseId == useId) == 1,
            "The real committed Alcohol invokes Bingcai's original single-card recast, physical discard, native Draw1 and issued business receipt exactly once.");
        s.Need(issued.Episode.Issue is { IssuerFrameId: var issuer, InstructionIndex: 0,
                Source.SkillId: Bingcai, Spec: { Scope: NativeCardEffectEpisodeScope.WholeUse,
                    Replay: NativeCardEffectEpisodeReplay.BoundSelfRescueRecovery, Count: 1 } } && issuer == paid.FrameId &&
            use is { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, Action: not null,
                LegacyDyingAlcoholReturn.ProgramFrameId: var parent } && parent == producerId &&
            use.EffectEpisodes is { StartedOrdinal: 0, Episodes: [{ Started: 0, Completed: 0 }] } &&
            s.G.State.Players[0].Hp == 1 && s.E<RecoveryAppliedEvent>().Count(e => e.TargetSeat == 0) == 1 &&
            s.E<ProgramDyingRescueEvent>().Count(e => e.DyingFrameId == dyingId && e.CardId == 0 && e.RecoveredHp == 1) == 1 &&
            !s.E<CardUseFinishedEvent>().Any(e => e.ResolutionId == useId),
            "Bingcai declares the accepted legacy recovery at issuance, before its original HP child returns, and waits for the real typed self-rescue return.");
        s.Cold(); s.Continue();
        s.Option(Hp, "hp-return");
        s.Need(s.G.State.Players[0].Hp == 2 &&
            s.E<RecoveryAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 0 && e.Amount == 1) == 2 &&
            s.E<NativeCardEffectEpisodeStartedEvent>().Count(e => e.UseFrameId == useId && e.Episode.Id == issued.Episode.Id) == 1 &&
            s.E<ProgramDyingRescueEvent>().Count(e => e.DyingFrameId == dyingId) == 1 &&
            !s.E<ProgramOptionChosenEvent>().Any(e => e.FrameId == producerId && e.ResultBind == "rescue-tail"),
            "The actual bound episode performs a second native recovery while its HP child retains the same original use and paused rescue instruction, without recording another rescue.");
        s.Cold(); s.Continue();
        s.Option(Rescue, "rescue-tail");
        s.Need(!s.G.ResolutionStack.OfType<CardUseFrame>().Any(u => u.Id == useId) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == producerId).InstructionIndex == 4 &&
            s.G.ResolutionStack.OfType<DyingFrame>().Any(f => f.Id == dyingId),
            "The finished original Alcohol returns once to its real paused producer before that producer's own dying return.");
        s.Cold(); s.Continue(); s.Option(Driver, "driver-tail"); s.Cold(); s.Continue();
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        s.Need(s.G.State.TurnNumber == turn && s.G.State.Players[0] is { IsAlive: true, Hp: 2 } &&
            s.E<CardUseDeclaredEvent>().Count(e => e.ResolutionId == useId && e.CardKind == CardKind.Alcohol) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == useId) == 1 &&
            s.E<LegacyActualUseCompletionCapturedEvent>().Count(e => e.CardUseFrameId == useId) == 1 &&
            s.E<CardActionAcceptedEvent>().Count(e => e.Action.ActionId == use.Action!.ActionId) == 1 &&
            s.E<NativeCardEffectEpisodeCompletedEvent>().Count(e => e.UseFrameId == useId &&
                e.Episode.Id == issued.Episode.Id && e.Executed && e.Episode.Completed == 1) == 1 &&
            s.E<ProgramDyingRescueEvent>().Count(e => e.DyingFrameId == dyingId) == 1 &&
            s.E<DyingResolvedEvent>().Count(e => e.ResolutionId == dyingId && e.Survived) == 1 &&
            s.E<ProgramOptionChosenEvent>().Count(e => e.FrameId == producerId && e.ResultBind == "rescue-tail") == 1 &&
            s.E<ProgramOptionChosenEvent>().Count(e => e.FrameId == driverId && e.ResultBind == "driver-tail") == 1 &&
            !s.G.ResolutionStack.Any(f => f is DyingFrame or CardUseFrame or RecoveryReplacementFrame) &&
            !s.G.CardMovements.Any(m => m.CardId == 0),
            "Cold returns settle one shared episode, one original Alcohol/action, one rescue record and each real parent tail once, with no manufactured physical card or renewed payment.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The native recovery fixture command was rejected."); }
        public void Answer(Func<PromptChoice, bool> select)
        { var prompt = P!; Accept(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(select).Id, G.Revision)); }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Option(string skill, string bind) => Reach(() => P?.SkillPrompt?.SkillId == skill &&
            P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == bind));
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 180; step++)
            {
                if (ready()) return;
                Need(P is not { PlayerSeat: 0 }, "An unexpected native recovery choice interrupts the actual typed return.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The verified legacy fixture did not reach the next recovery boundary.");
        }
        public void Cold()
        {
            try { G = (GameEngine)typeof(ActualTurnLegacyAlcoholChecks).GetMethod("Cold", Hidden)!.Invoke(null, [G, registry])!; }
            catch (TargetInvocationException error) when (error.InnerException is { } cause)
            { ExceptionDispatchInfo.Capture(cause).Throw(); throw; }
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + System.Text.Json.JsonSerializer.Serialize(new { G.State, Prompt = P })); }
        public static Scene Start()
        {
            var type = typeof(ActualTurnLegacyAlcoholChecks).GetNestedType("Fixture", BindingFlags.NonPublic)!;
            var original = (IGameContentPackage)Activator.CreateInstance(type,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, [false], null)!;
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(original));
            var game = GameEngine.CreateStandard(new() { Seed = 7, PlayerCount = 4, HumanSeat = 0,
                HumanRole = Role.Renegade, ModeId = "identity:classic-legacy-alcohol-fixture", UseInteractiveSetup = true,
                AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false, MaxTurns = 8 }, registry);
            var s = new Scene(game, registry); s.Accept(new StartGameCommand());
            s.Reach(() => s.P is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
            s.Accept(new SelectGeneralCommand(0, "fixture:legacy-alcohol-owner", game.Revision, s.P!.PromptId));
            s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            return s;
        }
    }
    private sealed class Fixture(IGameContentPackage original) : IGameContentPackage
    {
        public PackageManifest Manifest => original.Manifest;
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = CharacterFixtureContent.EmbeddedProgramBundle("mou-huang-yueying-ol");
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = program, ProgramPresentation = catalog.Presentations[id] });
            original.Register(new Builder(builder));
        }
        private sealed class Builder(IContentRegistryBuilder target) : IContentRegistryBuilder
        {
            public void AddCard(ContentCardDefinition value) => target.AddCard(value);
            public void AddSkill(ContentSkillDefinition value) => target.AddSkill(value);
            public void AddGeneral(ContentGeneralDefinition value) => target.AddGeneral(value.Id == "fixture:legacy-alcohol-owner"
                ? value with { AdditionalSkillIds = (value.AdditionalSkillIds ?? []).Append(Bingcai).ToArray() } : value);
            public void AddDeck(ContentDeckRecipe value) => target.AddDeck(value);
            public void AddMode(ContentModeDefinition value) => target.AddMode(value);
            public void AddEquipmentContent(EquipmentContentPackage value) => target.AddEquipmentContent(value);
            public void AddModeFeatureFactory(IGameModeFeatureFactory value) => target.AddModeFeatureFactory(value);
            public bool HasModeFeatureFactory(string id) => target.HasModeFeatureFactory(id);
        }
    }
}
