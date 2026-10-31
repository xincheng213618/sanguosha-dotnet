using System.Reflection;
using CardGame.Core;

internal static class ProgramIssuedTriggerChecks
{
    public static void OrderedDecisionsPreserveIssuedBoundary()
    {
        var methods = typeof(Host).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        var catalog = ProgramIssuedTriggerCatalog.Discover(methods.Reverse());
        Require(catalog.Rules.Select(rule => rule.Order).SequenceEqual([10, 20, 30, 40]),
            "Issued rule precedence must be independent of reflection order.");
        // Validate every production signature and precedence without a game fixture.
        _ = ProgramIssuedTriggerCatalog.Default;
        var candidate = new ProgramTriggerCandidate(2, "fixture:issued", "binding", "instance", "hash", 0);
        var context = new ProgramSkillWindowContext(SkillProgramTriggerWindow.WuLuKangPindian, 13, 2);
        var host = new Host { ExpectedCandidate = candidate, ExpectedContext = context };
        var runtime = new ProgramIssuedTriggerRuntime(catalog, _ => host);

        host.Allow = true;
        Require(runtime.Decide(candidate, context) == true && host.Calls.SequenceEqual([10]),
            "An earlier issued admission must bypass later vetoes and terminal rules.");
        host.Allow = false;
        host.Calls.Clear();
        Require(runtime.Decide(candidate, context) == false && host.Calls.SequenceEqual([10, 20]),
            "An explicit rejection must stop before later admissions.");
        host.Qualified = true;
        host.Calls.Clear();
        Require(runtime.Decide(candidate, context) == false && host.Calls.SequenceEqual([10, 20, 30]),
            "A false terminal-window decision must not fall through.");
        host.Terminal = null;
        host.Calls.Clear();
        Require(runtime.Decide(candidate, context) == true && host.Calls.SequenceEqual([10, 20, 30, 40]),
            "Only null may continue to the next issued rule.");
        var other = new Host { ExpectedCandidate = candidate, ExpectedContext = context, Qualified = true, Fail = true };
        try
        {
            new ProgramIssuedTriggerRuntime(catalog, _ => other).Decide(candidate, context);
            throw new Exception("The original issued-rule failure was swallowed.");
        }
        catch (InvalidOperationException error)
        {
            Require(ReferenceEquals(error, Host.Failure) && host.Calls.SequenceEqual([10, 20, 30, 40]),
                "Metadata must neither capture another match nor wrap a native rule failure.");
        }
        var declined = ProgramIssuedTriggerCatalog.Discover(methods.Where(method => method.Name == "First"));
        Require(new ProgramIssuedTriggerRuntime(declined, _ => host).Decide(candidate, context) is null,
            "Declined issued rules must leave ordinary trigger admission reachable.");
        var staticRule = typeof(StaticOwner).GetMethod("Decline", BindingFlags.Static | BindingFlags.NonPublic)!;
        Require(new ProgramIssuedTriggerRuntime(ProgramIssuedTriggerCatalog.Discover([staticRule]), _ => null)
                .Decide(candidate, context) is null,
            "A declared independent static owner may register an issued proof.");

        foreach (var names in new[]
                 {
                     new[] { "DuplicateA", "DuplicateB" }, new[] { "WrongReturn" },
                     new[] { "Ambiguous" }, new[] { "Negative" }, new[] { "Zero" },
                     new[] { "UnknownMatch" }, new[] { "Optional" }, new[] { "ByRef" }, new[] { "Output" }
                 })
        {
            try
            {
                ProgramIssuedTriggerCatalog.Discover(names.Select(name => typeof(InvalidHost)
                    .GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static)!));
            }
            catch (InvalidOperationException) { continue; }
            throw new Exception("An invalid issued-rule registration was accepted.");
        }
        try
        {
            ProgramIssuedTriggerCatalog.Discover([typeof(UnregisteredStaticOwner)
                .GetMethod("Decline", BindingFlags.NonPublic | BindingFlags.Static)!]);
        }
        catch (InvalidOperationException) { return; }
        throw new Exception("An undeclared static issued owner was accepted.");
    }

    private static void Require(bool value, string message)
    {
        if (!value) throw new InvalidOperationException(message);
    }

    private sealed class Host
    {
        internal static readonly InvalidOperationException Failure = new("original issued-rule failure");
        internal ProgramTriggerCandidate ExpectedCandidate = null!;
        internal ProgramSkillWindowContext ExpectedContext = null!;
        internal readonly List<int> Calls = [];
        internal bool Allow;
        internal bool Qualified;
        internal bool? Terminal = false;
        internal bool Fail;

        [ProgramIssuedTrigger(10)]
        private bool First(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
        {
            Require(ReferenceEquals(candidate, ExpectedCandidate) && ReferenceEquals(context, ExpectedContext),
                "Issued rules must receive the exact original candidate and window.");
            Calls.Add(10);
            return Allow;
        }

        [ProgramIssuedTrigger(20, ProgramIssuedTriggerMatch.RejectWhenFalse)]
        private bool Qualification(ProgramTriggerCandidate candidate)
        { Calls.Add(20); return Qualified; }

        [ProgramIssuedTrigger(30, ProgramIssuedTriggerMatch.Decision)]
        private bool? TerminalWindow(ProgramSkillWindowContext context)
        { Calls.Add(30); if (Fail) throw Failure; return Terminal; }

        [ProgramIssuedTrigger(40)]
        private bool Last(ProgramTriggerCandidate candidate)
        { Calls.Add(40); return true; }
    }

    [ProgramRuntimeModule]
    private static class StaticOwner
    {
        [ProgramIssuedTrigger(10)]
        private static bool Decline(ProgramTriggerCandidate candidate) => false;
    }

    private static class UnregisteredStaticOwner
    {
        [ProgramIssuedTrigger(10)]
        private static bool Decline(ProgramTriggerCandidate candidate) => false;
    }

    [ProgramRuntimeModule]
    private static class InvalidHost
    {
        [ProgramIssuedTrigger(10)]
        private static bool DuplicateA(ProgramTriggerCandidate candidate) => true;
        [ProgramIssuedTrigger(10)]
        private static bool DuplicateB(ProgramTriggerCandidate candidate) => true;
        [ProgramIssuedTrigger(10, ProgramIssuedTriggerMatch.Decision)]
        private static bool WrongReturn(ProgramTriggerCandidate candidate) => true;
        [ProgramIssuedTrigger(10)]
        private static bool Ambiguous(ProgramTriggerCandidate first, ProgramTriggerCandidate second) => true;
        [ProgramIssuedTrigger(-1)]
        private static bool Negative(ProgramTriggerCandidate candidate) => true;
        [ProgramIssuedTrigger(0)]
        private static bool Zero(ProgramTriggerCandidate candidate) => true;
        [ProgramIssuedTrigger(10, (ProgramIssuedTriggerMatch)int.MaxValue)]
        private static bool UnknownMatch(ProgramTriggerCandidate candidate) => true;
        [ProgramIssuedTrigger(10)]
        private static bool Optional(ProgramTriggerCandidate? candidate = null) => true;
        [ProgramIssuedTrigger(10)]
        private static bool ByRef(ref ProgramTriggerCandidate candidate) => true;
        [ProgramIssuedTrigger(10)]
        private static bool Output(out ProgramTriggerCandidate candidate) { candidate = null!; return true; }
    }
}
