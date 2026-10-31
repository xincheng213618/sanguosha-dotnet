#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess : IProgramRuntimeOperations
{
    ref (long Inner, long Movement, int Initial, int Shown) _yunzhengQualificationInputs { get; }

    ref GameEngine.YunzhengQualificationSeat[] _yunzhengQualificationSeats { get; }

    ref long _yunzhengQualificationStamp { get; }
}
