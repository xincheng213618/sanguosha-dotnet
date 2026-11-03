#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedCategoryUseProofEntries
{
    internal bool ActualHandGainMutationBinding(long frameId, string skill, int owner, SkillProgramTriggerWindow window, SkillProgramEffectOp operation) => _resolve().ActualHandGainMutationBinding(frameId, skill, owner, window, operation);
    internal bool HasExactAcceptedActualHandGainUse(CardUseFrame use, CardActionContext completed) => _resolve().HasExactAcceptedActualHandGainUse(use, completed);
}
