#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;
using AcquisitionNode = CardGame.Core.ProgramStageNode<CardGame.Core.ProgramSeatAcquisitionCall, CardGame.Core.ProgramSeatAcquisitionReceipt, CardGame.Core.ProgramSeatAcquisitionStage>;

namespace CardGame.Core.NativeOwners;
internal sealed class ProgramSeatAcquisitionOperations(global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IProgramSeatAcquisitionOperations
{
    public int NextRandom(int count) => getDeterministicRandom().Next(count);
    public void Obtain(int id, int recipient, CardMoveReason reason, CardMovementDisclosure disclosure, Action<long, IReadOnlyList<CardMovementRecord>> paid) => getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), reason, paid, disclosure: disclosure);
    public bool DrainChildren(ProgramSkillFrame f) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    public void TurnOver(ProgramSkillFrame f) => getCoreGameEngineProgramLifecycleRuntime().TurnOverProgramTarget(f.Id, f.OwnerSeat, f.OwnerSeat);
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
}
