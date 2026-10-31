#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtProgramsEntries
{
    internal bool CanRunHalfHandPhaseDebt(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunHalfHandPhaseDebt(candidate, trigger, context);
    internal global::CardGame.Core.HalfHandDrawIssuedEvent? CurrentHalfHandExtraDraw(int owner, string skill, string instance, string hash, string state) => _resolve().CurrentHalfHandExtraDraw(owner, skill, instance, hash, state);
    internal global::CardGame.Core.HalfHandTargetSupportIssuedEvent? CurrentHalfHandSupport(int owner, string skill, string instance, string hash, string state) => _resolve().CurrentHalfHandSupport(owner, skill, instance, hash, state);
    internal global::CardGame.Core.PhaseHandExchangeDebtIssuedEvent? CurrentPhaseHandDebt(int owner, string skill, string instance, string hash, string state) => _resolve().CurrentPhaseHandDebt(owner, skill, instance, hash, state);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawExtraHalfHandSupport(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().DrawExtraHalfHandSupport(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ExchangeAndArmPhaseHandDebt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExchangeAndArmPhaseHandDebt(frame, effect);
    internal void FinalizeDeferredHandExchange(long frameId) => _resolve().FinalizeDeferredHandExchange(frameId);
    internal global::CardGame.Core.SkillProgramStepOutcome GiveHalfHandSupport(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().GiveHalfHandSupport(frame, effect);
    internal bool HalfHandDebtLifecycleMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().HalfHandDebtLifecycleMatches(f, window);
    internal global::CardGame.Core.CardConversionSource HalfHandDebtSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HalfHandDebtSource(f);
    internal bool HalfHandDebtSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HalfHandDebtSourceCurrent(f);
    internal bool HasPayableDeferredHandPair(int owner) => _resolve().HasPayableDeferredHandPair(owner);
    internal bool IsPayableDeferredHandPair(int owner, int first, int second) => _resolve().IsPayableDeferredHandPair(owner, first, second);
    internal bool MatchesDeferredHandDebtCost(global::CardGame.Core.ProgramSkillFrame frame, string bind, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => _resolve().MatchesDeferredHandDebtCost(frame, bind, card, location);
    internal global::CardGame.Core.SkillProgramEffect PhaseHandDebtOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PhaseHandDebtOwnedSelectionEffect(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectPhaseHandDebtPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectPhaseHandDebtPayment(frame, effect);
    internal long HalfHandDebtSequence { get => _resolve().HalfHandDebtSequence; }
}
