#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceCaoSongTrioEntries
{
    internal bool CanOfferCaoSongSourceTargets(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferCaoSongSourceTargets(trigger, context, ownerSeat);
    internal IReadOnlyList<int>? CaoSongSourceTargetSeats(int ownerSeat, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => _resolve().CaoSongSourceTargetSeats(ownerSeat, kind, context);
}
