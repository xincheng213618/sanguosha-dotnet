#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraEffectMaterialClaimsEntries
{
    internal void CaptureExtraEffectMaterialClaims(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.GameEngine.IDamageAttempt attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, long before, global::CardGame.Core.CardMoveReason reason) => _resolve().CaptureExtraEffectMaterialClaims(producer, attack, cards, before, reason);
    internal bool HasExtraEffectMaterialClaim(long useId, int cardId) => _resolve().HasExtraEffectMaterialClaim(useId, cardId);
    internal void RestoreExtraEffectGroupMaterialClaims(global::CardGame.Core.NativeOwners.GroupCardHandle group) => _resolve().RestoreExtraEffectGroupMaterialClaims(group);
}
