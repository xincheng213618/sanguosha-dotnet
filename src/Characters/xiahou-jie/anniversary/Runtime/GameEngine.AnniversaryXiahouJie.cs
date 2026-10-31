namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref (long Inner, int Issued, int Expired) _anniversaryXiahouJieQualificationInputs => ref _characterXiahouJieAnniversary.ReferenceToAnniversaryXiahouJieQualificationInputs;
    private ref long _anniversaryXiahouJieQualificationStamp => ref _characterXiahouJieAnniversary.ReferenceToAnniversaryXiahouJieQualificationStamp;
}
