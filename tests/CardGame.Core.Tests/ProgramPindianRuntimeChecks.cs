using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ProgramPindianRuntimeChecks
{
    public static void NativeEntryPreservesEventSourcesAndExactPhaseFrames()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new ContestContent());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7 }, content);
        var capabilities = (ProgramCapabilityRegistry)typeof(GameEngine)
            .GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var originalTop = frames.Top;
        var zones = new CardZoneStore(2);
        zones.LoadInitialDeck([new(1, CardKind.Slash, Suit.Spade, 7), new(2, CardKind.Dodge, Suit.Heart, 3),
            new(3, CardKind.Slash, Suit.Club, 8)]);
        zones.Move(1, CardLocation.DrawPile, CardLocation.Hand(0));
        zones.Move(2, CardLocation.DrawPile, CardLocation.Hand(1));
        var participants = new Participants(zones);
        var turn = new Turn();
        var policies = new Policies();
        var producer = new Producer();
        var phaseFrames = new ProgramPhaseFrameQueries(frames, turn, turn, capabilities.Get<INativeFrameRoutingFacts>());
        var runtime = new ProgramPindianRuntime(frames, zones, content, participants, phaseFrames, policies, producer);
        var selected = new ProgramParticipantReference(ProgramParticipantRef.SelectedTarget);
        var eventTarget = new ProgramParticipantReference(ProgramParticipantRef.EventTarget);
        var active = new ProgramSkillFrame(frames.NextId(), 0, "ol:fenglve", "run",
            content.GetSkill("ol:fenglve").Program!.GameplayHash, 2, [], [1]);

        void Call(ProgramSkillFrame frame, ProgramParticipantReference reference, string bind, bool allowed)
        {
            var before = producer.Calls.Count;
            frames.Push(frame);
            try
            {
                if (!allowed)
                {
                    try { runtime.Start(frame, reference, bind, SkillProgramCardSetVisibility.Public); }
                    catch (InvalidOperationException)
                    {
                        Require(producer.Calls.Count == before, "Rejected entries must not issue a native contest.");
                        return;
                    }
                    throw new Exception("An invalid program Pindian entry was accepted.");
                }
                Require(runtime.Start(frame, reference, bind, SkillProgramCardSetVisibility.Public) == SkillProgramStepOutcome.AwaitChild &&
                        producer.Calls.Count == before + 1 && producer.Calls[^1] == (frame.Id, frame.OwnerSeat, 1, bind),
                    "A qualified entry must suspend its original program and issue the same native participants and result binding.");
            }
            finally { frames.Pop(frame.Id, frame.Kind); }
        }

        Call(active, selected, "contest", true);
        zones.Move(3, CardLocation.DrawPile, CardLocation.Processing);
        Call(active, selected, "contest", false);
        var damage = active with
        {
            TriggerId = "damage", SelectedTargetSeats = [],
            WindowContext = new(SkillProgramTriggerWindow.AfterDamageApplied, 91, 0, TargetSeat: 1)
        };
        Call(damage, eventTarget, "contest", true);
        Call(damage with { WindowContext = damage.WindowContext! with { TargetSeat = null } }, eventTarget, "contest", false);
        var bound = active with
        {
            Id = frames.NextId(),
            CardSetBindings = [new("paid", [3], SkillProgramCardSetVisibility.Public, [CardLocation.Processing])]
        };
        frames.Push(bound);
        try { Call(damage, eventTarget, "contest", false); }
        finally { frames.Pop(bound.Id, bound.Kind); }
        zones.Move(3, CardLocation.Processing, CardLocation.DiscardPile);
        Call(active with
        {
            PindianResultBindings = [new("contest", 0, 1, 7, 3, true, SkillProgramCardSetVisibility.Public)]
        }, selected, "contest", false);

        zones.Move(1, CardLocation.Hand(0), CardLocation.DiscardPile);
        Call(active, selected, "contest", false);
        policies.Alternative[0] = true;
        Call(active, selected, "contest", true);
        zones.Move(2, CardLocation.Hand(1), CardLocation.DiscardPile);
        Call(active, selected, "contest", false);
        policies.Alternative[1] = true;
        Call(active, selected, "contest", true);
        policies.Prohibited[1] = true;
        Call(active, selected, "contest", false);
        policies.Prohibited[1] = false;
        participants.Living[1] = false;
        Call(active, selected, "contest", false);
        participants.Living[1] = true;
        policies.Alternative[0] = policies.Alternative[1] = false;
        zones.Move(1, CardLocation.DiscardPile, CardLocation.Hand(0));
        zones.Move(2, CardLocation.DiscardPile, CardLocation.Hand(1));

        var skill = content.GetSkill("ol:shuomeng");
        var trigger = skill.Program!.Triggers.Single();
        var candidate = new ProgramTriggerCandidate(0, skill.Id, trigger.Id, "fixture-instance", skill.Program.GameplayHash, trigger.Priority);
        var parent = new ProgramLifecycleTriggerWindowFrame(frames.NextId(), 0, SkillProgramTriggerWindow.PlayEnding,
            [candidate], ProgramLifecycleContinuation.CompletePlayPhase, new(0, 4, true));
        var ending = new ProgramSkillFrame(frames.NextId(), 0, skill.Id, trigger.Id, skill.Program.GameplayHash, 2, [], [1])
        {
            TriggerId = trigger.Id, SkillInstanceId = candidate.SkillInstanceId,
            WindowContext = new(SkillProgramTriggerWindow.PlayEnding, parent.Id, 0)
        };
        frames.Push(parent);
        try
        {
            Call(ending, selected, "shuomeng-contest", true);
            Call(ending, selected, "different-result", false);
            Call(ending with { InstructionIndex = 1 }, selected, "shuomeng-contest", false);
            Call(ending with { ActivationId = "different-binding" }, selected, "shuomeng-contest", false);
            Call(ending with { WindowContext = ending.WindowContext! with { ParentFrameId = parent.Id + 100 } }, selected, "shuomeng-contest", false);
            turn.Phase = TurnPhase.Discard;
            Call(ending, selected, "shuomeng-contest", false);
            turn.Phase = TurnPhase.Play;
            var unrelated = new ProgramSkillFrame(frames.NextId(), 1, "fixture:unrelated", "run", "hash", 1, [], []);
            frames.Push(ending);
            frames.Push(unrelated);
            try
            {
                var before = producer.Calls.Count;
                try { runtime.Start(ending, selected, "shuomeng-contest", SkillProgramCardSetVisibility.Public); }
                catch (InvalidOperationException)
                {
                    Require(producer.Calls.Count == before, "A suspended phase instruction cannot issue a second contest.");
                    before = -1;
                }
                Require(before == -1, "Only the actual top program may use the own-PlayEnding exception.");
            }
            finally { frames.Pop(unrelated.Id, unrelated.Kind); frames.Pop(ending.Id, ending.Kind); }
        }
        finally { frames.Pop(parent.Id, parent.Kind); }

        turn.CurrentSeat = turn.TurnOwnerSeat = 1;
        var foreignContext = new ProgramSkillWindowContext(SkillProgramTriggerWindow.OtherActualTurnStarted, frames.NextId(), 0,
            SourceSeat: 1, TargetSeat: 1);
        var foreignParent = new ForeignActualTurnStartWindowFrame(foreignContext.ParentFrameId, turn.TurnNumber, 1,
            [candidate], [foreignContext]);
        var foreign = ending with { WindowContext = foreignContext };
        frames.Push(foreignParent);
        try
        {
            Call(foreign, selected, "foreign-contest", true);
            frames.ReplaceTop(foreignParent with { ActualTurnNumber = turn.TurnNumber - 1 });
            Call(foreign, selected, "foreign-contest", false);
        }
        finally { frames.Pop(foreignParent.Id, foreignParent.Kind); }
        Require(ReferenceEquals(frames.Top, originalTop), "The shared entry check must restore the original native frame owner.");
        Require(ReferenceEquals(capabilities.Get<IProgramMovementWindows>(), game.GetRuntimeModule<CardsMovedWindowRuntime>()) &&
                ReferenceEquals(capabilities.Get<IProgramMovementReturns>(), game.GetRuntimeModule<ProgramCompletionRuntime>()),
            "Character movement windows and returns must resolve the shared scheduler runtimes themselves.");
    }

    private sealed class ContestContent : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:program-pindian-runtime", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) =>
            CharacterFixtureContent.Register(builder, "ol:xun-chen", "ol:sun-qian");
    }

    private sealed class Producer : IProgramPindianProducer
    {
        internal List<(long FrameId, int Source, int Opponent, string Bind)> Calls { get; } = [];
        public void Begin(long programFrameId, SkillPromptPresentation presentation, int sourceSeat, int opponentSeat,
            string resultBind, SkillProgramCardSetVisibility visibility) => Calls.Add((programFrameId, sourceSeat, opponentSeat, resultBind));
    }

    private sealed class Policies : IProgramPindianPolicyQueries
    {
        internal bool[] Alternative { get; } = new bool[2];
        internal bool[] Prohibited { get; } = new bool[2];
        public bool ProhibitsPindianTarget(int seat) => Prohibited[seat];
        public bool CanDiscardForPindian(int seat) => Alternative[seat];
    }

    private sealed class Turn : IProgramTurnQueries, IProgramPhaseQueries
    {
        public int TurnNumber => 12;
        public int CurrentSeat { get; set; }
        public int TurnOwnerSeat { get; set; }
        public int TurnOwnerTurn => TurnNumber;
        public TurnPhase Phase { get; set; } = TurnPhase.Play;
    }

    private sealed class Participants(ICardZoneQueries zones) : ISelectionParticipantQueries
    {
        internal bool[] Living { get; } = [true, true];
        public IReadOnlyList<int> Seats => new[] { 0, 1 };
        public bool IsValidSeat(int seat) => seat is 0 or 1;
        public bool IsAlive(int seat) => Living[seat];
        public int HandCount(int seat) => zones.CardsAt(CardLocation.Hand(seat)).Count;
        public int EquipmentCount(int seat) => 0;
        public int Hp(int seat) => 4;
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new Exception(message); }
}
