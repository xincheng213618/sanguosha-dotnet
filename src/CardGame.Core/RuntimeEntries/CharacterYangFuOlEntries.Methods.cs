#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangFuOlEntries
{
    internal void BindSuyiEquipmentUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindSuyiEquipmentUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> BingzhongLegalTargets(global::CardGame.Core.CardUseFrame use) => _resolve().BingzhongLegalTargets(use);
    internal bool HasBingzhongTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasBingzhongTargetTail(use);
    internal bool HasBingzhongTargetTail2(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => _resolve().HasBingzhongTargetTail(use, action);
    internal bool IsSuyiEquipmentRide(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => _resolve().IsSuyiEquipmentRide(ride, beneath);
    internal bool IsSuyiSlashUse(long id) => _resolve().IsSuyiSlashUse(id);
    internal void IssueSuyiSlashUse(long id, global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.CardActionContext action) => _resolve().IssueSuyiSlashUse(id, supplied, action);
    internal void RecordBingzhongRedirect(global::CardGame.Core.ProgramSkillFrame frame, long useId, int previousSeat, int nextSeat) => _resolve().RecordBingzhongRedirect(frame, useId, previousSeat, nextSeat);
    internal bool ReturnSuyiAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnSuyiAttack(completion);
    internal bool TryGetSuyiVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryGetSuyiVirtualPrimaryReturn(use, parent, out targets);
}
