namespace CardGame.Core;

public sealed record NativeCounterspellResponseCompletion(NullificationWindowFrame Window,
    CardActionContext Action, int ResponderSeat, int CardId, bool Invalidated);

internal interface INativeCounterspellResponseContinuations : IProgramRuntimeOperations
{
    NullificationWindowFrame Prepare(NullificationWindowFrame window, int responderSeat, int cardId);
    NullificationWindowFrame Bind(long windowId, CardActionContext acceptedAction);
    NativeCounterspellResponseCompletion? Complete(long windowId);
}

// Every real native producer enters this protocol before publishing its accepted action.
// The current accepted node supplies identity; completing a response never searches its notifications.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCounterspellResponseContinuationRuntime(IProgramRuntimeFrames frames,
    INativeCardActionInvalidations invalidations, IAppliedResponseRules responseRules,
    IMatchParticipantStateAccess participants, RuntimeEntries.CoreGameEngineTableOrderEntries table)
    : INativeCounterspellResponseContinuations
{
    public NullificationWindowFrame Prepare(NullificationWindowFrame supplied, int responderSeat, int cardId)
    {
        var window = Window(supplied.Id);
        if (window.NamedAmbushResponse is not null || responderSeat < 0 || responderSeat >= participants.Players.Count || cardId < 0)
            throw new InvalidOperationException("A counterspell response requires its actual unoccupied native chain node.");
        var next = window with
        {
            NamedAmbushResponse = new(0, responderSeat, cardId, window.EffectNullified, window.ChainDepth),
            ChainDepth = window.ChainDepth + 1,
            CandidateSeats = Candidates(responderSeat), CandidateIndex = 0,
            Step = ResolutionFrameStep.AwaitingResponse
        };
        frames.Replace(window.Id, next);
        return next;
    }

    public NullificationWindowFrame Bind(long windowId, CardActionContext acceptedAction)
    {
        var window = Window(windowId);
        if (window.NamedAmbushResponse is not { } response ||
            response.ActionId != 0 && response.ActionId != acceptedAction.ActionId ||
            response.PreviousChainDepth + 1 != window.ChainDepth || window.EffectNullified != response.PreviousEffectNullified ||
            acceptedAction.Type != CardActionType.Response || acceptedAction.EffectiveKind != CardKind.Nullification ||
            acceptedAction.ActionId <= 0 || acceptedAction.ActorSeat != response.ResponderSeat ||
            response.CardId != 0 && !acceptedAction.PhysicalCards.Any(cost => cost.CardId == response.CardId) ||
            response.CardId == 0 && acceptedAction.PhysicalCards.Count != 0)
            throw new InvalidOperationException("A counterspell response lost its actual accepted action, paid material identity or native depth.");
        // Capture validates the native action roles and parent, retaining the same accepted node.
        responseRules.CaptureCounterspell(window, acceptedAction);
        window = Window(windowId);
        if (response.ActionId == 0)
            frames.Replace(windowId, window = window with { NamedAmbushResponse = response with { ActionId = acceptedAction.ActionId } });
        return window;
    }

    public NativeCounterspellResponseCompletion? Complete(long windowId)
    {
        var window = Window(windowId);
        if (window.NamedAmbushResponse is not { } response) return null;
        if (frames.Top?.Id != window.Id || response.ActionId <= 0 ||
            window.ChainDepth != response.PreviousChainDepth + 1 || window.EffectNullified != response.PreviousEffectNullified ||
            window.ResponseRuleNode is not { } node || node.ChainDepth != window.ChainDepth || node.Action.ActionId != response.ActionId)
            throw new InvalidOperationException("A counterspell can complete only its original current accepted native node.");
        var ineffective = invalidations.IsCounterspellIneffective(window, node.Action);
        var completed = window with
        {
            NamedAmbushResponse = null, CounterspellInvalidation = null,
            EffectNullified = ineffective ? response.PreviousEffectNullified : !response.PreviousEffectNullified,
            CandidateSeats = Candidates(response.ResponderSeat), CandidateIndex = 0
        };
        frames.Replace(windowId, completed);
        return new(completed, node.Action, response.ResponderSeat, response.CardId, ineffective);
    }

    private NullificationWindowFrame Window(long id) => frames.Find(id) as NullificationWindowFrame ??
        throw new InvalidOperationException("The counterspell response lost its original native window.");

    private IReadOnlyList<int> Candidates(int responder) => Array.AsReadOnly(Enumerable.Range(1, participants.Players.Count)
        .Select(offset => table.TableStepSeat(responder, offset)).Where(seat => participants.At(seat).IsAlive).ToArray());
}
