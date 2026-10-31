#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterCuiLieAnniversaryState : global::CardGame.Core.EngineStates.Contracts.ICharacterCuiLieAnniversaryCuiLieQualificationInputsCuiLieQualificationStampAccess
{
    private (long Inner, int Issued, int Ended) _cuiLieQualificationInputs = default !;
    internal ref (long Inner, int Issued, int Ended) ReferenceToCuiLieQualificationInputs => ref _cuiLieQualificationInputs;

    private long _cuiLieQualificationStamp = default !;
    internal ref long ReferenceToCuiLieQualificationStamp => ref _cuiLieQualificationStamp;

    ref long global::CardGame.Core.EngineStates.Contracts.ICharacterCuiLieAnniversaryCuiLieQualificationInputsCuiLieQualificationStampAccess._cuiLieQualificationStamp => ref _cuiLieQualificationStamp;

    ref (long Inner, int Issued, int Ended) global::CardGame.Core.EngineStates.Contracts.ICharacterCuiLieAnniversaryCuiLieQualificationInputsCuiLieQualificationStampAccess._cuiLieQualificationInputs => ref _cuiLieQualificationInputs;
}
