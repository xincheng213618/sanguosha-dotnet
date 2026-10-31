using System.Reflection;
using CardGame.Core;

internal static class ProgramTriggerProofChecks
{
    public static void StagedNativeDecisionsPreserveShortCircuit()
    {
        _ = ProgramTriggerProofCatalog.Default;
        var methods = typeof(Host).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        var catalog = ProgramTriggerProofCatalog.Discover(methods.Reverse());
        var candidate = new ProgramTriggerCandidate(2, "fixture:native", "binding", "instance", "hash", 0);
        var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.DyingExited, 13, 2);
        var request = new CandidateTriggerProofRequest(candidate, context);
        var host = new Host { Candidate = candidate, Context = context };
        var bound = new List<string>();
        var runtime = new ProgramTriggerProofRuntime(catalog, method => { bound.Add(method.Name); return host; });
        Require(catalog.Rules<CandidateTriggerProofRequest>(ProgramTriggerProofPhase.ForeignDraw)
            .Select(rule => rule.Order).SequenceEqual([10, 20, 30]), "Proof order must not depend on reflection order.");
        Require(runtime.Decide(ProgramTriggerProofPhase.OwnerException, request) is null && host.Calls.SequenceEqual(["owner"]),
            "A declined owner exception must not borrow an instance exception.");
        host.Calls.Clear();
        Require(runtime.Decide(ProgramTriggerProofPhase.InstanceException, request) == true && host.Calls.SequenceEqual(["instance"]),
            "An instance exception belongs only to its original admission stage.");
        host.Calls.Clear();
        Require(runtime.Decide(ProgramTriggerProofPhase.ForeignDraw, request) == true && host.Calls.SequenceEqual(["draw-first", "draw-match"]),
            "OR proofs must continue on false and stop on their first true.");
        foreach (var phase in new[] { ProgramTriggerProofPhase.HpChangeOwner, ProgramTriggerProofPhase.MovementOwner })
        {
            host.Calls.Clear(); host.Terminal = false;
            var decision = DecideOwner(phase);
            Require(decision == false && host.Calls.SequenceEqual(["qualified"]),
                "A matching HP/movement family must terminate on false before another family or fallback.");
            host.Calls.Clear(); host.Terminal = null;
            Require(DecideOwner(phase) == true && host.Calls.SequenceEqual(["qualified", "fallback-family"]),
                "Only an unmatched HP/movement family may continue.");
        }
        host.Calls.Clear(); bound.Clear();
        Require(runtime.Decide(ProgramTriggerProofPhase.NativeWindow,
                new NativeWindowTriggerProofRequest(null!, candidate, context, null!)) == true &&
            host.Calls.SequenceEqual(["dying-first", "dying-match"]) && bound.SequenceEqual(["DyingFirst", "DyingMatch"]),
            "Native windows must skip unrelated owner initialization and retain their OR ordering.");
        host.Calls.Clear();
        Require(runtime.Decide(ProgramTriggerProofPhase.LegacyWindow, request) == false && host.Calls.SequenceEqual(["legacy"]),
            "An existing legacy window's false decision must remain terminal.");
        try
        {
            runtime.Decide(ProgramTriggerProofPhase.HpChangeOwner, request);
            throw new Exception("A candidate request was admitted to an HP proof stage.");
        }
        catch (InvalidOperationException error) when (error.Message.Contains("requires", StringComparison.Ordinal)) { }
        try
        {
            ProgramTriggerProofCatalog.Discover(typeof(WrongStage).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance));
            throw new Exception("A stage-incompatible handler signature was accepted.");
        }
        catch (InvalidOperationException error) when (error.Message.Contains("invalid stage", StringComparison.Ordinal)) { }

        bool? DecideOwner(ProgramTriggerProofPhase phase) => phase == ProgramTriggerProofPhase.HpChangeOwner
            ? runtime.Decide(phase, new HpTriggerProofRequest(null!, null!, candidate, context, null!))
            : runtime.Decide(phase, new MovementTriggerProofRequest(null!, null!, null!, candidate, context, null!));
    }

    private static void Require(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Host
    {
        internal ProgramTriggerCandidate Candidate = null!;
        internal ProgramSkillWindowContext Context = null!;
        internal readonly List<string> Calls = [];
        internal bool? Terminal;

        [ProgramTriggerProof(ProgramTriggerProofPhase.OwnerException, 10)]
        private bool Owner(ProgramTriggerCandidate candidate) { Calls.Add("owner"); return false; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 10)]
        private bool Instance(ProgramTriggerCandidate candidate) { Calls.Add("instance"); return true; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.ForeignDraw, 10)]
        private bool DrawFirst(ProgramTriggerCandidate candidate) { Calls.Add("draw-first"); return false; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.ForeignDraw, 20)]
        private bool DrawMatch(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
        {
            Require(ReferenceEquals(candidate, Candidate) && ReferenceEquals(context, Context),
                "Proof dispatch must retain the exact original candidate and context.");
            Calls.Add("draw-match"); return true;
        }
        [ProgramTriggerProof(ProgramTriggerProofPhase.ForeignDraw, 30)]
        private bool DrawLate(ProgramTriggerCandidate candidate) => throw new InvalidOperationException("Late proof must be unreachable.");
        [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 10, ProgramTriggerProofMatch.Decision)]
        [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 10, ProgramTriggerProofMatch.Decision)]
        private bool? Qualified(ProgramTriggerCandidate candidate) { Calls.Add("qualified"); return Terminal; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 20)]
        [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 20)]
        private bool FallbackFamily(ProgramTriggerCandidate candidate) { Calls.Add("fallback-family"); return true; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 5, SkillProgramTriggerWindow.NativeDrawStarting)]
        private bool OtherNativeWindow(ProgramTriggerCandidate candidate) => throw new InvalidOperationException("Unrelated native owner must be unreachable.");
        [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 10, SkillProgramTriggerWindow.DyingExited)]
        private bool DyingFirst(ProgramSkillWindowContext context) { Calls.Add("dying-first"); return false; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 20, SkillProgramTriggerWindow.DyingExited)]
        private bool DyingMatch(ProgramSkillWindowContext context) { Calls.Add("dying-match"); return true; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.LegacyWindow, 10, ProgramTriggerProofMatch.Decision)]
        private bool? Legacy(ProgramSkillWindowContext context) { Calls.Add("legacy"); return false; }
        [ProgramTriggerProof(ProgramTriggerProofPhase.LegacyWindow, 20)]
        private bool OtherLegacy(ProgramSkillWindowContext context) => throw new InvalidOperationException("Late legacy proof must be unreachable.");
    }

    private sealed class WrongStage
    {
        [ProgramTriggerProof(ProgramTriggerProofPhase.OwnerException, 10)]
        private bool Wrong(HpTriggerProofRequest request) => true;
    }
}
