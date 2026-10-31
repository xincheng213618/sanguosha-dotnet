#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiJueOlRuntime
{
    internal int[] LangxiTargets(int owner) => !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[owner].HasHpValue ? [] : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue && p.Seat != owner && p.Hp <= _coreMatchParticipantsPlayersAccess._players[owner].Hp).Select(p => p.Seat).ToArray();
    internal IReadOnlyList<PromptChoice> LangxiChoices(ProgramSkillFrame f) => Array.AsReadOnly(LangxiTargets(f.OwnerSeat).Select(s => new PromptChoice(new($"langxi.{(f.Id)}.target-{(s)}"), $"对 {(_coreMatchParticipantsPlayersAccess._players[s].Name)} 造成{(_coreShanheS5LiJueOperations.ShanheS5LangxiMinimumDamage(f))}至2点随机伤害。", [], [s], new Dictionary<string, string> { { "program-action", "langxi-target" } })).ToArray());
    internal void PublishLangxi(ProgramSkillFrame f)
    {
        var choices = LangxiChoices(f);
        if (choices.Count == 0)
        {
            FinishLiJue(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择当前体力不大于你的其他角色。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("langxi-target")]
    internal void ResolveLangxiChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireLiJue(f);
        if (f.LiJue!.Stage != LiJueStage.ChoosingTarget || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], LangxiChoices(f).Where(c => c.Id == choice.Id).ToArray()) || !LiJueCurrentSource(f))
            throw new InvalidOperationException("Langxi changed its unpaid actual HP-qualified target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = choice.Targets.Single();
        var minimum = _coreShanheS5LiJueOperations.ShanheS5LangxiMinimumDamage(f);
        var roll = minimum + _randomOperations._random.Next(3 - minimum);
        var r = f.LiJue with
        {
            Stage = LiJueStage.DamageChildren,
            TargetSeat = target,
            Roll = roll
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiJue = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LangxiRandomResolvedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.WindowId, target, roll));
        if (roll == 0)
        {
            FinishLiJue(f);
            return;
        }

        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, roll, new(ProgramParticipantRef.Owner));
    }
}
