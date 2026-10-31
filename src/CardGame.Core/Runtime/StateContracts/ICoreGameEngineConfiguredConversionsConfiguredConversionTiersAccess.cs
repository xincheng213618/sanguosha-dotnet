#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<(int Seat, string State), int> _configuredConversionTiers { get; }
}
