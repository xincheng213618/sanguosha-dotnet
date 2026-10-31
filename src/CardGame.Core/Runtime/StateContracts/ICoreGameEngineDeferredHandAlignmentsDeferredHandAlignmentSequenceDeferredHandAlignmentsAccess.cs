#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DeferredHandAlignment> _deferredHandAlignments { get; }

    ref long _deferredHandAlignmentSequence { get; }
}
