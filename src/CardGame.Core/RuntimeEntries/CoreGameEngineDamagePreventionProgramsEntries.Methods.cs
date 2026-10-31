#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamagePreventionProgramsEntries
{
    internal void AdvanceBeforeDamageProgramCandidate(global::CardGame.Core.BeforeDamageProgramWindowFrame frame, bool activated, bool completed) => _resolve().AdvanceBeforeDamageProgramCandidate(frame, activated, completed);
    internal bool BeforeDamageHasNoSource(global::CardGame.Core.BeforeDamageProgramWindowFrame frame) => _resolve().BeforeDamageHasNoSource(frame);
    internal void ContinueBeforeDamageProgramWindowCore() => _resolve().ContinueBeforeDamageProgramWindowCore();
    internal global::CardGame.Core.ProgramSkillWindowContext CreateBeforeDamageProgramContext(global::CardGame.Core.BeforeDamageProgramWindowFrame frame, global::CardGame.Core.BeforeDamageProgramCandidate item) => _resolve().CreateBeforeDamageProgramContext(frame, item);
    internal void PreventProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program) => _resolve().PreventProgramCurrentDamage(program);
    internal void ResumeAfterBeforeDamageProgramWindow(global::CardGame.Core.BeforeDamageProgramWindowFrame frame) => _resolve().ResumeAfterBeforeDamageProgramWindow(frame);
    internal bool TryBeginBeforeDamageProgramWindow(int sourceSeat, int targetSeat, int amount, global::CardGame.Core.DamageNature nature, global::CardGame.Core.BeforeDamageProgramContinuation continuation, bool rangeChainOnly = false) => _resolve().TryBeginBeforeDamageProgramWindow(sourceSeat, targetSeat, amount, nature, continuation, rangeChainOnly);
    internal bool TryBeginBeforeDamageProgramWindowForAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount, global::CardGame.Core.DamageNature nature) => _resolve().TryBeginBeforeDamageProgramWindowForAttack(attack, amount, nature);
    internal bool TryVisitRangePreventionChainTarget(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().TryVisitRangePreventionChainTarget(attack);
    internal bool HasRangePreventionPrograms { get => _resolve().HasRangePreventionPrograms; }
}
