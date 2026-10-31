#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourcePojunEntries
{
    internal bool CanHoldShanheSourcePojunHand(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanHoldShanheSourcePojunHand(context);
    internal global::CardGame.Core.SkillProgramStepOutcome HoldShanheSourcePojunTargetHand(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().HoldShanheSourcePojunTargetHand(supplied);
    internal bool IsShanheSourcePojunMode { get => _resolve().IsShanheSourcePojunMode; }
}
