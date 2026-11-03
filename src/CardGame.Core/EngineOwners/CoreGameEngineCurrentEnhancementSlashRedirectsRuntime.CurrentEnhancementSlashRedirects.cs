#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCurrentEnhancementSlashRedirectsRuntime
{
    // This public notification has projection consumers. Native target state is
    // already committed by the owning boundary; it is not another execution ledger.
    internal void RecordCurrentEnhancementSlashRedirect(ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat)
    {
        if (frames.Find(useId) is not CardUseFrame
            { CardId: 0, CardKind: CardKind.Slash, PhysicalCardIds.Count: 0,
              ProgramUseCommitted: true, TargetSeats.Count: 2, CurrentSlashFirePolicy: null,
              CardAttack: { ProgramSkillCardUseFrameId: { } parentId },
              Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0 } action } use ||
            !use.Enhancements.HasFlag(CurrentCardEnhancement.ExtraTarget) ||
            frames.Find(parentId) is not ProgramSkillFrame parent ||
            ProgramInstructionResolver.Default.Resolve(parent, content.Skill(parent.SkillId).Program!)
                .GetPausedInstruction(parent.InstructionIndex).Effect is not
                { Op: SkillProgramEffectOp.UseVirtualCard, OutputKind: CardKind.Slash })
            return;
        var before = use.TargetSeats.ToArray();
        before[use.TargetIndex] = previousSeat;
        events.AdvanceFact(new CurrentEnhancementSlashRedirectedEvent(redirector.Id, use.Id, action.ActionId,
            new(redirector.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(redirector),
                redirector.OwnerSeat, redirector.SkillInstanceId), redirector.GameplayHash,
            redirector.InstructionIndex, use.TargetIndex, previousSeat, newSeat,
            Array.AsReadOnly(before), Array.AsReadOnly(use.TargetSeats.ToArray()),
            redirector.WindowContext!.ParentFrameId));
    }
}
