#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateOfferReturnsEntries
{
    internal SkillProgramEffect? GetPausedPrivateOfferEffect(ProgramSkillFrame f) => _resolve().GetPausedPrivateOfferEffect(f);
    internal bool IsPrivateOfferMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsPrivateOfferMovement(f, effect, pending);
    internal bool PrivateOfferSegmentMatches(ProgramSkillFrame f, PrivateOfferStage stage, int id, CardLocation? from, CardLocation to, string reason) => _resolve().PrivateOfferSegmentMatches(f, stage, id, from, to, reason);
    internal bool ResumePrivateOffer(long id) => _resolve().ResumePrivateOffer(id);
    internal bool ReturnPrivateOfferMovement(ProgramSkillFrame f) => _resolve().ReturnPrivateOfferMovement(f);
    internal bool ValidPrivateOfferReceipt(ProgramSkillFrame f) => _resolve().ValidPrivateOfferReceipt(f);
}
