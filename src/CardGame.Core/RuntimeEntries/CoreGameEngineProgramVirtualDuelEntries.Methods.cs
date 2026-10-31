#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramVirtualDuelEntries
{
    internal SkillProgramStepOutcome BeginProgramVirtualDuel(ProgramSkillFrame frame) => _resolve().BeginProgramVirtualDuel(frame);
    internal int? DamageProgramUseInstructionIndex(GameEngine.IDamageAttempt attack) => _resolve().DamageProgramUseInstructionIndex(attack);
    internal int? FrozenProgramDuelInstructionIndex(long frameId) => _resolve().FrozenProgramDuelInstructionIndex(frameId);
    internal bool IsProgramVirtualDuelAttackConsistent(NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing) => _resolve().IsProgramVirtualDuelAttackConsistent(attack, processing);
}
