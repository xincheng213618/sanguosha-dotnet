#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedSourceUsesEntries
{
    internal void AssertRoundGainedUseQualifications(CardUseFrame use) => _resolve().AssertRoundGainedUseQualifications(use);
    internal bool CanContinueIssuedRoundGainedUse(ProgramSkillFrame frame) => _resolve().CanContinueIssuedRoundGainedUse(frame);
    internal void CollectIssuedRoundGainedUseCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedRoundGainedUseCandidates(action, window, result);
    internal bool DistanceBorrowedPaidWeaponSlash30HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash30HasRoundGainedBasicBonus(q);
    internal bool DistanceBorrowedPhysicalSlash40HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash40HasRoundGainedBasicBonus(q);
    internal bool DistanceBorrowedSlash70HasPotentialRoundGainedBasicMaterials(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash70HasPotentialRoundGainedBasicMaterials(q);
    internal bool DistancePhysicalSlash110HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash110HasRoundGainedBasicBonus(q);
    internal bool DistancePhysicalSlash90HasIssuedRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash90HasIssuedRoundGainedBasicBonus(q);
    internal bool DistanceVirtualSlash50HasRoundGainedBasicBonus(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash50HasRoundGainedBasicBonus(q);
    internal RoundGainedUseQualification? ExactIssuedRoundGainedUseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool paid = false) => _resolve().ExactIssuedRoundGainedUseCandidate(candidate, context, paid);
    internal bool HasExactRoundGainedCommittedEquipmentUse(CardUseFrame use, RoundGainedUseQualification q) => _resolve().HasExactRoundGainedCommittedEquipmentUse(use, q);
    internal bool HasIssuedRoundGainedBasicBonus(long useId, int actor) => _resolve().HasIssuedRoundGainedBasicBonus(useId, actor);
    internal bool HasIssuedRoundGainedUseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasIssuedRoundGainedUseCandidate(candidate, context);
    internal bool HasPotentialRoundGainedBasicMaterials(CharacterState owner, CardKind kind) => _resolve().HasPotentialRoundGainedBasicMaterials(owner, kind);
    internal bool HasRoundGainedBasicBonus(CharacterState owner, CardKind kind, IReadOnlyList<int> ids) => _resolve().HasRoundGainedBasicBonus(owner, kind, ids);
    internal bool InRoundGainedOwnPlay(CharacterState owner) => _resolve().InRoundGainedOwnPlay(owner);
    internal void IssueRoundGainedUseQualifications(long useId, CardActionContext? action) => _resolve().IssueRoundGainedUseQualifications(useId, action);
    internal void ObserveRoundGainedRoundBoundary(IGameEvent payload) => _resolve().ObserveRoundGainedRoundBoundary(payload);
    internal RoundGainedRoundBoundaryEvent? RoundGainedCurrentBoundary() => _resolve().RoundGainedCurrentBoundary();
    internal IReadOnlyList<RoundGainedMaterialAcquisition>? RoundGainedMaterials(CharacterState owner, string skillId, string policyId, IReadOnlyList<int> ids, RoundGainedRoundBoundaryEvent boundary) => _resolve().RoundGainedMaterials(owner, skillId, policyId, ids, boundary);
    internal bool RoundGainedSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().RoundGainedSlashQuotaAfterLimit(query);
    internal bool SameRoundGainedQualification(RoundGainedUseQualification a, RoundGainedUseQualification b) => _resolve().SameRoundGainedQualification(a, b);
    internal bool ValidRoundGainedQualification(CardUseFrame use, RoundGainedUseQualification q) => _resolve().ValidRoundGainedQualification(use, q);
    internal long RoundGainedMovementSequence { get => _resolve().RoundGainedMovementSequence; }
    internal bool TracksRoundGainedSourceUses { get => _resolve().TracksRoundGainedSourceUses; }
}
