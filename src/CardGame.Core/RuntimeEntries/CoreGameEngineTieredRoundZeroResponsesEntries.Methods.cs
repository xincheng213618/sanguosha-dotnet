#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTieredRoundZeroResponsesEntries
{
    internal NullificationWindowFrame ClearTieredRoundCounterspellResponse(NullificationWindowFrame input) => _resolve().ClearTieredRoundCounterspellResponse(input);
    internal void ClearTieredRoundDodgeResponse(NativeOwners.CardAttackHandle attack) => _resolve().ClearTieredRoundDodgeResponse(attack);
    internal bool HasTieredRoundZeroDyingResponse(CharacterState responder) => _resolve().HasTieredRoundZeroDyingResponse(responder);
    internal bool IsIssuedTieredRoundResponse(ResolutionFrame owner, CardActionContext action, bool zeroOnly = false) => _resolve().IsIssuedTieredRoundResponse(owner, action, zeroOnly);
    internal bool IsIssuedTieredRoundZeroResponse(ResolutionFrame owner, CardActionContext action) => _resolve().IsIssuedTieredRoundZeroResponse(owner, action);
    internal TieredRoundConversionUseReceipt IssueTieredRoundResponse(ResolutionFrame owner, CardActionContext action, CardConversionSource source) => _resolve().IssueTieredRoundResponse(owner, action, source);
    internal CardActionContext RequireTieredRoundIssuedResponseAction(ResolutionFrame owner) => _resolve().RequireTieredRoundIssuedResponseAction(owner);
    internal void ResolveTieredRoundZeroCounterspell(CharacterState actor, CardConversionSource source) => _resolve().ResolveTieredRoundZeroCounterspell(actor, source);
    internal void ResolveTieredRoundZeroDodge(CharacterState actor, CardConversionSource source) => _resolve().ResolveTieredRoundZeroDodge(actor, source);
    internal void ResolveTieredRoundZeroResponse(PromptChoice choice, bool advance) => _resolve().ResolveTieredRoundZeroResponse(choice, advance);
    internal IReadOnlyList<PromptChoice> TieredRoundZeroResponseChoices(CharacterState actor, CardKind kind, bool dyingUse = false) => _resolve().TieredRoundZeroResponseChoices(actor, kind, dyingUse);
    internal void TryIssueTieredRoundResponse(ResolutionFrame owner, CardActionContext action) => _resolve().TryIssueTieredRoundResponse(owner, action);
    internal bool TryResolveTieredRoundZeroAiCounterspell() => _resolve().TryResolveTieredRoundZeroAiCounterspell();
    internal bool TryResolveTieredRoundZeroAiDodge() => _resolve().TryResolveTieredRoundZeroAiDodge();
    internal bool TryResolveTieredRoundZeroAiDying() => _resolve().TryResolveTieredRoundZeroAiDying();
}
