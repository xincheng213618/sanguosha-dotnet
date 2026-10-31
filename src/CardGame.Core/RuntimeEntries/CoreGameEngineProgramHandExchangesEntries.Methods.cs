#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandExchangesEntries
{
    internal SkillProgramStepOutcome ExchangeProgramSelectedTargetHands(ProgramSkillFrame frame) => _resolve().ExchangeProgramSelectedTargetHands(frame);
    internal SkillProgramStepOutcome ExchangeProgramSelectedTargetHands2(ProgramSkillFrame frame, IReadOnlyList<int> seats) => _resolve().ExchangeProgramSelectedTargetHands(frame, seats);
}
