#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess : IProgramRuntimeOperations
{
    ref CardConversionSource? _selectedResponseConversion { get; }

    ref bool _hasSelectedResponseConversionChoice { get; }
}
