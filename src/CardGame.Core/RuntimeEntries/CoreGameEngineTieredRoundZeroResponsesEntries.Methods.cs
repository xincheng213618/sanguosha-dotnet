#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTieredRoundZeroResponsesEntries
{
    internal void AssertTieredRoundResponseUses() => _resolve().AssertTieredRoundResponseUses();
    internal global::CardGame.Core.NullificationWindowFrame ClearTieredRoundCounterspellResponse(global::CardGame.Core.NullificationWindowFrame input) => _resolve().ClearTieredRoundCounterspellResponse(input);
    internal void ClearTieredRoundDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ClearTieredRoundDodgeResponse(attack);
    internal bool HasTieredRoundZeroDyingResponse(global::CardGame.Core.CharacterState responder) => _resolve().HasTieredRoundZeroDyingResponse(responder);
    internal bool IsIssuedTieredRoundResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action, bool zeroOnly = false) => _resolve().IsIssuedTieredRoundResponse(owner, action, zeroOnly);
    internal bool IsIssuedTieredRoundZeroResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action) => _resolve().IsIssuedTieredRoundZeroResponse(owner, action);
    internal global::CardGame.Core.TieredRoundConversionUseReceipt IssueTieredRoundResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource source) => _resolve().IssueTieredRoundResponse(owner, action, source);
    internal global::CardGame.Core.CardActionContext RequireTieredRoundIssuedResponseAction(global::CardGame.Core.ResolutionFrame owner) => _resolve().RequireTieredRoundIssuedResponseAction(owner);
    internal void ResolveTieredRoundZeroCounterspell(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source) => _resolve().ResolveTieredRoundZeroCounterspell(actor, source);
    internal void ResolveTieredRoundZeroDodge(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source) => _resolve().ResolveTieredRoundZeroDodge(actor, source);
    internal void ResolveTieredRoundZeroResponse(global::CardGame.Core.PromptChoice choice, bool advance) => _resolve().ResolveTieredRoundZeroResponse(choice, advance);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TieredRoundZeroResponseChoices(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool dyingUse = false) => _resolve().TieredRoundZeroResponseChoices(actor, kind, dyingUse);
    internal void TryIssueTieredRoundResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action) => _resolve().TryIssueTieredRoundResponse(owner, action);
    internal bool TryResolveTieredRoundZeroAiCounterspell() => _resolve().TryResolveTieredRoundZeroAiCounterspell();
    internal bool TryResolveTieredRoundZeroAiDodge() => _resolve().TryResolveTieredRoundZeroAiDodge();
    internal bool TryResolveTieredRoundZeroAiDying() => _resolve().TryResolveTieredRoundZeroAiDying();
}
