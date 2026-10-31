#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourcePojunEntries
{
    internal bool CanHoldShanheSourcePojunHand(ProgramTriggerLegalityContext context) => _resolve().CanHoldShanheSourcePojunHand(context);
    internal SkillProgramStepOutcome HoldShanheSourcePojunTargetHand(ProgramSkillFrame supplied) => _resolve().HoldShanheSourcePojunTargetHand(supplied);
    internal bool IsShanheSourcePojunMode { get => _resolve().IsShanheSourcePojunMode; }
}
