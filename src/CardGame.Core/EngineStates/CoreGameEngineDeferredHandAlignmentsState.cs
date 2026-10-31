#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeferredHandAlignmentsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess
{
    private long _deferredHandAlignmentSequence = default !;
    internal ref long ReferenceToDeferredHandAlignmentSequence => ref _deferredHandAlignmentSequence;

    private global::System.Collections.Generic.List<global::CardGame.Core.DeferredHandAlignment> _deferredHandAlignments = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DeferredHandAlignment> ReferenceToDeferredHandAlignments => ref _deferredHandAlignments;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.DeferredHandAlignment> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments => ref _deferredHandAlignments;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignmentSequence => ref _deferredHandAlignmentSequence;
}
