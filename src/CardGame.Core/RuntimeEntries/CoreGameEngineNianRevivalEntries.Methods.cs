#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNianRevivalEntries
{
    internal bool AllowsNianRevival(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsNianRevival(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginNianRevival(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginNianRevival(input);
    internal bool CanCompleteNianRevival(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanCompleteNianRevival(f);
    internal void CompleteNianSegment(long id, int count) => _resolve().CompleteNianSegment(id, count);
    internal void ContinueNianRevival(global::CardGame.Core.ProgramSkillFrame input) => _resolve().ContinueNianRevival(input);
    internal bool DrainNianRevival(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainNianRevival(f);
    internal bool IsNianAlliedSeat(int seat) => _resolve().IsNianAlliedSeat(seat);
    internal bool IsNianRevivalMovementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsNianRevivalMovementBatch(f, batch);
    internal global::System.Collections.Generic.IReadOnlyList<int> NianDeadAllies(int ownerSeat) => _resolve().NianDeadAllies(ownerSeat);
    internal void RequireNianRevival(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireNianRevival(f);
    internal bool ResumeNianRevival(long id) => _resolve().ResumeNianRevival(id);
    internal void ReturnNianRevivalDraw(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnNianRevivalDraw(input, outcome);
    internal bool ReturnNianRevivalMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnNianRevivalMovement(f);
    internal int NianUsedRevives { get => _resolve().NianUsedRevives; }
}
