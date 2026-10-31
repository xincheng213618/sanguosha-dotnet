#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRicePileReceiptsEntries
{
    internal bool IsRiceMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsRiceMovement(f, effect, p);
    internal void RequireRiceReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireRiceReceipt(f);
}
