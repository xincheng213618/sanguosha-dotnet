#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDefaultStatsEntries
{
    internal SkillProgramStepOutcome BeginTurnDefaultStatAllocation(ProgramSkillFrame f, string stateId) => _resolve().BeginTurnDefaultStatAllocation(f, stateId);
    internal bool CanOfferTurnDefaultStatProgram(int ownerSeat, string skillId, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferTurnDefaultStatProgram(ownerSeat, skillId, trigger, context);
    internal SkillProgramStepOutcome ExecuteTurnDefaultStatOperation(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteTurnDefaultStatOperation(supplied, effect);
    internal int? GetCurrentTurnDefaultStat(int seat, SkillRuleQuery query) => _resolve().GetCurrentTurnDefaultStat(seat, query);
    internal SkillProgramStepOutcome IncreaseNextTurnDefaultStatMinimum(ProgramSkillFrame f, string stateId) => _resolve().IncreaseNextTurnDefaultStatMinimum(f, stateId);
    internal void ObserveTurnDefaultStats(IGameEvent payload) => _resolve().ObserveTurnDefaultStats(payload);
    internal void PublishTurnDefaultStatChoice(ProgramSkillFrame f) => _resolve().PublishTurnDefaultStatChoice(f);
    internal void RecordTurnDefaultStatFact(IGameEvent fact) => _resolve().RecordTurnDefaultStatFact(fact);
    internal void ResolveTurnDefaultStatChoice(PromptChoice choice) => _resolve().ResolveTurnDefaultStatChoice(choice);
    internal IReadOnlyList<PromptChoice> TurnDefaultStatChoices(ProgramSkillFrame f) => _resolve().TurnDefaultStatChoices(f);
    internal bool TracksTurnDefaultStats { get => _resolve().TracksTurnDefaultStats; }
}
