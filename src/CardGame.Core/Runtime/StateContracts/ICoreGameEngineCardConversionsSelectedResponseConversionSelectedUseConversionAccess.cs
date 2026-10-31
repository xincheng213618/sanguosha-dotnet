#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess : IProgramRuntimeOperations
{
    ref CardConversionSource? _selectedUseConversion { get; }

    ref CardConversionSource? _selectedResponseConversion { get; }
}
