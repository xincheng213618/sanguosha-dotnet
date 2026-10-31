#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref (long Inner, long Movement, int Initial, int Shown) _yunzhengQualificationInputs { get; }

    ref global::CardGame.Core.GameEngine.YunzhengQualificationSeat[] _yunzhengQualificationSeats { get; }

    ref long _yunzhengQualificationStamp { get; }
}
