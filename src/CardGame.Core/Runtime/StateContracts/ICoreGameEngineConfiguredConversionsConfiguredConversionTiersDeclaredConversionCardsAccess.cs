#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Seat, string State), int> _configuredConversionTiers { get; }

    ref readonly Dictionary<(int Seat, string State), (int Turn, int Card, CardKind Kind)> _declaredConversionCards { get; }
}
