#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuZhiAnniversaryRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IAnniversaryZhuZhiHost.Qualify(ProgramSkillFrame frame, string resultBind) => QualifyAnniversaryAnguo(frame, resultBind);
    internal SkillProgramStepOutcome QualifyAnniversaryAnguo(ProgramSkillFrame supplied, string bind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != AnniversaryZhuZhiComposition.Skill || f.ActivationId != AnniversaryZhuZhiComposition.Activation || f.TriggerId is not null || f.SelectedTargetSeats is not [var selected] || selected == f.OwnerSeat || !AnniversaryZhuZhiComposition.Branches.Contains(bind) || f.ChoiceBindings.Any(b => b.Name == bind) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { Op: SkillProgramEffectOp.ResolveAnniversaryZhuZhiAnguo } paused || paused.ResultBind != bind)
            throw new InvalidOperationException("Anguo must qualify its original accepted other participant and current unbound branch.");
        var own = bind.StartsWith("owner-", StringComparison.Ordinal);
        var recipient = own ? f.OwnerSeat : selected;
        var branch = bind[(bind.IndexOf('-') + 1)..];
        var eligible = _coreMatchParticipantsPlayersAccess._players[recipient].IsAlive;
        if (own)
        {
            var targetResult = f.ChoiceBindings.Single(b => b.Name == "target-" + branch);
            eligible &= targetResult.ChooserSeat == f.OwnerSeat && targetResult.OptionId == "skip";
        }

        if (eligible && branch == "hp")
        {
            eligible = _coreMatchParticipantsPlayersAccess._players[recipient].HasHpValue;
            if (eligible)
            {
                var participants = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue).ToArray();
                foreach (var p in participants)
                    if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoQuery(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), p.Seat, "instruction-hp"))
                        return SkillProgramStepOutcome.AwaitChild;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var current = _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoQueryValue(f, recipient, "instruction-hp");
                eligible = participants.All(p => current <= _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoQueryValue(f, p.Seat, "instruction-hp"));
            }
        }
        else if (eligible)
        {
            var count = branch == "hand" ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[recipient]).Count : _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[recipient]).Count;
            eligible = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => count <= (branch == "hand" ? _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count : _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count));
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding(bind, eligible ? "perform" : "skip", f.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhuZhiBranchEvent(f.Id, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), bind, recipient, eligible));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsAnniversaryZhuZhiChoiceResult(ProgramSkillFrame f, ProgramChoiceResultBinding binding)
    {
        if (f.SkillId != AnniversaryZhuZhiComposition.Skill || f.ActivationId != AnniversaryZhuZhiComposition.Activation || f.TriggerId is not null || f.SelectedTargetSeats is not [var selected] || selected == f.OwnerSeat || binding.ChooserSeat != f.OwnerSeat || binding.OptionId is not ("perform" or "skip") || !AnniversaryZhuZhiComposition.Branches.Contains(binding.Name))
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (plan.Instructions.Take(f.InstructionIndex).Count(e => e.Op == SkillProgramEffectOp.ResolveAnniversaryZhuZhiAnguo && e.ResultBind == binding.Name) != 1)
            return false;
        var own = binding.Name.StartsWith("owner-", StringComparison.Ordinal);
        var recipient = own ? f.OwnerSeat : selected;
        var source = new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        if (own && binding.OptionId == "perform" && f.ChoiceBindings.SingleOrDefault(b => b.Name == "target-" + binding.Name[6..])?.OptionId != "skip")
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhuZhiBranchEvent>().Count(e => e == new AnniversaryZhuZhiBranchEvent(f.Id, source, binding.Name, recipient, binding.OptionId == "perform")) == 1;
    }
}
