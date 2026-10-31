#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<long, (int Events, long Stamp)> _mouHuCheErQualificationDependencies { get; }

    ref long _mouHuCheErQualificationStamp { get; }
}
