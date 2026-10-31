#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineDeferredHandAlignmentsState : Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess
{
    private long _deferredHandAlignmentSequence = default !;
    internal ref long ReferenceToDeferredHandAlignmentSequence => ref _deferredHandAlignmentSequence;

    private List<DeferredHandAlignment> _deferredHandAlignments = [];
    internal ref readonly List<DeferredHandAlignment> ReferenceToDeferredHandAlignments => ref _deferredHandAlignments;

    ref readonly List<DeferredHandAlignment> Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignments => ref _deferredHandAlignments;

    ref long Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess._deferredHandAlignmentSequence => ref _deferredHandAlignmentSequence;
}
