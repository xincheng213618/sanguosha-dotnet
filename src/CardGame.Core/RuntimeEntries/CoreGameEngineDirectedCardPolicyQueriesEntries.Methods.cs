#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDirectedCardPolicyQueriesEntries
{
    internal bool CanUseGlobalCard(CharacterState actor, CardKind kind, bool excludeOwner = false) => _resolve().CanUseGlobalCard(actor, kind, excludeOwner);
    internal bool DistancePhysicalSlash200HasCardDistanceExemption(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash200HasCardDistanceExemption(q);
    internal bool DistanceVirtualSlash100HasCardDistanceExemption(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash100HasCardDistanceExemption(q);
    internal IEnumerable<int> GetDeclaredCardTargets(CharacterState actor, LegalActionKind actionKind, IReadOnlyList<int> targets) => _resolve().GetDeclaredCardTargets(actor, actionKind, targets);
    internal IEnumerable<int> GetDeclaredGlobalCardTargets(CharacterState actor, CardKind kind) => _resolve().GetDeclaredGlobalCardTargets(actor, kind);
    internal bool HasCardArmorBypass(CharacterState actor, CharacterState target, CardKind kind) => _resolve().HasCardArmorBypass(actor, target, kind);
    internal bool HasCardDistanceExemption(CharacterState actor, CharacterState target, CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default, IReadOnlyList<int>? physicalCardIds = default) => _resolve().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
    internal bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat) => _resolve().HasDirectedCardArmorBypass(resolutionId, targetSeat);
    internal bool HasSlashAllowanceForAnyTarget(CharacterState actor, IEnumerable<CardKind>? kinds = default) => _resolve().HasSlashAllowanceForAnyTarget(actor, kinds);
    internal bool HasYuanchouCardPolicy { get => _resolve().HasYuanchouCardPolicy; }
}
