namespace CardGame.Core;
public sealed partial class GameEngine
{
    private IPendingGameEventOutput? _pendingGameEventOutput;
    private IPendingGameEventOutput PendingEventOutput => _pendingGameEventOutput ??= new PendingGameEventOutput(() => _started, payload => this.PublicPlayedCardsEntries.CapturePublicPlayedCard(payload), payload => this.ProgramEventHistoryEntries.AppendPendingProgramEvent(payload), () => _pendingEvents[^1], () => checked(_eventSequence + _pendingEvents.Count), (payload, sequence) => this.SimaYiMouEntries.CaptureMouSmSecretFact(payload, sequence));
}
