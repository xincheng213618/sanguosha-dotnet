#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNianRevivalEntries
{
    internal bool AllowsNianRevival(ProgramActivationLegalityContext gate) => _resolve().AllowsNianRevival(gate);
    internal SkillProgramStepOutcome BeginNianRevival(ProgramSkillFrame input) => _resolve().BeginNianRevival(input);
    internal bool CanCompleteNianRevival(ProgramSkillFrame f) => _resolve().CanCompleteNianRevival(f);
    internal void CompleteNianSegment(long id, int count) => _resolve().CompleteNianSegment(id, count);
    internal void ContinueNianRevival(ProgramSkillFrame input) => _resolve().ContinueNianRevival(input);
    internal bool DrainNianRevival(ProgramSkillFrame f) => _resolve().DrainNianRevival(f);
    internal bool IsNianAlliedSeat(int seat) => _resolve().IsNianAlliedSeat(seat);
    internal bool IsNianRevivalMovementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().IsNianRevivalMovementBatch(f, batch);
    internal IReadOnlyList<int> NianDeadAllies(int ownerSeat) => _resolve().NianDeadAllies(ownerSeat);
    internal void RequireNianRevival(ProgramSkillFrame f) => _resolve().RequireNianRevival(f);
    internal bool ResumeNianRevival(long id) => _resolve().ResumeNianRevival(id);
    internal void ReturnNianRevivalDraw(ProgramSkillFrame input, NativeDrawOutcome outcome) => _resolve().ReturnNianRevivalDraw(input, outcome);
    internal bool ReturnNianRevivalMovement(ProgramSkillFrame f) => _resolve().ReturnNianRevivalMovement(f);
    internal int NianUsedRevives { get => _resolve().NianUsedRevives; }
}
