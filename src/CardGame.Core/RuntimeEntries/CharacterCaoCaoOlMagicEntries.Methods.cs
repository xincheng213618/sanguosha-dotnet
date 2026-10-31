#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoOlMagicEntries
{
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMoCaoCaoBaibi(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates) => _resolve().IncludeMoCaoCaoBaibi(owner, window, candidates);
    internal bool MoCaoCaoAdvanceNativeChildren() => _resolve().MoCaoCaoAdvanceNativeChildren();
    internal bool MoCaoCaoDying() => _resolve().MoCaoCaoDying();
    internal string? MoCaoCaoEquipmentDescription(int seat) => _resolve().MoCaoCaoEquipmentDescription(seat);
    internal LegalAction? MoCaoCaoEquipmentPhysicalBan(ProgramLegalActionQuery q) => _resolve().MoCaoCaoEquipmentPhysicalBan(q);
    internal bool MoCaoCaoEquipmentResponseBan(ProgramNativeResponseQuery q) => _resolve().MoCaoCaoEquipmentResponseBan(q);
    internal void MoCaoCaoEquipmentSubmitted(ProgramSubmittedUseQuery q) => _resolve().MoCaoCaoEquipmentSubmitted(q);
    internal bool MoCaoCaoEquipmentViewAsBan(ProgramViewAsLegalityQuery q) => _resolve().MoCaoCaoEquipmentViewAsBan(q);
    internal bool MoCaoCaoFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MoCaoCaoFirstNativeChild(f, child);
    internal bool MoCaoCaoHandDistance(ProgramCardDistanceQuery q) => _resolve().MoCaoCaoHandDistance(q);
    internal bool MoCaoCaoHandMaterialPrivilege(ProgramNativeUseMaterialQuery q) => _resolve().MoCaoCaoHandMaterialPrivilege(q);
    internal bool MoCaoCaoHandSlashDistance(ProgramSlashDistanceQuery q) => _resolve().MoCaoCaoHandSlashDistance(q);
    internal bool MoCaoCaoHandSlashQuota(ProgramSlashQuotaQuery q) => _resolve().MoCaoCaoHandSlashQuota(q);
    internal bool MoCaoCaoHasHandPrivilege(int seat) => _resolve().MoCaoCaoHasHandPrivilege(seat);
    internal bool MoCaoCaoIssuedHandUse(long id, int actor) => _resolve().MoCaoCaoIssuedHandUse(id, actor);
    internal bool MoCaoCaoNativeEdge(int i) => _resolve().MoCaoCaoNativeEdge(i);
    internal ProgramSkillFrame? MoCaoCaoNativeRoot() => _resolve().MoCaoCaoNativeRoot();
    internal bool MoCaoCaoNativeSuffix(int n) => _resolve().MoCaoCaoNativeSuffix(n);
    internal bool MoCaoCaoPaidMovement(ProgramSkillFrame f, scoped out int card, scoped out int payer, scoped out long before, scoped out long after, scoped out string reason) => _resolve().MoCaoCaoPaidMovement(f, out card, out payer, out before, out after, out reason);
    internal int MoCaoCaoQiBaoDamage(GameEngine.IDamageAttempt attack) => _resolve().MoCaoCaoQiBaoDamage(attack);
    internal bool MoCaoCaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MoCaoCaoRecoveryParent(f, recovery);
    internal CardKind? MoCaoCaoVirtualEquipment(int seat, EquipmentSlot slot) => _resolve().MoCaoCaoVirtualEquipment(seat, slot);
    internal void MoCaoCaoVirtualWeaponRange(ProgramAttackRangeQuery q, List<RuleQueryBaseTerm> terms) => _resolve().MoCaoCaoVirtualWeaponRange(q, terms);
    internal MoCaoCaoBachaoRuntime MoCaoCaoBachao { get => _resolve().MoCaoCaoBachao; }
    internal MoCaoCaoFuzaiRuntime MoCaoCaoEquipment { get => _resolve().MoCaoCaoEquipment; }
}
