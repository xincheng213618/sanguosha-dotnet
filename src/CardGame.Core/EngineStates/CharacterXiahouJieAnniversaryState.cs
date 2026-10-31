#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterXiahouJieAnniversaryState : Contracts.ICharacterXiahouJieAnniversaryAnniversaryXiahouJieQualificationInputsAnniversaryXiahouJieQualificationStampAccess
{
    private (long Inner, int Issued, int Expired) _anniversaryXiahouJieQualificationInputs = default !;
    internal ref (long Inner, int Issued, int Expired) ReferenceToAnniversaryXiahouJieQualificationInputs => ref _anniversaryXiahouJieQualificationInputs;

    private long _anniversaryXiahouJieQualificationStamp = default !;
    internal ref long ReferenceToAnniversaryXiahouJieQualificationStamp => ref _anniversaryXiahouJieQualificationStamp;

    ref long Contracts.ICharacterXiahouJieAnniversaryAnniversaryXiahouJieQualificationInputsAnniversaryXiahouJieQualificationStampAccess._anniversaryXiahouJieQualificationStamp => ref _anniversaryXiahouJieQualificationStamp;

    ref (long Inner, int Issued, int Expired) Contracts.ICharacterXiahouJieAnniversaryAnniversaryXiahouJieQualificationInputsAnniversaryXiahouJieQualificationStampAccess._anniversaryXiahouJieQualificationInputs => ref _anniversaryXiahouJieQualificationInputs;
}
