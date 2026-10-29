namespace CardGame.Wpf.ViewModels;

internal static class ShanheDisplayText
{
    internal static string RewardName(string name)
    {
        foreach (var prefix in new[] { "战法·", "战法 ·", "传习·", "传习 ·" })
            if (name.StartsWith(prefix, StringComparison.Ordinal)) return name[prefix.Length..].TrimStart();
        return name;
    }

    internal static string Rule(string description, string rule) =>
        (string.IsNullOrWhiteSpace(rule) ? description : rule).Trim();

    internal static string Effect(string text)
    {
        foreach (var note in new[]
        {
            "不占额外技能槽，本次征程限获得一份。", "不占额外技能槽，本版每次征程限获得一份。",
            "占用一个额外技能槽。", "占用额外技能槽。", "不占额外技能槽。", "本次征程限获得一份。"
        }) text = text.Replace(note, string.Empty, StringComparison.Ordinal);
        foreach (var prefix in new[] { "2026普通战法：", "2026稀有战法：", "普通战法：", "稀有战法：", "史诗战法：", "传说战法：" })
            if (text.StartsWith(prefix, StringComparison.Ordinal)) { text = text[prefix.Length..]; break; }
        return string.Join("\n", text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n')
            .Where(line => !new[] { "来源：", "来源说明：", "实现说明：", "验证说明：", "兼容说明：", "本地约定：" }
                .Any(prefix => line.TrimStart().StartsWith(prefix, StringComparison.Ordinal)))).Trim();
    }
}
