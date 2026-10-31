#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuCheErMouEntries
{
    internal IEnumerable<MouHuCheErDebtEvent> ActiveMouHuCheErDebts(int target) => _resolve().ActiveMouHuCheErDebts(target);
    internal void BindMouHuCheErSlashUse(CardUseFrame supplied) => _resolve().BindMouHuCheErSlashUse(supplied);
    internal bool CanMouHuCheErSlash(ProgramSkillFrame f) => _resolve().CanMouHuCheErSlash(f);
    internal bool CanStartMouHuCheEr(CharacterState owner) => _resolve().CanStartMouHuCheEr(owner);
    internal long CaptureMouHuCheErEquipmentQualificationStamp(long original) => _resolve().CaptureMouHuCheErEquipmentQualificationStamp(original);
    internal void ContinueMouHuCheEr(long id) => _resolve().ContinueMouHuCheEr(id);
    internal bool FinishMouHuCheErAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouHuCheErAttack(attack, out paused);
    internal bool IsIssuedMouHuCheErUse(CardUseFrame use) => _resolve().IsIssuedMouHuCheErUse(use);
    internal bool IsMouHuCheErUse(long id) => _resolve().IsMouHuCheErUse(id);
    internal void IssueMouHuCheErSlash(ProgramSkillFrame f) => _resolve().IssueMouHuCheErSlash(f);
    internal IReadOnlyList<Card> KongwuEffectiveEquipment(CharacterState player) => _resolve().KongwuEffectiveEquipment(player);
    internal IReadOnlyList<Card> KongwuEffectiveEquipment2(int seat) => _resolve().KongwuEffectiveEquipment(seat);
    internal IReadOnlyList<Card> KongwuUsableWoodenOxGrain(CharacterState player) => _resolve().KongwuUsableWoodenOxGrain(player);
    internal bool MouHuCheErAdvanceChildren() => _resolve().MouHuCheErAdvanceChildren();
    internal int MouHuCheErDrawPenalty(CharacterState player) => _resolve().MouHuCheErDrawPenalty(player);
    internal bool MouHuCheErDying() => _resolve().MouHuCheErDying();
    internal bool MouHuCheErEdge(int i) => _resolve().MouHuCheErEdge(i);
    internal bool MouHuCheErEquipmentDisabled(int target) => _resolve().MouHuCheErEquipmentDisabled(target);
    internal bool MouHuCheErEquipmentGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().MouHuCheErEquipmentGrantQualified(owner, grant);
    internal bool MouHuCheErFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouHuCheErFirstChild(f, child);
    internal bool MouHuCheErMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => _resolve().MouHuCheErMovement(f, op, p);
    internal bool MouHuCheErRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouHuCheErRecoveryParent(f, recovery);
    internal Card MouHuCheErRepresentation(long id) => _resolve().MouHuCheErRepresentation(id);
    internal ProgramSkillFrame? MouHuCheErRoot() => _resolve().MouHuCheErRoot();
    internal bool MouHuCheErSuffix(int n) => _resolve().MouHuCheErSuffix(n);
    internal CardActionContext PrepareMouHuCheErSlashAction(CardActionContext action) => _resolve().PrepareMouHuCheErSlashAction(action);
    internal bool ReturnMouHuCheErAttack(GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouHuCheErAttack(c);
    internal void ReturnMouHuCheErNative(CardUseFrame use) => _resolve().ReturnMouHuCheErNative(use);
    internal void SettleMouHuCheErNative(MouHuCheErSlashOrigin o) => _resolve().SettleMouHuCheErNative(o);
    internal bool ValidMouHuCheEr(ProgramSkillFrame f) => _resolve().ValidMouHuCheEr(f);
    internal void ValidateMouHuCheErNative(CardUseFrame use) => _resolve().ValidateMouHuCheErNative(use);
    internal bool HasMouHuCheEr { get => _resolve().HasMouHuCheEr; }
    internal MouHuCheErRuntime MouHuCheErRuntime { get => _resolve().MouHuCheErRuntime; }
}
