#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundPricedPileAlcoholEntries
{
    internal void AssertRoundPileAlcohol(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertRoundPileAlcohol(f);
    internal void AssertRoundPileAlcoholUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertRoundPileAlcoholUse(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRoundPileAlcohol(global::CardGame.Core.ProgramSkillFrame input, string stateId, global::CardGame.Core.CardZoneKind zone) => _resolve().BeginRoundPileAlcohol(input, stateId, zone);
    internal bool CanOfferRoundPileAlcohol(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferRoundPileAlcohol(candidate, trigger, context);
    internal void ContinueRoundPileAlcoholUse(long id) => _resolve().ContinueRoundPileAlcoholUse(id);
    internal bool IsRoundPricedPileAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().IsRoundPricedPileAlcoholRide(dyingIndex, dying);
    internal int NextRoundPileAlcoholPrice(int ownerSeat, string skillId, string stateId) => _resolve().NextRoundPileAlcoholPrice(ownerSeat, skillId, stateId);
    internal void PublishRoundPileAlcohol(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishRoundPileAlcohol(f);
    internal void ResolveRoundPileAlcohol(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveRoundPileAlcohol(choice);
    internal bool ResumeRoundPileAlcohol(long id) => _resolve().ResumeRoundPileAlcohol(id);
    internal void ReturnRoundPileAlcoholUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnRoundPileAlcoholUse(use);
    internal bool RoundPileAlcoholUnpaidValid(global::CardGame.Core.ProgramSkillFrame f, long dyingId, int victimSeat, string stateId, int round, int price, global::CardGame.Core.CardZoneKind zone) => _resolve().RoundPileAlcoholUnpaidValid(f, dyingId, victimSeat, stateId, round, price, zone);
}
