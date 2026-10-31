#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), int> _configuredConversionTiers { get; }
}
