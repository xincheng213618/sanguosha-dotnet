#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ProgramSeatAcquisitionOperations(Func<DeterministicRandom> getDeterministicRandom, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IProgramSeatAcquisitionOperations
{
    public int NextRandom(int count) => getDeterministicRandom().Next(count);
    public void Obtain(int id, int recipient, CardMoveReason reason, CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>> paid) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), reason, paid, disclosure: disclosure);
    public bool DrainChildren(ProgramSkillFrame f) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    public void TurnOver(ProgramSkillFrame f) => getCoreGameEngineProgramLifecycleRuntime().TurnOverProgramTarget(f.Id, f.OwnerSeat, f.OwnerSeat);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
}
