#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZouShiLeState : Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess
{
    private (long Inner, long Movement, int Initial, int Shown) _yunzhengQualificationInputs = default !;
    internal ref (long Inner, long Movement, int Initial, int Shown) ReferenceToYunzhengQualificationInputs => ref _yunzhengQualificationInputs;

    private GameEngine.YunzhengQualificationSeat[] _yunzhengQualificationSeats = [];
    internal ref GameEngine.YunzhengQualificationSeat[] ReferenceToYunzhengQualificationSeats => ref _yunzhengQualificationSeats;

    private long _yunzhengQualificationStamp = default !;
    internal ref long ReferenceToYunzhengQualificationStamp => ref _yunzhengQualificationStamp;

    ref (long Inner, long Movement, int Initial, int Shown) Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationInputs => ref _yunzhengQualificationInputs;

    ref GameEngine.YunzhengQualificationSeat[] Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationSeats => ref _yunzhengQualificationSeats;

    ref long Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationStamp => ref _yunzhengQualificationStamp;
}
