#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterWangGuanOlMiuyanQualificationDependenciesMiuyanQualificationStampAccess : IProgramRuntimeOperations
{
    ref long _miuyanQualificationStamp { get; }

    ref (long Base, int Round, int Count) _miuyanQualificationDependencies { get; }
}
