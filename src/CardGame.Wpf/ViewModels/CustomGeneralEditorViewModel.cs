using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Persistence;

namespace CardGame.Wpf.ViewModels;

public sealed record CustomEditorChoice<T>(T Value, string Label);
public sealed record CustomPortraitChoice(string Key, string Name);

public sealed class CustomGeneralEditorViewModel : ObservableObject
{
    private readonly ContentRegistry _baseRegistry;
    private readonly ICustomGeneralLibraryStore _store;
    private readonly Action<CustomGeneralLibrary>? _saved;
    private readonly IReadOnlyList<ContentSkillDefinition> _catalog;
    private CustomGeneralDraftViewModel? _selectedGeneral;
    private CustomSkillDraftViewModel? _selectedCustomSkill;
    private CustomExistingSkillChoice[] _allSkillChoices = [];
    private string _skillSearch = string.Empty, _preview = string.Empty, _status = string.Empty;
    private string? _operationError;
    private bool _dirty, _loading, _loadFailed;

    public CustomGeneralEditorViewModel(ContentRegistry baseRegistry, ICustomGeneralLibraryStore store, Action<CustomGeneralLibrary>? saved = null)
    {
        _baseRegistry = baseRegistry; _store = store; _saved = saved;
        _catalog = CustomGeneralContent.SelectableSkills(baseRegistry);
        PortraitChoices = baseRegistry.Generals.Values.Where(g => !string.IsNullOrWhiteSpace(g.PortraitKey))
            .OrderBy(g => g.Name, StringComparer.CurrentCulture)
            .Select(g => new CustomPortraitChoice(g.Id, g.Name)).Prepend(new("supporter", "默认形象")).ToArray();
        NewCommand = new RelayCommand(NewGeneral, () => !_loadFailed);
        DuplicateCommand = new RelayCommand(DuplicateGeneral, () => SelectedGeneral is not null && !_loadFailed);
        DeleteCommand = new RelayCommand(DeleteGeneral, () => SelectedGeneral is not null && !_loadFailed);
        ToggleEnabledCommand = new RelayCommand(() => SelectedGeneral!.Enabled = !SelectedGeneral.Enabled, () => SelectedGeneral is not null && !_loadFailed);
        SaveCommand = new RelayCommand(Save, () => !_loadFailed && HasUnsavedChanges);
        ReloadCommand = new RelayCommand(Reload);
        AddCustomSkillCommand = new RelayCommand(AddCustomSkill, () => SelectedGeneral is { SkillCount: < 12 });
        DeleteCustomSkillCommand = new RelayCommand(DeleteCustomSkill, () => SelectedGeneral is not null && SelectedCustomSkill is not null);
        AddEffectCommand = new RelayCommand(() => SelectedCustomSkill!.Effects.Add(new(new CustomSkillEffect(), Changed)), () => SelectedCustomSkill is { Effects.Count: < 8 });
        DeleteEffectCommand = new RelayCommand<CustomSkillEffectViewModel>(e => SelectedCustomSkill!.Effects.Remove(e), e => SelectedCustomSkill is { Effects.Count: > 1 } skill && skill.Effects.Contains(e));
        MoveEffectUpCommand = new RelayCommand<CustomSkillEffectViewModel>(e => MoveEffect(e, -1), e => SelectedCustomSkill?.Effects.IndexOf(e) > 0);
        MoveEffectDownCommand = new RelayCommand<CustomSkillEffectViewModel>(e => MoveEffect(e, 1), e => SelectedCustomSkill is { } skill && skill.Effects.IndexOf(e) is var index && index >= 0 && index < skill.Effects.Count - 1);
        Reload();
    }

