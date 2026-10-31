#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnDefaultStatsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginTurnDefaultStatAllocation(global::CardGame.Core.ProgramSkillFrame f, string stateId) => _resolve().BeginTurnDefaultStatAllocation(f, stateId);
    internal bool CanOfferTurnDefaultStatProgram(int ownerSeat, string skillId, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferTurnDefaultStatProgram(ownerSeat, skillId, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteTurnDefaultStatOperation(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteTurnDefaultStatOperation(supplied, effect);
    internal int? GetCurrentTurnDefaultStat(int seat, global::CardGame.Core.SkillRuleQuery query) => _resolve().GetCurrentTurnDefaultStat(seat, query);
    internal global::CardGame.Core.SkillProgramStepOutcome IncreaseNextTurnDefaultStatMinimum(global::CardGame.Core.ProgramSkillFrame f, string stateId) => _resolve().IncreaseNextTurnDefaultStatMinimum(f, stateId);
    internal void ObserveTurnDefaultStats(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveTurnDefaultStats(payload);
    internal void PublishTurnDefaultStatChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishTurnDefaultStatChoice(f);
    internal void RecordTurnDefaultStatFact(global::CardGame.Core.IGameEvent fact) => _resolve().RecordTurnDefaultStatFact(fact);
    internal void ResolveTurnDefaultStatChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveTurnDefaultStatChoice(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TurnDefaultStatChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TurnDefaultStatChoices(f);
    internal bool TracksTurnDefaultStats { get => _resolve().TracksTurnDefaultStats; }
}
