using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal sealed record OriginalHandStateKey(int OwnerSeat, string SkillId, string StateId);
    internal sealed record OriginalHandEntity(int CardId, long DealSequence, long? FirstLossSequence);
    internal sealed record OriginalHandEntityState(long InitializationFrameId, CardConversionSource Source, string GameplayHash, IReadOnlyList<global::CardGame.Core.GameEngine.OriginalHandEntity> Entities);
    // Durable gameplay state, not pending resolution. A departed physical
    // entity never becomes original again on return or source reacquisition.
    private ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.OriginalHandStateKey, global::CardGame.Core.GameEngine.OriginalHandEntityState> _originalHandEntityStates => ref _originalHandEntities.ReferenceToOriginalHandEntityStates;
    private ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.OriginalHandPermanentBonus> _originalHandPermanentBonuses => ref _originalHandEntities.ReferenceToOriginalHandPermanentBonuses;
}
