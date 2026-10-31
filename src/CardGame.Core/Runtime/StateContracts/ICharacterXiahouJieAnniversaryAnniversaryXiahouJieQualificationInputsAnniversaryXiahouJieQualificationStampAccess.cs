#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterXiahouJieAnniversaryAnniversaryXiahouJieQualificationInputsAnniversaryXiahouJieQualificationStampAccess : IProgramRuntimeOperations
{
    ref long _anniversaryXiahouJieQualificationStamp { get; }

    ref (long Inner, int Issued, int Expired) _anniversaryXiahouJieQualificationInputs { get; }
}
