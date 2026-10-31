#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ForeignTurnContestSlashProducer(Func<RuntimeEntries.CoreGameEngineForeignTurnContestsEntries> getCoreGameEngineForeignTurnContestsRuntime) : IForeignTurnContestSlashProducer
{
    public bool CanIssue(int actorSeat, int targetSeat) => getCoreGameEngineForeignTurnContestsRuntime().CanUseForeignTurnContestSlash(actorSeat, targetSeat);
    public ForeignTurnContestSlashIssue Capture(long programFrameId, int instructionIndex, CardConversionSource source, ForeignTurnPindianOrigin origin) => getCoreGameEngineForeignTurnContestsRuntime().CaptureForeignTurnContestSlash(programFrameId, instructionIndex, source, origin);
    public void Commit(ForeignTurnContestSlashIssue issue) => getCoreGameEngineForeignTurnContestsRuntime().CommitForeignTurnContestSlash(issue);
}
