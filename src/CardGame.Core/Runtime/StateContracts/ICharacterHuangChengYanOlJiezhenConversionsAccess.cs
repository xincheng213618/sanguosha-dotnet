#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHuangChengYanOlJiezhenConversionsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, GameEngine.JiezhenConversionState> _jiezhenConversions { get; }
}
