#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineNativeFactionChangesFieldsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, global::CardGame.Core.GameEngine.NativeFactionObservation> _nativeFactionObservations { get; }

    ref long _nativeFactionRevision { get; }

    ref bool _synchronizingNativeFactions { get; }

    ref long _nativeFactionQualificationStamp { get; }

    ref (long Base, long Revision) _nativeFactionQualificationDependencies { get; }
}
