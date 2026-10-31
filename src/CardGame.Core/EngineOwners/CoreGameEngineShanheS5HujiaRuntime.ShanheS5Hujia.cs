#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5HujiaRuntime
{
    internal void ClaimShanheS5JianxiongCards(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "shanhe:s5-jianxiong-14198" || frame.TriggerId != "lost-hp-draw-then-claim" || frame.InstructionIndex != 2 || frame.NativeDraw is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var parentId, DamageFrameId: var damageId } || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } window || window.Id != parentId || window.ParentFrameId != damageId || window.TargetSeat != frame.OwnerSeat)
            throw new InvalidOperationException("S5 Jianxiong must return its native Draw before claiming the original damage's physical cards.");
        // This existing helper rechecks the causal attack and Processing zone,
        // retains group/extra-effect claim proofs, and returns normally if no
        // original material remains after the draw's actual child windows.
        _coreProgramLifecycleOperations.ClaimProgramDamageCards(frame.Id, frame.OwnerSeat, new($"skill-program.{(frame.SkillId)}.ClaimDamageCards"));
    }

    internal void BindShanheS5HujiaReward(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "shanhe:s5-hujia-995" || f.ShanheS5Hujia is not null || f.InstructionIndex != 1 || f.WindowContext is not { OptionalChooserSeat: { } chooser } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramCardTriggerWindowFrame parent || parent.Id != context.ParentFrameId || context.CardUse?.CardActionId != parent.Action.ActionId || context.Window != SkillProgramTriggerWindow.CardResponseAccepted || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] is not { } candidate || candidate.SkillId != f.SkillId || candidate.SkillInstanceId != f.SkillInstanceId || candidate.OwnerSeat != f.OwnerSeat || candidate.TriggerId != f.TriggerId || parent.Action.EffectiveKind != CardKind.Dodge || _coreProgramFactionActionRewardOperations.FactionActionRewardChooser(f.OwnerSeat, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Triggers.Single(t => t.Id == f.TriggerId), parent.Action) != chooser || !_coreLordSkillProjectionsOperations.HasSkillRoleQualification(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId, Role.Lord))
            throw new InvalidOperationException("S5 Hujia lost its actual qualified Lord, off-turn Wei chooser or native Dodge action.");
        var receipt = new ShanheS5HujiaRewardReceipt(new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, parent.Action.ActionId, parent.Id, chooser, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5Hujia = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5HujiaRewardIssuedEvent(f.Id, receipt));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 59600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 59600)]
    internal bool ContinueAcceptedShanheS5HujiaReward(ProgramSkillFrame f)
    {
        if (f.ShanheS5Hujia is null)
            return false;
        return true;
    }

    void IShanheS5HujiaHost.BindShanheS5HujiaReward(ProgramSkillFrame frame) => BindShanheS5HujiaReward(frame);
    void IShanheS5JianxiongHost.ClaimShanheS5JianxiongCards(ProgramSkillFrame frame) => ClaimShanheS5JianxiongCards(frame);
}
