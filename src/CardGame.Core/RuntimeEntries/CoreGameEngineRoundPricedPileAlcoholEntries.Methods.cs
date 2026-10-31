#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundPricedPileAlcoholEntries
{
    internal void AssertRoundPileAlcohol(ProgramSkillFrame f) => _resolve().AssertRoundPileAlcohol(f);
    internal void AssertRoundPileAlcoholUse(CardUseFrame use) => _resolve().AssertRoundPileAlcoholUse(use);
    internal SkillProgramStepOutcome BeginRoundPileAlcohol(ProgramSkillFrame input, string stateId, CardZoneKind zone) => _resolve().BeginRoundPileAlcohol(input, stateId, zone);
    internal bool CanOfferRoundPileAlcohol(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferRoundPileAlcohol(candidate, trigger, context);
    internal void ContinueRoundPileAlcoholUse(long id) => _resolve().ContinueRoundPileAlcoholUse(id);
    internal bool IsRoundPricedPileAlcoholRide(int dyingIndex, DyingFrame dying) => _resolve().IsRoundPricedPileAlcoholRide(dyingIndex, dying);
    internal int NextRoundPileAlcoholPrice(int ownerSeat, string skillId, string stateId) => _resolve().NextRoundPileAlcoholPrice(ownerSeat, skillId, stateId);
    internal void PublishRoundPileAlcohol(ProgramSkillFrame f) => _resolve().PublishRoundPileAlcohol(f);
    internal void ResolveRoundPileAlcohol(PromptChoice choice) => _resolve().ResolveRoundPileAlcohol(choice);
    internal bool ResumeRoundPileAlcohol(long id) => _resolve().ResumeRoundPileAlcohol(id);
    internal void ReturnRoundPileAlcoholUse(CardUseFrame use) => _resolve().ReturnRoundPileAlcoholUse(use);
    internal bool RoundPileAlcoholUnpaidValid(ProgramSkillFrame f, long dyingId, int victimSeat, string stateId, int round, int price, CardZoneKind zone) => _resolve().RoundPileAlcoholUnpaidValid(f, dyingId, victimSeat, stateId, round, price, zone);
}
