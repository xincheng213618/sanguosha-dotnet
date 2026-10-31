#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterCuiLieAnniversaryCuiLieQualificationInputsCuiLieQualificationStampAccess : IProgramRuntimeOperations
{
    ref long _cuiLieQualificationStamp { get; }

    ref (long Inner, int Issued, int Ended) _cuiLieQualificationInputs { get; }
}
