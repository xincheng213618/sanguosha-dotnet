#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineConfiguredConversionsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess
{
    private global::System.Collections.Generic.Dictionary<(int Seat, string State), int> _configuredConversionTiers = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), int> ReferenceToConfiguredConversionTiers => ref _configuredConversionTiers;

    private global::System.Collections.Generic.Dictionary<(int Seat, string State), (int Turn, int Card, global::CardGame.Core.CardKind Kind)> _declaredConversionCards = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), (int Turn, int Card, global::CardGame.Core.CardKind Kind)> ReferenceToDeclaredConversionCards => ref _declaredConversionCards;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._configuredConversionTiers => ref _configuredConversionTiers;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), (int Turn, int Card, global::CardGame.Core.CardKind Kind)> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess._declaredConversionCards => ref _declaredConversionCards;

    ref readonly global::System.Collections.Generic.Dictionary<(int Seat, string State), int> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess._configuredConversionTiers => ref _configuredConversionTiers;
}
