#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuCheErMouEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouHuCheErDebtEvent> ActiveMouHuCheErDebts(int target) => _resolve().ActiveMouHuCheErDebts(target);
    internal void BindMouHuCheErSlashUse(global::CardGame.Core.CardUseFrame supplied) => _resolve().BindMouHuCheErSlashUse(supplied);
    internal bool CanMouHuCheErSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanMouHuCheErSlash(f);
    internal bool CanStartMouHuCheEr(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouHuCheEr(owner);
    internal long CaptureMouHuCheErEquipmentQualificationStamp(long original) => _resolve().CaptureMouHuCheErEquipmentQualificationStamp(original);
    internal void ContinueMouHuCheEr(long id) => _resolve().ContinueMouHuCheEr(id);
    internal bool FinishMouHuCheErAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouHuCheErAttack(attack, out paused);
    internal bool IsIssuedMouHuCheErUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsIssuedMouHuCheErUse(use);
    internal bool IsMouHuCheErUse(long id) => _resolve().IsMouHuCheErUse(id);
    internal void IssueMouHuCheErSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouHuCheErSlash(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player) => _resolve().KongwuEffectiveEquipment(player);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment2(int seat) => _resolve().KongwuEffectiveEquipment(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuUsableWoodenOxGrain(global::CardGame.Core.CharacterState player) => _resolve().KongwuUsableWoodenOxGrain(player);
    internal bool MouHuCheErAdvanceChildren() => _resolve().MouHuCheErAdvanceChildren();
    internal int MouHuCheErDrawPenalty(global::CardGame.Core.CharacterState player) => _resolve().MouHuCheErDrawPenalty(player);
    internal bool MouHuCheErDying() => _resolve().MouHuCheErDying();
    internal bool MouHuCheErEdge(int i) => _resolve().MouHuCheErEdge(i);
    internal bool MouHuCheErEquipmentDisabled(int target) => _resolve().MouHuCheErEquipmentDisabled(target);
    internal bool MouHuCheErEquipmentGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().MouHuCheErEquipmentGrantQualified(owner, grant);
    internal bool MouHuCheErFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouHuCheErFirstChild(f, child);
    internal bool MouHuCheErMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouHuCheErMovement(f, op, p);
    internal bool MouHuCheErRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouHuCheErRecoveryParent(f, recovery);
    internal global::CardGame.Core.Card MouHuCheErRepresentation(long id) => _resolve().MouHuCheErRepresentation(id);
    internal global::CardGame.Core.ProgramSkillFrame? MouHuCheErRoot() => _resolve().MouHuCheErRoot();
    internal bool MouHuCheErSuffix(int n) => _resolve().MouHuCheErSuffix(n);
    internal global::CardGame.Core.CardActionContext PrepareMouHuCheErSlashAction(global::CardGame.Core.CardActionContext action) => _resolve().PrepareMouHuCheErSlashAction(action);
    internal bool ReturnMouHuCheErAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouHuCheErAttack(c);
    internal void ReturnMouHuCheErNative(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouHuCheErNative(use);
    internal void SettleMouHuCheErNative(global::CardGame.Core.MouHuCheErSlashOrigin o) => _resolve().SettleMouHuCheErNative(o);
    internal bool ValidMouHuCheEr(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouHuCheEr(f);
    internal void ValidateMouHuCheErNative(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMouHuCheErNative(use);
    internal bool HasMouHuCheEr { get => _resolve().HasMouHuCheEr; }
    internal global::CardGame.Core.MouHuCheErRuntime MouHuCheErRuntime { get => _resolve().MouHuCheErRuntime; }
}
