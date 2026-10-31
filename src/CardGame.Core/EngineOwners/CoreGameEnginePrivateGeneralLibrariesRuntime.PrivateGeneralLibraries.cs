#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrivateGeneralLibrariesRuntime
{
    internal bool HasPrivateGeneralLibraryCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InitializePrivateGeneralLibrary);

    SkillProgramStepOutcome IPrivateGeneralLibraryProgramHost.ExecutePrivateGeneralLibrary(SkillProgramEffect e, ProgramSkillFrame f) => ExecutePrivateGeneralLibrary(e, f);
    internal bool LibraryBaseSourceLive(PrivateGeneralLibrarySource source)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(source.OwnerSeat))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat];
        if (!owner.IsAlive)
            return false;
        var grant = owner.SkillGrants.Grants.FirstOrDefault(g => g.GrantId == source.CapabilityGrantId && g.SkillId == source.CapabilitySkillId && g.SkillInstanceId == source.CapabilitySkillInstanceId);
        if (grant is null || !_coreNationalSkillsOperations.IsRawSkillSourceAdmitted(owner, grant) || grant.GeneralLibraryProjection is not null)
            return false;
        return grant.LordProjection is null || LibraryBaseLordProjectionLive(owner, grant);
    }

    internal bool LibrarySourceQualified(PrivateGeneralLibrarySource source)
    {
        if (!LibraryBaseSourceLive(source))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat];
        var cap = owner.SkillGrants.Grants.Single(g => g.GrantId == source.CapabilityGrantId);
        return _coreNationalSkillsOperations.IsSkillGrantStateEffective(owner, cap);
    }

    internal bool LibraryBaseGrantLive(CharacterState owner, SkillGrant grant) => owner.IsAlive && _coreNationalSkillsOperations.IsRawSkillSourceAdmitted(owner, grant) && (grant.SourceId is not (CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource) || !_contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Lord) || owner.Role == Role.Lord || _corePrintedLordSkillQualificationsOperations.IsPrintedLordGrantQualified(owner, grant));
    internal bool LibraryBaseLordProjectionLive(CharacterState owner, SkillGrant grant)
    {
        if (grant.LordProjection is not { } p)
            return true;
        if (!owner.IsAlive || !_coreModeAllegianceOperations.ModeAllegiancePolicy.AllowsLordSkillProjection || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.LordSeat) || p.LordSeat == owner.Seat)
            return false;
        var lord = _coreMatchParticipantsPlayersAccess._players[p.LordSeat];
        if (!lord.IsAlive || lord.Role != Role.Lord)
            return false;
        return owner.SkillGrants.Grants.Any(g => g.GrantId == p.CapabilityGrantId && g.SkillInstanceId == p.CapabilitySkillInstanceId && g.LordProjection is null && LibraryBaseGrantLive(owner, g) && _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Program?.LordSkillProjection == true && (g.GeneralLibraryProjection is null || g.GeneralLibraryProjection is { } a && LibraryProjectionRelationLive(g, a) && LibraryBaseSourceLive(a.LibrarySource))) && lord.SkillGrants.Grants.Any(g => g.GrantId == p.LordGrantId && g.SkillInstanceId == p.LordSkillInstanceId && g.SkillId == grant.SkillId && g.LordProjection is null && g.GeneralLibraryProjection is null && LibraryBaseGrantLive(lord, g) && _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Tags.HasFlag(SkillTag.Lord));
    }

    internal bool LibraryProjectionRelationLive(SkillGrant grant, GeneralLibraryProjectionSource relation) => _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.GetValueOrDefault(relation.LibrarySource)is { } library && library.Initialized && library.RevealedGeneralId == relation.GeneralId && library.DeclaredSkillId == relation.SkillId && grant.SkillId == relation.SkillId;
    internal bool IsGeneralLibraryGrantQualified(CharacterState owner, SkillGrant grant) => _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, grant) && (grant.GeneralLibraryProjection is not { } source || source.LibrarySource.OwnerSeat == owner.Seat && LibraryProjectionRelationLive(grant, source) && LibrarySourceQualified(source.LibrarySource));
    internal PrivateGeneralLibrary? EffectiveGeneralLibrary(CharacterState owner) => _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.Count == 0 ? null : FindEffectiveGeneralLibrary(owner);
    internal PrivateGeneralLibrary? FindEffectiveGeneralLibrary(CharacterState owner) => _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.Values.Where(l => l.Source.OwnerSeat == owner.Seat && l.RevealedGeneralId is not null && LibrarySourceQualified(l.Source)).OrderBy(l => l.Source.CapabilitySkillInstanceId, StringComparer.Ordinal).ThenBy(l => l.Source.CapabilityGrantId, StringComparer.Ordinal).FirstOrDefault();
    internal GeneralGender? GetPrivateGeneralLibraryGender(CharacterState owner) => EffectiveGeneralLibrary(owner)is { } l ? _contentRegistryOperations._contentRegistry.Generals[l.RevealedGeneralId!].Gender : null;
    internal string? GetPrivateGeneralLibraryFaction(CharacterState owner) => EffectiveGeneralLibrary(owner)is { } l ? l.SelectedFactionId ?? _contentRegistryOperations._contentRegistry.Generals[l.RevealedGeneralId!].FactionId : null;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 30)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 10)]
    internal void SynchronizePrivateGeneralLibraries()
    {
        if (!HasPrivateGeneralLibraryCapability || _coreGameEnginePrivateGeneralLibrariesFieldsAccess._synchronizingPrivateGeneralLibraries)
            return;
        _coreGameEnginePrivateGeneralLibrariesFieldsAccess._synchronizingPrivateGeneralLibraries = true;
        try
        {
            foreach (var library in _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.Values.ToArray())
            {
                var owner = _coreMatchParticipantsPlayersAccess._players[library.Source.OwnerSeat];
                if (owner.IsAlive && owner.SkillGrants.Grants.Any(g => g.GrantId == library.Source.CapabilityGrantId && g.SkillInstanceId == library.Source.CapabilitySkillInstanceId && g.SkillId == library.Source.CapabilitySkillId && g.GeneralLibraryProjection is null))
                    continue;
                _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.Remove(library.Source);
                _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraryRevision++;
                foreach (var grant in owner.SkillGrants.Grants.Where(g => g.GeneralLibraryProjection?.LibrarySource == library.Source).ToArray())
                    owner.SkillGrants.RemoveGrant(grant.GrantId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateGeneralLibraryRemovedEvent(library.Source));
            }
        }
        finally
        {
            _coreGameEnginePrivateGeneralLibrariesFieldsAccess._synchronizingPrivateGeneralLibraries = false;
        }
    }

    internal long CaptureCombinedProjectionDependencyStamp()
    {
        var lord = _coreLordSkillProjectionsOperations.CaptureLordProjectionDependencyStamp();
        if (!HasPrivateGeneralLibraryCapability)
            return lord;
        var changed = _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateLibraryOwnerDependencies.Length != _coreMatchParticipantsPlayersAccess._players.Count;
        if (changed)
            _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateLibraryOwnerDependencies = new GameEngine.ProjectionDependency[_coreMatchParticipantsPlayersAccess._players.Count];
        for (var i = 0; i < _coreMatchParticipantsPlayersAccess._players.Count; i++)
        {
            var player = _coreMatchParticipantsPlayersAccess._players[i];
            var currentOwner = new GameEngine.ProjectionDependency(player.SkillGrants.Revision, player.IsAlive, player.Hp, player.Role, player.GeneralSelected, player.GeneralRevealed, player.SecondaryGeneralSelected, player.SecondaryGeneralRevealed);
            if (_coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateLibraryOwnerDependencies[i] == currentOwner)
                continue;
            _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateLibraryOwnerDependencies[i] = currentOwner;
            changed = true;
        }

        var current = (lord, _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraryRevision);
        if (changed || current != _coreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionDependencies)
        {
            _coreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionDependencies = current;
            _coreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionStamp++;
        }

        return _coreGameEnginePrivateGeneralLibrariesFieldsAccess._combinedProjectionStamp;
    }

    internal PrivateGeneralLibrarySource? ResolvePrivateGeneralLibrarySource(ProgramSkillFrame f, SkillProgramEffect e)
    {
        var grants = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].SkillGrants.Grants;
        var cap = grants.Where(g => g.IsEnabled && g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId && g.GeneralLibraryProjection is null).OrderBy(g => g.GrantId, StringComparer.Ordinal).FirstOrDefault();
        if (cap is null)
            return null;
        if (e.Op == SkillProgramEffectOp.AcquirePrivateGeneralAvatar)
        {
            var skill = e.SkillIds.Single();
            cap = grants.Where(g => g.IsEnabled && g.SkillId == skill && g.GeneralLibraryProjection is null && (g.SkillInstanceId == f.SkillInstanceId || g.SourceId == cap.SourceId)).OrderByDescending(g => g.SkillInstanceId == f.SkillInstanceId).ThenBy(g => g.SkillInstanceId, StringComparer.Ordinal).ThenBy(g => g.GrantId, StringComparer.Ordinal).FirstOrDefault();
        }

        return cap is null ? null : new(f.OwnerSeat, cap.SkillId, cap.GrantId, cap.SkillInstanceId);
    }

    internal IEnumerable<string> PrivateGeneralCandidates()
    {
        var excluded = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => new[] { p.GeneralSelected ? p.General.Id : null, p.SecondaryGeneralSelected ? p.SecondaryGeneral?.Id : null }).OfType<string>().Concat(_coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.Values.SelectMany(l => l.GeneralIds)).ToHashSet(StringComparer.Ordinal);
        // A campaign's undeployed second form is reserved for its original seat.
        if (_coreMatchDefinitionGeneralPoolOptionsAccess._options.ShanheBattle?.BossTransition is { } transition && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheBossStageStartedEvent>().Count == 0)
            excluded.Add(transition.GeneralId);
        return _coreMatchDefinitionGeneralPoolOptionsAccess._generalPool.Select(g => g.Id).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).Where(id => !excluded.Contains(id));
    }

    internal PrivateGeneralLibrary DrawPrivateGeneralAvatars(PrivateGeneralLibrary library, int amount, IReadOnlyCollection<string>? excludedIds = null)
    {
        var ids = library.GeneralIds.ToList();
        for (var i = 0; i < amount; i++)
        {
            var candidates = PrivateGeneralCandidates().Where(id => !ids.Contains(id, StringComparer.Ordinal) && (excludedIds is null || !excludedIds.Contains(id, StringComparer.Ordinal))).ToArray();
            if (candidates.Length == 0)
                break;
            ids.Add(candidates[_randomOperations._random.Next(candidates.Length)]);
        }

        library = library with
        {
            GeneralIds = Array.AsReadOnly(ids.ToArray()),
            Initialized = true,
            Revision = library.Revision + 1
        };
        _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries[library.Source] = library;
        _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraryRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateGeneralLibraryCountChangedEvent(library.Source, ids.Count));
        return library;
    }

    internal IReadOnlyList<string> PrivateGeneralDeclarableSkills(string generalId, PrivateGeneralLibraryPolicy policy)
    {
        var tags = policy.ExcludedSkillTags.Aggregate(SkillTag.None, (a, b) => a | b);
        return Array.AsReadOnly(_contentRegistryOperations._contentRegistry.Generals[generalId].SkillIds.Where(id => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(id, out var skill) && skill.ImplementationStatus == SkillImplementationStatus.Complete && (skill.Tags & tags) == 0).Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray());
    }

    internal SkillProgramStepOutcome ExecutePrivateGeneralLibrary(SkillProgramEffect e, ProgramSkillFrame f)
    {
        var source = ResolvePrivateGeneralLibrarySource(f, e);
        if (source is null || !LibrarySourceQualified(source))
            return SkillProgramStepOutcome.Continue;
        var library = _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.GetValueOrDefault(source);
        if (e.Op == SkillProgramEffectOp.InitializePrivateGeneralLibrary)
        {
            if (f.WindowContext?.Window != SkillProgramTriggerWindow.GameStarting)
                throw new InvalidOperationException("Initial private library draw requires GameStarting.");
            library ??= new(source, false, []);
            if (!library.Initialized)
                library = DrawPrivateGeneralAvatars(library, e.Amount);
        }
        else if (library is null)
            return SkillProgramStepOutcome.Continue;
        if (e.Op == SkillProgramEffectOp.AcquirePrivateGeneralAvatar)
        {
            DrawPrivateGeneralAvatars(library!, e.Amount);
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GeneralLibraryDraft = new(GeneralLibraryStep.SelectGeneral, source, e.Op == SkillProgramEffectOp.InitializePrivateGeneralLibrary) });
        if (PrivateGeneralLibraryChoices(f).Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { GeneralLibraryDraft = null });
            return SkillProgramStepOutcome.Continue;
        }

        PublishPrivateGeneralLibraryChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> PrivateGeneralLibraryChoices(ProgramSkillFrame f)
    {
        var d = f.GeneralLibraryDraft!;
        var library = _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.GetValueOrDefault(d.Source);
        if (library is null || !LibrarySourceQualified(d.Source))
            return[];
        var e = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        PromptChoice Choice(string value, string label) => new(new($"general-library.{(f.Id)}.{(d.Step)}.{(value)}"), label, [], [], new Dictionary<string, string> { { "program-action", "private-general-library" }, { "frame-id", f.Id.ToString() }, { "value", value } });
        var choices = d.Step switch
        {
            GeneralLibraryStep.SelectGeneral => library.GeneralIds.Where(id => PrivateGeneralDeclarableSkills(id, e.GeneralLibraryPolicy!).Count > 0).Order(StringComparer.Ordinal).Select(id => Choice(id, "展示【" + _contentRegistryOperations._contentRegistry.Generals[id].Name + "】")).ToArray(),
            GeneralLibraryStep.SelectSkill => PrivateGeneralDeclarableSkills(d.SelectedGeneralId!, e.GeneralLibraryPolicy!).Select(id => Choice(id, "获得【" + _contentRegistryOperations._contentRegistry.Skills[id].Name + "】")).ToArray(),
            GeneralLibraryStep.SelectFaction => new[]
            {
                "wei",
                "shu",
                "wu",
                "qun"
            }.Select(id => Choice(id, "选择势力 " + id)).ToArray(),
            _ => throw new InvalidOperationException("Invalid private library selection stage.")};
        if (d.Step != GeneralLibraryStep.SelectGeneral || !d.InitialDrawCommitted)
            choices = [..choices, new(new($"general-library.{(f.Id)}.{(d.Step)}.back"), "返回", [], [], new Dictionary<string, string> { ["program-action"] = "private-general-library", ["frame-id"] = f.Id.ToString(), ["navigation"] = "back" })];
        return choices;
    }

    internal void PublishPrivateGeneralLibraryChoice(ProgramSkillFrame f)
    {
        var choices = PrivateGeneralLibraryChoices(f);
        if (choices.Count == 0)
        {
            FinishPrivateGeneralLibrarySelection(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Description, [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("private-general-library")]
    internal void ResolvePrivateGeneralLibraryChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Private library selection lost its owning frame.");
        if (choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString() || !PrivateGeneralLibraryChoices(f).Any(c => c.Id == choice.Id))
            throw new InvalidOperationException("Stale private library choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var d = f.GeneralLibraryDraft!;
        if (choice.Parameters.GetValueOrDefault("navigation") == "back")
        {
            if (d.Step == GeneralLibraryStep.SelectGeneral)
            {
                FinishPrivateGeneralLibrarySelection(f);
                return;
            }

            d = d.Step == GeneralLibraryStep.SelectFaction ? d with
            {
                Step = GeneralLibraryStep.SelectSkill,
                SelectedSkillId = null,
                SelectedFactionId = null
            }

            : d with
            {
                Step = GeneralLibraryStep.SelectGeneral,
                SelectedGeneralId = null,
                SelectedSkillId = null,
                SelectedFactionId = null
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GeneralLibraryDraft = d });
            PublishPrivateGeneralLibraryChoice(f);
            return;
        }

        var value = choice.Parameters["value"];
        if (d.Step == GeneralLibraryStep.SelectGeneral)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GeneralLibraryDraft = d with { Step = GeneralLibraryStep.SelectSkill, SelectedGeneralId = value } });
            PublishPrivateGeneralLibraryChoice(f);
            return;
        }

        if (d.Step == GeneralLibraryStep.SelectSkill)
        {
            d = d with
            {
                SelectedSkillId = value
            };
            if (_contentRegistryOperations._contentRegistry.Generals[d.SelectedGeneralId!].FactionId == "god")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GeneralLibraryDraft = d with { Step = GeneralLibraryStep.SelectFaction } });
                PublishPrivateGeneralLibraryChoice(f);
                return;
            }
        }
        else
            d = d with
            {
                SelectedFactionId = value
            };
        var library = _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries[d.Source];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var nativeFactionBeforeDeclaration = _coreNativeFactionChangesOperations.CaptureNativeAvatarFactionBeforeDeclaration(owner);
        var id = $"general-library:{(f.OwnerSeat)}:{(d.Source.CapabilityGrantId.Length)}:{(d.Source.CapabilityGrantId)}:{(d.Source.CapabilitySkillInstanceId.Length)}:{(d.Source.CapabilitySkillInstanceId)}:{(d.SelectedGeneralId)}:{(d.SelectedSkillId)}";
        foreach (var old in owner.SkillGrants.Grants.Where(g => g.GeneralLibraryProjection?.LibrarySource == d.Source && g.GrantId != id).ToArray())
            owner.SkillGrants.RemoveGrant(old.GrantId);
        if (!owner.SkillGrants.Grants.Any(g => g.GrantId == id))
            owner.SkillGrants.Grant(new(id, d.SelectedSkillId!, id, "general-library:" + d.Source.CapabilityGrantId, GeneralLibraryProjection: new(d.Source, d.SelectedGeneralId!, d.SelectedSkillId!)));
        _coreConversionStatesOperations.RegisterTaggedConversionSkill(owner, d.SelectedSkillId!);
        library = library with
        {
            RevealedGeneralId = d.SelectedGeneralId,
            DeclaredSkillId = d.SelectedSkillId,
            SelectedFactionId = d.SelectedFactionId,
            Revision = library.Revision + 1
        };
        _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries[d.Source] = library;
        _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraryRevision++;
        _coreNativeFactionChangesOperations.CompleteNativeAvatarFactionDeclaration(f, owner, nativeFactionBeforeDeclaration);
        var general = _contentRegistryOperations._contentRegistry.Generals[d.SelectedGeneralId!];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrivateGeneralLibraryDeclaredEvent(d.Source, d.SelectedGeneralId!, d.SelectedSkillId!, general.Gender, d.SelectedFactionId ?? general.FactionId));
        FinishPrivateGeneralLibrarySelection(f);
    }

    internal void FinishPrivateGeneralLibrarySelection(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GeneralLibraryDraft = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 890)]
    internal bool ResumePrivateGeneralLibrarySelection(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GeneralLibraryDraft: { } } f || f.Id != id)
            return false;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || PrivateGeneralLibraryChoices(f).Count == 0)
        {
            FinishPrivateGeneralLibrarySelection(f);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishPrivateGeneralLibraryChoice(f);
        return true;
    }

    internal IReadOnlyList<PrivateGeneralLibrarySnapshot>? ProjectPrivateGeneralLibraries(CharacterState owner, int viewerSeat, bool revealAll)
    {
        var libraries = _coreGameEnginePrivateGeneralLibrariesFieldsAccess._privateGeneralLibraries.Values.Where(l => l.Source.OwnerSeat == owner.Seat).OrderBy(l => l.Source.CapabilitySkillInstanceId, StringComparer.Ordinal).ThenBy(l => l.Source.CapabilityGrantId, StringComparer.Ordinal).Select(l =>
        {
            var live = l.RevealedGeneralId is not null && LibrarySourceQualified(l.Source);
            var general = live ? _contentRegistryOperations._contentRegistry.Generals[l.RevealedGeneralId!] : null;
            return new PrivateGeneralLibrarySnapshot(l.Source, l.GeneralIds.Count, l.RevealedGeneralId, l.DeclaredSkillId, general?.Gender, live ? l.SelectedFactionId ?? general?.FactionId : null, viewerSeat == owner.Seat || revealAll ? Array.AsReadOnly(l.GeneralIds.ToArray()) : null);
        }).ToArray();
        return libraries.Length == 0 ? null : Array.AsReadOnly(libraries);
    }
}
