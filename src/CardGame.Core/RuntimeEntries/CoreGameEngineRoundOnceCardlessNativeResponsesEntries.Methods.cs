#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeResponsesEntries
{
    internal void ClearRoundOnceCardlessDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ClearRoundOnceCardlessDodgeResponse(attack);
    internal void ClearRoundOnceCardlessResponse(global::CardGame.Core.ResolutionFrame owner) => _resolve().ClearRoundOnceCardlessResponse(owner);
    internal bool IsIssuedRoundOnceCardlessResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action) => _resolve().IsIssuedRoundOnceCardlessResponse(owner, action);
    internal bool IsRoundOnceCardlessResponseCardUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => _resolve().IsRoundOnceCardlessResponseCardUse(actor, kind);
    internal bool IsRoundOnceCardlessResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().IsRoundOnceCardlessResponseUse(action, continuation);
    internal global::CardGame.Core.CardActionContext RequireRoundOnceCardlessResponseAction(global::CardGame.Core.ResolutionFrame owner) => _resolve().RequireRoundOnceCardlessResponseAction(owner);
    internal void ResolveRoundOnceCardlessDodge(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source) => _resolve().ResolveRoundOnceCardlessDodge(actor, source);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RoundOnceCardlessResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind) => _resolve().RoundOnceCardlessResponseChoices(actor, kind);
    internal global::CardGame.Core.ResolutionFrame RoundOnceCardlessResponseOwner(long id) => _resolve().RoundOnceCardlessResponseOwner(id);
    internal global::CardGame.Core.RoundOnceCardlessBasicUseReceipt StoreRoundOnceCardlessResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource source) => _resolve().StoreRoundOnceCardlessResponse(owner, action, source);
    internal bool TryResolveRoundOnceCardlessResponse(global::CardGame.Core.PromptChoice choice, bool advance) => _resolve().TryResolveRoundOnceCardlessResponse(choice, advance);
}
