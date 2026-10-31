using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref (long Inner, int Issued, int Ended) _cuiLieQualificationInputs => ref _characterCuiLieAnniversary.ReferenceToCuiLieQualificationInputs;
    private ref long _cuiLieQualificationStamp => ref _characterCuiLieAnniversary.ReferenceToCuiLieQualificationStamp;
}
