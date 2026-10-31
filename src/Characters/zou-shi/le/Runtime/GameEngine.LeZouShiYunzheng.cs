namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal readonly record struct YunzhengQualificationSeat(long Revision, int Hp, bool Alive, bool Concealed, bool PrimarySelected, bool PrimaryRevealed, bool SecondarySelected, bool SecondaryRevealed, Role Role);
    private ref global::CardGame.Core.GameEngine.YunzhengQualificationSeat[] _yunzhengQualificationSeats => ref _characterZouShiLe.ReferenceToYunzhengQualificationSeats;
    private ref (long Inner, long Movement, int Initial, int Shown) _yunzhengQualificationInputs => ref _characterZouShiLe.ReferenceToYunzhengQualificationInputs;
    private ref long _yunzhengQualificationStamp => ref _characterZouShiLe.ReferenceToYunzhengQualificationStamp;
}
