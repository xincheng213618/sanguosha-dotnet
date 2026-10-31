#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess : IProgramRuntimeOperations
{
    ref CardConversionSource? _selectedUseConversion { get; }

    ref bool _hasSelectedUseConversionChoice { get; }
}
