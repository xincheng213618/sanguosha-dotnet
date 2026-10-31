#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCategoryTargetLedgerEntries
{
    internal bool CanActivateCategoryTargetLedger(int ownerSeat, string skillId, string usageId, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => _resolve().CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, cardIds, targetSeats);
    internal void ConsumeProgramCategoryTargetLedger(global::CardGame.Core.ProgramSkillFrame program, string usageId) => _resolve().ConsumeProgramCategoryTargetLedger(program, usageId);
    internal int GetProgramCategoryTargetTurnUsage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GetProgramCategoryTargetTurnUsage(frame);
    internal int GetProgramPhaseSkillUsage(global::CardGame.Core.ProgramSkillFrame frame, string usageId) => _resolve().GetProgramPhaseSkillUsage(frame, usageId);
}