    public ObservableRangeCollection<CustomGeneralDraftViewModel> Generals { get; } = [];
    public ObservableRangeCollection<CustomExistingSkillChoice> ExistingSkillChoices { get; } = [];
    public ObservableRangeCollection<string> Errors { get; } = [];
    public IReadOnlyList<CustomPortraitChoice> PortraitChoices { get; }
    public IReadOnlyList<int> HpValues { get; } = Enumerable.Range(1, 20).ToArray();
    public IReadOnlyList<int> AmountValues { get; } = Enumerable.Range(1, 10).ToArray();
    public IReadOnlyList<CustomEditorChoice<string>> FactionChoices { get; } = [new("wei", "魏"), new("shu", "蜀"), new("wu", "吴"), new("qun", "群"), new("jin", "晋"), new("god", "神")];
    public IReadOnlyList<CustomEditorChoice<GeneralGender>> GenderChoices { get; } = [new(GeneralGender.Male, "男"), new(GeneralGender.Female, "女")];
    public IReadOnlyList<CustomEditorChoice<CustomSkillTrigger>> TriggerChoices { get; } = [
        new(CustomSkillTrigger.Active, "出牌阶段主动发动"), new(CustomSkillTrigger.TurnStart, "回合开始时"), new(CustomSkillTrigger.PlayStart, "出牌阶段开始时"),
        new(CustomSkillTrigger.PlayEnd, "出牌阶段结束时"), new(CustomSkillTrigger.TurnEnd, "回合结束时"), new(CustomSkillTrigger.AfterDamage, "受到伤害后"), new(CustomSkillTrigger.AfterDealtDamage, "造成伤害后")];
    public IReadOnlyList<CustomEditorChoice<CustomSkillTarget>> TargetChoices { get; } = [new(CustomSkillTarget.Self, "自己"), new(CustomSkillTarget.Other, "选择一名其他角色"), new(CustomSkillTarget.Any, "选择一名角色（可选自己）")];
    public IReadOnlyList<CustomEditorChoice<CustomSkillCondition>> ConditionChoices { get; } = [new(CustomSkillCondition.Always, "无额外条件"), new(CustomSkillCondition.Wounded, "自己已受伤"), new(CustomSkillCondition.HandEmpty, "自己没有手牌"), new(CustomSkillCondition.HpAtMost, "自己的体力不高于指定值")];
    public IReadOnlyList<CustomEditorChoice<CustomSkillEffectKind>> EffectChoices { get; } = [new(CustomSkillEffectKind.Draw, "摸牌"), new(CustomSkillEffectKind.Recover, "回复体力"), new(CustomSkillEffectKind.Damage, "造成伤害"), new(CustomSkillEffectKind.LoseHp, "失去体力")];
    public IReadOnlyList<CustomEditorChoice<DamageNature>> DamageChoices { get; } = [new(DamageNature.Normal, "普通"), new(DamageNature.Fire, "火焰"), new(DamageNature.Thunder, "雷电"), new(DamageNature.Cold, "冰冻")];

    public CustomGeneralDraftViewModel? SelectedGeneral
    {
        get => _selectedGeneral;
        set { if (!SetProperty(ref _selectedGeneral, value)) return; SelectedCustomSkill = value?.CustomSkills.FirstOrDefault(); RebuildSkillChoices(); RefreshPreview(); RefreshCommands(); RaisePropertyChanged(nameof(HasSelection)); RaisePropertyChanged(nameof(SelectedSkillCountText)); }
    }
    public CustomSkillDraftViewModel? SelectedCustomSkill
    { get => _selectedCustomSkill; set { if (SetProperty(ref _selectedCustomSkill, value)) { RaisePropertyChanged(nameof(HasCustomSkillSelection)); RefreshCommands(); } } }
    public bool HasSelection => SelectedGeneral is not null;
    public bool HasCustomSkillSelection => SelectedCustomSkill is not null;
    public bool HasErrors => Errors.Count > 0;
    public bool HasUnsavedChanges { get => _dirty; private set { if (SetProperty(ref _dirty, value)) { RaisePropertyChanged(nameof(ChangeStatus)); RefreshCommands(); } } }
    public string ChangeStatus => HasUnsavedChanges ? "有未保存的修改" : "本地库已保存";
    public string Status { get => _status; private set => SetProperty(ref _status, value); }
    public string Preview { get => _preview; private set => SetProperty(ref _preview, value); }
    public string SelectedSkillCountText => $"已选 {SelectedGeneral?.SkillCount ?? 0} / 12 个技能";
    public string SkillSearch { get => _skillSearch; set { if (SetProperty(ref _skillSearch, value ?? string.Empty)) FilterSkills(); } }
    public ICommand NewCommand { get; }
    public ICommand DuplicateCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ToggleEnabledCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand ReloadCommand { get; }
    public ICommand AddCustomSkillCommand { get; }
    public ICommand DeleteCustomSkillCommand { get; }
    public ICommand AddEffectCommand { get; }
    public ICommand DeleteEffectCommand { get; }
    public ICommand MoveEffectUpCommand { get; }
    public ICommand MoveEffectDownCommand { get; }

