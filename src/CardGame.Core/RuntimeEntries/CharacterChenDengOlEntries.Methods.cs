#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenDengOlEntries
{
    internal void CommitFengjiOption(ProgramSkillFrame active, FengjiOptionKind option, bool accepted, int recipientSeat) => _resolve().CommitFengjiOption(active, option, accepted, recipientSeat);
    internal IReadOnlyList<ProgramFengjiOptionChosenEvent> CurrentRoundFengjiOptions(int ownerSeat) => _resolve().CurrentRoundFengjiOptions(ownerSeat);
    internal SkillProgramStepOutcome FengjiProgramRoundChoice(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().FengjiProgramRoundChoice(frame, effect);
    internal int GetFengjiRoundAdjustment(int seat, SkillRuleQuery query) => _resolve().GetFengjiRoundAdjustment(seat, query);
    internal bool IsFengjiRoundChoicePending(int ownerSeat) => _resolve().IsFengjiRoundChoicePending(ownerSeat);
    internal bool IsFengjiSwapAvailable(int ownerSeat) => _resolve().IsFengjiSwapAvailable(ownerSeat);
    internal bool IsXuanhuiDisabledByOwnSwap(int ownerSeat) => _resolve().IsXuanhuiDisabledByOwnSwap(ownerSeat);
    internal FengjiOptionKind? PendingFengjiOption(int ownerSeat) => _resolve().PendingFengjiOption(ownerSeat);
    internal void PresentFengjiRecipientPrompt(ProgramSkillFrame frame, FengjiOptionKind option) => _resolve().PresentFengjiRecipientPrompt(frame, option);
    internal void ResolveFengjiOptionChoice(PromptChoice selected) => _resolve().ResolveFengjiOptionChoice(selected);
    internal void ResolveFengjiRecipientChoice(PromptChoice selected) => _resolve().ResolveFengjiRecipientChoice(selected);
    internal void ResumeFengjiRoundChoice(ProgramSkillFrame active) => _resolve().ResumeFengjiRoundChoice(active);
    internal SkillProgramStepOutcome XuanhuiProgramSwapEffects(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().XuanhuiProgramSwapEffects(frame, effect);
    internal bool TracksFengjiRoundLedger { get => _resolve().TracksFengjiRoundLedger; }
}
