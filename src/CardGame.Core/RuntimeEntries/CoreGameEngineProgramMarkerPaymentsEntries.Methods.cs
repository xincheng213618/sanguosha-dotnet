#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMarkerPaymentsEntries
{
    internal void PayProgramMarkerCost(CharacterState owner, SkillProgramMarkerCost? cost, string skillId, string bindingId) => _resolve().PayProgramMarkerCost(owner, cost, skillId, bindingId);
}
