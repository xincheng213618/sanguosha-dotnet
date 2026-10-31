#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenDengOlEntries
{
    internal void CommitFengjiOption(global::CardGame.Core.ProgramSkillFrame active, global::CardGame.Core.FengjiOptionKind option, bool accepted, int recipientSeat) => _resolve().CommitFengjiOption(active, option, accepted, recipientSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramFengjiOptionChosenEvent> CurrentRoundFengjiOptions(int ownerSeat) => _resolve().CurrentRoundFengjiOptions(ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome FengjiProgramRoundChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().FengjiProgramRoundChoice(frame, effect);
    internal int GetFengjiRoundAdjustment(int seat, global::CardGame.Core.SkillRuleQuery query) => _resolve().GetFengjiRoundAdjustment(seat, query);
    internal bool IsFengjiRoundChoicePending(int ownerSeat) => _resolve().IsFengjiRoundChoicePending(ownerSeat);
    internal bool IsFengjiSwapAvailable(int ownerSeat) => _resolve().IsFengjiSwapAvailable(ownerSeat);
    internal bool IsXuanhuiDisabledByOwnSwap(int ownerSeat) => _resolve().IsXuanhuiDisabledByOwnSwap(ownerSeat);
    internal global::CardGame.Core.FengjiOptionKind? PendingFengjiOption(int ownerSeat) => _resolve().PendingFengjiOption(ownerSeat);
    internal void PresentFengjiRecipientPrompt(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.FengjiOptionKind option) => _resolve().PresentFengjiRecipientPrompt(frame, option);
    internal void ResolveFengjiOptionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFengjiOptionChoice(selected);
    internal void ResolveFengjiRecipientChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFengjiRecipientChoice(selected);
    internal void ResumeFengjiRoundChoice(global::CardGame.Core.ProgramSkillFrame active) => _resolve().ResumeFengjiRoundChoice(active);
    internal global::CardGame.Core.SkillProgramStepOutcome XuanhuiProgramSwapEffects(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().XuanhuiProgramSwapEffects(frame, effect);
    internal bool TracksFengjiRoundLedger { get => _resolve().TracksFengjiRoundLedger; }
}
