#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestSlashProducer(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineForeignTurnContestsRuntime>> getCoreGameEngineForeignTurnContestsRuntime) : IForeignTurnContestSlashProducer
{
    public bool CanIssue(int actorSeat, int targetSeat) => getCoreGameEngineForeignTurnContestsRuntime().Value.CanUseForeignTurnContestSlash(actorSeat, targetSeat);
    public ForeignTurnContestSlashIssue Capture(long programFrameId, int instructionIndex, CardConversionSource source, ForeignTurnPindianOrigin origin) => getCoreGameEngineForeignTurnContestsRuntime().Value.CaptureForeignTurnContestSlash(programFrameId, instructionIndex, source, origin);
    public void Commit(ForeignTurnContestSlashIssue issue) => getCoreGameEngineForeignTurnContestsRuntime().Value.CommitForeignTurnContestSlash(issue);
}
