#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), int> _configuredConversionTiers { get; }

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), (int Turn, int Card, global::CardGame.Core.CardKind Kind)> _declaredConversionCards { get; }
}
