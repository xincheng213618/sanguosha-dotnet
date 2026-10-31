#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangFuOlEntries
{
    internal void BindSuyiEquipmentUse(CardUseFrame use) => _resolve().BindSuyiEquipmentUse(use);
    internal IReadOnlyList<IReadOnlyList<int>> BingzhongLegalTargets(CardUseFrame use) => _resolve().BingzhongLegalTargets(use);
    internal bool HasBingzhongTargetTail(CardUseFrame use) => _resolve().HasBingzhongTargetTail(use);
    internal bool HasBingzhongTargetTail2(CardUseFrame use, CardActionContext action) => _resolve().HasBingzhongTargetTail(use, action);
    internal bool IsSuyiEquipmentRide(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().IsSuyiEquipmentRide(ride, beneath);
    internal bool IsSuyiSlashUse(long id) => _resolve().IsSuyiSlashUse(id);
    internal void IssueSuyiSlashUse(long id, ProgramSkillFrame supplied, CardActionContext action) => _resolve().IssueSuyiSlashUse(id, supplied, action);
    internal void RecordBingzhongRedirect(ProgramSkillFrame frame, long useId, int previousSeat, int nextSeat) => _resolve().RecordBingzhongRedirect(frame, useId, previousSeat, nextSeat);
    internal bool ReturnSuyiAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnSuyiAttack(completion);
    internal bool TryGetSuyiVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> targets) => _resolve().TryGetSuyiVirtualPrimaryReturn(use, parent, out targets);
}
