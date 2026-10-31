#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess : IProgramRuntimeOperations
{
    ref readonly List<DeferredHandAlignment> _deferredHandAlignments { get; }

    ref long _deferredHandAlignmentSequence { get; }
}
