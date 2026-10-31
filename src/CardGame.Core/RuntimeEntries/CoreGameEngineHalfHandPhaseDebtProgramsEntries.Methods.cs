#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtProgramsEntries
{
    internal bool CanRunHalfHandPhaseDebt(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunHalfHandPhaseDebt(candidate, trigger, context);
    internal HalfHandDrawIssuedEvent? CurrentHalfHandExtraDraw(int owner, string skill, string instance, string hash, string state) => _resolve().CurrentHalfHandExtraDraw(owner, skill, instance, hash, state);
    internal HalfHandTargetSupportIssuedEvent? CurrentHalfHandSupport(int owner, string skill, string instance, string hash, string state) => _resolve().CurrentHalfHandSupport(owner, skill, instance, hash, state);
    internal PhaseHandExchangeDebtIssuedEvent? CurrentPhaseHandDebt(int owner, string skill, string instance, string hash, string state) => _resolve().CurrentPhaseHandDebt(owner, skill, instance, hash, state);
    internal SkillProgramStepOutcome DrawExtraHalfHandSupport(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().DrawExtraHalfHandSupport(frame, effect);
    internal SkillProgramStepOutcome ExchangeAndArmPhaseHandDebt(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ExchangeAndArmPhaseHandDebt(frame, effect);
    internal void FinalizeDeferredHandExchange(long frameId) => _resolve().FinalizeDeferredHandExchange(frameId);
    internal SkillProgramStepOutcome GiveHalfHandSupport(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().GiveHalfHandSupport(frame, effect);
    internal bool HalfHandDebtLifecycleMatches(ProgramSkillFrame f, SkillProgramTriggerWindow window) => _resolve().HalfHandDebtLifecycleMatches(f, window);
    internal CardConversionSource HalfHandDebtSource(ProgramSkillFrame f) => _resolve().HalfHandDebtSource(f);
    internal bool HalfHandDebtSourceCurrent(ProgramSkillFrame f) => _resolve().HalfHandDebtSourceCurrent(f);
    internal bool HasPayableDeferredHandPair(int owner) => _resolve().HasPayableDeferredHandPair(owner);
    internal bool IsPayableDeferredHandPair(int owner, int first, int second) => _resolve().IsPayableDeferredHandPair(owner, first, second);
    internal bool MatchesDeferredHandDebtCost(ProgramSkillFrame frame, string bind, Card card, CardLocation location) => _resolve().MatchesDeferredHandDebtCost(frame, bind, card, location);
    internal SkillProgramEffect PhaseHandDebtOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().PhaseHandDebtOwnedSelectionEffect(frame, effect);
    internal SkillProgramStepOutcome SelectPhaseHandDebtPayment(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().SelectPhaseHandDebtPayment(frame, effect);
    internal long HalfHandDebtSequence { get => _resolve().HalfHandDebtSequence; }
}
