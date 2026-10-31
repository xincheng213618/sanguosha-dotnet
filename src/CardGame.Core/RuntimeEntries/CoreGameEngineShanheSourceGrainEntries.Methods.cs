#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceGrainEntries
{
    internal bool CanOfferShanheSourceGrain(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheSourceGrain(gate);
    internal bool IsShanheSourceGrainContext(string skillId, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().IsShanheSourceGrainContext(skillId, operation);
}
