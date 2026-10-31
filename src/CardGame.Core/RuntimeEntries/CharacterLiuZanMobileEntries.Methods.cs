#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuZanMobileEntries
{
    internal (global::CardGame.Core.AlternateActualUseColorRecordedEvent Current, global::CardGame.Core.AlternateActualUseColorRecordedEvent Previous)? AlternateActualUseColorPair(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AlternateActualUseColorPair(owner, context);
    internal bool CanOfferAlternateActualUseColor(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferAlternateActualUseColor(candidate, trigger, context);
    internal void GrantAlternateActualUseColorDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().GrantAlternateActualUseColorDraw(supplied);
    internal bool IsAlternateColorActualUse(global::CardGame.Core.CardActionContext action) => _resolve().IsAlternateColorActualUse(action);
    internal void RecordAlternateActualUseColor(global::CardGame.Core.CardActionContext action) => _resolve().RecordAlternateActualUseColor(action);
    internal bool TracksAlternateActualUseColor { get => _resolve().TracksAlternateActualUseColor; }
}
