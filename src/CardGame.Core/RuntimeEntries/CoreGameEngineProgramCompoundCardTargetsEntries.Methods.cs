#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCompoundCardTargetsEntries
{
    internal void ResolveProgramCompoundCardTarget(PromptChoice selected) => _resolve().ResolveProgramCompoundCardTarget(selected);
    internal SkillProgramStepOutcome SelectProgramAddedBorrowedSwordVictim(ProgramSkillFrame frame, CardUseFrame use, int weaponOwner) => _resolve().SelectProgramAddedBorrowedSwordVictim(frame, use, weaponOwner);
}
