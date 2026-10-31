#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramCompoundCardTargetsRuntime
{
    internal SkillProgramStepOutcome SelectProgramAddedBorrowedSwordVictim(ProgramSkillFrame frame, CardUseFrame use, int weaponOwner)
    {
        var choices = _coreMatchParticipantsPlayersAccess._players.Where(victim => _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[weaponOwner], victim)).Select(victim => new PromptChoice(new ChoiceId($"program-compound-target.frame-{(frame.Id)}.victim-{(victim.Seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[weaponOwner].Name)} 对 {(victim.Name)} 使用杀", [], [victim.Seat], new Dictionary<string, string> { ["program-action"] = "add-compound-card-target", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-use-id"] = use.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["weapon-owner"] = weaponOwner.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        if (choices.Length == 0)
            throw new InvalidOperationException("A borrowed-sword invitee lost all legal Slash victims.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, "为借刀杀人的额外目标选择杀的目标。", [], choices.SelectMany(choice => choice.Targets).ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = weaponOwner,
            IsPrivate = false,
            SkillPrompt = new(frame.SkillId, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Name, "选择借刀的杀目标", _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Description),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("add-compound-card-target")]
    internal void ResolveProgramCompoundCardTarget(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A compound target lost its frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var use = _coreProgramCardUseRolesOperations.GetProgramRoleCardUse(frame);
        var owner = frame.SelectedTargetSeats.Single();
        if (effect.Op != SkillProgramEffectOp.AddCurrentCardUseTarget || use.CardKind != CardKind.BorrowedSword || selected.Parameters.GetValueOrDefault("program-action") != "add-compound-card-target" || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("card-use-id") != use.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("weapon-owner") != owner.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Cards.Count != 0 || selected.Targets.Count != 1 || !_coreProgramCardUseRolesOperations.GetProgramCardUseRoleTargets(frame).Contains(owner) || !_coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[selected.Targets[0]]))
            throw new InvalidOperationException("The compound target answer lost its frozen instruction or legal participants.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreProgramCardUseRolesOperations.CompleteProgramCardUseTargetAddition(frame, use, [..use.TargetSeats, owner, selected.Targets[0]], owner);
        _coreCardUseLifecycleOperations.TryMarkTargetsAdjusted(use.Id);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
