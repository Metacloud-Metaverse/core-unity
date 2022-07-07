using UnityEngine.UI;
using UnityEngine;
public static class RectTransformExtensions
{
    ///<summary>
    /// Returns a Rect in WorldSpace dimensions using <see cref="RectTransform.GetWorldCorners"/>
    ///</summary>
    public static Rect GetWorldRect(this RectTransform rectTransform)
    {
        var r = rectTransform.rect;
        r.center = rectTransform.TransformPoint(r.center);
        r.size = rectTransform.TransformVector(r.size);
        return r;
    }

    ///<summary>
    /// Checks if a <see cref="RectTransform"/> fully encloses another one
    ///</summary>
    public static bool Overlap(this RectTransform rectTransform, RectTransform other)
    {
        var rect = rectTransform.GetWorldRect();
        var otherRect = other.GetWorldRect();
        return rect.Overlaps(otherRect);
    }
    public static bool Contains(this RectTransform rectTransform, RectTransform other)
    {
        var rect = rectTransform.GetWorldRect();
        var otherRect = other.GetWorldRect();

        return rect.Contains(otherRect.center);
    }
}