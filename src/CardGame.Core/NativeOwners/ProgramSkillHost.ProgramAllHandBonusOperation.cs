#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost
{
    public void DrawAllHandSelectedBonus(ProgramSkillFrame frame, int amount)
    {
        if (frame.SelectedAllOwnerHandCards is not true)
            return;
        getCoreGameEngineProgramLifecycleRuntime().DrawProgramCards(frame.Id, frame.OwnerSeat, amount, null, null, SkillProgramCardSetVisibility.Private, new CardMoveReason($"skill-program.{frame.SkillId}.{frame.ActivationId}.all-hand-bonus"));
    }
}