    private void Reload()
    {
        try
        {
            var library = CustomGeneralContent.Snapshot(_store.Read());
            _loading = true; _loadFailed = false; _operationError = null;
            Generals.ReplaceAll(library.Generals.Select(d => new CustomGeneralDraftViewModel(d, Changed)));
            SelectedGeneral = Generals.FirstOrDefault(); _loading = false; HasUnsavedChanges = false;
            Status = "修改只在保存后用于下一新局，当前对局不会变化。"; RefreshPreview(); RefreshCommands();
        }
        catch (Exception error) when (IsEditorError(error))
        { _loading = false; _loadFailed = true; _operationError = "读取武将库失败：" + error.Message; Status = "原武将库没有被覆盖。处理文件问题后可重新读取。"; RefreshPreview(); RefreshCommands(); }
    }
    private void NewGeneral()
    {
        var item = new CustomGeneralDraftViewModel(new() { Id = NewId("general"), Name = "新武将", PortraitKey = _baseRegistry.Generals.ContainsKey("classic:zhao-yun") ? "classic:zhao-yun" : "supporter" }, Changed);
        Generals.Add(item); SelectedGeneral = item; Changed();
    }
    private void DuplicateGeneral()
    {
        var draft = SelectedGeneral!.ToDraft();
        var copy = draft with { Id = NewId("general"), Name = draft.Name + "（副本）", CustomSkills = draft.CustomSkills.Select(s => s with { Id = NewId("skill") }).ToArray() };
        var item = new CustomGeneralDraftViewModel(copy, Changed); Generals.Add(item); SelectedGeneral = item; Changed();
    }
    private void DeleteGeneral()
    { var index = Generals.IndexOf(SelectedGeneral!); Generals.RemoveAt(index); SelectedGeneral = Generals.ElementAtOrDefault(Math.Min(index, Generals.Count - 1)); Changed(); Status = "已从草稿库移除；保存后才会从本地库删除。"; }
    private void AddCustomSkill()
    {
        var skill = new CustomSkillDraftViewModel(new() { Id = NewId("skill"), Name = "新技能", Effects = [new()] }, Changed);
        SelectedGeneral!.CustomSkills.Add(skill); SelectedCustomSkill = skill;
    }
    private void DeleteCustomSkill()
    { SelectedGeneral!.CustomSkills.Remove(SelectedCustomSkill!); SelectedCustomSkill = SelectedGeneral.CustomSkills.FirstOrDefault(); }
    private void MoveEffect(CustomSkillEffectViewModel effect, int delta)
    { var list = SelectedCustomSkill!.Effects; var index = list.IndexOf(effect); list.Move(index, index + delta); }
    private void Changed()
    {
        if (_loading) return;
        _operationError = null; HasUnsavedChanges = true; Status = "修改尚未保存。保存后用于下一新局。";
        foreach (var choice in _allSkillChoices) choice.CanSelect = choice.IsSelected || SelectedGeneral?.SkillCount < 12;
        RaisePropertyChanged(nameof(SelectedSkillCountText)); RefreshPreview(); RefreshCommands();
    }
    private void RebuildSkillChoices()
    {
        var general = SelectedGeneral;
        _allSkillChoices = general is null ? [] : _catalog.Select(skill => new CustomExistingSkillChoice(skill.Id, skill.Name, skill.Description,
            general.ExistingSkillIds.Contains(skill.Id), selected => general.SetExistingSkill(skill.Id, selected)))
            .Concat(general.ExistingSkillIds.Where(id => !_catalog.Any(skill => skill.Id == id)).Select(id =>
                new CustomExistingSkillChoice(id, "不可用技能：" + id, "当前规则库没有此技能，请取消选择后重新保存。", true, selected => general.SetExistingSkill(id, selected))))
            .ToArray();
        foreach (var choice in _allSkillChoices) choice.CanSelect = choice.IsSelected || general!.SkillCount < 12;
        FilterSkills();
    }
    private void FilterSkills() => ExistingSkillChoices.ReplaceAll(_allSkillChoices.Where(s => SkillSearch.Length == 0 ||
        s.Name.Contains(SkillSearch, StringComparison.OrdinalIgnoreCase) || s.Description.Contains(SkillSearch, StringComparison.OrdinalIgnoreCase) || s.Id.Contains(SkillSearch, StringComparison.OrdinalIgnoreCase)));
    private void RefreshPreview()
    {
        var errors = new List<string>(); if (_operationError is not null) errors.Add(_operationError);
        if (SelectedGeneral is not { } general) { Preview = "从左侧新建武将，或选择已有草稿开始编辑。"; SetErrors(errors); return; }
        var draft = general.ToDraft(); var paragraphs = new List<string> { $"{general.DisplayName} · {FactionChoices.FirstOrDefault(c => c.Value == draft.FactionId)?.Label ?? draft.FactionId} · {(draft.Gender == GeneralGender.Female ? "女" : "男")} · {(draft.InitialHp is { } hp ? hp + "/" : "")}{draft.BaseHp} 体力" };
        if (string.IsNullOrWhiteSpace(draft.Name)) errors.Add("请填写武将姓名。");
        if (general.SkillCount is < 1 or > 12) errors.Add("每位武将需要选择或创建 1 至 12 个技能。");
        foreach (var id in draft.SkillIds)
            if (_baseRegistry.Skills.TryGetValue(id, out var skill)) paragraphs.Add($"【{skill.Name}】{skill.Description}");
            else errors.Add($"已有技能 {id} 不可用，请取消选择。");
        foreach (var custom in draft.CustomSkills)
            try { var compiled = CustomGeneralContent.CompileSkill(custom); paragraphs.Add($"【{compiled.Name}】{compiled.Description}"); }
            catch (Exception error) when (IsEditorError(error)) { errors.Add($"{(string.IsNullOrWhiteSpace(custom.Name) ? "自捏技能" : custom.Name)}：{error.Message}"); }
        Preview = string.Join("\n\n", paragraphs); SetErrors(errors);
    }
    private void Save()
    {
        try
        {
            var library = CustomGeneralContent.Snapshot(new CustomGeneralLibrary { Generals = Generals.Select(g => g.ToDraft()).ToArray() });
            _ = CustomGeneralContent.CreateRegistry(_baseRegistry, library);
            _store.Write(library); HasUnsavedChanges = false; _operationError = null; SetErrors([]);
            Status = "已保存到本地武将库。下一新局生效，当前对局保持原规则。"; _saved?.Invoke(library);
        }
        catch (Exception error) when (IsEditorError(error))
        { _operationError = "保存失败：" + error.Message; Status = "修改仍保留在编辑器中，请修正后重试。"; RefreshPreview(); }
    }
    private void SetErrors(IEnumerable<string> values) { Errors.ReplaceAll(values); RaisePropertyChanged(nameof(HasErrors)); }
    private void RefreshCommands()
    {
        foreach (var command in new[] { NewCommand, DuplicateCommand, DeleteCommand, ToggleEnabledCommand, SaveCommand, AddCustomSkillCommand, DeleteCustomSkillCommand, AddEffectCommand })
            (command as RelayCommand)?.NotifyCanExecuteChanged();
        (DeleteEffectCommand as RelayCommand<CustomSkillEffectViewModel>)?.NotifyCanExecuteChanged();
        (MoveEffectUpCommand as RelayCommand<CustomSkillEffectViewModel>)?.NotifyCanExecuteChanged();
        (MoveEffectDownCommand as RelayCommand<CustomSkillEffectViewModel>)?.NotifyCanExecuteChanged();
    }
    private static bool IsEditorError(Exception error) => error is ArgumentException or InvalidOperationException or IOException or UnauthorizedAccessException or System.Text.Json.JsonException;
    private static string NewId(string kind) => $"custom:{kind}-{Guid.NewGuid():N}";
}

