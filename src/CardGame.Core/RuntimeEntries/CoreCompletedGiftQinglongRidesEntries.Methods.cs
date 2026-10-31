#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCompletedGiftQinglongRidesEntries
{
    internal bool IsUnselectedCompletedGiftQinglongRide(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsUnselectedCompletedGiftQinglongRide(frame);
    internal bool TryGetCompletedGiftRecipientSlashRide(global::CardGame.Core.ProgramCardTriggerWindowFrame window, scoped out global::CardGame.Core.CardUseFrame child) => _resolve().TryGetCompletedGiftRecipientSlashRide(window, out child);
}
