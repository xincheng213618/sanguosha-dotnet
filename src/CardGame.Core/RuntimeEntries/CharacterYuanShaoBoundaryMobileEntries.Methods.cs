#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanShaoBoundaryMobileEntries
{
    internal bool CanOfferMobileYuanShaoDraw(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMobileYuanShaoDraw(candidate, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawMobileYuanShao(global::CardGame.Core.ProgramSkillFrame supplied, bool responder) => _resolve().DrawMobileYuanShao(supplied, responder);
    internal bool IsMobileYuanShaoViewAsAllowed(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource source) => _resolve().IsMobileYuanShaoViewAsAllowed(owner, cardIds, source);
    internal int? MobileYuanShaoResponder(global::CardGame.Core.ProgramSkillWindowContext context, int owner) => _resolve().MobileYuanShaoResponder(context, owner);
    internal bool MobileYuanShaoSuitAvailable(int owner, global::CardGame.Core.Suit suit) => _resolve().MobileYuanShaoSuitAvailable(owner, suit);
    internal void RecordMobileYuanShaoSuits(global::CardGame.Core.ProgramViewAsConvertedEvent fact) => _resolve().RecordMobileYuanShaoSuits(fact);
    internal void ReturnMobileYuanShaoDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileYuanShaoDraw(frame, outcome);
}
