#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZouShiLeState : global::CardGame.Core.EngineStates.Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess
{
    private (long Inner, long Movement, int Initial, int Shown) _yunzhengQualificationInputs = default !;
    internal ref (long Inner, long Movement, int Initial, int Shown) ReferenceToYunzhengQualificationInputs => ref _yunzhengQualificationInputs;

    private global::CardGame.Core.GameEngine.YunzhengQualificationSeat[] _yunzhengQualificationSeats = [];
    internal ref global::CardGame.Core.GameEngine.YunzhengQualificationSeat[] ReferenceToYunzhengQualificationSeats => ref _yunzhengQualificationSeats;

    private long _yunzhengQualificationStamp = default !;
    internal ref long ReferenceToYunzhengQualificationStamp => ref _yunzhengQualificationStamp;

    ref (long Inner, long Movement, int Initial, int Shown) global::CardGame.Core.EngineStates.Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationInputs => ref _yunzhengQualificationInputs;

    ref global::CardGame.Core.GameEngine.YunzhengQualificationSeat[] global::CardGame.Core.EngineStates.Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationSeats => ref _yunzhengQualificationSeats;

    ref long global::CardGame.Core.EngineStates.Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess._yunzhengQualificationStamp => ref _yunzhengQualificationStamp;
}
