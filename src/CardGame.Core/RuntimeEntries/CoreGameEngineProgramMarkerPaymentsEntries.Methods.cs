#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMarkerPaymentsEntries
{
    internal void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId) => _resolve().PayProgramMarkerCost(owner, cost, skillId, bindingId);
}
