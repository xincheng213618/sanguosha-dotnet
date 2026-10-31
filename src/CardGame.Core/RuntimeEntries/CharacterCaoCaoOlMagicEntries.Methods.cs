#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoOlMagicEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeMoCaoCaoBaibi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().IncludeMoCaoCaoBaibi(owner, window, candidates);
    internal bool MoCaoCaoAdvanceNativeChildren() => _resolve().MoCaoCaoAdvanceNativeChildren();
    internal bool MoCaoCaoBeforeDamageWindow(long id) => _resolve().MoCaoCaoBeforeDamageWindow(id);
    internal bool MoCaoCaoDamageWindow(long id) => _resolve().MoCaoCaoDamageWindow(id);
    internal bool MoCaoCaoDying() => _resolve().MoCaoCaoDying();
    internal string? MoCaoCaoEquipmentDescription(int seat) => _resolve().MoCaoCaoEquipmentDescription(seat);
    internal global::CardGame.Core.LegalAction? MoCaoCaoEquipmentPhysicalBan(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().MoCaoCaoEquipmentPhysicalBan(q);
    internal bool MoCaoCaoEquipmentResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().MoCaoCaoEquipmentResponseBan(q);
    internal void MoCaoCaoEquipmentSubmitted(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().MoCaoCaoEquipmentSubmitted(q);
    internal bool MoCaoCaoEquipmentViewAsBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().MoCaoCaoEquipmentViewAsBan(q);
    internal bool MoCaoCaoFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MoCaoCaoFirstNativeChild(f, child);
    internal bool MoCaoCaoHandDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().MoCaoCaoHandDistance(q);
    internal bool MoCaoCaoHandMaterialPrivilege(global::CardGame.Core.ProgramNativeUseMaterialQuery q) => _resolve().MoCaoCaoHandMaterialPrivilege(q);
    internal bool MoCaoCaoHandSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().MoCaoCaoHandSlashDistance(q);
    internal bool MoCaoCaoHandSlashQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().MoCaoCaoHandSlashQuota(q);
    internal bool MoCaoCaoHasHandPrivilege(int seat) => _resolve().MoCaoCaoHasHandPrivilege(seat);
    internal bool MoCaoCaoIssuedHandUse(long id, int actor) => _resolve().MoCaoCaoIssuedHandUse(id, actor);
    internal bool MoCaoCaoMovementReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MoCaoCaoMovementReturn(f, effect, p);
    internal bool MoCaoCaoNativeEdge(int i) => _resolve().MoCaoCaoNativeEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? MoCaoCaoNativeRoot() => _resolve().MoCaoCaoNativeRoot();
    internal bool MoCaoCaoNativeSuffix(int n) => _resolve().MoCaoCaoNativeSuffix(n);
    internal bool MoCaoCaoPaidMovement(global::CardGame.Core.ProgramSkillFrame f, scoped out int card, scoped out int payer, scoped out long before, scoped out long after, scoped out string reason) => _resolve().MoCaoCaoPaidMovement(f, out card, out payer, out before, out after, out reason);
    internal int MoCaoCaoQiBaoDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MoCaoCaoQiBaoDamage(attack);
    internal bool MoCaoCaoRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MoCaoCaoRecoveryParent(f, recovery);
    internal global::CardGame.Core.CardKind? MoCaoCaoVirtualEquipment(int seat, global::CardGame.Core.EquipmentSlot slot) => _resolve().MoCaoCaoVirtualEquipment(seat, slot);
    internal void MoCaoCaoVirtualWeaponRange(global::CardGame.Core.ProgramAttackRangeQuery q, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().MoCaoCaoVirtualWeaponRange(q, terms);
    internal global::CardGame.Core.MoCaoCaoBachaoRuntime MoCaoCaoBachao { get => _resolve().MoCaoCaoBachao; }
    internal global::CardGame.Core.MoCaoCaoFuzaiRuntime MoCaoCaoEquipment { get => _resolve().MoCaoCaoEquipment; }
}
