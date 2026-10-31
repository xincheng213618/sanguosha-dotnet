#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.CardConversionSource? _selectedResponseConversion { get; }

    ref bool _hasSelectedResponseConversionChoice { get; }
}
