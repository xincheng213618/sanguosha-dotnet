#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreMatchDefinitionGeneralPoolOptionsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.GameOptions _options { get; }

    ref readonly global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GeneralDefinition> _generalPool { get; }
}
