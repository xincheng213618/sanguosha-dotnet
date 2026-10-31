#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsSelectedUseConversionAccess : IProgramRuntimeOperations
{
    ref CardConversionSource? _selectedUseConversion { get; }
}
