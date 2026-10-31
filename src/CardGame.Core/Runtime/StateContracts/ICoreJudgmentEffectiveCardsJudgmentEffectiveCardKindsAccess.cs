#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.CardKind> _judgmentEffectiveCardKinds { get; }
}
