#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterWangGuanOlMiuyanQualificationDependenciesMiuyanQualificationStampAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _miuyanQualificationStamp { get; }

    ref (long Base, int Round, int Count) _miuyanQualificationDependencies { get; }
}
