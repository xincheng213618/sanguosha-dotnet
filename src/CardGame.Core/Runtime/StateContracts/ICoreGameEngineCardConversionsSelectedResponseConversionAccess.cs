#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineCardConversionsSelectedResponseConversionAccess : IProgramRuntimeOperations
{
    ref CardConversionSource? _selectedResponseConversion { get; }
}
