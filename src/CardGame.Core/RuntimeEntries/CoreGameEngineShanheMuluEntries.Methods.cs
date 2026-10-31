#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMuluEntries
{
    internal SkillProgramStepOutcome BeginShanheMuluRevival(ProgramSkillFrame supplied) => _resolve().BeginShanheMuluRevival(supplied);
    internal bool CanCompleteShanheMuluRevival(ProgramSkillFrame frame) => _resolve().CanCompleteShanheMuluRevival(frame);
    internal void ContinueShanheMuluRevival(ProgramSkillFrame supplied) => _resolve().ContinueShanheMuluRevival(supplied);
    internal void FreezeShanheElephantRoundDamage(ProgramSkillFrame supplied) => _resolve().FreezeShanheElephantRoundDamage(supplied);
    internal bool ResumeShanheMuluRevival(long id) => _resolve().ResumeShanheMuluRevival(id);
    internal bool ReturnShanheMuluRevivalMovement(ProgramSkillFrame frame) => _resolve().ReturnShanheMuluRevivalMovement(frame);
    internal int ShanheElephantFrozenDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShanheElephantFrozenDamageBonus(attack);
}
