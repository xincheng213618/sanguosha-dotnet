#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceGrainEntries
{
    internal bool CanOfferShanheSourceGrain(ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheSourceGrain(gate);
    internal bool IsShanheSourceGrainContext(string skillId, SkillProgramEffectOp operation) => _resolve().IsShanheSourceGrainContext(skillId, operation);
}
