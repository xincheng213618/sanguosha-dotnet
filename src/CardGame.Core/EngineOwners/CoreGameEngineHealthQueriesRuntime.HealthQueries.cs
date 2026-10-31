#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHealthQueriesRuntime
{
    internal int? UnambiguousHp(CharacterState player) => !player.HasHpValue || player.HappyQiaoHealth is { } health && health.DaQiaoHp != health.XiaoQiaoHp && player.HappyQiaoPoolQuery?.Invoke(player)is null ? null : player.Hp;
    internal bool TryBeginHappyQiaoAttackHpLossChoice(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(attack.TargetSeat))
            return false;
        var amount = _coreCardDamageMaximumHpReplacementOperations.CalculateCardDamageLayer(attack);
        if (amount <= 0)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.Last();
        return _characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoHealthChoice(parent.Id, attack.TargetSeat, HpChangeKind.Loss, amount, HappyQiaoHealthReturnKind.Damage, $"damage-loss:{(attack.ResolutionId)}:{(attack.TargetSeat)}:{(_characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(attack.ResolutionId))}", attack.IsSourceLess ? null : attack.SourceSeat, attack.ResolutionId);
    }

    internal bool HasHappyQiaoNativeDamageChoice(long attackId)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1] is not HappyQiaoHealthChoiceFrame { IsQuery: false } choice || _resolutionStackOperations._resolutionStack[^2] is not { HappyQiaoHealth: { ReturnKind: HappyQiaoHealthReturnKind.Damage, Applied: false, Pool: null } receipt } parent || parent.Id != attackId || choice.ParentFrameId != parent.Id || receipt.AttackResolutionId != attackId || _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(attackId)is not { DamageWasApplied: false } attack || attack.TargetSeat != receipt.TargetSeat || choice.OwnerSeat != attack.TargetSeat || receipt.SourceSeat != (attack.IsSourceLess ? null : attack.SourceSeat))
            return false;
        _characterDaQiaoXiaoQiaoHappyGodOperations.RequireHappyQiaoHealthChoice(choice);
        return true;
    }

    internal bool TryBeginHappyQiaoInstructionQuery(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        IEnumerable<int> subjects = [];
        if (effect.Condition.ReferencesOwnerHp || effect.NumberExpression is SkillProgramNumberExpression.CurrentHp or SkillProgramNumberExpression.OwnerLostHp or SkillProgramNumberExpression.OwnerLostHpAtLeastOne)
            subjects = subjects.Append(frame.OwnerSeat);
        if (effect.Condition.ReferencesSelectedTargetHp)
            subjects = subjects.Concat(frame.SelectedTargetSeats);
        if (effect.NumberExpression == SkillProgramNumberExpression.LivingPlayersMinHp)
            subjects = subjects.Concat(_coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.HasHpValue).Select(player => player.Seat));
        if (effect.NumberExpression is SkillProgramNumberExpression.RecipientCurrentHp)
            subjects = subjects.Concat(effect.Target == SkillProgramEffectTarget.Owner ? [frame.OwnerSeat] : frame.SelectedTargetSeats);
        if (effect.OnUnknownAmount == SkillProgramUnknownAmountBehavior.CancelBinding && effect.AmountExpression is { } amount)
            subjects = subjects.Concat(ExpressionHpSubjects(amount, frame));
        if (effect.ValueExpression is { } captured)
            subjects = subjects.Concat(ExpressionHpSubjects(captured, frame));
        foreach (var seat in subjects.Distinct().Where(_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth))
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoQuery(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), seat, "instruction-hp"))
                return true;
        return false;
    }

    internal IEnumerable<int> ExpressionHpSubjects(ProgramExpression expression, ProgramSkillFrame frame, int? iterator = null)
    {
        var world = _kernelCoreGameEngineProgramExpressionsOperations.CreateProgramExpressionWorld(frame);
        if (expression.Op is "hp" or "lostHp")
        {
            var name = expression.Args[0].Name!;
            if ((name == "it" ? iterator : world.Participant(name))is { } seat)
                yield return seat;
            yield break;
        }

        if (expression.Op is "all" or "any" or "count")
        {
            foreach (var seat in world.ParticipantSet(expression.Args[0].Name!) ?? [])
                foreach (var subject in ExpressionHpSubjects(expression.Args[1], frame, seat))
                    yield return subject;
            yield break;
        }

        foreach (var child in expression.Args)
            foreach (var subject in ExpressionHpSubjects(child, frame, iterator))
                yield return subject;
    }

    internal IReadOnlyList<LegalAction> HappyQiaoActionQueryUnion(Func<IReadOnlyList<LegalAction>> query)
    {
        var unresolved = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.HappyQiaoHealth is { } health && health.DaQiaoHp != health.XiaoQiaoHp && player.HappyQiaoPoolQuery?.Invoke(player)is null).ToArray();
        if (unresolved.Length == 0)
            return query();
        var actions = new List<LegalAction>();
        void Visit(int index)
        {
            if (index == unresolved.Length)
            {
                var previous = _coreGameEngineActionQueriesActionQueryAccess._actionQuery;
                _coreGameEngineActionQueriesActionQueryAccess._actionQuery = new global::CardGame.Core.NativeOwners.ActionQuery(_coreMatchParticipantsPlayersAccess._players.Count);
                using var scope = _kernelCoreGameEngineActionQueriesOperations.CreateActionQueryScope(previous);
                actions.AddRange(query());
                return;
            }

            var player = unresolved[index];
            var original = player.HappyQiaoPoolQuery;
            try
            {
                foreach (var pool in Enum.GetValues<HappyQiaoPool>())
                {
                    player.HappyQiaoPoolQuery = _ => pool;
                    Visit(index + 1);
                }
            }
            finally
            {
                player.HappyQiaoPoolQuery = original;
            }
        }

        Visit(0);
        // Pool-dependent AI estimates do not create another executable action.
        // Keep every selection and identity field in the key; the real effect
        // still asks for its owning health receipt when it resolves.
        return Array.AsReadOnly(actions.DistinctBy(action => System.Text.Json.JsonSerializer.Serialize(action with { ProgramAiHint = null })).ToArray());
    }
}
