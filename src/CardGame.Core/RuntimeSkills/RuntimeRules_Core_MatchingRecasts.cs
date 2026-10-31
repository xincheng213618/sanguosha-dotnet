
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_MatchingRecasts
{
    internal static bool MatchingRecastGroup(IReadOnlyList<MatchingRecastMaterial> materials) => materials.Count >= 2 &&
        materials.Select(m => m.CardId).Distinct().Count() == materials.Count &&
        (materials.All(m => EquipmentCatalog.IsEquipment(m.PrintedKind)) ||
         materials.All(m => RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(m.PrintedKind) == RuntimeRules_Core_AlternativeViewAsCosts.ProgramBasicCardName(materials[0].PrintedKind)));

    internal static bool MatchingRecastPeerCursor(ProgramMatchingRecastReceipt r) => r.Stage is
        MatchingRecastStage.PeerCostChildren or MatchingRecastStage.PeerDrawChildren;

    internal static MatchingRecastParticipantReceipt CurrentMatchingRecast(ProgramMatchingRecastReceipt r) => RuntimeRules_Core_MatchingRecasts.MatchingRecastPeerCursor(r) ? r.Peer : r.Owner;

    internal static ProgramMatchingRecastReceipt WithCurrentMatchingRecast(ProgramMatchingRecastReceipt r, MatchingRecastParticipantReceipt p) =>
        RuntimeRules_Core_MatchingRecasts.MatchingRecastPeerCursor(r) ? r with { Peer = p } : r with { Owner = p };
}
