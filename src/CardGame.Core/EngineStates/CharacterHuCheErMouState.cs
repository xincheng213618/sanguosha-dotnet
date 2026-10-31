#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuCheErMouState : global::CardGame.Core.EngineStates.Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess
{
    private global::System.Collections.Generic.Dictionary<long, (int Events, long Stamp)> _mouHuCheErQualificationDependencies = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<long, (int Events, long Stamp)> ReferenceToMouHuCheErQualificationDependencies => ref _mouHuCheErQualificationDependencies;

    private long _mouHuCheErQualificationStamp = default !;
    internal ref long ReferenceToMouHuCheErQualificationStamp => ref _mouHuCheErQualificationStamp;

    ref readonly global::System.Collections.Generic.Dictionary<long, (int Events, long Stamp)> global::CardGame.Core.EngineStates.Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess._mouHuCheErQualificationDependencies => ref _mouHuCheErQualificationDependencies;

    ref long global::CardGame.Core.EngineStates.Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess._mouHuCheErQualificationStamp => ref _mouHuCheErQualificationStamp;
}
