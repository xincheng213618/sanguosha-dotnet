using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ModePresentationChecks
{

    public static void RevealCommandRetainsPromptOrderAndLazyNativeHost()
    {
        var host = new RevealHost();
        var resolves = 0;
        var feature = (IMatchModeCommandHandler)Feature(Mode("national:test", ContentModeKind.NationalWarLite),
            () => { resolves++; return host; });
        Require(resolves == 0 && feature.CommandTypes.SequenceEqual([typeof(RevealGeneralCommand)]),
            "Constructing a pure setup/presentation feature does not resolve its reveal host.");
        var command = new RevealGeneralCommand(0, GeneralSelectionSlot.Primary, 0, new PromptId(5));
        Require(feature.Submit(command).Accepted && resolves == 1 && host.Trace.SequenceEqual(["validate", "slots", "accept", "reveal:Primary:False"]),
            "A response-phase reveal uses native prompt ownership, exact legal slots and one original command-session action.");
        host.Trace.Clear(); host.Validation = new(CommandErrorCode.InvalidPrompt, "stale");
        Require(feature.Submit(command with { Slot = (GeneralSelectionSlot)999 }).Error?.Code == CommandErrorCode.InvalidPrompt &&
            host.Trace.SequenceEqual(["validate", "reject"]),
            "Prompt validation retains precedence over malformed slots and never opens a second payment or action boundary.");
        host.Trace.Clear(); host.Validation = null; host.Kind = DecisionKind.SelectGeneral;
        Require(feature.Submit(command).Error?.Code == CommandErrorCode.IllegalAction && host.Trace.SequenceEqual(["validate", "reject"]),
            "A general-selection prompt cannot become a revealable response merely because the mode supports reveal.");
        host.Trace.Clear(); host.Kind = DecisionKind.PlayCard;
        var ordinary = (IMatchModeCommandHandler)Feature(Mode("identity:test", ContentModeKind.Identity), () => host);
        Require(ordinary.Submit(command).Error?.Message == "General reveal is only available in the national-war lite mode." &&
            host.Trace.SequenceEqual(["validate", "reject"]),
            "Forbidden-mode rejection remains compatible while its qualification lives in the installed mode implementation.");
    }

    private static IMatchModePresentation Feature(ContentModeDefinition mode, Func<IModeNationalRevealHost>? reveal = null,
        GameOptions? options = null) =>
        (IMatchModePresentation)new StandardModeProfileFactory().Create(Context(mode, reveal, options));

    private static ModeFeatureCreationContext Context(ContentModeDefinition mode,
        Func<IModeNationalRevealHost>? reveal = null, GameOptions? options = null)
    {
        var hosts = new ModeHostRegistry().Register<IModeSetupRandom>(() => new NoRandom());
        if (reveal is not null) hosts.Register(reveal);
        return new ModeFeatureCreationContext(
            options ?? new GameOptions { PlayerCount = 8, AdvanceAfterHumanCommands = false }, mode, ContentRegistry.Build(), hosts);
    }
    private static ContentModeDefinition Mode(string id, ContentModeKind kind) => new(id, "测试", 8, 8,
        new Dictionary<string, int>(), ModeKind: kind,
        TeamCounts: kind == ContentModeKind.Team ? new Dictionary<string, int> { ["team:a"] = 4, ["team:b"] = 4 } : null);
    private sealed class NoRandom : IModeSetupRandom
    {
        public int Next(int maximum) => throw new InvalidOperationException("A presentation constructor cannot consume gameplay random.");
        public void Shuffle<T>(IList<T> values) => throw new InvalidOperationException("A presentation constructor cannot shuffle gameplay state.");
    }
    private sealed class RevealHost : IModeNationalRevealHost
    {
        internal List<string> Trace { get; } = [];
        internal DecisionKind Kind { get; set; } = DecisionKind.RespondDodge;
        internal CommandError? Validation { get; set; }
        public DecisionKind? PendingKind => Kind;
        public CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId, CommandErrorCode inactiveCode)
        { Trace.Add("validate"); return Validation; }
        public IReadOnlyList<GeneralSelectionSlot> LegalRevealSlots(int actorSeat) { Trace.Add("slots"); return [GeneralSelectionSlot.Primary]; }
        public CommandResult Accept(Action action) { Trace.Add("accept"); action(); return new(true, null, 0, null!); }
        public CommandResult Reject(CommandErrorCode code, string message) { Trace.Add("reject"); return new(false, new(code, message), 0, null!); }
        public void RevealHumanGeneral(GeneralSelectionSlot slot, bool advanceToHumanBoundary) => Trace.Add($"reveal:{slot}:{advanceToHumanBoundary}");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
