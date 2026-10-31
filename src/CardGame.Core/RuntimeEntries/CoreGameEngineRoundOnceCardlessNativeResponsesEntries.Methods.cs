#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeResponsesEntries
{
    internal void ClearRoundOnceCardlessDodgeResponse(NativeOwners.CardAttackHandle attack) => _resolve().ClearRoundOnceCardlessDodgeResponse(attack);
    internal void ClearRoundOnceCardlessResponse(ResolutionFrame owner) => _resolve().ClearRoundOnceCardlessResponse(owner);
    internal bool IsIssuedRoundOnceCardlessResponse(ResolutionFrame owner, CardActionContext action) => _resolve().IsIssuedRoundOnceCardlessResponse(owner, action);
    internal bool IsRoundOnceCardlessResponseCardUse(CharacterState actor, CardKind kind) => _resolve().IsRoundOnceCardlessResponseCardUse(actor, kind);
    internal bool IsRoundOnceCardlessResponseUse(CardActionContext action, ProgramCardContinuation continuation) => _resolve().IsRoundOnceCardlessResponseUse(action, continuation);
    internal CardActionContext RequireRoundOnceCardlessResponseAction(ResolutionFrame owner) => _resolve().RequireRoundOnceCardlessResponseAction(owner);
    internal void ResolveRoundOnceCardlessDodge(CharacterState actor, CardConversionSource source) => _resolve().ResolveRoundOnceCardlessDodge(actor, source);
    internal IReadOnlyList<PromptChoice> RoundOnceCardlessResponseChoices(CharacterState actor, CardKind kind) => _resolve().RoundOnceCardlessResponseChoices(actor, kind);
    internal ResolutionFrame RoundOnceCardlessResponseOwner(long id) => _resolve().RoundOnceCardlessResponseOwner(id);
    internal RoundOnceCardlessBasicUseReceipt StoreRoundOnceCardlessResponse(ResolutionFrame owner, CardActionContext action, CardConversionSource source) => _resolve().StoreRoundOnceCardlessResponse(owner, action, source);
    internal bool TryResolveRoundOnceCardlessResponse(PromptChoice choice, bool advance) => _resolve().TryResolveRoundOnceCardlessResponse(choice, advance);
}
