#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuZanMobileEntries
{
    internal (AlternateActualUseColorRecordedEvent Current, AlternateActualUseColorRecordedEvent Previous)? AlternateActualUseColorPair(int owner, ProgramSkillWindowContext context) => _resolve().AlternateActualUseColorPair(owner, context);
    internal bool CanOfferAlternateActualUseColor(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferAlternateActualUseColor(candidate, trigger, context);
    internal void GrantAlternateActualUseColorDraw(ProgramSkillFrame supplied) => _resolve().GrantAlternateActualUseColorDraw(supplied);
    internal bool IsAlternateColorActualUse(CardActionContext action) => _resolve().IsAlternateColorActualUse(action);
    internal void RecordAlternateActualUseColor(CardActionContext action) => _resolve().RecordAlternateActualUseColor(action);
    internal bool TracksAlternateActualUseColor { get => _resolve().TracksAlternateActualUseColor; }
}
