#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMuluEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheMuluRevival(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheMuluRevival(supplied);
    internal bool CanCompleteShanheMuluRevival(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanCompleteShanheMuluRevival(frame);
    internal void ContinueShanheMuluRevival(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ContinueShanheMuluRevival(supplied);
    internal void FreezeShanheElephantRoundDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().FreezeShanheElephantRoundDamage(supplied);
    internal bool ResumeShanheMuluRevival(long id) => _resolve().ResumeShanheMuluRevival(id);
    internal bool ReturnShanheMuluRevivalMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnShanheMuluRevivalMovement(frame);
    internal int ShanheElephantFrozenDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShanheElephantFrozenDamageBonus(attack);
}
