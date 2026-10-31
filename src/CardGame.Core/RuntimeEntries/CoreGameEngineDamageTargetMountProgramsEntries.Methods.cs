#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetMountProgramsEntries
{
    internal CardUseFrame? ActualDamageTargetMountUse(int ownerSeat, ProgramSkillWindowContext context) => _resolve().ActualDamageTargetMountUse(ownerSeat, context);
    internal SkillProgramStepOutcome BeginDamageTargetMount(ProgramSkillFrame f) => _resolve().BeginDamageTargetMount(f);
    internal bool CanContinuePaidDamageTargetMount(ProgramSkillFrame f) => _resolve().CanContinuePaidDamageTargetMount(f);
    internal bool CanOfferDamageTargetMount(int ownerSeat, ProgramSkillWindowContext context, string? skillId = default) => _resolve().CanOfferDamageTargetMount(ownerSeat, context, skillId);
    internal bool CanOfferDamageTargetMountEdition(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanOfferDamageTargetMountEdition(candidate, context);
    internal IReadOnlyList<PromptChoice> DamageTargetMountChoices(long frameId, int ownerSeat, int targetSeat, string? skillId = default) => _resolve().DamageTargetMountChoices(frameId, ownerSeat, targetSeat, skillId);
    internal bool IsValidDamageTargetMount(ProgramSkillFrame f) => _resolve().IsValidDamageTargetMount(f);
    internal void ResolveDamageTargetMount(PromptChoice choice) => _resolve().ResolveDamageTargetMount(choice);
    internal bool ResumeDamageTargetMount(long id) => _resolve().ResumeDamageTargetMount(id);
}
