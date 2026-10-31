#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangChengYanOlRuntime
{
    internal SkillProgramStepOutcome JiezhenProgramReplaceSkills(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var targetSeat] || targetSeat == active.OwnerSeat)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), "解阵没有可选的其他角色，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive || _characterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions.ContainsKey(targetSeat))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), _characterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions.ContainsKey(targetSeat) ? "该角色的技能已被“解阵”替换，剩余结算取消。" : "解阵的目标角色已失效，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var replaceable = _coreNationalSkillsOperations.EnabledContentSkillIds(target).Where(skillId => (_contentRegistryOperations._contentRegistry!.GetSkill(skillId).Tags & (SkillTag.Locked | SkillTag.Limited | SkillTag.Lord)) == 0).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        foreach (var skillId in replaceable)
            foreach (var grant in target.SkillGrants.Grants.Where(grant => grant.IsEnabled && grant.SkillId == skillId).ToArray())
                target.SkillGrants.SetEnabled(grant.GrantId, false);
        var sourceSkillId = $"jiezhen:{(targetSeat)}";
        var bazhenGrantId = $"acquired:{(sourceSkillId)}:classic:bazhen";
        _coreCharacterHostOperations.AcquireRuntimeSkills(target, sourceSkillId, ["classic:bazhen"]);
        _characterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions[targetSeat] = new(active.OwnerSeat, replaceable, bazhenGrantId);
        _coreMatchLogOperations.AddLog("SkillTriggered", replaceable.Length == 0 ? $"{(target.Name)} 的技能已被替换为“八阵”。" : $"{(target.Name)} 的【{(string.Join("】【", replaceable.Select(GetSkillDisplayName)))}】被替换为“八阵”。", active.OwnerSeat, targetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJiezhenConvertedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, targetSeat, replaceable));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome JiezhenProgramRestoreSkills(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var window = frame.WindowContext?.Window;
        int? targetSeat = window switch
        {
            SkillProgramTriggerWindow.TurnStartBeforeNormalFlow => _characterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions.Where(pair => pair.Value.OwnerSeat == active.OwnerSeat).Select(pair => (int? )pair.Key).FirstOrDefault(),
            SkillProgramTriggerWindow.JudgmentFinalized when frame.WindowContext?.Judgment is { } judgment && _characterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions.TryGetValue(judgment.SubjectSeat, out var converted) && converted.OwnerSeat == active.OwnerSeat => judgment.SubjectSeat,
            _ => null
        };
        if (targetSeat is not { } seat)
            return SkillProgramStepOutcome.Continue;
        var state = _characterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions[seat];
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        foreach (var skillId in state.ReplacedSkillIds)
            foreach (var grant in target.SkillGrants.Grants.Where(grant => !grant.IsEnabled && grant.SkillId == skillId && grant.GrantId != state.BazhenGrantId).ToArray())
                target.SkillGrants.SetEnabled(grant.GrantId, true);
        if (target.SkillGrants.Grants.Any(grant => grant.GrantId == state.BazhenGrantId))
            target.SkillGrants.RemoveGrant(state.BazhenGrantId);
        _characterHuangChengYanOlJiezhenConversionsAccess._jiezhenConversions.Remove(seat);
        var takenCardId = JiezhenTakeOneCardFromZones(active, seat);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(target.Name)} 失去“八阵”并获得原技能。", active.OwnerSeat, seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJiezhenRestoredEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, seat, state.ReplacedSkillIds, takenCardId));
        if (takenCardId >= 0 && !_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return takenCardId >= 0 ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }

    internal int JiezhenTakeOneCardFromZones(ProgramSkillFrame frame, int targetSeat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return -1;
        var candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(target).Select(card => (Card: card, Location: CardLocation.Hand(target.Seat))).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Select(card => (Card: card, Location: CardLocation.Equipment(target.Seat)))).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(target).Select(card => (Card: card, Location: CardLocation.Judgment(target.Seat)))).OrderBy(entry => entry.Card.Id).ToArray();
        if (candidates.Length == 0)
            return -1;
        var pick = candidates[_randomOperations._random.Next(candidates.Length)];
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.jiezhen-take");
        _corePhysicalMovementFacadesOperations.MoveCard(pick.Card, pick.Location, CardLocation.Processing, reason, movement => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(movement, frame.OwnerSeat, frame.Id));
        if (!_coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(pick.Card, CardLocation.Hand(frame.OwnerSeat), reason))
            return -1;
        _coreMatchLogOperations.AddLog("CardGained", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 获得 {(target.Name)} 区域里的【{(pick.Card.DisplayName)}】。", frame.OwnerSeat, targetSeat);
        return pick.Card.Id;
    }
}
