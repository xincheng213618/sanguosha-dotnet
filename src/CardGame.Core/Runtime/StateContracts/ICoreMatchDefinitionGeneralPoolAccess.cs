#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionGeneralPoolAccess : IProgramRuntimeOperations
{
    ref readonly IReadOnlyList<GeneralDefinition> _generalPool { get; }
}
