#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<long, (int Events, long Stamp)> _mouHuCheErQualificationDependencies { get; }

    ref long _mouHuCheErQualificationStamp { get; }
}
