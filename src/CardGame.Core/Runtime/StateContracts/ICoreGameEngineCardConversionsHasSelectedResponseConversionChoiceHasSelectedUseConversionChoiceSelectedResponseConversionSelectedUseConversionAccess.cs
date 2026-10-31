#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess : IProgramRuntimeOperations
{
    ref CardConversionSource? _selectedUseConversion { get; }

    ref bool _hasSelectedUseConversionChoice { get; }

    ref CardConversionSource? _selectedResponseConversion { get; }

    ref bool _hasSelectedResponseConversionChoice { get; }
}
