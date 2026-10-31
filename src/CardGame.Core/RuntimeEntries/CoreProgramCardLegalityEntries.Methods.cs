#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramCardLegalityEntries
{
    internal bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => _resolve().IsProgramEntityPhysicalRestricted(actorSeat, cardId);
    internal bool IsProgramHandPhysicalRestricted(int actorSeat, int cardId) => _resolve().IsProgramHandPhysicalRestricted(actorSeat, cardId);
    internal bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false) => _resolve().IsProgramMaterialUseForbidden(actorSeat, kind, cardIds, response);
    internal bool IsProgramNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource? source = default, bool dyingUse = false) => _resolve().IsProgramNativeResponseAllowed(provider, kind, cardIds, source, dyingUse);
    internal bool IsProgramViewAsAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => _resolve().IsProgramViewAsAllowed(owner, kind, cardIds, source, forResponse, dyingUse, requestedUseTarget);
}
