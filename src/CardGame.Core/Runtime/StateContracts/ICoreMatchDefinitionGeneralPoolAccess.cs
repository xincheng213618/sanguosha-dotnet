#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionGeneralPoolAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> _generalPool { get; }
}
