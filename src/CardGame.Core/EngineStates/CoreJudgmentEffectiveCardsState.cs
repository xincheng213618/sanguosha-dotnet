#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreJudgmentEffectiveCardsState : global::CardGame.Core.EngineStates.Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess
{
    private global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardKind> _judgmentEffectiveCardKinds = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardKind> ReferenceToJudgmentEffectiveCardKinds => ref _judgmentEffectiveCardKinds;

    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardKind> global::CardGame.Core.EngineStates.Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds => ref _judgmentEffectiveCardKinds;
}
