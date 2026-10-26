using System.Text.Json;
using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Compiles the bounded visual authoring model into the normal content/skill pipeline.</summary>
public static class CustomGeneralContent
{
    public const string PackageId = "custom-generals";
    public const string ActivationId = "use";

    public static CustomGeneralLibrary Snapshot(CustomGeneralLibrary library)
    {
        if (library is null) throw Invalid("武将库不能为空。");
        if (library.FormatVersion != CustomGeneralLibrary.CurrentFormatVersion) throw Invalid("不支持此自定义武将库版本。");
        if (library.Generals is null) throw Invalid("武将列表不能为空。");
        var ids = new HashSet<string>(StringComparer.Ordinal);
        var skillIds = new HashSet<string>(StringComparer.Ordinal);
        var generals = new List<CustomGeneralDraft>();
        foreach (var draft in library.Generals)
        {
            if (draft is null) throw Invalid("武将列表包含空项。");
            ValidateId(draft.Id, "custom:general-", "武将");
            if (!ids.Add(draft.Id)) throw Invalid($"武将ID重复：{draft.Id}");
            ValidateText(draft.Name, "武将名称", 64);
            ValidateText(draft.PortraitKey, "立绘标识", 256);
            if (draft.FactionId is not ("wei" or "shu" or "wu" or "qun" or "jin" or "god")) throw Invalid("请选择有效势力。");
            if (!Enum.IsDefined(draft.Gender)) throw Invalid("请选择有效性别。");
            if (draft.BaseHp is < 1 or > 20 || draft.InitialHp is { } hp && (hp < 1 || hp > draft.BaseHp)) throw Invalid("体力上限须为1至20，初始体力须介于1与体力上限之间。");
            if (draft.SkillIds is null || draft.CustomSkills is null) throw Invalid("技能列表不能为空。");
            if (draft.SkillIds.Count + draft.CustomSkills.Count > 12) throw Invalid("每名武将最多拥有12个技能。");
            if (draft.SkillIds.Any(string.IsNullOrWhiteSpace) || draft.SkillIds.Distinct(StringComparer.Ordinal).Count() != draft.SkillIds.Count) throw Invalid("已有技能ID不能为空或重复。");
            var custom = draft.CustomSkills.Select(FreezeSkill).ToArray();
            foreach (var skill in custom)
            {
                if (!skillIds.Add(skill.Id) || draft.SkillIds.Contains(skill.Id, StringComparer.Ordinal)) throw Invalid($"自建技能ID重复：{skill.Id}");
                _ = CompileFrozenSkill(skill);
            }
            generals.Add(draft with { SkillIds = Array.AsReadOnly(draft.SkillIds.ToArray()), CustomSkills = Array.AsReadOnly(custom) });
        }
        return library with { Generals = Array.AsReadOnly(generals.ToArray()) };
    }

    public static ContentSkillDefinition CompileSkill(CustomSkillDraft draft) => CompileFrozenSkill(FreezeSkill(draft));

    public static IReadOnlyList<ContentSkillDefinition> SelectableSkills(ContentRegistry baseRegistry)
    {
        ArgumentNullException.ThrowIfNull(baseRegistry);
        var printed = baseRegistry.Generals.Values.Where(g => baseRegistry.IsGeneralPlayable(g.Id))
            .SelectMany(g => g.SkillIds).ToHashSet(StringComparer.Ordinal);
        return Array.AsReadOnly(baseRegistry.Skills.Values.Where(s => printed.Contains(s.Id) &&
                s.ImplementationStatus == SkillImplementationStatus.Complete && !s.Id.EndsWith(":none", StringComparison.Ordinal))
            .OrderBy(s => s.Name, StringComparer.Ordinal).ThenBy(s => s.Id, StringComparer.Ordinal).ToArray());
    }

