#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRongBoundaryMobileEntries
{
    internal void CaptureMobileZhuRongPindianReturn(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.PindianFrame native) => _resolve().CaptureMobileZhuRongPindianReturn(parent, native);
    internal global::CardGame.Core.SkillProgramStepOutcome ExchangeMobileZhuRongCards(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ExchangeMobileZhuRongCards(supplied);
    internal bool MobileZhuRongExchangeMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().MobileZhuRongExchangeMovement(frame, effect, pending);
}
