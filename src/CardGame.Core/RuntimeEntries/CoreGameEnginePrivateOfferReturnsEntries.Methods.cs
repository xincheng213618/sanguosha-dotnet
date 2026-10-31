#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateOfferReturnsEntries
{
    internal void AssertPrivateOfferFrames() => _resolve().AssertPrivateOfferFrames();
    internal global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GetPausedPrivateOfferEffect(f);
    internal bool IsPrivateOfferMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPrivateOfferMovement(f, effect, pending);
    internal bool PrivateOfferSegmentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PrivateOfferStage stage, int id, global::CardGame.Core.CardLocation? from, global::CardGame.Core.CardLocation to, string reason) => _resolve().PrivateOfferSegmentMatches(f, stage, id, from, to, reason);
    internal bool ResumePrivateOffer(long id) => _resolve().ResumePrivateOffer(id);
    internal bool ReturnPrivateOfferMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPrivateOfferMovement(f);
    internal bool ValidPrivateOfferReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPrivateOfferReceipt(f);
}
