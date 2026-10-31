#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamagePreventionProgramsEntries
{
    internal void AdvanceBeforeDamageProgramCandidate(BeforeDamageProgramWindowFrame frame, bool activated, bool completed) => _resolve().AdvanceBeforeDamageProgramCandidate(frame, activated, completed);
    internal bool BeforeDamageHasNoSource(BeforeDamageProgramWindowFrame frame) => _resolve().BeforeDamageHasNoSource(frame);
    internal void ContinueBeforeDamageProgramWindowCore() => _resolve().ContinueBeforeDamageProgramWindowCore();
    internal ProgramSkillWindowContext CreateBeforeDamageProgramContext(BeforeDamageProgramWindowFrame frame, BeforeDamageProgramCandidate item) => _resolve().CreateBeforeDamageProgramContext(frame, item);
    internal void PreventProgramCurrentDamage(ProgramSkillFrame program) => _resolve().PreventProgramCurrentDamage(program);
    internal void ResumeAfterBeforeDamageProgramWindow(BeforeDamageProgramWindowFrame frame) => _resolve().ResumeAfterBeforeDamageProgramWindow(frame);
    internal bool TryBeginBeforeDamageProgramWindow(int sourceSeat, int targetSeat, int amount, DamageNature nature, BeforeDamageProgramContinuation continuation, bool rangeChainOnly = false) => _resolve().TryBeginBeforeDamageProgramWindow(sourceSeat, targetSeat, amount, nature, continuation, rangeChainOnly);
    internal bool TryBeginBeforeDamageProgramWindowForAttack(GameEngine.IDamageAttempt attack, int amount, DamageNature nature) => _resolve().TryBeginBeforeDamageProgramWindowForAttack(attack, amount, nature);
    internal bool TryVisitRangePreventionChainTarget(GameEngine.IDamageAttempt attack) => _resolve().TryVisitRangePreventionChainTarget(attack);
    internal bool HasRangePreventionPrograms { get => _resolve().HasRangePreventionPrograms; }
}