    public static ContentRegistry CreateRegistry(ContentRegistry baseRegistry, CustomGeneralLibrary library)
    {
        ArgumentNullException.ThrowIfNull(baseRegistry);
        var snapshot = Snapshot(library);
        var enabled = snapshot.Generals.Where(g => g.Enabled).OrderBy(g => g.Id, StringComparer.Ordinal).ToArray();
        if (enabled.Length == 0) return baseRegistry;
        var selectable = SelectableSkills(baseRegistry).Select(s => s.Id).ToHashSet(StringComparer.Ordinal);
        foreach (var general in enabled)
        {
            if (baseRegistry.Generals.ContainsKey(general.Id)) throw Invalid($"武将ID已存在：{general.Id}");
            foreach (var id in general.SkillIds)
                if (!selectable.Contains(id)) throw Invalid($"不存在或不可选择的已有技能：{id}");
            foreach (var skill in general.CustomSkills)
                if (baseRegistry.Skills.ContainsKey(skill.Id)) throw Invalid($"技能ID已存在：{skill.Id}");
        }
        var package = new CustomPackage(baseRegistry, enabled);
        var generalIds = enabled.Select(g => g.Id).ToArray();
        try { return baseRegistry.WithExtension(package, mode => ExtendMode(mode, generalIds)); }
        catch (Exception error) when (error is ArgumentException or InvalidOperationException)
        { throw new ArgumentException("自定义武将内容无效：" + error.Message, nameof(library), error); }
    }

    private static ContentModeDefinition ExtendMode(ContentModeDefinition mode, string[] ids)
    {
        if (mode.ModeKind != ContentModeKind.Identity || mode.TeamActivity is not null || mode.BossActivity is not null ||
            mode.HulaoActivity is not null || mode.ShenwuActivity is not null || mode.NianActivity is not null ||
            mode.HellActivity is not null || mode.WenheActivity is not null || mode.DragonBoatActivity is not null || mode.GuanduActivity is not null) return mode;
        return mode with
        {
            GeneralPoolIds = mode.GeneralPoolIds is null ? null : Array.AsReadOnly(mode.GeneralPoolIds.Concat(ids).ToArray()),
            GeneralSelectionPoolIds = mode.GeneralSelectionPoolIds is null ? null : Array.AsReadOnly(mode.GeneralSelectionPoolIds.Concat(ids).ToArray())
        };
    }

    private static CustomSkillDraft FreezeSkill(CustomSkillDraft draft)
    {
        if (draft is null) throw Invalid("自建技能不能为空。");
        ValidateId(draft.Id, "custom:skill-", "技能");
        ValidateText(draft.Name, "技能名称", 64);
        if (!Enum.IsDefined(draft.Trigger) || !Enum.IsDefined(draft.Target) || !Enum.IsDefined(draft.Condition)) throw Invalid("技能包含无效的时机、目标或条件。");
        if (draft.UsesPerTurn is < 1 or > 10) throw Invalid("每回合次数须为1至10。");
        if (draft.Condition == CustomSkillCondition.HpAtMost && draft.ConditionValue is < 1 or > 20) throw Invalid("体力条件须为1至20。");
        if (draft.Effects is null || draft.Effects.Count is < 1 or > 8) throw Invalid("每个技能须包含1至8个效果。");
        var effects = draft.Effects.Select(effect =>
        {
            if (effect is null || !Enum.IsDefined(effect.Kind) || !Enum.IsDefined(effect.DamageNature)) throw Invalid("技能包含无效效果或伤害属性。");
            if (effect.Amount is < 1 or > 10) throw Invalid("每项效果的数值须为1至10。");
            return effect with { };
        }).ToArray();
        return draft with { Effects = Array.AsReadOnly(effects) };
    }

