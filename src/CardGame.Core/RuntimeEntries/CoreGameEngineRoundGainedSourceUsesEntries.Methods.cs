#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedSourceUsesEntries
{
    internal void AssertRoundGainedUseQualifications(global::CardGame.Core.CardUseFrame use) => _resolve().AssertRoundGainedUseQualifications(use);
    internal bool CanContinueIssuedRoundGainedUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueIssuedRoundGainedUse(frame);
    internal void CollectIssuedRoundGainedUseCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedRoundGainedUseCandidates(action, window, result);
    internal bool DistanceBorrowedPaidWeaponSlash30HasRoundGainedBasicBonus(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash30HasRoundGainedBasicBonus(q);
    internal bool DistanceBorrowedPhysicalSlash40HasRoundGainedBasicBonus(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPhysicalSlash40HasRoundGainedBasicBonus(q);
    internal bool DistanceBorrowedSlash70HasPotentialRoundGainedBasicMaterials(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedSlash70HasPotentialRoundGainedBasicMaterials(q);
    internal bool DistancePhysicalSlash110HasRoundGainedBasicBonus(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash110HasRoundGainedBasicBonus(q);
    internal bool DistancePhysicalSlash90HasIssuedRoundGainedBasicBonus(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash90HasIssuedRoundGainedBasicBonus(q);
    internal bool DistanceVirtualSlash50HasRoundGainedBasicBonus(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash50HasRoundGainedBasicBonus(q);
    internal global::CardGame.Core.RoundGainedUseQualification? ExactIssuedRoundGainedUseCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, bool paid = false) => _resolve().ExactIssuedRoundGainedUseCandidate(candidate, context, paid);
    internal bool HasExactRoundGainedCommittedEquipmentUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RoundGainedUseQualification q) => _resolve().HasExactRoundGainedCommittedEquipmentUse(use, q);
    internal bool HasIssuedRoundGainedBasicBonus(long useId, int actor) => _resolve().HasIssuedRoundGainedBasicBonus(useId, actor);
    internal bool HasIssuedRoundGainedUseCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedRoundGainedUseCandidate(candidate, context);
    internal bool HasPotentialRoundGainedBasicMaterials(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind) => _resolve().HasPotentialRoundGainedBasicMaterials(owner, kind);
    internal bool HasRoundGainedBasicBonus(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().HasRoundGainedBasicBonus(owner, kind, ids);
    internal bool InRoundGainedOwnPlay(global::CardGame.Core.CharacterState owner) => _resolve().InRoundGainedOwnPlay(owner);
    internal void IssueRoundGainedUseQualifications(long useId, global::CardGame.Core.CardActionContext? action) => _resolve().IssueRoundGainedUseQualifications(useId, action);
    internal void ObserveRoundGainedRoundBoundary(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveRoundGainedRoundBoundary(payload);
    internal global::CardGame.Core.RoundGainedRoundBoundaryEvent? RoundGainedCurrentBoundary() => _resolve().RoundGainedCurrentBoundary();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RoundGainedMaterialAcquisition>? RoundGainedMaterials(global::CardGame.Core.CharacterState owner, string skillId, string policyId, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.RoundGainedRoundBoundaryEvent boundary) => _resolve().RoundGainedMaterials(owner, skillId, policyId, ids, boundary);
    internal bool RoundGainedSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().RoundGainedSlashQuotaAfterLimit(query);
    internal bool SameRoundGainedQualification(global::CardGame.Core.RoundGainedUseQualification a, global::CardGame.Core.RoundGainedUseQualification b) => _resolve().SameRoundGainedQualification(a, b);
    internal bool ValidRoundGainedQualification(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RoundGainedUseQualification q) => _resolve().ValidRoundGainedQualification(use, q);
    internal long RoundGainedMovementSequence { get => _resolve().RoundGainedMovementSequence; }
    internal bool TracksRoundGainedSourceUses { get => _resolve().TracksRoundGainedSourceUses; }
}
