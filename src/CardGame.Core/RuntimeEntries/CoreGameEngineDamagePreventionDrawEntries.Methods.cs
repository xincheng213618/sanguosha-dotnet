#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamagePreventionDrawEntries
{
    internal bool IsPreventionDrawMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paidEffect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsPreventionDrawMovement(frame, paidEffect, movement);
    internal global::CardGame.Core.SkillProgramStepOutcome PreventDamageAndDrawMultiple(global::CardGame.Core.ProgramSkillFrame frame, int multiplier) => _resolve().PreventDamageAndDrawMultiple(frame, multiplier);
}
