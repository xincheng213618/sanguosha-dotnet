#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCompoundCardTargetsEntries
{
    internal void ResolveProgramCompoundCardTarget(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramCompoundCardTarget(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectProgramAddedBorrowedSwordVictim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardUseFrame use, int weaponOwner) => _resolve().SelectProgramAddedBorrowedSwordVictim(frame, use, weaponOwner);
}
