#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangFuOlEntries
{
    internal void BindSuyiEquipmentUse(CardUseFrame use) => _resolve().BindSuyiEquipmentUse(use);
    internal IReadOnlyList<IReadOnlyList<int>> BingzhongLegalTargets(CardUseFrame use) => _resolve().BingzhongLegalTargets(use);
    internal bool IsSuyiEquipmentRide(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().IsSuyiEquipmentRide(ride, beneath);
    internal bool IsSuyiSlashUse(long id) => _resolve().IsSuyiSlashUse(id);
    internal void IssueSuyiSlashUse(long id, ProgramSkillFrame supplied, CardActionContext action) => _resolve().IssueSuyiSlashUse(id, supplied, action);
    internal bool ReturnSuyiAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnSuyiAttack(completion);
}