public sealed class CustomExistingSkillChoice(string id, string name, string description, bool selected, Action<bool> changed) : ObservableObject
{
    private bool _selected = selected, _canSelect = true;
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public bool IsSelected { get => _selected; set { if (SetProperty(ref _selected, value)) changed(value); } }
    public bool CanSelect { get => _canSelect; set => SetProperty(ref _canSelect, value); }
}

public sealed class CustomGeneralDraftViewModel : ObservableObject
{
    private readonly Action _changed;
    private string _name, _faction, _portrait;
    private int _hp, _initialHp;
    private bool _enabled, _useInitialHp;
    private GeneralGender _gender;
    public CustomGeneralDraftViewModel(CustomGeneralDraft draft, Action changed)
    {
        _changed = changed; Id = draft.Id; _name = draft.Name; _faction = draft.FactionId; _portrait = draft.PortraitKey;
        _hp = draft.BaseHp; _initialHp = draft.InitialHp ?? draft.BaseHp; _useInitialHp = draft.InitialHp.HasValue; _enabled = draft.Enabled; _gender = draft.Gender;
        ExistingSkillIds = new(draft.SkillIds); ExistingSkillIds.CollectionChanged += (_, _) => Edit();
        CustomSkills = new(draft.CustomSkills.Select(s => new CustomSkillDraftViewModel(s, Edit)));
        CustomSkills.CollectionChanged += (_, _) => Edit();
    }
    public string Id { get; }
    public ObservableCollection<string> ExistingSkillIds { get; }
    public ObservableCollection<CustomSkillDraftViewModel> CustomSkills { get; }
    public int SkillCount => ExistingSkillIds.Count + CustomSkills.Count;
    public string DisplayName => string.IsNullOrWhiteSpace(Name) ? "未命名武将" : Name;
    public string EnabledText => Enabled ? "已启用" : "已停用";
    public string Name { get => _name; set { if (SetProperty(ref _name, value)) { RaisePropertyChanged(nameof(DisplayName)); Edit(); } } }
    public string FactionId { get => _faction; set { if (SetProperty(ref _faction, value)) Edit(); } }
    public string PortraitKey { get => _portrait; set { if (SetProperty(ref _portrait, value)) Edit(); } }
    public int BaseHp { get => _hp; set { if (SetProperty(ref _hp, value)) Edit(); } }
    public int InitialHp { get => _initialHp; set { if (SetProperty(ref _initialHp, value)) Edit(); } }
    public bool UseInitialHp { get => _useInitialHp; set { if (SetProperty(ref _useInitialHp, value)) Edit(); } }
    public GeneralGender Gender { get => _gender; set { if (SetProperty(ref _gender, value)) Edit(); } }
    public bool Enabled { get => _enabled; set { if (SetProperty(ref _enabled, value)) { RaisePropertyChanged(nameof(EnabledText)); Edit(); } } }
    public void SetExistingSkill(string id, bool selected) { if (selected && !ExistingSkillIds.Contains(id)) ExistingSkillIds.Add(id); else if (!selected) ExistingSkillIds.Remove(id); }
    public CustomGeneralDraft ToDraft() => new() { Id = Id, Name = Name, FactionId = FactionId, PortraitKey = PortraitKey, BaseHp = BaseHp,
        InitialHp = UseInitialHp ? InitialHp : null, Gender = Gender, Enabled = Enabled, SkillIds = ExistingSkillIds.ToArray(), CustomSkills = CustomSkills.Select(s => s.ToDraft()).ToArray() };
    private void Edit() { RaisePropertyChanged(nameof(SkillCount)); _changed(); }
}

