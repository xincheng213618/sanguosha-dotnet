#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNativeFactionChangesFieldsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, GameEngine.NativeFactionObservation> _nativeFactionObservations { get; }

    ref long _nativeFactionRevision { get; }

    ref bool _synchronizingNativeFactions { get; }

    ref long _nativeFactionQualificationStamp { get; }

    ref (long Base, long Revision) _nativeFactionQualificationDependencies { get; }
}
