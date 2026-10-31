#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.CardConversionSource? _selectedUseConversion { get; }

    ref bool _hasSelectedUseConversionChoice { get; }
}
