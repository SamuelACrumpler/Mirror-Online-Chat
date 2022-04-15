
using UnityEngine;
using TMPro;

public class TextWidthApproximator : MonoBehaviour
{ //Pulled from a forum post.

    public string Text;
    public TMP_FontAsset FontAsset;
    public int FontSize;
    public FontStyles Style;
    public TMP_Text TextComponent;

    private void Awake()
    {
        Debug.Log("Preferred Width: " + TextComponent.GetPreferredValues(Text).x);
        Debug.Log("Approximated Width: " + TextWidthApproximation(Text, FontAsset, FontSize, Style));
    }

    public float TextWidthApproximation(string text, TMP_FontAsset fontAsset, int fontSize, FontStyles style)
    {
        // Compute scale of the target point size relative to the sampling point size of the font asset.
        float pointSizeScale = fontSize / (fontAsset.faceInfo.pointSize * fontAsset.faceInfo.scale);
        float emScale = FontSize * 0.01f;

        float styleSpacingAdjustment = (style & FontStyles.Bold) == FontStyles.Bold ? fontAsset.boldSpacing : 0;
        float normalSpacingAdjustment = fontAsset.normalSpacingOffset;

        float width = 0;

        for (int i = 0; i < text.Length; i++)
        {
            char unicode = text[i];
            TMP_Character character;
            // Make sure the given unicode exists in the font asset.
            if (fontAsset.characterLookupTable.TryGetValue(unicode, out character))
                width += character.glyph.metrics.horizontalAdvance * pointSizeScale + (styleSpacingAdjustment + normalSpacingAdjustment) * emScale;
        }

        return width;
    }
}
