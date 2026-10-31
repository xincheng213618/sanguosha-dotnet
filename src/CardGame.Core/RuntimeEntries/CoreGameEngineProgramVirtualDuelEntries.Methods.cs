#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramVirtualDuelEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramVirtualDuel(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginProgramVirtualDuel(frame);
    internal int? DamageProgramUseInstructionIndex(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().DamageProgramUseInstructionIndex(attack);
    internal int? FrozenProgramDuelInstructionIndex(long frameId) => _resolve().FrozenProgramDuelInstructionIndex(frameId);
    internal bool IsProgramVirtualDuelAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsProgramVirtualDuelAttackConsistent(attack, processing);
}
