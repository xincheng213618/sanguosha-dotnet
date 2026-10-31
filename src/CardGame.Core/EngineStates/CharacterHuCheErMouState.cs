#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuCheErMouState : Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess
{
    private Dictionary<long, (int Events, long Stamp)> _mouHuCheErQualificationDependencies = [];
    internal ref readonly Dictionary<long, (int Events, long Stamp)> ReferenceToMouHuCheErQualificationDependencies => ref _mouHuCheErQualificationDependencies;

    private long _mouHuCheErQualificationStamp = default !;
    internal ref long ReferenceToMouHuCheErQualificationStamp => ref _mouHuCheErQualificationStamp;

    ref readonly Dictionary<long, (int Events, long Stamp)> Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess._mouHuCheErQualificationDependencies => ref _mouHuCheErQualificationDependencies;

    ref long Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess._mouHuCheErQualificationStamp => ref _mouHuCheErQualificationStamp;
}
