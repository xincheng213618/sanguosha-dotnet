#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreJudgmentEffectiveCardsState : Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess
{
    private Dictionary<int, CardKind> _judgmentEffectiveCardKinds = [];
    internal ref readonly Dictionary<int, CardKind> ReferenceToJudgmentEffectiveCardKinds => ref _judgmentEffectiveCardKinds;

    ref readonly Dictionary<int, CardKind> Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds => ref _judgmentEffectiveCardKinds;
}
