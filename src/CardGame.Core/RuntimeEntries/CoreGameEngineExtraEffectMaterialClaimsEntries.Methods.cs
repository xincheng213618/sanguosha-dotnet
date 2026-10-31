#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectMaterialClaimsEntries
{
    internal void CaptureExtraEffectMaterialClaims(ProgramSkillFrame producer, GameEngine.IDamageAttempt attack, IReadOnlyList<Card> cards, long before, CardMoveReason reason) => _resolve().CaptureExtraEffectMaterialClaims(producer, attack, cards, before, reason);
    internal bool HasExtraEffectMaterialClaim(long useId, int cardId) => _resolve().HasExtraEffectMaterialClaim(useId, cardId);
    internal void RestoreExtraEffectGroupMaterialClaims(NativeOwners.GroupCardHandle group) => _resolve().RestoreExtraEffectGroupMaterialClaims(group);
}
