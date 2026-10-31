#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineConfiguredConversionsState : Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess, Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess
{
    private Dictionary<(int Seat, string State), int> _configuredConversionTiers = [];
    internal ref readonly Dictionary<(int Seat, string State), int> ReferenceToConfiguredConversionTiers => ref _configuredConversionTiers;

    private Dictionary<(int Seat, string State), (int Turn, int Card, CardKind Kind)> _declaredConversionCards = [];
    internal ref readonly Dictionary<(int Seat, string State), (int Turn, int Card, CardKind Kind)> ReferenceToDeclaredConversionCards => ref _declaredConversionCards;

    ref readonly Dictionary<(int Seat, string State), int> Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._configuredConversionTiers => ref _configuredConversionTiers;

    ref readonly Dictionary<(int Seat, string State), (int Turn, int Card, CardKind Kind)> Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._declaredConversionCards => ref _declaredConversionCards;

    ref readonly Dictionary<(int Seat, string State), int> Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess._configuredConversionTiers => ref _configuredConversionTiers;
}
