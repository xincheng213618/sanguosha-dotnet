#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDirectedCardPolicyQueriesEntries
{
    internal bool CanUseGlobalCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool excludeOwner = false) => _resolve().CanUseGlobalCard(actor, kind, excludeOwner);
    internal bool DistancePhysicalSlash200HasCardDistanceExemption(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash200HasCardDistanceExemption(q);
    internal bool DistanceVirtualSlash100HasCardDistanceExemption(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash100HasCardDistanceExemption(q);
    internal global::System.Collections.Generic.IEnumerable<int> GetDeclaredCardTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalActionKind actionKind, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().GetDeclaredCardTargets(actor, actionKind, targets);
    internal global::System.Collections.Generic.IEnumerable<int> GetDeclaredGlobalCardTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => _resolve().GetDeclaredGlobalCardTargets(actor, kind);
    internal bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind) => _resolve().HasCardArmorBypass(actor, target, kind);
    internal bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default) => _resolve().HasCardDistanceExemption(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot, physicalCardIds);
    internal bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat) => _resolve().HasDirectedCardArmorBypass(resolutionId, targetSeat);
    internal bool HasSlashAllowanceForAnyTarget(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardKind>? kinds = default) => _resolve().HasSlashAllowanceForAnyTarget(actor, kinds);
    internal bool HasYuanchouCardPolicy { get => _resolve().HasYuanchouCardPolicy; }
}
