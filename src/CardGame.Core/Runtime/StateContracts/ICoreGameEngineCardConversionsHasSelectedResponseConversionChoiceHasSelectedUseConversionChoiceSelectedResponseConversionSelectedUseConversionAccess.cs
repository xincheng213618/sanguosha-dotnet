#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.CardConversionSource? _selectedUseConversion { get; }

    ref bool _hasSelectedUseConversionChoice { get; }

    ref global::CardGame.Core.CardConversionSource? _selectedResponseConversion { get; }

    ref bool _hasSelectedResponseConversionChoice { get; }
}