    private static ContentSkillDefinition CompileFrozenSkill(CustomSkillDraft draft)
    {
        var active = draft.Trigger == CustomSkillTrigger.Active;
        var target = draft.Target == CustomSkillTarget.Self ? "owner" : "selectedTarget";
        var effects = new List<object>();
        if (!active && draft.Target != CustomSkillTarget.Self)
            effects.Add(new { op = "selectTarget", target = "owner", targetKind = draft.Target == CustomSkillTarget.Other ? "otherLiving" : "anyLiving" });
        foreach (var effect in draft.Effects)
        {
            if (effect.Kind == CustomSkillEffectKind.Damage)
                effects.Add(new { op = "damage", target, amount = effect.Amount, nature = Camel(effect.DamageNature) });
            else effects.Add(new { op = effect.Kind switch { CustomSkillEffectKind.Draw => "draw", CustomSkillEffectKind.Recover => "recover", _ => "loseHp" }, target, amount = effect.Amount });
            if (effect.Kind == CustomSkillEffectKind.Draw) effects.Add(new { op = "awaitBoundCardMovements", target = "owner" });
        }
        var skill = new Dictionary<string, object?> { ["id"] = draft.Id, ["revision"] = 1 };
        if (active)
            skill["activations"] = new[] { new { id = ActivationId, minCards = 0, maxCards = 0,
                minTargets = draft.Target == CustomSkillTarget.Self ? 0 : 1, maxTargets = draft.Target == CustomSkillTarget.Self ? 0 : 1,
                targetKind = draft.Target == CustomSkillTarget.Other ? "otherLiving" : "anyLiving", usesPerTurn = draft.UsesPerTurn,
                condition = ActivationCondition(draft), effects } };
        else
        {
            var trigger = new Dictionary<string, object?>
            {
                ["id"] = "trigger", ["window"] = draft.Trigger switch
                {
                    CustomSkillTrigger.TurnStart => "turnStartBeforeNormalFlow", CustomSkillTrigger.PlayStart => "playPhaseStarting",
                    CustomSkillTrigger.PlayEnd => "playEnding", CustomSkillTrigger.TurnEnd => "turnEnding", _ => "afterDamageApplied"
                },
                ["subject"] = draft.Trigger == CustomSkillTrigger.AfterDealtDamage ? "damageSource" : "owner",
                ["optional"] = draft.Optional, ["usageScope"] = "turn", ["usageLimit"] = draft.UsesPerTurn,
                ["condition"] = TriggerCondition(draft), ["effects"] = effects
            };
            if (draft.Trigger is CustomSkillTrigger.AfterDamage or CustomSkillTrigger.AfterDealtDamage) trigger["damageOccurrence"] = "perDamage";
            skill["triggers"] = new[] { trigger };
        }
        var description = Describe(draft);
        try
        {
            var catalog = SkillProgramCatalog.Load(
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { skill } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [draft.Id] = new { name = draft.Name, description } } }));
            return new(draft.Id, draft.Name, description)
            {
                Program = catalog.Programs[draft.Id], ProgramPresentation = catalog.Presentations[draft.Id],
                ExecutionForms = active ? SkillExecutionForm.None : SkillExecutionForm.Trigger,
                ActionForms = active ? SkillActionForm.Active : SkillActionForm.None,
                Tags = !active && !draft.Optional ? SkillTag.Locked : SkillTag.None
            };
        }
        catch (Exception error) when (error is ArgumentException or InvalidOperationException or FormatException)
        { throw new ArgumentException($"自建技能“{draft.Name}”无法编译：{error.Message}", nameof(draft), error); }
    }

    private static object ActivationCondition(CustomSkillDraft draft) => draft.Condition switch
    {
        CustomSkillCondition.Wounded => new { kind = "wounded" },
        CustomSkillCondition.HandEmpty => new { kind = "not", children = new[] { new { kind = "handCountAtLeast", value = 1 } } },
        CustomSkillCondition.HpAtMost => new { kind = "not", children = new[] { new { kind = "hpAtLeast", value = draft.ConditionValue + 1 } } },
        _ => new { kind = "always" }
    };

    private static object TriggerCondition(CustomSkillDraft draft)
    {
        if (draft.Condition == CustomSkillCondition.Always) return new { kind = "always" };
        var left = new { kind = draft.Condition == CustomSkillCondition.HandEmpty ? "currentHandCount" : "currentHp" };
        object right = draft.Condition == CustomSkillCondition.Wounded ? new { kind = "currentMaxHp" } :
            new { kind = "integerConstant", value = draft.Condition == CustomSkillCondition.HandEmpty ? 0 : draft.ConditionValue };
        return new Dictionary<string, object> { ["kind"] = "compare", ["left"] = left,
            ["operator"] = draft.Condition switch { CustomSkillCondition.Wounded => "lessThan", CustomSkillCondition.HandEmpty => "equal", _ => "lessThanOrEqual" }, ["right"] = right };
    }

    private static string Describe(CustomSkillDraft draft)
    {
        var timing = draft.Trigger switch { CustomSkillTrigger.Active => "出牌阶段", CustomSkillTrigger.TurnStart => "你的回合开始时",
            CustomSkillTrigger.PlayStart => "你的出牌阶段开始时", CustomSkillTrigger.PlayEnd => "你的出牌阶段结束时",
            CustomSkillTrigger.TurnEnd => "你的回合结束时", CustomSkillTrigger.AfterDamage => "你每次受到伤害后", _ => "你每次造成伤害后" };
        var condition = draft.Condition switch { CustomSkillCondition.Wounded => "若你已受伤，", CustomSkillCondition.HandEmpty => "若你没有手牌，",
            CustomSkillCondition.HpAtMost => $"若你的体力值不大于{draft.ConditionValue}，", _ => "" };
        var target = draft.Target switch { CustomSkillTarget.Self => "你", CustomSkillTarget.Other => "你选择的一名其他角色", _ => "你选择的一名角色" };
        var effects = string.Join("，然后", draft.Effects.Select(effect => effect.Kind switch
        {
            CustomSkillEffectKind.Draw => $"{target}摸{effect.Amount}张牌", CustomSkillEffectKind.Recover => $"{target}回复{effect.Amount}点体力",
            CustomSkillEffectKind.LoseHp => $"{target}失去{effect.Amount}点体力", _ => $"你对{target}造成{effect.Amount}点{effect.DamageNature switch { DamageNature.Fire => "火焰", DamageNature.Thunder => "雷电", DamageNature.Cold => "冰冻", _ => "普通" }}伤害"
        }));
        return $"每回合限{draft.UsesPerTurn}次。{timing}，{condition}{(draft.Trigger == CustomSkillTrigger.Active || draft.Optional ? "你可以发动此技能：" : "发动此技能：")}{effects}。";
    }

    private static string Camel<T>(T value) where T : struct, Enum => JsonNamingPolicy.CamelCase.ConvertName(value.ToString());
    private static ArgumentException Invalid(string message) => new(message);
    private static void ValidateText(string value, string label, int maximum)
    { if (string.IsNullOrWhiteSpace(value) || value.Length > maximum) throw Invalid($"{label}不能为空，且不能超过{maximum}个字符。"); }
    private static void ValidateId(string id, string prefix, string label)
    {
        if (id is null || !id.StartsWith(prefix, StringComparison.Ordinal) ||
            !(Guid.TryParseExact(id[prefix.Length..], "N", out _) || Guid.TryParseExact(id[prefix.Length..], "D", out _)))
            throw Invalid($"{label}ID须使用{prefix}加GUID的稳定格式。");
    }

    private sealed class CustomPackage(ContentRegistry registry, CustomGeneralDraft[] generals) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new(PackageId, new Version(1, 0, 0),
            Array.AsReadOnly(registry.Packages.Select(p => new PackageDependency(p.Id, p.Version)).ToArray()));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var skill in generals.SelectMany(g => g.CustomSkills).OrderBy(s => s.Id, StringComparer.Ordinal)) builder.AddSkill(CompileFrozenSkill(skill));
            foreach (var general in generals)
            {
                var skills = general.SkillIds.Concat(general.CustomSkills.Select(s => s.Id)).ToArray();
                builder.AddGeneral(new(general.Id, general.Name, general.PortraitKey, skills.FirstOrDefault(), general.FactionId,
                    general.BaseHp, skills.Length > 1 ? Array.AsReadOnly(skills[1..]) : null, general.Gender)
                { CharacterId = general.Id, VariantId = "custom", RulesetId = "custom", InitialHp = general.InitialHp });
            }
        }
    }
}
