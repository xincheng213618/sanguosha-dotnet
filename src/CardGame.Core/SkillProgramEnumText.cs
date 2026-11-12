namespace CardGame.Core;

internal static class SkillProgramEnumText
{
    internal static bool TryParse<T>(string? text, out T value) where T : struct, Enum
    {
        // Resolve the value without enumerating and formatting every candidate.
        // Only the canonical name with its first letter lowercased is supported;
        // numeric values, flags lists, alternate aliases and other casing stay invalid.
        if (Enum.TryParse(text, ignoreCase: true, out value) && Enum.GetName(value) is { } name &&
            text!.Length == name.Length && text[0] == char.ToLowerInvariant(name[0]) &&
            text.AsSpan(1).SequenceEqual(name.AsSpan(1)))
            return true;
        value = default;
        return false;
    }
}
