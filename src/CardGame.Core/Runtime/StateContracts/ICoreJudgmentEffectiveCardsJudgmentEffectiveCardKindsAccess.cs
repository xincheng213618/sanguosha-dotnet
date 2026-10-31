#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, CardKind> _judgmentEffectiveCardKinds { get; }
}
