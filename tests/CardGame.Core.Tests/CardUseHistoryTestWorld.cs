using System.Text.Json;
using CardGame.Core;

// The same expression evaluator as content, backed by the real journal index.
internal sealed class CardUseHistoryTestWorld(IActualCardUseHistoryQueries history, CardUseHistoryPoint point,
    int owner = 0, int? target = null) : IProgramExpressionWorld
{
    internal const string OtherDeclaredTarget = """["any",["cardUses","window","latest"],["and",["useDeclared","use"],["same",["useActor","use"],"owner"],["any",["useTargets","use"],["not",["same","it","owner"]]]]]""";
    internal const string EstablishedOtherTargets = """["filter",["targets",["filter",["cardUses","now","observations"],["and",["same",["useActor","use"],"owner"],["not",["useIsResponse","use"]],["useEstablished","use"]]]],["not",["same","it","owner"]]]""";
    internal const string DirectedCount = """["count",["cardUses","window","latest"],["and",["same",["useDeclarer","use"],["useDeclarer","currentUse"]],["or",["not",["useIsResponse","use"]],["useIsSelfNativeResponse","use"]],["contains",["useTargets","use"],"eventTarget"],["atOrBefore","use","currentUse","accepted"]]]""";

    public int? Participant(string name) => name switch { "owner" => owner, "turnOwner" => point.TurnOwnerSeat, "eventTarget" => target, _ => null };
    public IReadOnlyList<int>? ParticipantSet(string name) => null;
    public int? Stat(string stat, int seat) => null;
    public bool? Flag(string flag, int seat) => null;
    public int? PhaseCount(string moment, int actor, ActualDiscardRecoveryPhaseKind? kind, ActualPhaseHistoryOutcome outcome) =>
        (history as IActualPhaseHistoryQueries)?.CountPhases(point.ActualTurn, point.TurnOwnerSeat, actor, kind, outcome, point.Cursor);
    public IReadOnlyList<CardUseHistoryFact>? CardUses(string moment, string view) =>
        history.ReadCardUseFacts(point, Enum.Parse<CardUseHistoryView>(view, ignoreCase: true));
    public CardUseHistoryFact? CurrentUse() => CardUses("window", "latest")!.FirstOrDefault(f =>
        point.CurrentActionId is { } action ? f.ActionId == action : point.CurrentNativeUseId is { } id && f.NativeUseId == id);

    internal bool? Bool(string expression)
    { using var json = JsonDocument.Parse(expression); return ProgramExpressions.EvaluateBool(ProgramExpressions.Parse(json.RootElement, "test"), this); }
    internal int? Int(string expression)
    { using var json = JsonDocument.Parse(expression); return ProgramExpressions.EvaluateInt(ProgramExpressions.ParseInt(json.RootElement, "test"), this); }
    internal IReadOnlyList<int>? Seats(string expression)
    { using var json = JsonDocument.Parse(expression); return ProgramExpressions.EvaluateParticipantSet(ProgramExpressions.ParseParticipantSet(json.RootElement, "test"), this); }
}