public sealed class CustomSkillDraftViewModel : ObservableObject
{
    private readonly Action _changed;
    private string _name; private CustomSkillTrigger _trigger; private CustomSkillTarget _target; private CustomSkillCondition _condition;
    private int _conditionValue, _uses; private bool _optional;
    public CustomSkillDraftViewModel(CustomSkillDraft draft, Action changed)
    {
        _changed = changed; Id = draft.Id; _name = draft.Name; _trigger = draft.Trigger; _target = draft.Target; _condition = draft.Condition;
        _conditionValue = draft.ConditionValue; _uses = draft.UsesPerTurn; _optional = draft.Optional;
        Effects = new(draft.Effects.Select(e => new CustomSkillEffectViewModel(e, changed))); Effects.CollectionChanged += (_, _) => changed();
    }
    public string Id { get; }
    public ObservableCollection<CustomSkillEffectViewModel> Effects { get; }
    public bool NeedsConditionValue => Condition == CustomSkillCondition.HpAtMost;
    public bool IsTriggered => Trigger != CustomSkillTrigger.Active;
    public string Name { get => _name; set { if (SetProperty(ref _name, value)) _changed(); } }
    public CustomSkillTrigger Trigger { get => _trigger; set { if (SetProperty(ref _trigger, value)) { RaisePropertyChanged(nameof(IsTriggered)); _changed(); } } }
    public CustomSkillTarget Target { get => _target; set { if (SetProperty(ref _target, value)) _changed(); } }
    public CustomSkillCondition Condition { get => _condition; set { if (SetProperty(ref _condition, value)) { RaisePropertyChanged(nameof(NeedsConditionValue)); _changed(); } } }
    public int ConditionValue { get => _conditionValue; set { if (SetProperty(ref _conditionValue, value)) _changed(); } }
    public int UsesPerTurn { get => _uses; set { if (SetProperty(ref _uses, value)) _changed(); } }
    public bool Optional { get => _optional; set { if (SetProperty(ref _optional, value)) _changed(); } }
    public CustomSkillDraft ToDraft() => new() { Id = Id, Name = Name, Trigger = Trigger, Target = Target, Condition = Condition,
        ConditionValue = ConditionValue, UsesPerTurn = UsesPerTurn, Optional = Optional, Effects = Effects.Select(e => e.ToDraft()).ToArray() };
}

public sealed class CustomSkillEffectViewModel(CustomSkillEffect effect, Action changed) : ObservableObject
{
    private CustomSkillEffectKind _kind = effect.Kind; private int _amount = effect.Amount; private DamageNature _nature = effect.DamageNature;
    public bool IsDamage => Kind == CustomSkillEffectKind.Damage;
    public CustomSkillEffectKind Kind { get => _kind; set { if (SetProperty(ref _kind, value)) { RaisePropertyChanged(nameof(IsDamage)); changed(); } } }
    public int Amount { get => _amount; set { if (SetProperty(ref _amount, value)) changed(); } }
    public DamageNature DamageNature { get => _nature; set { if (SetProperty(ref _nature, value)) changed(); } }
    public CustomSkillEffect ToDraft() => new() { Kind = Kind, Amount = Amount, DamageNature = DamageNature };
}
