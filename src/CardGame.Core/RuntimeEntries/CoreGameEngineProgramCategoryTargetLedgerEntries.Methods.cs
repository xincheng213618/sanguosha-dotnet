#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCategoryTargetLedgerEntries
{
    internal bool CanActivateCategoryTargetLedger(int ownerSeat, string skillId, string usageId, IReadOnlyList<int> cardIds, IReadOnlyList<int> targetSeats) => _resolve().CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, cardIds, targetSeats);
    internal void ConsumeProgramCategoryTargetLedger(ProgramSkillFrame program, string usageId) => _resolve().ConsumeProgramCategoryTargetLedger(program, usageId);
    internal int GetProgramCategoryTargetTurnUsage(ProgramSkillFrame frame) => _resolve().GetProgramCategoryTargetTurnUsage(frame);
    internal int GetProgramPhaseSkillUsage(ProgramSkillFrame frame, string usageId) => _resolve().GetProgramPhaseSkillUsage(frame, usageId);
}
