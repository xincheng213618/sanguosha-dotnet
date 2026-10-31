#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterWangGuanOlState : Contracts.ICharacterWangGuanOlMiuyanQualificationDependenciesMiuyanQualificationStampAccess
{
    private (long Base, int Round, int Count) _miuyanQualificationDependencies = default !;
    internal ref (long Base, int Round, int Count) ReferenceToMiuyanQualificationDependencies => ref _miuyanQualificationDependencies;

    private long _miuyanQualificationStamp = default !;
    internal ref long ReferenceToMiuyanQualificationStamp => ref _miuyanQualificationStamp;

    ref long Contracts.ICharacterWangGuanOlMiuyanQualificationDependenciesMiuyanQualificationStampAccess._miuyanQualificationStamp => ref _miuyanQualificationStamp;

    ref (long Base, int Round, int Count) Contracts.ICharacterWangGuanOlMiuyanQualificationDependenciesMiuyanQualificationStampAccess._miuyanQualificationDependencies => ref _miuyanQualificationDependencies;
}
