#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.CardConversionSource? _selectedUseConversion { get; }

    ref global::CardGame.Core.CardConversionSource? _selectedResponseConversion { get; }
}
