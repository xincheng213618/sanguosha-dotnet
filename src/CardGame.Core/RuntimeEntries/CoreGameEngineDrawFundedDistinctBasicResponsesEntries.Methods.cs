#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicResponsesEntries
{
    internal void ClearDrawFundedDistinctBasicDodgeResponse(NativeOwners.CardAttackHandle attack) => _resolve().ClearDrawFundedDistinctBasicDodgeResponse(attack);
    internal CardActionContext RequireDrawFundedDistinctBasicResponseAction(CardUseFrame use) => _resolve().RequireDrawFundedDistinctBasicResponseAction(use);
    internal void ResolveDrawFundedDistinctBasicDodge(DrawFundedDistinctBasicPayment payment) => _resolve().ResolveDrawFundedDistinctBasicDodge(payment);
    internal bool TryAdvanceDrawFundedDistinctBasicAi() => _resolve().TryAdvanceDrawFundedDistinctBasicAi();
    internal bool TryResolveDrawFundedDistinctBasicAiDodge() => _resolve().TryResolveDrawFundedDistinctBasicAiDodge();
    internal bool TryResolveDrawFundedDistinctBasicAiDying() => _resolve().TryResolveDrawFundedDistinctBasicAiDying();
    internal bool TryResolveDrawFundedDistinctBasicAiForcedSlash() => _resolve().TryResolveDrawFundedDistinctBasicAiForcedSlash();
}
