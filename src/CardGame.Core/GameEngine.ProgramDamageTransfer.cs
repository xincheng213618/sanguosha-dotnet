namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record ProgramDamageTransferFollowup(
        string SkillId, int OwnerSeat, int TargetSeat, bool DrawLostHp);
}
