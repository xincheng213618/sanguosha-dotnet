#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceCaoSongTrioEntries
{
    internal bool CanOfferCaoSongSourceTargets(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferCaoSongSourceTargets(trigger, context, ownerSeat);
    internal global::System.Collections.Generic.IReadOnlyList<int>? CaoSongSourceTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind kind, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().CaoSongSourceTargetSeats(ownerSeat, kind, context);
}
