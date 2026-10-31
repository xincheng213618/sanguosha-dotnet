#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionGeneralPoolOptionsAccess : IProgramRuntimeOperations
{
    ref readonly GameOptions _options { get; }

    ref readonly IReadOnlyList<GeneralDefinition> _generalPool { get; }
}
