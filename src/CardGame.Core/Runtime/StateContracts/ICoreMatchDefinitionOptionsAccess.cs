#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionOptionsAccess : IProgramRuntimeOperations
{
    ref readonly GameOptions _options { get; }
}
