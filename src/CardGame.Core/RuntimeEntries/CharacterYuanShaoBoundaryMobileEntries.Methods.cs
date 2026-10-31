#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanShaoBoundaryMobileEntries
{
    internal bool CanOfferMobileYuanShaoDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMobileYuanShaoDraw(candidate, trigger, context);
    internal SkillProgramStepOutcome DrawMobileYuanShao(ProgramSkillFrame supplied, bool responder) => _resolve().DrawMobileYuanShao(supplied, responder);
    internal bool IsMobileYuanShaoViewAsAllowed(CharacterState owner, IReadOnlyList<int> cardIds, CardConversionSource source) => _resolve().IsMobileYuanShaoViewAsAllowed(owner, cardIds, source);
    internal int? MobileYuanShaoResponder(ProgramSkillWindowContext context, int owner) => _resolve().MobileYuanShaoResponder(context, owner);
    internal bool MobileYuanShaoSuitAvailable(int owner, Suit suit) => _resolve().MobileYuanShaoSuitAvailable(owner, suit);
    internal void RecordMobileYuanShaoSuits(ProgramViewAsConvertedEvent fact) => _resolve().RecordMobileYuanShaoSuits(fact);
    internal void ReturnMobileYuanShaoDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnMobileYuanShaoDraw(frame, outcome);
}
